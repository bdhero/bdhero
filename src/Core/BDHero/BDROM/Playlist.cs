using System;
using System.Collections.Generic;
using System.Linq;
using I18N;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace BDHero.BDROM
{
    /// <summary>
    /// Represents a .MPLS file
    /// </summary>
    public class Playlist
    {
        #region Private constants

        /// <summary>
        /// Threshold for considering a playlist to be "feature length" when comparing its length to the length of the longest playlist.
        /// </summary>
        private const double FeatureLengthPercentage = 0.85;

        /// <summary>
        /// Minimum length (in seconds) for a playlist to be considered a "Main Feature."
        /// </summary>
        private const int MinLengthSec = 120;

        #endregion

        #region Private static properties

        private static TimeSpan MinLength 
        {
            get { return TimeSpan.FromSeconds(MinLengthSec); }
        }

        #endregion

        #region DB Fields (filename, file size, length)

        /// <summary>
        /// Name of the playlist file in all uppercase (e.g., "00200.MPLS").
        /// </summary>
        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Size of the playlist file in bytes.
        /// </summary>
        [JsonProperty(PropertyName = "filesize")]
        public ulong FileSize { get; set; }

        /// <summary>
        /// Duration of the playlist.
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public TimeSpan Length { get; set; }

        #endregion

        #region DB "Cut" (a.k.a. "release" or "edition")

        /// <summary>
        /// Cut (a.k.a. "release" or "edition") of the film.
        /// </summary>
        public PlaylistCut Cut;

        /// <summary>
        /// Theatrical edition.
        /// </summary>
        public bool IsTheatricalEdition { get { return Cut == PlaylistCut.Theatrical; } }

        /// <summary>
        /// Special edition.
        /// </summary>
        public bool IsSpecialEdition { get { return Cut == PlaylistCut.Special; } }

        /// <summary>
        /// Extended edition.
        /// </summary>
        public bool IsExtendedEdition { get { return Cut == PlaylistCut.Extended; } }

        /// <summary>
        /// Unrated edition.
        /// </summary>
        public bool IsUnratedEdition { get { return Cut == PlaylistCut.Unrated; } }

        #endregion

        #region DB Tracks and Chapters

        /// <summary>
        /// List of all tracks (TSStreams) in the order they appear in the playlist.
        /// Video, audio, and subtitles (includes hidden tracks and unsupported codecs).
        /// </summary>
        public IList<Track> Tracks = new List<Track>();

        public IList<Chapter> Chapters = new List<Chapter>();

        /// <summary>
        /// List of possible chapter matches that the user can choose from.
        /// </summary>
        public IList<ChapterSearchResult> ChapterSearchResults = new List<ChapterSearchResult>();

        #endregion

        #region Non-DB Flags (max quality, duplicate, loops, hidden first tracks, bogus)

        /// TODO: Rewrite summary
        /// <summary>
        /// Has a lower video resolution than the highest-resolution playlist or
        /// a lower number of audio channels than the playlist with the highest number of channels.
        /// E.G., this playlist is 480p or 720i but the highest-resolution playlist is 1080p,
        /// or this playlist has at most 2.0 audio channels but another playlist has 5.1.
        /// </summary>
        [JsonIgnore]
        public bool IsMaxQuality;

        /// <summary>
        /// Is a duplicate of another playlist.
        /// </summary>
        [JsonIgnore]
        public bool IsDuplicate;

        /// <summary>
        /// Contains duplicate stream clips.
        /// </summary>
        [JsonIgnore]
        public bool HasDuplicateStreamClips;

        /// <summary>
        /// Contains loops.
        /// </summary>
        [JsonIgnore]
        public bool HasLoops;

        /// <summary>
        /// First video track and/or first audio track is hidden.
        /// </summary>
        [JsonIgnore]
        public bool HasHiddenFirstTracks
        {
            get
            {
                return
                    (VideoTracks.Any() && VideoTracks.First().IsHidden) ||
                    (AudioTracks.Any() && AudioTracks.First().IsHidden);
            }
        }

        /// <summary>
        /// Has repeated stream clips (.M2TS files), contains loops, is a duplicate of another playlist, or has hidden primary audio/video tracks.
        /// </summary>
        [JsonIgnore]
        public bool IsBogus
        {
            get { return IsDuplicate || HasDuplicateStreamClips || HasLoops || HasHiddenFirstTracks; }
        }

        /// <summary>
        /// Gets or sets whether this playlist represents BDHero's "best guess" based on the user's preferences.
        /// </summary>
        [JsonIgnore]
        public bool IsBestGuess { get; set; }

        #endregion

        #region Non-DB Public properties (full path, stream clips, video language)

        /// <summary>
        /// Full absolute path to the .MPLS file.
        /// </summary>
        [JsonIgnore]
        public string FullPath;

        /// <summary>
        /// List of .M2TS files referenced by this playlist.
        /// </summary>
        [JsonIgnore]
        public List<StreamClip> StreamClips = new List<StreamClip>();

        /// <summary>
        /// Gets or sets the language of the first video track.
        /// </summary>
        [JsonIgnore]
        public Language FirstVideoLanguage
        {
            get { return VideoTracks.Any() ? VideoTracks.First().Language : Language.Undetermined; }
            set { if (VideoTracks.Any()) { VideoTracks.First().Language = value; } }
        }

        #endregion

        #region UI Properties (human length, warnings, chapter count)

        [JsonIgnore]
        public string LengthHuman
        {
            get
            {
                return string.Format("{0}:{1}:{2}", Length.Hours.ToString("00"), Length.Minutes.ToString("00"), Length.Seconds.ToString("00"));
            }
        }

        [JsonIgnore]
        public string Warnings
        {
            get
            {
                var warnings = new List<string>();
                if (IsBogus)
                    warnings.Add("Bogus");
                if (!IsMaxQuality)
                    warnings.Add("Low Quality");
                return string.Join(", ", warnings);
            }
        }

        [JsonIgnore]
        public int ChapterCount
        {
            get { return Chapters.Count; }
        }

        #endregion

        #region Non-DB Video track properties (main feature, video commentary, special feature, misc.)

        /// <summary>
        /// The type of the first (primary) video track.
        /// </summary>
        [JsonIgnore]
        public TrackType Type
        {
            get
            {
                var video = VideoTracks.FirstOrDefault();
                return video != null ? video.Type : TrackType.Misc;
            }
            set
            {
                var video = VideoTracks.FirstOrDefault();
                if (video != null)
                    video.Type = value;
            }
        }

        /// <summary>
        /// The main movie (a.k.a. feature film) without forced (burned in) video commentary.
        /// </summary>
        [JsonIgnore]
        public bool IsMainFeature
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsMainFeature);
            }
        }

        /// <summary>
        /// Director or other commentary is burned in to the primary video track.
        /// </summary>
        [JsonIgnore]
        public bool IsVideoCommentary
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsCommentary);
            }
        }

        /// <summary>
        /// Special feature.
        /// </summary>
        [JsonIgnore]
        public bool IsSpecialFeature
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsSpecialFeature);
            }
        }

        /// <summary>
        /// Miscellaneous / extra / other playlist (e.g., trailer, FBI warning).
        /// </summary>
        [JsonIgnore]
        public bool IsMisc
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsMisc);
            }
        }

        #endregion

        #region Non-DB Audio / Video properties

        [JsonIgnore]
        public IList<Track> VideoTracks
        {
            get { return Tracks.Where(track => track.IsVideo).ToList(); }
        }

        [JsonIgnore]
        public IList<Track> AudioTracks
        {
            get { return Tracks.Where(track => track.IsAudio).ToList(); }
        }

        [JsonIgnore]
        public IList<Track> SubtitleTracks
        {
            get { return Tracks.Where(track => track.IsSubtitle).ToList(); }
        }

        /// <summary>
        /// Gets the maximum height (in pixels) of the playlist's video tracks (e.g., 1080 or 720).
        /// </summary>
        [JsonIgnore]
        public int MaxAvailableVideoResolution
        {
            get
            {
                return VideoTracks.OrderByDescending(v => v.VideoHeight).Select(track => track.VideoHeight).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the maximum height (in pixels) of the playlist's selected video tracks (e.g., 1080 or 720).
        /// </summary>
        [JsonIgnore]
        public int MaxSelectedVideoResolution
        {
            get
            {
                return VideoTracks.Where(track => track.Keep).OrderByDescending(v => v.VideoHeight).Select(track => track.VideoHeight).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the maximum height (in pixels) and type (progressive/interlaced) of the playlist's video tracks in a human friendly format (e.g., 1080p or 720i).
        /// </summary>
        [JsonIgnore]
        public string MaxAvailableVideoResolutionDisplayable
        {
            get
            {
                return VideoTracks.OrderByDescending(v => v.VideoHeight).Select(track => track.VideoFormatDisplayable).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the maximum height (in pixels) and type (progressive/interlaced) of the playlist's selected video tracks in a human friendly format (e.g., 1080p or 720i).
        /// </summary>
        [JsonIgnore]
        public string MaxSelectedVideoResolutionDisplayable
        {
            get
            {
                return VideoTracks.Where(track => track.Keep).OrderByDescending(v => v.VideoHeight).Select(track => track.VideoFormatDisplayable).FirstOrDefault();
            }
        }

        /// <summary>
        /// The maximum number of audio channels found in this playlist's audio tracks (e.g., 8, 6, or 2).
        /// </summary>
        [JsonIgnore]
        public double MaxAudioChannels
        {
            get
            {
                return AudioTracks.OrderByDescending(v => v.ChannelCount).Select(track => track.ChannelCount).FirstOrDefault();
            }
        }

        #endregion

        #region Feature detection

        public bool IsMainFeaturePlaylist(TimeSpan maxPlaylistLength)
        {
            return
                IsMaxQuality &&
                IsFeatureLength(maxPlaylistLength) &&
                VideoTracks.Count >= 1 &&
                AudioTracks.Count >= 1 &&
                SubtitleTracks.Count >= 1 &&
                Chapters.Count >= 2 &&
                Length > MinLength;
        }

        public bool IsSpecialFeaturePlaylist(TimeSpan maxPlaylistLength)
        {
            return (IsMaxQuality && !IsFeatureLength(maxPlaylistLength) && AudioTracks.Count == 1 && Length > MinLength) ||
                   (!IsMaxQuality && IsFeatureLength(maxPlaylistLength) && AudioTracks.Count == 1 && Length > MinLength);
        }

        public bool IsFeatureLength(TimeSpan maxPlaylistLength)
        {
            return Length >= TimeSpan.FromMilliseconds(FeatureLengthPercentage * maxPlaylistLength.TotalMilliseconds);
        }

        #endregion

        #region Protected utilities

        /// <summary>
        /// Null-safe method for testing if the first video track meets certain criteria.
        /// </summary>
        /// <param name="delegate">Does NOT need to check if the video track is null.  It will not be invoked if there are no video tracks.</param>
        /// <returns>The delegate's return value if this playlist has a video track; otherwise false.</returns>
        bool TestFirstVideoTrack(FirstVideoTrackDelegate @delegate)
        {
            var video = VideoTracks.FirstOrDefault();
            return video != null && @delegate(video);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2} bytes, {3} chapters, {4}", FileName, Length, FileSize, ChapterCount, Type);
        }

        #endregion
    }

    public enum PlaylistCut
    {
        Theatrical,
        Special,
        Extended,
        Unrated
    }

    delegate bool FirstVideoTrackDelegate(Track track);
}
