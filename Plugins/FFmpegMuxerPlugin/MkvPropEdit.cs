using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using BDHero.BDROM;
using DotNetUtils;
using DotNetUtils.Annotations;
using I18N;
using OSUtils.JobObjects;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class MkvPropEdit : BackgroundProcessWorker
    {
        private const string MkvPropEditFileName = "mkvpropedit.exe";

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

        [UsedImplicitly]
        public MkvPropEdit(IJobObjectManager jobObjectManager)
            : base(jobObjectManager)
        {
            SetExePath();
        }

        private void SetExePath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var mkvPropEditAssemblyDir = Path.GetDirectoryName(assemblyPath);
            ExePath = Path.Combine(mkvPropEditAssemblyDir, MkvPropEditFileName);
        }

        public MkvPropEdit SetChapters(IEnumerable<Chapter> chapters)
        {
            var chapterXmlPath = SaveChaptersToXml(chapters);
            Arguments.AddAll("--chapters", chapterXmlPath);
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

        public MkvPropEdit AddCoverArt([CanBeNull] Image coverArt)
        {
            var coverImagePathLarge = ResizeCoverArt(coverArt, CoverArtSize.Large, "cover.jpg");
            var coverImagePathSmall = ResizeCoverArt(coverArt, CoverArtSize.Small, "small_cover.jpg");

            if (coverImagePathLarge != null)
                AddAttachment(coverImagePathLarge);

            if (coverImagePathSmall != null)
                AddAttachment(coverImagePathSmall);

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

        /// <summary>
        /// Saves the given <paramref name="chapters"/> in Matroska XML format to a temporary file and returns the path to the file.
        /// </summary>
        /// <param name="chapters"></param>
        /// <returns>Full, absolute path to the chapter XML file</returns>
        [NotNull]
        private static string SaveChaptersToXml(IEnumerable<Chapter> chapters)
        {
            var path = AssemblyUtils.GetTempFilePath(typeof(MkvPropEdit), "chapters.xml");
            ChapterWriterV3.SaveAsXml(chapters, path);
            return path;
        }

        /// <summary>
        /// Resizes the cover art image to the appropriate dimensions and saves it to a temporary file with the given filename.
        /// </summary>
        /// <param name="image">Full size cover art image from TMDb.  If <paramref name="image"/> is null, this method will return null.</param>
        /// <param name="width">120 for small or 600 for large</param>
        /// <param name="filename">cover.{jpg,png} or small_cover.{jpg,png}</param>
        /// <returns>Full, absolute path to the resized image on disk if <paramref name="image"/> is not null; otherwise null.</returns>
        [NotNull]
        private string ResizeCoverArt([CanBeNull] Image image, CoverArtSize width, [NotNull] string filename)
        {
            if (image == null) return null;
            var ext = Path.GetExtension(filename.ToLowerInvariant());
            var format = ext == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
            var path = AssemblyUtils.GetTempFilePath(GetType(), filename);
            ScaleImage(image, (int)width, int.MaxValue).Save(path, format);
            return path;
        }

        [NotNull]
        private static Image ScaleImage([NotNull] Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

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
//                .AddCoverArt(coverArt)
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
    }

    public enum CoverArtSize
    {
        Small = 120,
        Large = 600
    }

    public class MkvTrackProps
    {
        public string Name;
        public bool? Default;
        public bool? Forced;
        public Language Language;
    }

    public static class ChapterWriterV3
    {
        public static void SaveAsXml(IEnumerable<Chapter> chapters, string path)
        {
            var writer = new XmlTextWriter(path, Encoding.GetEncoding("ISO-8859-1"));
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
                writer.WriteDocType("Chapters", null, "matroskachapters.dtd", null);
                writer.WriteStartElement("Chapters");
                    writer.WriteStartElement("EditionEntry");
                    foreach (var chapter in chapters.Where(chapter => chapter.Keep))
                    {
                        writer.WriteStartElement("ChapterAtom");
                            writer.WriteStartElement("ChapterTimeStart");
                                writer.WriteString(chapter.StartTimeXmlFormat); // 00:00:00.000
                            writer.WriteEndElement();
                            writer.WriteStartElement("ChapterDisplay");
                                writer.WriteStartElement("ChapterString");
                                    writer.WriteString(chapter.Title); // Chapter 1
                                writer.WriteEndElement();
                                if (chapter.Language != null && chapter.Language != Language.Undetermined)
                                {
                                    writer.WriteStartElement("ChapterLanguage");
                                        writer.WriteString(chapter.Language.ISO_639_2); // eng
                                    writer.WriteEndElement();
                                }   
                            writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
