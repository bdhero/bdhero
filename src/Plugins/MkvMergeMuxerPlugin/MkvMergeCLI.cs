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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using MkvToolNixUtils;
using ProcessUtils;

namespace BDHero.Plugin.MkvMergeMuxer
{
    class MkvMergeCLI
    {
        private const string MkvMergeExeFilename = "mkvmerge.exe";

        public string ExePath { get; private set; }

        /// <summary>
        ///     Gets the list of arguments passed to the executable.
        /// </summary>
        public ArgumentList Arguments { get; private set; }

        private readonly ITempFileRegistrar _tempFileRegistrar;
        
        private MkvMergeTrackIndexer _trackIndexer;

        public MkvMergeCLI(ArgumentList arguments, ITempFileRegistrar tempFileRegistrar)
        {
            ExePath = GetExePath();
            Arguments = arguments;
            _tempFileRegistrar = tempFileRegistrar;
        }

        private static string GetExePath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var pluginAssemblyDir = Path.GetDirectoryName(assemblyPath);
            return Path.Combine(pluginAssemblyDir, MkvMergeExeFilename);
        }

        #region Output file

        public MkvMergeCLI SetOutputPath(string outputMKVPath)
        {
            Arguments.AddAll("--output", outputMKVPath);
            return this;
        }

        #endregion

        #region Per-input options

        #region Tracks

        public MkvMergeCLI SetSelectedTracks(Playlist playlist)
        {
            _trackIndexer = new MkvMergeTrackIndexer(playlist);

            playlist.Tracks.Where(KeepTrack).ForEach(VisitTrack);

            Arguments.AddAll("--audio-tracks",    string.Join(",", playlist.AudioTracks.Where(KeepTrack).Select(TrackOutputIndex)));
            Arguments.AddAll("--video-tracks",    string.Join(",", playlist.VideoTracks.Where(KeepTrack).Select(TrackOutputIndex)));
            Arguments.AddAll("--subtitle-tracks", string.Join(",", playlist.SubtitleTracks.Where(KeepTrack).Select(TrackOutputIndex)));

            return this;
        }

        private bool KeepTrack(Track track)
        {
            if (!track.Keep)
                return false;

            var index = _trackIndexer[track];
            if (!index.IsSupported)
                return false;

            return true;
        }

        private void VisitTrack(Track track)
        {
            var index = _trackIndexer[track];
            var outputIndex = index.OutputIndex;

            Arguments.AddAll("--language", string.Format("{0:0}:{1}", outputIndex, track.Language.ISO_639_2));
            Arguments.AddAll("--track-name", string.Format("{0:0}:{1}", outputIndex, track.Title));
            Arguments.AddAll("--forced-track", string.Format("{0:0}:{1}", outputIndex, "no"));
        }

        private string TrackOutputIndex(Track track)
        {
            var index = _trackIndexer[track];
            var outputIndex = index.OutputIndex;
            return outputIndex.ToString("0");
        }

        #endregion

        #region Tags

        public MkvMergeCLI NoGlobalTags()
        {
            Arguments.AddAll("--no-global-tags");
            return this;
        }

        public MkvMergeCLI NoTrackTags()
        {
            Arguments.AddAll("--no-track-tags");
            return this;
        }

        #endregion

        #region Input files

        public MkvMergeCLI SetInputPath(string mplsFilePath)
        {
            Arguments.AddAll("(", mplsFilePath, ")");
            return this;
        }

        #endregion

        #endregion

        #region Global options

        #region Attachments

        public MkvMergeCLI AttachCoverArt(ReleaseMedium releaseMedium)
        {
            new CoverArtResizer(_tempFileRegistrar).AttachCoverArt(Arguments, releaseMedium);
            return this;
        }

        #endregion

        #region Title

        public MkvMergeCLI SetMovieTitle(Job job)
        {
            Arguments.AddAll("--title", job.Title);
            return this;
        }

        #endregion

        #region Chapters

        public MkvMergeCLI SetChapters(IList<Chapter> chapters)
        {
            new ChapterWriterV3(_tempFileRegistrar).SetChapters(Arguments, chapters);
            return this;
        }

        #endregion

        #endregion
    }
}
