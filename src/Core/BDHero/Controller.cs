// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BDHero.Plugin;
using BDHero.JobQueue;
using BDHero.Prefs;
using DotNetUtils.Concurrency;
using DotNetUtils.FS;

namespace BDHero
{
    public class Controller : IController
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IPluginRepository _pluginRepository;
        private readonly IPreferenceManager _preferenceManager;

        /// <summary>
        /// Needed for <see cref="ProgressProviderOnUpdated"/> to invoke progress update callbacks on the correct thread.
        /// </summary>
        private ISynchronizeInvoke _uiContext;

        private IInvoker _uiInvoker;

        private readonly ConcurrentDictionary<Guid, int> _progressMap = new ConcurrentDictionary<Guid, int>();

        #region Properties

        public Job Job { get; private set; }

        public IList<IPlugin> PluginsByType
        {
            get { return _pluginRepository.PluginsByType; }
        }

        #endregion

        #region Events

        public event BeforePromiseHandler<bool> BeforeScanStart;
        public event SuccessPromiseHandler<bool> ScanSucceeded;
        public event FailurePromiseHandler<bool> ScanFailed;
        public event AlwaysPromiseHandler<bool> ScanCompleted;

        public event BeforePromiseHandler<bool> ConvertStarted;
        public event SuccessPromiseHandler<bool> ConvertSucceeded;
        public event FailurePromiseHandler<bool> ConvertFailed;
        public event AlwaysPromiseHandler<bool> ConvertCompleted;

        public event PluginProgressHandler PluginProgressUpdated;
        public event UnhandledExceptionEventHandler UnhandledException;

        #endregion

        public Controller(IPluginRepository pluginRepository, IPreferenceManager preferenceManager)
        {
            _pluginRepository = pluginRepository;
            _preferenceManager = preferenceManager;
        }

        public void SetUIContextCurrentThread()
        {
            _uiContext = new Control();
            _uiInvoker = new UIInvoker(_uiContext);
        }

        public void SetUIContext(ISynchronizeInvoke uiContext)
        {
            _uiContext = uiContext;
            _uiInvoker = new UIInvoker(_uiContext);
        }

        #region User-invokable tasks

        public void RenameSync(string mkvPath)
        {
            CreateRenamePhase(CancellationToken.None, mkvPath)();
        }

        public IPromise<bool> CreateMetadataTask(CancellationToken cancellationToken, BeforePromiseHandler<bool> start, FailurePromiseHandler<bool> fail, SuccessPromiseHandler<bool> succeed, string mkvPath = null)
        {
            var token = cancellationToken;
            var optionalPhases = new[] { CreateGetMetadataPhase(token), CreateAutoDetectPhase(token), CreateRenamePhase(token, mkvPath) };
            return CreateStageTask(
                token,
                start,
                () => true,
                optionalPhases,
                fail,
                succeed
            );
        }

        #endregion

        #region Stages

        public IPromise<bool> CreateScanTask(CancellationToken cancellationToken, string bdromPath, string mkvPath = null)
        {
            SaveRecentBDROMBeforeScan(bdromPath);
            var token = cancellationToken;
            var readBDROMPhase = new CriticalPhase(() => ReadBDROM(token, bdromPath));
            var optionalPhases = new[] { CreateGetMetadataPhase(token), CreateAutoDetectPhase(token), CreateRenamePhase(token, mkvPath) };
            return CreateStageTask(
                token,
                ScanStart,
                readBDROMPhase,
                optionalPhases,
                ScanFail,
                ScanSucceed
            );
        }

        private OptionalPhase CreateGetMetadataPhase(CancellationToken cancellationToken)
        {
            return () => GetMetadata(cancellationToken);
        }

        private OptionalPhase CreateAutoDetectPhase(CancellationToken cancellationToken)
        {
            return () => AutoDetect(cancellationToken);
        }

        private OptionalPhase CreateRenamePhase(CancellationToken cancellationToken, string mkvPath = null)
        {
            return () => Rename(cancellationToken, mkvPath);
        }

        public IPromise<bool> CreateConvertTask(CancellationToken cancellationToken, string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            var token = cancellationToken;
            var muxPhase = new CriticalPhase(() => Mux(token));
            var optionalPhases = new OptionalPhase[] { () => PostProcess(token) };
            return CreateStageTask(
                token,
                ConvertStart,
                muxPhase,
                optionalPhases,
                ConvertFail,
                ConvertSucceed
            );
        }

        /// <summary>
        /// Creates an asynchronous Task object that executes the given critical phase and optional phases in a background thread
        /// and invokes all other callbacks on the UI thread.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="beforeStart">Invoked on UI thread</param>
        /// <param name="criticalPhase">First phase to run.  Must succeed (by returning <c>true</c> and not throwing an exception) for the optional phases to run.  Invoked on the background thread.</param>
        /// <param name="optionalPhases">Collection of phases that can fail (by throwing an exception) without preventing subsequent phases from running.  Invoked on the background thread.</param>
        /// <param name="fail">Called if the operation is canceled or the critical phase throws an exception.  Invoked on the UI thread.</param>
        /// <param name="succeed">Called if the operation completes successfully without being canceled.  Invoked on the UI thread.</param>
        /// <returns>
        /// Task object that returns <c>false</c> if the operation was canceled by the user or
        /// the critical phase threw an exception; otherwise <c>true</c>.
        /// </returns>
        private IPromise<bool> CreateStageTask(CancellationToken cancellationToken, BeforePromiseHandler<bool> beforeStart, CriticalPhase criticalPhase, IEnumerable<OptionalPhase> optionalPhases, FailurePromiseHandler<bool> fail, SuccessPromiseHandler<bool> succeed)
        {
            return new SimplePromise(_uiContext)
                .CancelWith(cancellationToken)
                .Before(beforeStart)
                .Fail(fail)
                .Canceled(promise => fail(promise)) // TODO: Fix warning
                .Work(delegate(IPromise<bool> promise)
                      {
                          cancellationToken.Register(() => Logger.Warn("User canceled current operation"));

                          if (criticalPhase())
                          {
                              foreach (var phase in optionalPhases.TakeWhile(phase => CanContinue(promise)))
                              {
                                  phase();
                              }

                              if (CanContinue(promise))
                              {
                                  _uiInvoker.InvokeAsync(() => succeed(promise));
                                  return;
                              }
                          }

                          // TODO: How should we handle exceptions here?
                          // The rest of the code assumes exceptions are being handled by the plugin runner.
                          _uiInvoker.InvokeAsync(() => fail(promise));
                      })
                ;
        }

        private static bool CanContinue(IPromise<bool> promise)
        {
            return !promise.IsCancellationRequested && promise.LastException == null;
        }

        private IPromise<bool> RunPluginSync(CancellationToken cancellationToken, IPlugin plugin, ExecutePluginHandler pluginRunner)
        {
            var promise =
                new SimplePromise(_uiContext)
                    .CancelWith(cancellationToken)
                    .Before(delegate
                            {
                                var progressProvider = _pluginRepository.GetProgressProvider(plugin);

                                progressProvider.Updated -= ProgressProviderOnUpdated;
                                progressProvider.Updated += ProgressProviderOnUpdated;

                                progressProvider.Reset();
                                progressProvider.Start();
                            })
                    .Work(delegate
                          {
                              pluginRunner(cancellationToken);
                          })
                    .Fail(delegate(IPromise<bool> p)
                          {
                              var progressProvider = _pluginRepository.GetProgressProvider(plugin);

                              // Sanity check
                              if (cancellationToken.IsCancellationRequested ||
                                  progressProvider.State == ProgressProviderState.Canceled)
                              {
                                  progressProvider.Cancel();
                                  return;
                              }

                              progressProvider.Error(p.LastException);
                              HandleUnhandledException(p.LastException);
                          })
                    .Canceled(delegate
                              {
                                  _pluginRepository.GetProgressProvider(plugin).Cancel();
                              })
                    .Done(delegate
                          {
                              var progressProvider = _pluginRepository.GetProgressProvider(plugin);
                              if (cancellationToken.IsCancellationRequested)
                              {
                                  progressProvider.Cancel();
                              }
                              else
                              {
                                  progressProvider.Succeed();
                              }
                          })
                ;
            promise.Start().Wait();
            return promise;
        }

        #endregion

        #region Phases

        #region 1 - Disc Reader

        private bool ReadBDROM(CancellationToken cancellationToken, string bdromPath)
        {
            IDiscReaderPlugin discReader = _pluginRepository.DiscReaderPlugins.First(plugin => plugin.Enabled);
            var pluginTask = RunPluginSync(cancellationToken, discReader, delegate(CancellationToken token)
                {
                    var disc = discReader.ReadBDROM(token, bdromPath);
                    if (!token.IsCancellationRequested)
                    {
                        Job = new Job(disc);
                    }
                });
            return pluginTask.IsCompleted && pluginTask.Result;
        }

        #endregion

        #region 2 - Metadata API Search

        private void GetMetadata(CancellationToken cancellationToken)
        {
            Job.Movies.Clear();
            Job.TVShows.Clear();
            Job.Disc.Playlists.ForEach(playlist => playlist.ChapterSearchResults.Clear());

            foreach (var plugin in _pluginRepository.MetadataProviderPlugins.Where(plugin => plugin.Enabled))
            {
                if (cancellationToken.IsCancellationRequested) return;
                GetMetadata(cancellationToken, plugin);
            }
        }

        private void GetMetadata(CancellationToken cancellationToken, IMetadataProviderPlugin plugin)
        {
            RunPluginSync(cancellationToken, plugin, token => plugin.GetMetadata(token, Job));
        }

        #endregion

        #region 3 - Auto Detect

        private void AutoDetect(CancellationToken cancellationToken)
        {
            foreach (var plugin in _pluginRepository.AutoDetectorPlugins.Where(plugin => plugin.Enabled))
            {
                if (cancellationToken.IsCancellationRequested) return;
                AutoDetect(cancellationToken, plugin);
            }

            foreach (var playlist in Job.Disc.ValidMainFeaturePlaylists)
            {
                Logger.Info(playlist);
            }
        }

        private void AutoDetect(CancellationToken cancellationToken, IAutoDetectorPlugin plugin)
        {
            RunPluginSync(cancellationToken, plugin, token => plugin.AutoDetect(token, Job));
        }

        #endregion

        #region 4 - Name Providers

        private void Rename(CancellationToken cancellationToken, string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;
            foreach (var plugin in _pluginRepository.NameProviderPlugins.Where(plugin => plugin.Enabled))
            {
                if (cancellationToken.IsCancellationRequested) return;
                Rename(cancellationToken, plugin);
            }
        }

        private void Rename(CancellationToken cancellationToken, INameProviderPlugin plugin)
        {
            RunPluginSync(cancellationToken, plugin, token => plugin.Rename(token, Job));
        }

        #endregion

        #region 5 - Mux

        private void EnsureOutputDirExists()
        {
            if (string.IsNullOrWhiteSpace(Job.OutputPath))
                throw new FileNotFoundException("Required output path not specified");

            FileUtils.CreateDirectory(Job.OutputPath);
        }

        private bool Mux(CancellationToken cancellationToken)
        {
            var enabledMuxerPlugins = _pluginRepository.MuxerPlugins.Where(plugin => plugin.Enabled).ToArray();

            if (!enabledMuxerPlugins.Any())
                return false;

            EnsureOutputDirExists();

            return enabledMuxerPlugins.All(muxer => Mux(cancellationToken, muxer));
        }

        private bool Mux(CancellationToken cancellationToken, IMuxerPlugin plugin)
        {
            var pluginTask = RunPluginSync(cancellationToken, plugin, token => plugin.Mux(token, Job));
            return pluginTask.IsCompleted && pluginTask.Result;
        }

        #endregion

        #region 6 - Post Process

        private void PostProcess(CancellationToken cancellationToken)
        {
            foreach (var plugin in _pluginRepository.PostProcessorPlugins.Where(plugin => plugin.Enabled))
            {
                if (cancellationToken.IsCancellationRequested) return;
                PostProcess(cancellationToken, plugin);
            }
        }

        private void PostProcess(CancellationToken cancellationToken, IPostProcessorPlugin plugin)
        {
            RunPluginSync(cancellationToken, plugin, token => plugin.PostProcess(token, Job));
        }

        #endregion

        #endregion

        #region Plugin progress reporting

        private void ProgressProviderOnUpdated(ProgressProvider progressProvider)
        {
            if (PluginProgressUpdated != null)
            {
                _uiInvoker.InvokeAsync(() => ProgressProviderOnUpdatedSync(progressProvider));
            }
        }

        private void ProgressProviderOnUpdatedSync(ProgressProvider progressProvider)
        {
            var guid = progressProvider.Plugin.AssemblyInfo.Guid;
            var hashCode = progressProvider.GetHashCode();

            var containsKey = _progressMap.ContainsKey(guid);
            var prevHashCode = containsKey ? _progressMap[guid] : -1;

#if DEBUG_CONCURRENCY
            Logger.DebugFormat("ProgressProviderOnUpdated() - Plugin \"{0}\": prev progress hashCode = {1}, cur progress hashCode = {2}",
                               progressProvider.Plugin.Name, prevHashCode, hashCode
                );
#endif

            // Progress hasn't changed since last update
            if (containsKey && prevHashCode == hashCode)
            {
                return;
            }

            _progressMap[guid] = hashCode;

#if DEBUG_CONCURRENCY
            Logger.Debug("ProgressProviderOnUpdated() - Calling PluginProgressUpdated event handlers");
#endif

            PluginProgressUpdated(progressProvider.Plugin, progressProvider);
        }

        #endregion

        #region Exception handling

        private void HandleUnhandledException(Exception exception)
        {
            var message = string.Format("Unhandled exception was thrown by plugin");
            Logger.Error(message, exception);
            if (UnhandledException != null)
                UnhandledException(this, new UnhandledExceptionEventArgs(exception, false));
        }

        #endregion

        #region Event calling methods

        private void ScanStart(IPromise<bool> promise)
        {
            if (BeforeScanStart != null)
                BeforeScanStart(promise);
        }

        private void ScanFail(IPromise<bool> promise)
        {
            if (ScanFailed != null)
                ScanFailed(promise);

            ScanComplete(promise);
        }

        private void ScanSucceed(IPromise<bool> promise)
        {
            SaveRecentBDROMAfterSuccessfulScan(Job.Disc.FileSystem.Directories.Root.FullName);

            if (ScanSucceeded != null)
                ScanSucceeded(promise);

            ScanComplete(promise);
        }

        private void ScanComplete(IPromise<bool> promise)
        {
            if (ScanCompleted != null)
                ScanCompleted(promise);
        }

        private void ConvertStart(IPromise<bool> promise)
        {
            if (ConvertStarted != null)
                ConvertStarted(promise);
        }

        private void ConvertFail(IPromise<bool> promise)
        {
            if (ConvertFailed != null)
                ConvertFailed(promise);

            ConvertComplete(promise);
        }

        private void ConvertSucceed(IPromise<bool> promise)
        {
            if (ConvertSucceeded != null)
                ConvertSucceeded(promise);

            ConvertComplete(promise);
        }

        private void ConvertComplete(IPromise<bool> promise)
        {
            if (ConvertCompleted != null)
                ConvertCompleted(promise);
        }

        #endregion

        #region User Preferences

        private void SaveRecentBDROMBeforeScan(string bdromPath)
        {
            SaveRecentBDROM(bdromPath, false);
        }

        private void SaveRecentBDROMAfterSuccessfulScan(string bdromPath)
        {
            SaveRecentBDROM(bdromPath, true);
        }

        private void SaveRecentBDROM(string bdromPath, bool isSuccessfulScan)
        {
            var recentFiles = _preferenceManager.Preferences.RecentFiles;
            if (!recentFiles.RememberRecentFiles) { return; }
            if (recentFiles.SaveOnlyOnSuccessfulScan && !isSuccessfulScan) { return; }
            _preferenceManager.UpdatePreferences(prefs => prefs.RecentFiles.AddBDROM(bdromPath));
        }

        #endregion
    }

    internal delegate void ExecutePluginHandler(CancellationToken token);

    internal delegate bool CriticalPhase();

    internal delegate void OptionalPhase();
}
