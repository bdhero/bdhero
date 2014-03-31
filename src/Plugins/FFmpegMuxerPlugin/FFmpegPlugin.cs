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

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpegPlugin : IMuxerPlugin
    {
        private readonly IJobObjectManager _jobObjectManager;
        private readonly ITempFileRegistrar _tempFileRegistrar;

        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "FFmpeg"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.ffmpeg_icon; } }

        public int RunOrder { get { return 0; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public MatroskaFeatures SupportedFeatures
        {
            get
            {
                return MatroskaFeatures.Chapters
                     | MatroskaFeatures.CoverArt
                     | MatroskaFeatures.LPCM
                    ;
            }
        }

        private readonly AutoResetEvent _mutex = new AutoResetEvent(false);

        private Exception _exception;

        [UsedImplicitly]
        public FFmpegPlugin(IJobObjectManager jobObjectManager, ITempFileRegistrar tempFileRegistrar)
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

            const string startStatus = "Starting FFmpeg process...";

            Host.ReportProgress(this, 0.0, startStatus);
            Logger.Info(startStatus);

            _exception = null;

            var ffmpeg = new FFmpeg(job, job.SelectedPlaylist, job.OutputPath, _jobObjectManager, _tempFileRegistrar);
            ffmpeg.ProgressUpdated += state => OnProgressUpdated(ffmpeg, state, cancellationToken);
            ffmpeg.Exited += FFmpegOnExited;
            ffmpeg.StartAsync();
            cancellationToken.Register(ffmpeg.Kill, true);
            WaitForThreadToExit();

            if (_exception == null) return;

            throw _exception;
        }

        private void OnProgressUpdated(FFmpeg ffmpeg, ProgressState progressState, CancellationToken cancellationToken)
        {
            var status = string.Format("Muxing to MKV with FFmpeg: {0} - {1} @ {2} fps",
                TimeSpan.FromMilliseconds(ffmpeg.CurOutTimeMs).ToStringMedium(),
                FileUtils.HumanFriendlyFileSize(ffmpeg.CurSize),
                ffmpeg.CurFps.ToString("0.0"));

            Host.ReportProgress(this, progressState.PercentComplete, status);

            if (cancellationToken.IsCancellationRequested)
                ffmpeg.Kill();
        }

        private void FFmpegOnExited(NonInteractiveProcessState state, int exitCode, Exception exception, TimeSpan runTime)
        {
            Logger.InfoFormat("FFmpeg exited with state {0} and code {1}", state, exitCode);
            if (state != NonInteractiveProcessState.Completed)
            {
                if (state == NonInteractiveProcessState.Killed)
                {
                    _exception = exception ?? new OperationCanceledException("FFmpeg was canceled");
                }
                else
                {
                    _exception = exception ?? new FFmpegException(string.Format("FFmpeg exited with state: {0}", state));
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
