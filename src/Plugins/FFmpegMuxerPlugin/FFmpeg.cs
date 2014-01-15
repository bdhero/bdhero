// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.FS;
using OSUtils.JobObjects;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpeg : BackgroundProcessWorker
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string FFmpegExeFilename = "ffmpeg.exe";

        private static readonly Regex FrameRegex = new Regex(@"^frame=(\d+)$");
        private static readonly Regex FpsRegex = new Regex(@"^fps=([\d.]+)$");
        private static readonly Regex TotalSizeRegex = new Regex(@"^total_size=(\d+)$");
        private static readonly Regex OutTimeMsRegex = new Regex(@"^out_time_ms=(\d+)$");
        private static readonly Regex ProgressRegex = new Regex(@"^progress=(\w+)$");

        private readonly TimeSpan _playlistLength;
        private readonly List<string> _inputM2TSPaths;
        private readonly List<Track> _selectedTracks;
        private readonly string _outputMKVPath;
        private readonly IJobObjectManager _jobObjectManager;
        private readonly ITempFileRegistrar _tempFileRegistrar;
        private readonly string _progressFilePath;
        private readonly string _inputFileListPath;
        private readonly FFmpegTrackIndexer _indexer;

        public long CurFrame { get; private set; }
        public double CurFps { get; private set; }
        public long CurSize { get; private set; }
        public long CurOutTimeMs { get; private set; }

        private readonly IList<string> _errors = new List<string>();

        private readonly BackgroundWorker _progressWorker = new BackgroundWorker();

        public FFmpeg(Job job, Playlist playlist, string outputMKVPath, IJobObjectManager jobObjectManager, ITempFileRegistrar tempFileRegistrar)
            : base(jobObjectManager)
        {
            _playlistLength = playlist.Length;
            _inputM2TSPaths = playlist.StreamClips.Select(clip => clip.FileInfo.FullName).ToList();
            _selectedTracks = playlist.Tracks.Where(track => track.Keep).ToList();
            _outputMKVPath = outputMKVPath;
            _jobObjectManager = jobObjectManager;
            _tempFileRegistrar = tempFileRegistrar;

            _progressFilePath = _tempFileRegistrar.CreateTempFile(GetType(), "progress.log");
            _inputFileListPath = _tempFileRegistrar.CreateTempFile(GetType(), "inputFileList.txt");
            _indexer = new FFmpegTrackIndexer(playlist);

            VerifyInputPaths();
            VerifySelectedTracks();

            SetExePath();

            SetFFmpegLogLevel();
            RedirectProgressToFile();
            ReplaceExistingFiles();
            SetInputFiles();
            SetMovieTitle(job);
            MapSelectedTracks();
            CopyAllCodecs();
            ConvertLPCM();
            SetOutputMKVPath();

            BeforeStart += OnBeforeStart;
            StdErr += OnStdErr;
            Exited += (state, code, exception, time) => OnExited(state, code, job.SelectedReleaseMedium, playlist, _selectedTracks, outputMKVPath);

            foreach (var track in playlist.Tracks)
            {
                var index = _indexer[track];
                Logger.InfoFormat("Track w/ stream PID {0} (0x{0:x4}): index {1} => {2} ({3})",
                    track.PID, index.InputIndex, index.OutputIndex, track.Codec);
            }
        }

        private void OnStdErr(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;

            _errors.Add(line);

            Logger.Error(line);

            try
            {
                // Preserve stack trace by throwing and catching exception
                throw new Exception(string.Join(Environment.NewLine, _errors));
            }
            catch (Exception e)
            {
                Exception = e;
            }
        }

        private void VerifyInputPaths()
        {
            if (_inputM2TSPaths.Count == 0)
                throw new ArgumentOutOfRangeException("At least one input M2TS file is required.");
        }

        private void VerifySelectedTracks()
        {
            if (_selectedTracks.Count == 0)
                throw new ArgumentOutOfRangeException("At least one track must be selected.");
        }

        private static string GetInputFiles(IList<string> inputM2TsPaths)
        {
            return inputM2TsPaths.Count == 1 ? inputM2TsPaths[0] : "concat:" + string.Join("|", inputM2TsPaths);
        }

        /// <summary>
        /// `-loglevel [repeat+]loglevel | -v [repeat+]loglevel`
        /// 
        /// Set the logging level used by the library. Adding "repeat+" indicates that repeated log output should not be compressed
        /// to the first line and the "Last message repeated n times" line will be omitted. "repeat" can also be used alone.
        /// If "repeat" is used alone, and with no prior loglevel set, the default loglevel will be used. If multiple loglevel parameters
        /// are given, using `repeat` will not change the loglevel. `repeat` is only available in ffmpeg builds after 2013/04/01.
        /// 
        /// loglevel is a number or a string containing one of the following values:
        /// 
        ///     `quiet`
        ///         Show nothing at all; be silent.
        /// 
        ///     `panic`
        ///         Only show fatal errors which could lead the process to crash, such as and assert failure. This is not currently used for anything.
        /// 
        ///     `fatal`
        ///         Only show fatal errors. These are errors after which the process absolutely cannot continue after.
        /// 
        ///     `error`
        ///         Show all errors, including ones which can be recovered from.
        /// 
        ///     `warning`
        ///         Show all warnings and errors. Any message related to possibly incorrect or unexpected events will be shown.
        /// 
        ///     `info`
        ///         Show informative messages during processing. This is in addition to warnings and errors. This is the default value.
        /// 
        ///     `verbose`
        ///         Same as info, except more verbose.
        /// 
        ///     `debug`
        ///         Show everything, including debugging information.
        /// 
        /// By default the program logs to stderr, if coloring is supported by the terminal, colors are used to mark errors and warnings. Log coloring can be disabled setting the environment variable AV_LOG_FORCE_NOCOLOR or NO_COLOR, or can be forced setting the environment variable AV_LOG_FORCE_COLOR. The use of the environment variable NO_COLOR is deprecated and will be dropped in a following FFmpeg version.
        /// </summary>
        /// <seealso cref="http://ffmpeg.org/ffmpeg.html#Generic-options"/>
        private void SetFFmpegLogLevel(bool compressRepeatedLogMessages = true)
        {
            const string level = "fatal";
            var value = compressRepeatedLogMessages ? level : string.Format("repeat+{0}", level);
            Arguments.AddAll("-loglevel", value);
        }

        private void ReplaceExistingFiles()
        {
            Arguments.AddAll("-y");
        }

        private void RedirectProgressToFile()
        {
            Arguments.AddAll("-progress", _progressFilePath);
        }

        /// <summary>
        ///     New input file list implementation.  Writes the list of input M2TS files to a temporary text file
        ///     and passes the path to the text file in to the FFmpeg CLI.  Avoids the 8191 character limit
        ///     imposed by cmd.exe in Windows XP and newer.  Works in FFmpeg 1.1+.
        /// </summary>
        /// <seealso cref="https://its.ffmpeg.org/wiki/How%20to%20concatenate%20(join,%20merge)%20media%20files#samecodec"/>
        /// <seealso cref="http://support.microsoft.com/kb/830473"/>
        private void SetInputFiles()
        {
            var fileList = _inputM2TSPaths.Select(EscapeInputPath)
                                          .Select(FormatInputPath);
            File.WriteAllLines(_inputFileListPath, fileList);
            Arguments.AddAll("-f", "concat", "-i", _inputFileListPath);
        }

        private static string EscapeInputPath(string m2tsPath)
        {
            // Escape single quotes
            return m2tsPath.Replace(@"'", @"\'");
        }

        private static string FormatInputPath(string m2tsPath)
        {
            return string.Format("file '{0}'", m2tsPath);
        }

        /// <summary>
        ///     Original input file list implementation.  Passes the concatenated list of input files directly to the
        ///     FFmpeg CLI.  Simple and easy to use, but complex Blu-ray playlists with many .m2ts files may cause an
        ///     error if the command length exceeds 8191 characters in Windows XP or newer.  Works in FFmpeg 1.0+.
        /// </summary>
        /// <seealso cref="https://its.ffmpeg.org/wiki/How%20to%20concatenate%20(join,%20merge)%20media%20files#samecodec"/>
        /// <seealso cref="http://support.microsoft.com/kb/830473"/>
        private void SetInputFilesV1()
        {
            var inputFiles = GetInputFiles(_inputM2TSPaths);
            Arguments.AddAll("-i", inputFiles);
        }

        private void SetMovieTitle(Job job)
        {
            var title = job.SearchQuery.Title;
            var releaseMedium = job.SelectedReleaseMedium;
            if (releaseMedium != null)
            {
                var movie = releaseMedium as Movie;
                var tvShow = releaseMedium as TVShow;
                if (movie != null)
                {
                    title = movie.ToString();
                }
                else if (tvShow != null)
                {
                    title = string.Format("{0} - {1}, season {2}, episode {3} ({4})",
                                          tvShow.SelectedEpisode.Title,
                                          tvShow.Title,
                                          tvShow.SelectedEpisode.SeasonNumber,
                                          tvShow.SelectedEpisode.EpisodeNumber,
                                          tvShow.SelectedEpisode.ReleaseDate.ToString("yyyy'-'MM'-'dd"));
                }
                else
                {
                    title = releaseMedium.Title;
                }
            }
            Arguments.AddAll("-metadata", "title=" + title);
        }

        private void MapSelectedTracks()
        {
            Arguments.AddRange(_selectedTracks.SelectMany(TrackMetadataArgs));
        }

        private IEnumerable<string> TrackMetadataArgs(Track track)
        {
            var index = _indexer[track];
            return new[]
                       {
                           "-map", "0:" + index.InputIndex,
                           "-metadata:s:" + index.OutputIndex, "language=" + track.Language.ISO_639_2,
                           "-metadata:s:" + index.OutputIndex, "title=" + track.Title
                       };
        }

        private void CopyAllCodecs()
        {
            Arguments.AddAll("-c", "copy");
        }

        /// <summary>
        /// Converts Blu-ray LPCM tracks to signed, little endian PCM for MKV.
        /// Blu-ray LPCM is signed, big endian, and either 16-, 20-, or 24-bit.
        /// FFmpeg only outputs 16- or 24- bit PCM, so 20-bit Blu-ray LPCM
        /// needs to be converted to 24-bit PCM to avoid losing quality.
        /// </summary>
        private void ConvertLPCM()
        {
            Arguments.AddRange(_selectedTracks.Where(IsLPCM).SelectMany(LPCMCodecArgs));
        }

        private static bool IsLPCM(Track track)
        {
            return track.Codec == Codec.LPCM;
        }

        private IEnumerable<string> LPCMCodecArgs(Track track)
        {
            var index = _indexer[track];
            return new[] { "-c:a:" + index.OutputIndexOfType, "pcm_s" + (track.BitDepth == 16 ? 16 : 24) + "le" };
        }

        private void SetOutputMKVPath()
        {
            Arguments.Add(_outputMKVPath);
        }

        private void OnBeforeStart(object sender, EventArgs eventArgs)
        {
            _progressWorker.DoWork += ProgressWorkerOnDoWork;
            _progressWorker.RunWorkerCompleted += ProgressWorkerOnRunWorkerCompleted;
            _progressWorker.RunWorkerAsync();
        }

        private void ProgressWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            using (var stream = CreateProgressFileStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    while (KeepParsingProgress)
                    {
                        ParseProgressLine(reader.ReadLine());
                    }
                }
            }
        }

        private void ProgressWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (runWorkerCompletedEventArgs.Error != null)
            {
                Exception = runWorkerCompletedEventArgs.Error;
                Kill();
            }
        }

        private bool KeepParsingProgress
        {
            get
            {
                return (_progress < 100) &&
                       (State == NonInteractiveProcessState.Ready ||
                        State == NonInteractiveProcessState.Running ||
                        State == NonInteractiveProcessState.Paused);
            }
        }

        private void ParseProgressLine(string line)
        {
            if (line == null)
            {
                Thread.Sleep(500);
                return;
            }

            if (FrameRegex.IsMatch(line))
                CurFrame = long.Parse(FrameRegex.Match(line).Groups[1].Value);
            else if (FpsRegex.IsMatch(line))
                CurFps = double.Parse(FpsRegex.Match(line).Groups[1].Value);
            else if (TotalSizeRegex.IsMatch(line))
                CurSize = long.Parse(TotalSizeRegex.Match(line).Groups[1].Value);
            else if (OutTimeMsRegex.IsMatch(line))
                CurOutTimeMs = long.Parse(OutTimeMsRegex.Match(line).Groups[1].Value) / 1000;

            var prevProgress = _progress;

            _progress = 100 * (CurOutTimeMs / _playlistLength.TotalMilliseconds);
            _progress = Math.Min(_progress, 100);

            if ("progress=end" == line)
                _progress = 100;
        }

        private FileStream CreateProgressFileStream()
        {
            return new FileStream(_progressFilePath,
                                  FileMode.Open,
                                  FileAccess.Read,
                                  FileShare.ReadWrite | FileShare.Delete);
        }

        private void SetExePath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var ffmpegAssemblyDir = Path.GetDirectoryName(assemblyPath);
            ExePath = Path.Combine(ffmpegAssemblyDir, FFmpegExeFilename);
        }

        private void OnExited(NonInteractiveProcessState processState, int exitCode, ReleaseMedium releaseMedium, Playlist playlist, List<Track> selectedTracks, string outputMKVPath)
        {
            LogExit(processState, exitCode);

            _tempFileRegistrar.DeleteTempFiles(_progressFilePath, _inputFileListPath);

            if (processState != NonInteractiveProcessState.Completed)
                return;

            var coverArt = releaseMedium != null ? releaseMedium.CoverArtImages.FirstOrDefault(image => image.IsSelected) : null;
            var coverArtImage = coverArt != null ? coverArt.Image : null;
            var mkvPropEdit = new MkvPropEdit(_jobObjectManager, _tempFileRegistrar) { SourceFilePath = outputMKVPath }
                .RemoveAllTags()
                .AddCoverArt(coverArtImage)
                .SetChapters(playlist.Chapters)
//                .SetDefaultTracksAuto(selectedTracks) // Breaks MediaInfo
            ;
            mkvPropEdit.Start();
        }

        private static void LogExit(NonInteractiveProcessState processState, int exitCode)
        {
            const string message = "FFmpeg exited with state {0} and code {1}";
            switch (processState)
            {
                case NonInteractiveProcessState.Completed:
                    Logger.DebugFormat(message, processState, exitCode);
                    break;
                case NonInteractiveProcessState.Killed:
                    Logger.WarnFormat(message, processState, exitCode);
                    break;
                default:
                    Logger.ErrorFormat(message, processState, exitCode);
                    break;
            }
        }
    }
}
