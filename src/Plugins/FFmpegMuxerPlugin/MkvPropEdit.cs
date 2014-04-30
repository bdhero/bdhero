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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.FS;
using I18N;
using MkvToolNixUtils;
using OSUtils.JobObjects;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class MkvPropEdit : BackgroundProcessWorker
    {
        private const string MkvPropEditFileName = "mkvpropedit.exe";

        private readonly ITempFileRegistrar _tempFileRegistrar;

        /// <summary>
        /// Gets or sets the path to the source Matroska file that will be modified by MkvPropEdit.
        /// The setter automatically adds the source path to the list of <see cref="NonInteractiveProcess.Arguments"/>;
        /// you should not add it manually.
        /// </summary>
        public string SourceFilePath
        {
            get { return _sourceFilePath; }
            set
            {
                if (_sourceFilePath != null)
                    throw new InvalidOperationException("Source file path cannot be set more than once.");
                if (Arguments.Any())
                    throw new InvalidOperationException("Source file must be the first argument in the list; another agument is already present.");
                _sourceFilePath = value;
                Arguments.Add(_sourceFilePath);
            }
        }
        private string _sourceFilePath;

        public MkvPropEdit(IJobObjectManager jobObjectManager, ITempFileRegistrar tempFileRegistrar)
            : base(jobObjectManager)
        {
            _tempFileRegistrar = tempFileRegistrar;
            SetExePath();
        }

        private void SetExePath()
        {
            var exeDir = AssemblyUtils.GetInstallDir(GetType());
            ExePath = Path.Combine(exeDir, MkvPropEditFileName);
        }

        public MkvPropEdit SetChapters(ICollection<Chapter> chapters)
        {
            new ChapterWriterV3(_tempFileRegistrar).SetChapters(Arguments, chapters);
            return this;
        }

        public MkvPropEdit RemoveAllTags()
        {
            Arguments.AddAll("--tags", "all:");
            return this;
        }

        /// <summary>
        /// Automatically sets the "default track" flag to <c>true</c> for the first track of each type (video, audio, and subtitle),
        /// and the remaining tracks to <c>false</c>.
        /// </summary>
        public MkvPropEdit SetDefaultTracksAuto(List<Track> selectedTracks)
        {
            var numVideoTracks = selectedTracks.Count(track => track.IsVideo);
            var numAudioTracks = selectedTracks.Count(track => track.IsAudio);
            var numSubtitleTracks = selectedTracks.Count(track => track.IsSubtitle);

            for (var i = 1; i <= numVideoTracks; i++)
                SetDefaultTrackFlag("v", i, i == 1);
            for (var i = 1; i <= numAudioTracks; i++)
                SetDefaultTrackFlag("a", i, i == 1);
            for (var i = 1; i <= numSubtitleTracks; i++)
                SetDefaultTrackFlag("s", i, i == 1);

            return this;
        }

        /// <summary>
        /// Sets the "default track" flag of the specified track.
        /// WARNING: This method BREAKS playback in MPC-HC!
        /// </summary>
        /// <param name="trackType">"v", "a", or "s" for video, audio, and subtitle</param>
        /// <param name="indexOfType"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public MkvPropEdit SetDefaultTrackFlag(string trackType, int indexOfType, bool isDefault)
        {
            Arguments.AddAll("--edit", string.Format("track:{0}{1}", trackType, indexOfType),
                             "--set", string.Format("flag-default={0}", isDefault ? 1 : 0));
            return this;
        }

        public MkvPropEdit AddAttachment([NotNull] string attachmentFilePath)
        {
            Arguments.AddAll("--add-attachment", attachmentFilePath);
            return this;
        }

        public MkvPropEdit DeleteAttachment(int? attachmentNumber)
        {
            if (attachmentNumber != null)
            {
                var attachedNumber = attachmentNumber ?? default(int);
                Arguments.AddAll("--delete-attachment", attachedNumber.ToString());
            }
            else
                Arguments.AddAll("--delete-attachment", "mime-type:image/jpeg");
            return this;
        }

        public MkvPropEdit AttachCoverArt([CanBeNull] ReleaseMedium releaseMedium)
        {
            new CoverArtResizer(Arguments, _tempFileRegistrar).AddCoverArt(releaseMedium);
            return this;
        }

        public MkvPropEdit SetMovieTitle([CanBeNull] string movieTitle)
        {
            if (!string.IsNullOrWhiteSpace(movieTitle))
                Arguments.AddAll("--edit", "segment_info", "--set", "title=" + movieTitle);
            return this;
        }

        public MkvPropEdit SetTrackProps(List<MkvTrackProps> trackProps)
        {
            for (var i = 0; i < trackProps.Count; i++)
            {
                var track = trackProps[i];

                Arguments.AddAll("--edit", "track:" + (i + 1));
                Arguments.AddAll("--set", "name=" + track.Name);

                if (track.Default.HasValue)
                    Arguments.AddAll("--set", "flag-default=" + (track.Default == true ? 1 : 0));
                else
                    Arguments.AddAll("--delete", "flag-default");

                if (track.Forced.HasValue)
                    Arguments.AddAll("--set", "flag-forced=" + (track.Forced == true ? 1 : 0));
                else
                    Arguments.AddAll("--delete", "flag-forced");

                Arguments.AddAll("--set", "language=" + track.Language.ISO_639_2);
            }

            return this;
        }

#if TestMkvPropEdit

        public static void Test(string mkvFilePath = null)
        {
            mkvFilePath = mkvFilePath ?? @"Y:\BDAM\out\progress\BLACK_HAWK_DOWN_00000.mpls.propedit2.mkv";
            var movieTitle = "Black Hawk Down";
            var coverArt = Image.FromFile(@"Y:\BDAM\cover-art\black-hawk-down\full.jpg");
            var language = Language.FromCode("eng");
            var trackProps = new List<MkvTrackProps>
                                 {
                                     new MkvTrackProps { Name = "1080p MPEG-2", Language = language, Default = true, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: AC-3 (5.1 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: PCM (5.1 ch)", Language = language, Default = true, Forced = false },
                                     new MkvTrackProps { Name = "Commentary: AC-3 (2.0 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Commentary: AC-3 (2.0 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Commentary: AC-3 (2.0 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: HDMV PGS", Language = language, Default = true, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: HDMV PGS", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Unknown: HDMV PGS", Language = language, Default = false, Forced = false }
                                 };
            var chapters = new List<Chapter>
                               {
                                   new Chapter(1, TimeSpan.FromMinutes(5).TotalSeconds),
                                   new Chapter(2, TimeSpan.FromMinutes(10).TotalSeconds),
                                   new Chapter(3, TimeSpan.FromMinutes(15).TotalSeconds),
                                   new Chapter(4, TimeSpan.FromMinutes(20).TotalSeconds),
                                   new Chapter(5, TimeSpan.FromMinutes(25).TotalSeconds)
                               };
//            var mkvPropEdit = new MkvPropEdit { SourceFilePath = mkvFilePath }
//                .SetMovieTitle(movieTitle)
//                .AttachCoverArt(coverArt)
//                .SetChapters(chapters)
//                .SetTrackProps(trackProps);
//            mkvPropEdit.Start();
        }

        public static void DeleteAttachements(string mkvFilePath, int? attachmentNumber)
        {
//            var mkvPropEdit = new MkvPropEdit { SourceFilePath = mkvFilePath }
//                .DeleteAttachment(attachmentNumber);
//            mkvPropEdit.Start();
        }

#endif
    }

    public class MkvTrackProps
    {
        public string Name;
        public bool? Default;
        public bool? Forced;
        public Language Language;
    }
}
