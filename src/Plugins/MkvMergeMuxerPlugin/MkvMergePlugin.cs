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
using System.Drawing;
using System.Threading;
using BDHero.JobQueue;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using OSUtils.JobObjects;
using ProcessUtils;

namespace BDHero.Plugin.MkvMergeMuxer
{
    public class MkvMergePlugin : IMuxerPlugin
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "mkvmerge (mkvtoolnix)"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.mkvmerge_icon; } }

        public int RunOrder { get { return 1; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public MatroskaFeatures SupportedFeatures
        {
            get
            {
                return MatroskaFeatures.Chapters
                     | MatroskaFeatures.CoverArt
                     | MatroskaFeatures.DefaultFlag
                     | MatroskaFeatures.ForcedFlag
                    ;
            }
        }

        private readonly IJobObjectManager _jobObjectManager;
        private readonly ITempFileRegistrar _tempFileRegistrar;

        private readonly AutoResetEvent _mutex = new AutoResetEvent(false);

        private Exception _exception;

        [UsedImplicitly]
        public MkvMergePlugin(IJobObjectManager jobObjectManager, ITempFileRegistrar tempFileRegistrar)
        {
            _jobObjectManager = jobObjectManager;
            _tempFileRegistrar = tempFileRegistrar;
        }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(CancellationToken cancellationToken, Job job)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            const string startStatus = "Starting mkvmerge process...";

            Host.ReportProgress(this, 0.0, startStatus);
            Logger.Info(startStatus);

            _exception = null;

            var mkvmerge = new MkvMerge(job, job.SelectedPlaylist, job.OutputPath, _jobObjectManager, _tempFileRegistrar);
            mkvmerge.ProgressUpdated += state => OnProgressUpdated(mkvmerge, state, cancellationToken);
            mkvmerge.Exited += OnExited;
            mkvmerge.StartAsync();
            cancellationToken.Register(mkvmerge.Kill, true);
            WaitForThreadToExit();

            if (_exception == null)
                return;

//            Log(job, mkvmerge);

            throw new MkvMergeException(_exception.Message, _exception);
        }

        private void OnProgressUpdated(MkvMerge mkvmerge, ProgressState progressState, CancellationToken cancellationToken)
        {
//            var shortStatus = string.Format("Muxing {0} @ {2:F0} fps - {1}",
//                TimeSpan.FromMilliseconds(mkvmerge.CurOutTimeMs).ToStringShort(),
//                FileUtils.HumanFriendlyFileSize(mkvmerge.CurSize),
//                mkvmerge.CurFps);
//
//            var longStatus = string.Format("Muxing to MKV: {0} - {1} @ {2:N1} fps",
//                TimeSpan.FromMilliseconds(mkvmerge.CurOutTimeMs).ToStringMedium(),
//                FileUtils.HumanFriendlyFileSize(mkvmerge.CurSize),
//                mkvmerge.CurFps);

            Host.ReportProgress(this, progressState.PercentComplete, "short status", "long status");

            if (cancellationToken.IsCancellationRequested)
                mkvmerge.Kill();
        }

        private void OnExited(NonInteractiveProcessState state, int exitCode, Exception exception, TimeSpan runTime)
        {
            Logger.InfoFormat("mkvmerge exited with state {0} and code {1}", state, exitCode);

            _exception = _exception ?? exception;

            if (_exception == null && state != NonInteractiveProcessState.Completed)
            {
                try
                {
                    if (state == NonInteractiveProcessState.Killed)
                    {
                        throw new MkvMergeException("mkvmerge was canceled", new OperationCanceledException())
                              {
                                  IsReportable = false
                              };
                    }
                    throw new MkvMergeException(string.Format("mkvmerge exited with state: {0}", state))
                          {
                              IsReportable = true
                          };
                }
                catch (MkvMergeException e)
                {
                    _exception = e;
                }
            }

            SignalThreadExited();
        }

        private void WaitForThreadToExit()
        {
            _mutex.WaitOne();
        }

        private void SignalThreadExited()
        {
            _mutex.Set();
        }
    }
}
