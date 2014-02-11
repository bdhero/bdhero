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
using System.Linq;
using BDInfo;
using I18N;

// ReSharper disable InconsistentNaming
namespace BDHero.BDROM
{
    /// <summary>
    /// Represents an audio, video, or subtitle track in a .M2TS file.
    /// </summary>
    public class Track
    {
        #region DB User-configurable fields (language)

        /// <summary>
        /// Language of the track.
        /// </summary>
        public Language Language = Language.Undetermined;

        #endregion

        #region DB Matching fields (index, PID, hidden, codec)

        /// <summary>
        /// Zero-based index of the track (i.e., its position or order) in the playlist.
        /// </summary>
        public int Index;

        // MPEG-2 Transport Stream (M2TS) packet ID (PID) that uniquely identifies the track in the playlist.
        public int PID;

        /// <summary>
        /// Track is physically present in the underlying .M2TS stream file(s), but is not listed in the .MPLS playlist file.
        /// </summary>
        public bool IsHidden;

        /// <summary>
        /// Value of MICodec.SerializableName.
        /// </summary>
        public Codec Codec = Codec.UnknownCodec;

        #endregion

        #region DB "FYI" fields (not used for comparison, searching, or filtering)

        public bool IsVideo;
        public bool IsAudio;
        public bool IsSubtitle;

        /// <summary>
        /// Number of audio channels (e.g., 2.0, 5.1, 7.1).
        /// </summary>
        public double ChannelCount;

        /// <summary>
        /// Audio only (e.g., 16-, 20-, or 24-bit for LPCM).
        /// </summary>
        public int BitDepth;

        public TSVideoFormat VideoFormat;
        public TSFrameRate FrameRate;
        public TSAspectRatio AspectRatio;

        #endregion

        #region DB Track type (main, commentary, special feature, descriptive, misc)

        public TrackType Type = TrackType.Misc;

        /// <summary>
        /// Main feature.
        /// </summary>
        public bool IsMainFeature { get { return Type == TrackType.MainFeature; } }

        /// <summary>
        /// Director or other commentary.
        /// </summary>
        public bool IsCommentary { get { return Type == TrackType.Commentary; } }

        /// <summary>
        /// Special feature.
        /// </summary>
        public bool IsSpecialFeature { get { return Type == TrackType.SpecialFeature; } }

        /// <summary>
        /// Descriptive audio for the blind.
        /// </summary>
        public bool IsDescriptive { get { return Type == TrackType.Descriptive; } }

        /// <summary>
        /// Miscellaneous / extra / other track (e.g., trailer, FBI warning).
        /// </summary>
        public bool IsMisc { get { return Type == TrackType.Misc; } }

        #endregion

        #region UI selection properties

        /// <summary>
        /// Position or order of the track relative to other tracks of the same type (audio, video, or subtitles).
        /// </summary>
        public int IndexOfType;

        /// <summary>
        /// Should this track be kept when muxing to MKV?
        /// </summary>
        public bool Keep;

        /// <summary>
        /// Gets or sets whether this track represents BDHero's "best guess" for the user's preferred track
        /// based on the user's preferences.
        /// </summary>
        public bool IsBestGuess;

        private string DefaultTitle
        {
            get
            {
                var type = IsMainFeature    ? "Main Feature" :
                           IsCommentary     ? "Commentary" :
                           IsSpecialFeature ? "Special Feature" :
                           IsDescriptive    ? "Descriptive" :
                                              "Misc";
                var title = string.Format("{0}: {1}", type, Codec.CommonName);
                if (IsVideo)
                    return string.Format("{0} ({1})", title, VideoFormatDisplayable);
                if (IsAudio)
                    return string.Format("{0} ({1} ch)", title, ChannelCount.ToString("0.0"));
                return title;
            }
        }

        public string Title
        {
            get { return _title ?? DefaultTitle; }
            set { _title = value; }
        }

        private string _title;

        #endregion

        #region UI display properties

        /// <summary>
        /// Number of audio channels (e.g., 2, 6, 8).
        /// </summary>
        public string QualityDisplayable
        {
            get
            {
                if (IsVideo) return VideoFormatDisplayable;
                if (IsAudio) return ChannelCount.ToString("0.0");
                return "";
            }
        }

        /// <summary>
        /// Video height (e.g., 1080, 720, 480).
        /// </summary>
        public int VideoHeight
        {
            get
            {
                return
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_1080i || VideoFormat == TSVideoFormat.VIDEOFORMAT_1080p ? 1080 :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_720p ? 720 :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_576i || VideoFormat == TSVideoFormat.VIDEOFORMAT_576p ? 576 :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_480i || VideoFormat == TSVideoFormat.VIDEOFORMAT_480p ? 480 : 0;
            }
        }

        /// <summary>
        /// Video height and format (e.g., 1080p, 720i, 480p).
        /// </summary>
        public string VideoFormatDisplayable
        {
            get
            {
                return
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_1080i ? "1080i" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_1080p ? "1080p" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_720p ? "720p" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_576i ? "576i" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_576p ? "576p" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_480i ? "480i" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_480p ? "480p" : "unknown";
            }
        }

        public string FrameRateDisplayable
        {
            get
            {
                return
                    FrameRate == TSFrameRate.FRAMERATE_23_976 ? "23.976" :
                    FrameRate == TSFrameRate.FRAMERATE_24 ? "24" :
                    FrameRate == TSFrameRate.FRAMERATE_25 ? "25" :
                    FrameRate == TSFrameRate.FRAMERATE_29_97 ? "29.97" :
                    FrameRate == TSFrameRate.FRAMERATE_50 ? "50" :
                    FrameRate == TSFrameRate.FRAMERATE_59_94 ? "59.94" : "unknown";
            }
        }

        public string AspectRatioDisplayable
        {
            get
            {
                return
                    AspectRatio == TSAspectRatio.ASPECT_16_9 ? "16:9" :
                    AspectRatio == TSAspectRatio.ASPECT_4_3 ? "4:3" : "unknown";
            }
        }

        #endregion
    }

    public enum TrackType
    {
        MainFeature,
        Commentary,
        SpecialFeature,
        Descriptive,
        Misc
    }
}
