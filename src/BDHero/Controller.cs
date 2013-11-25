using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils;
using DotNetUtils.FS;
using DotNetUtils.TaskUtils;

namespace BDHero
{
    public class Controller : IController
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IPluginRepository _pluginRepository;

        /// <summary>
        /// Needed for <see cref="ProgressProviderOnUpdated"/> to invoke progress update callbacks on the correct thread.
        /// </summary>
        private TaskScheduler _callbackScheduler;

        private readonly ConcurrentDictionary<string, int> _progressMap = new ConcurrentDictionary<string, int>();

        #region Properties

        public Job Job { get; private set; }

        public IList<IPlugin> PluginsByType
        {
            get { return _pluginRepository.PluginsByType; }
        }

        #endregion

        #region Events

        public event TaskStartedEventHandler ScanStarted;
        public event TaskSucceededEventHandler ScanSucceeded;
        public event ExceptionEventHandler ScanFailed;
        public event TaskCompletedEventHandler ScanCompleted;

        public event TaskStartedEventHandler ConvertStarted;
        public event TaskSucceededEventHandler ConvertSucceeded;
        public event ExceptionEventHandler ConvertFailed;
        public event TaskCompletedEventHandler ConvertCompleted;

        public event PluginProgressHandler PluginProgressUpdated;
        public event UnhandledExceptionEventHandler UnhandledException;

        #endregion

        public Controller(IPluginRepository pluginRepository)
        {
            _pluginRepository = pluginRepository;
        }

        public void SetEventScheduler(TaskScheduler scheduler = null)
        {
            // Get the calling thread's context
            _callbackScheduler = scheduler ??
                                (SynchronizationContext.Current != null
                                     ? TaskScheduler.FromCurrentSynchronizationContext()
                                     : TaskScheduler.Default);
        }

        #region User-invokable tasks

        public void RenameSync(string mkvPath)
        {
            CreateRenamePhase(CancellationToken.None, mkvPath)();
        }

        public Task<bool> CreateMetadataTask(CancellationToken cancellationToken, TaskStartedEventHandler start, ExceptionEventHandler fail, TaskSucceededEventHandler succeed, string mkvPath = null)
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

        public Task<bool> CreateScanTask(CancellationToken cancellationToken, string bdromPath, string mkvPath = null)
        {
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

        public Task<bool> CreateConvertTask(CancellationToken cancellationToken, string mkvPath = null)
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
        private Task<bool> CreateStageTask(CancellationToken cancellationToken, TaskStartedEventHandler beforeStart, CriticalPhase criticalPhase, IEnumerable<OptionalPhase> optionalPhases, ExceptionEventHandler fail, TaskSucceededEventHandler succeed)
        {
            var canContinue = CreateCanContinueFunc(cancellationToken);
            return new TaskBuilder()
                .OnThread(_callbackScheduler)
                .CancelWith(cancellationToken)
                .BeforeStart(beforeStart)
                .Fail(fail)
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        cancellationToken.Register(() => Logger.Warn("User canceled current operation"));

                        if (criticalPhase())
                        {
                            foreach (var phase in optionalPhases.TakeWhile(phase => canContinue()))
                            {
                                phase();
                            }

                            if (canContinue())
                            {
                                invoker.InvokeOnUIThreadAsync(_ => succeed());
                                return;
                            }
                        }

                        // TODO: How should we handle exceptions here?
                        // The rest of the code assumes exceptions are being handled by the plugin runner.
                        invoker.InvokeOnUIThreadAsync(_ => fail(new ExceptionEventArgs()));
                    })
                .Build()
            ;
        }

        private static Func<bool> CreateCanContinueFunc(CancellationToken cancellationToken)
        {
            return () => !cancellationToken.IsCancellationRequested;
        }

        /// <summary>
        /// Creates an asynchronous Task object that executes the given plugin on a background thread and
        /// invokes all other callbacks (success, failure, etc.) on the UI thread.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="plugin"></param>
        /// <param name="pluginRunner"></param>
        /// <returns></returns>
        private Task<bool> RunPluginSync(CancellationToken cancellationToken, IPlugin plugin, ExecutePluginHandler pluginRunner)
        {
            var task = new TaskBuilder()
                .OnThread(_callbackScheduler)
                .CancelWith(cancellationToken)
                .BeforeStart(delegate
                    {
                        var progressProvider = _pluginRepository.GetProgressProvider(plugin);

                        progressProvider.Updated -= ProgressProviderOnUpdated;
                        progressProvider.Updated += ProgressProviderOnUpdated;

                        progressProvider.Reset();
                        progressProvider.Start();
                    })
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        pluginRunner(token);
                    })
                .Fail(delegate(ExceptionEventArgs args)
                    {
                        var progressProvider = _pluginRepository.GetProgressProvider(plugin);
                        if (args.Exception is OperationCanceledException)
                        {
                            progressProvider.Cancel();
                        }
                        else
                        {
                            progressProvider.Error(args.Exception);
                            HandleUnhandledException(args.Exception);
                        }
                    })
                .Succeed(delegate
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
                .Build()
            ;
            task.RunSynchronously();
            return task;
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
            RunPluginSync(cancellationToken, plugin, token => plugin.PostProcess(token, Job)).RunSynchronously();
        }

        #endregion

        #endregion

        #region Plugin progress reporting

        private void ProgressProviderOnUpdated(ProgressProvider progressProvider)
        {
            if (PluginProgressUpdated != null)
            {
                // Marshal event back to UI thread
                Task.Factory.StartNew(delegate
                    {
                        var guid = progressProvider.Plugin.AssemblyInfo.Guid;
                        var hashCode = progressProvider.GetHashCode();

                        var containsKey = _progressMap.ContainsKey(guid);
                        var prevHashCode = containsKey ? _progressMap[guid] : -1;

                        Logger.DebugFormat(
                            "ProgressProviderOnUpdated() - Plugin \"{0}\": prev progress hashCode = {1}, cur progress hashCode = {2}",
                            progressProvider.Plugin.Name, prevHashCode, hashCode
                        );

                        // Progress hasn't changed since last update
                        if (containsKey && prevHashCode == hashCode)
                            return;

                        _progressMap[guid] = hashCode;

                        Logger.Debug("ProgressProviderOnUpdated() - Calling PluginProgressUpdated event handlers");

                        PluginProgressUpdated(progressProvider.Plugin, progressProvider);
                    }, CancellationToken.None, TaskCreationOptions.None, _callbackScheduler);
            }
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

        private void ScanStart()
        {
            if (ScanStarted != null)
                ScanStarted();
        }

        private void ScanFail(ExceptionEventArgs args)
        {
            if (ScanFailed != null)
                ScanFailed(args);

            ScanComplete();
        }

        private void ScanSucceed()
        {
            if (ScanSucceeded != null)
                ScanSucceeded();

            ScanComplete();
        }

        private void ScanComplete()
        {
            if (ScanCompleted != null)
                ScanCompleted();
        }

        private void ConvertStart()
        {
            if (ConvertStarted != null)
                ConvertStarted();
        }

        private void ConvertFail(ExceptionEventArgs args)
        {
            if (ConvertFailed != null)
                ConvertFailed(args);

            ConvertComplete();
        }

        private void ConvertSucceed()
        {
            if (ConvertSucceeded != null)
                ConvertSucceeded();

            ConvertComplete();
        }

        private void ConvertComplete()
        {
            if (ConvertCompleted != null)
                ConvertCompleted();
        }

        #endregion
    }

    internal delegate void ExecutePluginHandler(CancellationToken token);

    internal delegate bool CriticalPhase();

    internal delegate void OptionalPhase();
}
