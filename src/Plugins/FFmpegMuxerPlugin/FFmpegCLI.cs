using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.Extensions;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    class FFmpegCLI
    {
        private const string FFmpegExeFilename = "ffmpeg.exe";

        public string ExePath { get; private set; }

        private readonly ArgumentList _arguments;

        private List<string> _inputM2TSPaths;
        private string _inputFileListPath;
        private List<Track> _selectedTracks;
        private FFmpegTrackIndexer _indexer;

        public FFmpegCLI(ArgumentList arguments)
        {
            _arguments = arguments;
            ExePath = GetExePath();
        }

        private static string GetExePath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var pluginAssemblyDir = Path.GetDirectoryName(assemblyPath);
            return Path.Combine(pluginAssemblyDir, FFmpegExeFilename);
        }

        #region Logging

        public FFmpegCLI DumpLogFile()
        {
            _arguments.AddAll("-report");
            return this;
        }

        /// <summary>
        ///     <para>
        ///         <code>-loglevel [repeat+]loglevel | -v [repeat+]loglevel</code>
        ///     </para>
        ///     <para>
        ///         Set the logging level used by the library. Adding <c>repeat+</c> indicates that repeated log output
        ///         should not be compressed to the first line and the "Last message repeated n times" line will be omitted.
        ///         <c>repeat</c> can also be used alone.  If <c>repeat</c> is used alone, and with no prior loglevel set,
        ///         the default loglevel will be used.  If multiple loglevel parameters are given, using <c>repeat</c>
        ///         will not change the loglevel.  <c>repeat</c> is only available in ffmpeg builds after 2013/04/01.
        ///     </para>
        ///     <para>
        ///         By default the program logs to stderr, if coloring is supported by the terminal, colors are used
        ///         to mark errors and warnings. Log coloring can be disabled setting the environment variable
        ///         <c>AV_LOG_FORCE_NOCOLOR</c> or <c>NO_COLOR</c>, or can be forced setting the environment variable
        ///         <c>AV_LOG_FORCE_COLOR</c>. The use of the environment variable <c>NO_COLOR</c> is deprecated
        ///         and will be dropped in a following FFmpeg version.
        ///     </para>
        /// </summary>
        /// <param name="logLevel">
        ///     The level of logging verbosity to output to stderr.
        /// </param>
        /// <param name="compressRepeatedLogMessages">
        ///     If <c>false</c>, repeated log output will not be compressed to the first line and the
        ///     "Last message repeated n times" line will be omitted.
        /// </param>
        /// <seealso cref="http://ffmpeg.org/ffmpeg.html#Generic-options"/>
        public FFmpegCLI SetLogLevel(FFmpegLogLevel logLevel, bool compressRepeatedLogMessages = true)
        {
            var level = logLevel.GetAttributeProperty<ArgumentAttribute, string>(attribute => attribute.Name);
            var value = compressRepeatedLogMessages ? level : string.Format("repeat+{0}", level);
            _arguments.AddAll("-loglevel", value);
            return this;
        }

        #endregion

        public FFmpegCLI RedirectProgressToFile(string progressFilePath)
        {
            _arguments.AddAll("-progress", progressFilePath);
            return this;
        }

        /// <summary>
        ///     Generate Presentation Time Stamps to avoid errors like the following:
        ///     <code>
        ///         [matroska @ 051051a0] Can't write packet with unknown timestamp
        ///         av_interleaved_write_frame(): Invalid argument
        ///     </code>
        /// </summary>
        /// <seealso cref="https://github.com/bdhero/bdhero/issues/30" />
        /// <seealso cref="http://stackoverflow.com/a/6044365/467582" />
        /// <seealso cref="https://www.ffmpeg.org/ffmpeg-formats.html#Format-Options" />
        public FFmpegCLI GenPTS()
        {
            _arguments.AddAll("-fflags", "+genpts");
            return this;
        }

        #region Input files

        public FFmpegCLI ReplaceExistingFiles()
        {
            _arguments.AddAll("-y");
            return this;
        }

        public FFmpegCLI SetInputPaths(List<string> inputM2TSPaths, string inputFileListPath)
        {
            _inputM2TSPaths = inputM2TSPaths;
            _inputFileListPath = inputFileListPath;

            SetInputFilesV2();

            return this;
        }

        #region V2 - Text file

        /// <summary>
        ///     New input file list implementation.  Writes the list of input M2TS files to a temporary text file
        ///     and passes the path to the text file in to the FFmpeg CLI.  Avoids the 8191 character limit
        ///     imposed by cmd.exe in Windows XP and newer.  Works in FFmpeg 1.1+.
        /// </summary>
        /// <seealso cref="https://its.ffmpeg.org/wiki/How%20to%20concatenate%20(join,%20merge)%20media%20files#samecodec"/>
        /// <seealso cref="http://support.microsoft.com/kb/830473"/>
        private void SetInputFilesV2()
        {
            var fileList = _inputM2TSPaths.Select(EscapeInputPath)
                                          .Select(FormatInputPath);
            File.WriteAllLines(_inputFileListPath, fileList);
            _arguments.AddAll("-f", "concat", "-i", _inputFileListPath);
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

        #endregion

        #region V1 - Command line

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
            _arguments.AddAll("-i", inputFiles);
        }

        private static string GetInputFiles(IList<string> inputM2TsPaths)
        {
            return inputM2TsPaths.Count == 1 ? inputM2TsPaths[0] : "concat:" + string.Join("|", inputM2TsPaths);
        }

        #endregion

        #endregion

        #region Metadata

        public FFmpegCLI SetMovieTitle(Job job)
        {
            _arguments.AddAll("-metadata", "title=" + job.Title);
            return this;
        }

        #endregion

        #region Tracks & Codecs

        public FFmpegCLI SetSelectedTracks(List<Track> selectedTracks, FFmpegTrackIndexer indexer)
        {
            _selectedTracks = selectedTracks;
            _indexer = indexer;

            MapSelectedTracks();

            return this;
        }

        private void MapSelectedTracks()
        {
            _arguments.AddRange(_selectedTracks.SelectMany(TrackMetadataArgs));
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

        public FFmpegCLI CopyAllCodecs()
        {
            _arguments.AddAll("-c", "copy");
            return this;
        }

        /// <summary>
        ///     Converts Blu-ray LPCM tracks to signed, little endian PCM for MKV.
        ///     Blu-ray LPCM is signed, big endian, and either 16-, 20-, or 24-bit.
        ///     FFmpeg only outputs 16- or 24- bit PCM, so 20-bit Blu-ray LPCM
        ///     needs to be converted to 24-bit PCM to avoid losing quality.
        /// </summary>
        public FFmpegCLI ConvertLPCM()
        {
            _arguments.AddRange(_selectedTracks.Where(IsLPCM).SelectMany(LPCMCodecArgs));
            return this;
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

        #endregion

        #region Output file

        public FFmpegCLI SetOutputPath(string outputMKVPath)
        {
            _arguments.Add(outputMKVPath);
            return this;
        }

        #endregion
    }
}
