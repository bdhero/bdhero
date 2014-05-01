//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

#undef DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BDInfo
{
    /// <summary>
    /// Represents a BDMV/PLAYLIST/XXXXX.MPLS file.
    /// </summary>
    public class TSPlaylistFile
    {
        private readonly FileInfo _fileInfo;

        /// <summary>
        /// "MPLS0100" or "MPLS0200"
        /// </summary>
        private string _fileType;

        private bool _isInitialized;

        /// <summary>
        /// Name of the playlist file in all uppercase (e.g., "00100.MPLS")
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Full path to the playlist file (e.g., "D:\BDMV\PLAYLIST\00100.MPLS").
        /// </summary>
        public readonly string FullName;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public readonly BDROM BDROM;
        public bool HasLoops;
        public bool IsCustom;
        public int HiddenTrackCount;
        public bool HasHiddenTracks { get { return HiddenTrackCount > 0; }}
// ReSharper restore FieldCanBeMadeReadOnly.Global
// ReSharper restore MemberCanBePrivate.Global

        public bool IsFeatureLength;
        public bool IsMaxQuality;
        public bool IsDuplicate;

// ReSharper disable MemberCanBePrivate.Global
        public readonly List<double> Chapters = new List<double>();

        public readonly Dictionary<ushort, TSStream> Streams = 
            new Dictionary<ushort, TSStream>();
        public readonly Dictionary<ushort, TSStream> PlaylistStreams =
            new Dictionary<ushort, TSStream>();
        public readonly List<TSStreamClip> StreamClips =
            new List<TSStreamClip>();
        public readonly List<Dictionary<ushort, TSStream>> AngleStreams =
            new List<Dictionary<ushort, TSStream>>();
        public readonly List<Dictionary<double, TSStreamClip>> AngleClips = 
            new List<Dictionary<double, TSStreamClip>>();
        public int AngleCount;

        public readonly List<TSStream> SortedStreams = 
            new List<TSStream>();
        public readonly List<TSVideoStream> VideoStreams = 
            new List<TSVideoStream>();
        public readonly List<TSAudioStream> AudioStreams = 
            new List<TSAudioStream>();
        public readonly List<TSTextStream> TextStreams = 
            new List<TSTextStream>();
        public readonly List<TSGraphicsStream> GraphicsStreams = 
            new List<TSGraphicsStream>();
// ReSharper restore MemberCanBePrivate.Global

        public TSPlaylistFile(
            BDROM bdrom,
            FileInfo fileInfo)
        {
            BDROM = bdrom;
            _fileInfo = fileInfo;
            Name = fileInfo.Name.ToUpper();
            FullName = fileInfo.FullName.ToUpper();
        }

        public TSPlaylistFile(
            BDROM bdrom,
            string name,
            IEnumerable<TSStreamClip> clips)
        {
            BDROM = bdrom;
            Name = name;
            FullName = Path.Combine(bdrom.DirectoryPLAYLIST.FullName, Name).ToUpper();
            IsCustom = true;

            foreach (var clip in clips)
            {
                var newClip = new TSStreamClip(clip.StreamFile, clip.StreamClipFile)
                                  {
                                      Name = clip.Name,
                                      TimeIn = clip.TimeIn,
                                      TimeOut = clip.TimeOut,
                                      RelativeTimeIn = TotalLength,
                                      AngleIndex = clip.AngleIndex
                                  };

                newClip.Length = newClip.TimeOut - newClip.TimeIn;
                newClip.RelativeTimeOut = newClip.RelativeTimeIn + newClip.Length;
                newClip.Chapters.Add(clip.TimeIn);

                StreamClips.Add(newClip);

                if (newClip.AngleIndex > AngleCount)
                {
                    AngleCount = newClip.AngleIndex;
                }
                if (newClip.AngleIndex == 0)
                {
                    Chapters.Add(newClip.RelativeTimeIn);
                }
            }

            LoadStreamClips();
            _isInitialized = true;
        }

        private bool _autoConfigured;
        private bool _isMainMovieAuto;

        public bool IsMainMovieAuto
        {
            get { return _isMainMovieAuto; }
            set
            {
                _autoConfigured = true;
                _isMainMovieAuto = value;
            }
        }

        public bool IsLikelyMainMovie
        {
            get { return _autoConfigured ? IsMainMovieAuto : (IsFeatureLength && !HasDuplicateClips && !IsDuplicate && IsMaxQuality); }
        }

        public bool IsShort
        {
            get { return !IsFeatureLength; }
        }

        /// <summary>
        /// Feature-length playlist that has duplicate clips or is itself a duplicate playlist.
        /// </summary>
        public bool IsBogus
        {
            get { return (HasDuplicateClips || IsDuplicate) && IsFeatureLength; }
        }

        /// <summary>
        /// Shortcut for <c>!IsMaxQuality</c>.
        /// </summary>
        public bool IsLowQuality
        {
            get { return !IsMaxQuality; }
        }

        /// <summary>
        /// Low quality feature-length playlist (NOT bogus).
        /// </summary>
        public bool IsLowQualityOnly
        {
            get { return IsLowQuality && !IsBogus && IsFeatureLength; }
        }

        /// <summary>
        /// Bogus, maximum-quality, feature-length playlist.
        /// </summary>
        public bool IsBogusOnly
        {
            get { return IsBogus && IsMaxQuality; }
        }

        /// <summary>
        /// Numeric value used to sort playlists by likelyhood that they are the main movie.  Lower is better.
        /// </summary>
        public TSPlaylistRank Rank
        {
            get
            {
                if (IsLikelyMainMovie) return TSPlaylistRank.MainMovieHq;
                if (IsLowQualityOnly) return TSPlaylistRank.MainMovieLq;
                if (IsBogusOnly) return TSPlaylistRank.BogusFeature;
                return TSPlaylistRank.Short;
            }
        }

        public string RankToolTipText
        {
            get
            {
                if (IsLikelyMainMovie) return "Best guess for main movie playlist";
                if (IsLowQualityOnly) return "Low quality feature-length playlist";
                if (IsBogusOnly) return "Bogus (duplicate) feature-length playlist";
                return "Too short to be the main movie";
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool HasDuplicateClips
        {
            get
            {
                var clips = new Dictionary<string, TSStreamClip>();
                foreach (var clip in StreamClips)
                {
                    var key = GetClipKey(clip);
                    if (clips.ContainsKey(key))
                    {
                        return true;
                    }
                    clips.Add(key, clip);
                }
                return false;
            }
        }

        private static string GetClipKey(TSStreamClip clip)
        {
            return string.Format("{0}{1}", clip.Length, clip.FileSize);
        }

        public ulong InterleavedFileSize
        {
            get
            {
                return StreamClips.Aggregate<TSStreamClip, ulong>(0, (current, clip) => current + clip.InterleavedFileSize);
            }
        }
        public ulong FileSize
        {
            get
            {
                return StreamClips.Aggregate<TSStreamClip, ulong>(0, (current, clip) => current + clip.FileSize);
            }
        }
        public double TotalLength
        {
            get
            {
                return StreamClips.Where(clip => clip.AngleIndex == 0).Sum(clip => clip.Length);
            }
        }

        public double TotalAngleLength
        {
            get
            {
                return StreamClips.Sum(clip => clip.Length);
            }
        }

        public ulong TotalSize
        {
            get
            {
                return StreamClips.Where(clip => clip.AngleIndex == 0).Aggregate<TSStreamClip, ulong>(0, (current, clip) => current + clip.PacketSize);
            }
        }

        public ulong TotalAngleSize
        {
            get
            {
                return StreamClips.Aggregate<TSStreamClip, ulong>(0, (current, clip) => current + clip.PacketSize);
            }
        }

        public ulong TotalBitRate
        {
            get { return TotalLength > 0 ? (ulong) Math.Round(((TotalSize*8.0)/TotalLength)) : 0; }
        }

        public ulong TotalAngleBitRate
        {
            get { return TotalAngleLength > 0 ? (ulong) Math.Round(((TotalAngleSize*8.0)/TotalAngleLength)) : 0; }
        }

        public int MaxVideoHeight
        {
            get
            {
                var bestVideoTrack = VideoStreams.OrderByDescending(v => v.Height).FirstOrDefault();
                return bestVideoTrack != null ? bestVideoTrack.Height : 0;
            }
        }

        public int MaxAudioChannels
        {
            get
            {
                var bestAudioTrack = AudioStreams.OrderByDescending(a => a.ChannelCount).FirstOrDefault();
                return bestAudioTrack != null ? bestAudioTrack.ChannelCount : 0;
            }
        }

        public void Scan(
            Dictionary<string, TSStreamFile> streamFiles,
            Dictionary<string, TSStreamClipFile> streamClipFiles)
        {
            FileStream fileStream = null;
            BinaryReader fileReader = null;

            try
            {
                Streams.Clear();
                StreamClips.Clear();

                fileStream = File.OpenRead(_fileInfo.FullName);
                fileReader = new BinaryReader(fileStream);

                byte[] data = new byte[fileStream.Length];
                int dataLength = fileReader.Read(data, 0, data.Length);

                int pos = 0;

                _fileType = ReadString(data, 8, ref pos);
                if (_fileType != "MPLS0100" &
                    _fileType != "MPLS0200")
                {
                    throw new InvalidDataException(string.Format(
                        "Playlist {0} has an unknown file type {1}.  The disc may be damaged or corrupt.",
                        _fileInfo.Name, _fileType));
                }

                int playlistOffset = ReadInt32(data, ref pos);
                int chaptersOffset = ReadInt32(data, ref pos);
                int extensionsOffset = ReadInt32(data, ref pos);

                pos = playlistOffset;

                int playlistLength = ReadInt32(data, ref pos);
                int playlistReserved = ReadInt16(data, ref pos);
                int itemCount = ReadInt16(data, ref pos);
                int subitemCount = ReadInt16(data, ref pos);

                List<TSStreamClip> chapterClips = new List<TSStreamClip>();
                for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
                {
                    int itemStart = pos;
                    int itemLength = ReadInt16(data, ref pos);
                    string itemName = ReadString(data, 5, ref pos);
                    string itemType = ReadString(data, 4, ref pos);

                    TSStreamFile streamFile = null;
                    string streamFileName = string.Format(
                        "{0}.M2TS", itemName);
                    if (streamFiles.ContainsKey(streamFileName))
                    {
                        streamFile = streamFiles[streamFileName];
                    }
                    if (streamFile == null)
                    {
                        Debug.WriteLine(string.Format(
                            "Playlist {0} referenced missing file {1}.",
                            _fileInfo.Name, streamFileName));
                    }

                    TSStreamClipFile streamClipFile = null;
                    string streamClipFileName = string.Format(
                        "{0}.CLPI", itemName);
                    if (streamClipFiles.ContainsKey(streamClipFileName))
                    {
                        streamClipFile = streamClipFiles[streamClipFileName];
                    }
                    if (streamClipFile == null)
                    {
                        throw new FileNotFoundException(string.Format(
                            "Playlist {0} referenced missing file {1}.",
                            _fileInfo.Name, streamFileName), _fileInfo.FullName);
                    }

                    pos += 1;
                    int multiangle = (data[pos] >> 4) & 0x01;
                    int condition = data[pos] & 0x0F;
                    pos += 2;

                    int inTime = ReadInt32(data, ref pos);
                    if (inTime < 0) inTime &= 0x7FFFFFFF;
                    double timeIn = (double)inTime / 45000;

                    int outTime = ReadInt32(data, ref pos);
                    if (outTime < 0) outTime &= 0x7FFFFFFF;
                    double timeOut = (double)outTime / 45000;

                    TSStreamClip streamClip = new TSStreamClip(
                        streamFile, streamClipFile);

                    streamClip.Name = streamFileName; //TODO
                    streamClip.TimeIn = timeIn;
                    streamClip.TimeOut = timeOut;
                    streamClip.Length = streamClip.TimeOut - streamClip.TimeIn;
                    streamClip.RelativeTimeIn = TotalLength;
                    streamClip.RelativeTimeOut = streamClip.RelativeTimeIn + streamClip.Length;
                    StreamClips.Add(streamClip);
                    chapterClips.Add(streamClip);

                    pos += 12;
                    if (multiangle > 0)
                    {
                        int angles = data[pos];
                        pos += 2;
                        for (int angle = 0; angle < angles - 1; angle++)
                        {
                            string angleName = ReadString(data, 5, ref pos);
                            string angleType = ReadString(data, 4, ref pos);
                            pos += 1;

                            TSStreamFile angleFile = null;
                            string angleFileName = string.Format(
                                "{0}.M2TS", angleName);
                            if (streamFiles.ContainsKey(angleFileName))
                            {
                                angleFile = streamFiles[angleFileName];
                            }
                            if (angleFile == null)
                            {
                                throw new FileNotFoundException(string.Format(
                                    "Playlist {0} referenced missing angle file {1}.",
                                    _fileInfo.Name, angleFileName), _fileInfo.FullName);
                            }

                            TSStreamClipFile angleClipFile = null;
                            string angleClipFileName = string.Format(
                                "{0}.CLPI", angleName);
                            if (streamClipFiles.ContainsKey(angleClipFileName))
                            {
                                angleClipFile = streamClipFiles[angleClipFileName];
                            }
                            if (angleClipFile == null)
                            {
                                throw new FileNotFoundException(string.Format(
                                    "Playlist {0} referenced missing angle file {1}.",
                                    _fileInfo.Name, angleClipFileName), _fileInfo.FullName);
                            }

                            TSStreamClip angleClip =
                                new TSStreamClip(angleFile, angleClipFile);
                            angleClip.AngleIndex = angle + 1;
                            angleClip.TimeIn = streamClip.TimeIn;
                            angleClip.TimeOut = streamClip.TimeOut;
                            angleClip.RelativeTimeIn = streamClip.RelativeTimeIn;
                            angleClip.RelativeTimeOut = streamClip.RelativeTimeOut;
                            angleClip.Length = streamClip.Length;
                            StreamClips.Add(angleClip);
                        }
                        if (angles - 1 > AngleCount) AngleCount = angles - 1;
                    }

                    int streamInfoLength = ReadInt16(data, ref pos);
                    pos += 2;
                    int streamCountVideo = data[pos++];
                    int streamCountAudio = data[pos++];
                    int streamCountPG = data[pos++];
                    int streamCountIG = data[pos++];
                    int streamCountSecondaryAudio = data[pos++];
                    int streamCountSecondaryVideo = data[pos++];
                    int streamCountPIP = data[pos++];
                    pos += 5;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "{0} : {1} -> V:{2} A:{3} PG:{4} IG:{5} 2A:{6} 2V:{7} PIP:{8}", 
                        Name, streamFileName, streamCountVideo, streamCountAudio, streamCountPG, streamCountIG, 
                        streamCountSecondaryAudio, streamCountSecondaryVideo, streamCountPIP));
#endif

                    for (int i = 0; i < streamCountVideo; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountAudio; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountPG; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountIG; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountSecondaryAudio; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                        pos += 2;
                    }
                    for (int i = 0; i < streamCountSecondaryVideo; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                        pos += 6;
                    }
                    /*
                     * TODO
                     * 
                    for (int i = 0; i < streamCountPIP; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    */

                    pos += itemLength - (pos - itemStart) + 2;
                }

                pos = chaptersOffset + 4;

                int chapterCount = ReadInt16(data, ref pos);

                for (int chapterIndex = 0;
                    chapterIndex < chapterCount;
                    chapterIndex++)
                {
                    int chapterType = data[pos+1];

                    if (chapterType == 1)
                    {
                        int streamFileIndex =
                            ((int)data[pos + 2] << 8) + data[pos + 3];

                        long chapterTime =
                            ((long)data[pos + 4] << 24) +
                            ((long)data[pos + 5] << 16) +
                            ((long)data[pos + 6] << 8) +
                            ((long)data[pos + 7]);

                        TSStreamClip streamClip = chapterClips[streamFileIndex];

                        double chapterSeconds = (double)chapterTime / 45000;

                        double relativeSeconds =
                            chapterSeconds -
                            streamClip.TimeIn +
                            streamClip.RelativeTimeIn;

                        // Ignore short last chapter
                        // If the last chapter is < 1.0 Second before end of film Ignore
                        if (TotalLength - relativeSeconds > 1.0)
                        {
                            streamClip.Chapters.Add(chapterSeconds);
                            this.Chapters.Add(relativeSeconds);
                        }
                    }
                    else
                    {
                        // TODO: Handle other chapter types?
                    }
                    pos += 14;
                }
            }
            finally
            {
                if (fileReader != null)
                {
                    fileReader.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public void Initialize()
        {
            LoadStreamClips();

            Dictionary<string, List<double>> clipTimes = new Dictionary<string, List<double>>();
            foreach (TSStreamClip clip in StreamClips)
            {
                if (clip.AngleIndex == 0)
                {
                    if (clipTimes.ContainsKey(clip.Name))
                    {
                        if (clipTimes[clip.Name].Contains(clip.TimeIn))
                        {
                            HasLoops = true;
                            break;
                        }
                        else
                        {
                            clipTimes[clip.Name].Add(clip.TimeIn);
                        }
                    }
                    else
                    {
                        clipTimes[clip.Name] = new List<double> { clip.TimeIn };
                    }
                }
            }
            ClearBitrates();
            _isInitialized = true;
        }

        protected TSStream CreatePlaylistStream(byte[] data, ref int pos)
        {
            TSStream stream = null;

            int start = pos;

            int headerLength = data[pos++];
            int headerPos = pos;
            int headerType = data[pos++];

            int pid = 0;
            int subpathid = 0;
            int subclipid = 0;

            switch (headerType)
            {
                case 1:
                    pid = ReadInt16(data, ref pos);
                    break;
                case 2:
                    subpathid = data[pos++];
                    subclipid = data[pos++];
                    pid = ReadInt16(data, ref pos);
                    break;
                case 3:
                    subpathid = data[pos++];
                    pid = ReadInt16(data, ref pos);
                    break;
                case 4:
                    subpathid = data[pos++];
                    subclipid = data[pos++];
                    pid = ReadInt16(data, ref pos);
                    break;
                default:
                    break;
            }

            pos = headerPos + headerLength;

            int streamLength = data[pos++];
            int streamPos = pos;

            TSStreamType streamType = (TSStreamType)data[pos++];
            switch (streamType)
            {
                case TSStreamType.MVC_VIDEO:
                    // TODO
                    break;

                case TSStreamType.AVC_VIDEO:
                case TSStreamType.MPEG1_VIDEO:
                case TSStreamType.MPEG2_VIDEO:
                case TSStreamType.VC1_VIDEO:

                    TSVideoFormat videoFormat = (TSVideoFormat)
                        (data[pos] >> 4);
                    TSFrameRate frameRate = (TSFrameRate)
                        (data[pos] & 0xF);
                    TSAspectRatio aspectRatio = (TSAspectRatio)
                        (data[pos + 1] >> 4);

                    stream = new TSVideoStream();
                    ((TSVideoStream)stream).VideoFormat = videoFormat;
                    ((TSVideoStream)stream).AspectRatio = aspectRatio;
                    ((TSVideoStream)stream).FrameRate = frameRate;

#if DEBUG
                            Debug.WriteLine(string.Format(
                                "\t{0} {1} {2} {3} {4}",
                                pid,
                                streamType,
                                videoFormat,
                                frameRate,
                                aspectRatio));
#endif

                    break;

                case TSStreamType.AC3_AUDIO:
                case TSStreamType.AC3_PLUS_AUDIO:
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                case TSStreamType.DTS_AUDIO:
                case TSStreamType.DTS_HD_AUDIO:
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                case TSStreamType.LPCM_AUDIO:
                case TSStreamType.MPEG1_AUDIO:
                case TSStreamType.MPEG2_AUDIO:

                    int audioFormat = ReadByte(data, ref pos);

                    TSChannelLayout channelLayout = (TSChannelLayout)
                        (audioFormat >> 4);
                    TSSampleRate sampleRate = (TSSampleRate)
                        (audioFormat & 0xF);

                    string audioLanguage = ReadString(data, 3, ref pos);

                    stream = new TSAudioStream();
                    ((TSAudioStream)stream).ChannelLayout = channelLayout;
                    ((TSAudioStream)stream).SampleRate = TSAudioStream.ConvertSampleRate(sampleRate);
                    ((TSAudioStream)stream).LanguageCode = audioLanguage;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2} {3} {4}",
                        pid,
                        streamType,
                        audioLanguage,
                        channelLayout,
                        sampleRate));
#endif

                    break;

                case TSStreamType.INTERACTIVE_GRAPHICS:
                case TSStreamType.PRESENTATION_GRAPHICS:

                    string graphicsLanguage = ReadString(data, 3, ref pos);

                    stream = new TSGraphicsStream();
                    ((TSGraphicsStream)stream).LanguageCode = graphicsLanguage;

                    if (data[pos] != 0)
                    {
                    }

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2}",
                        pid,
                        streamType,
                        graphicsLanguage));
#endif

                    break;

                case TSStreamType.SUBTITLE:

                    int code = ReadByte(data, ref pos); // TODO
                    string textLanguage = ReadString(data, 3, ref pos);

                    stream = new TSTextStream();
                    ((TSTextStream)stream).LanguageCode = textLanguage;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2}",
                        pid,
                        streamType,
                        textLanguage));
#endif

                    break;

                default:
                    break;
            }

            pos = streamPos + streamLength;

            if (stream != null)
            {
                stream.PID = (ushort)pid;
                stream.StreamType = streamType;
            }

            return stream;
        }

        private void LoadStreamClips()
        {
            AngleClips.Clear();
            if (AngleCount > 0)
            {
                for (int angleIndex = 0; angleIndex < AngleCount; angleIndex++)
                {
                    AngleClips.Add(new Dictionary<double, TSStreamClip>());
                }
            }

            TSStreamClip referenceClip = null;
            if (StreamClips.Count > 0)
            {
                referenceClip = StreamClips[0];
            }
            foreach (TSStreamClip clip in StreamClips)
            {
                if (clip.StreamClipFile.Streams.Count > referenceClip.StreamClipFile.Streams.Count)
                {
                    referenceClip = clip;
                }
                else if (clip.Length > referenceClip.Length)
                {
                    referenceClip = clip;
                }
                if (AngleCount > 0)
                {
                    if (clip.AngleIndex == 0)
                    {
                        for (int angleIndex = 0; angleIndex < AngleCount; angleIndex++)
                        {
                            AngleClips[angleIndex][clip.RelativeTimeIn] = clip;
                        }
                    }
                    else
                    {
                        AngleClips[clip.AngleIndex - 1][clip.RelativeTimeIn] = clip;
                    }
                }
            }

            foreach (TSStream clipStream
                in referenceClip.StreamClipFile.Streams.Values)
            {
                if (!Streams.ContainsKey(clipStream.PID))
                {
                    TSStream stream = clipStream.Clone();
                    Streams[clipStream.PID] = stream;

                    if (!IsCustom && !PlaylistStreams.ContainsKey(stream.PID))
                    {
                        stream.IsHidden = true;
                        HiddenTrackCount++;
                    }

                    if (stream.IsVideoStream)
                    {
                        VideoStreams.Add((TSVideoStream)stream);
                    }
                    else if (stream.IsAudioStream)
                    {
                        AudioStreams.Add((TSAudioStream)stream);
                    }
                    else if (stream.IsGraphicsStream)
                    {
                        GraphicsStreams.Add((TSGraphicsStream)stream);
                    }
                    else if (stream.IsTextStream)
                    {
                        TextStreams.Add((TSTextStream)stream);
                    }
                }
            }

            if (referenceClip.StreamFile != null)
            {
#if BDInfo
                // TODO: Better way to add this in?
                if (BDInfoSettings.EnableSSIF &&
                    referenceClip.StreamFile.InterleavedFile != null &&
                    referenceClip.StreamFile.Streams.ContainsKey(4114) &&
                    !Streams.ContainsKey(4114))
                {
                    TSStream stream = referenceClip.StreamFile.Streams[4114].Clone();
                    Streams[4114] = stream;
                    if (stream.IsVideoStream)
                    {
                        VideoStreams.Add((TSVideoStream)stream);
                    }
                }
#endif

                foreach (TSStream clipStream
                    in referenceClip.StreamFile.Streams.Values)
                {
                    if (Streams.ContainsKey(clipStream.PID))
                    {
                        TSStream stream = Streams[clipStream.PID];

                        if (stream.StreamType != clipStream.StreamType) continue;

                        if (clipStream.BitRate > stream.BitRate)
                        {
                            stream.BitRate = clipStream.BitRate;
                        }
                        stream.IsVBR = clipStream.IsVBR;

                        if (stream.IsVideoStream &&
                            clipStream.IsVideoStream)
                        {
                            ((TSVideoStream)stream).EncodingProfile =
                                ((TSVideoStream)clipStream).EncodingProfile;
                        }
                        else if (stream.IsAudioStream &&
                            clipStream.IsAudioStream)
                        {
                            TSAudioStream audioStream = (TSAudioStream)stream;
                            TSAudioStream clipAudioStream = (TSAudioStream)clipStream;

                            if (clipAudioStream.ChannelCount > audioStream.ChannelCount)
                            {
                                audioStream.ChannelCount = clipAudioStream.ChannelCount;
                            }
                            if (clipAudioStream.LFE > audioStream.LFE)
                            {
                                audioStream.LFE = clipAudioStream.LFE;
                            }
                            if (clipAudioStream.SampleRate > audioStream.SampleRate)
                            {
                                audioStream.SampleRate = clipAudioStream.SampleRate;
                            }
                            if (clipAudioStream.BitDepth > audioStream.BitDepth)
                            {
                                audioStream.BitDepth = clipAudioStream.BitDepth;
                            }
                            if (clipAudioStream.DialNorm < audioStream.DialNorm)
                            {
                                audioStream.DialNorm = clipAudioStream.DialNorm;
                            }
                            if (clipAudioStream.AudioMode != TSAudioMode.Unknown)
                            {
                                audioStream.AudioMode = clipAudioStream.AudioMode;
                            }
                            if (clipAudioStream.CoreStream != null &&
                                audioStream.CoreStream == null)
                            {
                                audioStream.CoreStream = (TSAudioStream)
                                    clipAudioStream.CoreStream.Clone();
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < AngleCount; i++)
            {
                AngleStreams.Add(new Dictionary<ushort, TSStream>());
            }

            foreach (TSStream stream in VideoStreams)
            {
                SortedStreams.Add(stream);
                for (int i = 0; i < AngleCount; i++)
                {
                    TSStream angleStream = stream.Clone();
                    angleStream.AngleIndex = i + 1;
                    AngleStreams[i][angleStream.PID] = angleStream;
                    SortedStreams.Add(angleStream);
                }
            }

            foreach (TSStream stream in AudioStreams)
            {
                SortedStreams.Add(stream);
            }


            foreach (TSStream stream in GraphicsStreams)
            {
                SortedStreams.Add(stream);
            }

            foreach (TSStream stream in TextStreams)
            {
                SortedStreams.Add(stream);
            }
        }

        public void ClearBitrates()
        {
            foreach (TSStreamClip clip in StreamClips)
            {
                clip.PayloadBytes = 0;
                clip.PacketCount = 0;
                clip.PacketSeconds = 0;

                if (clip.StreamFile != null)
                {
                    foreach (TSStream stream in clip.StreamFile.Streams.Values)
                    {
                        stream.PayloadBytes = 0;
                        stream.PacketCount = 0;
                        stream.PacketSeconds = 0;
                    }

                    if (clip.StreamFile != null &&
                        clip.StreamFile.StreamDiagnostics != null)
                    {
                        clip.StreamFile.StreamDiagnostics.Clear();
                    }
                }
            }

            foreach (TSStream stream in SortedStreams)
            {
                stream.PayloadBytes = 0;
                stream.PacketCount = 0;
                stream.PacketSeconds = 0;
            }
        }

        /// <summary>
        /// Is initialized, longer than 20 seconds, and doesn't have loops.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (!_isInitialized) return false;

                if (BDInfoSettings.FilterShortPlaylists &&
                    TotalLength < BDInfoSettings.FilterShortPlaylistsValue)
                {
                    return false;
                }

                if (HasLoops &&
                    BDInfoSettings.FilterLoopingPlaylists)
                {
                    return false;
                }

                return true;
            }
        }

        #region Sorting

        public static int CompareVideoStreams(
            TSVideoStream x, 
            TSVideoStream y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return 1;
            }
            else if (x != null && y == null)
            {
                return -1;
            }
            else
            {
                if (x.Height > y.Height)
                {
                    return -1;
                }
                else if (y.Height > x.Height)
                {
                    return 1;
                }
                else if (x.PID > y.PID)
                {
                    return 1;
                }
                else if (y.PID > x.PID)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int CompareAudioStreams(
            TSAudioStream x, 
            TSAudioStream y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return -1;
            }
            else if (x != null && y == null)
            {
                return 1;
            }
            else
            {
                if (x.ChannelCount > y.ChannelCount)
                {
                    return -1;
                }
                else if (y.ChannelCount > x.ChannelCount)
                {
                    return 1;
                }
                else
                {
                    int sortX = GetStreamTypeSortIndex(x.StreamType);
                    int sortY = GetStreamTypeSortIndex(y.StreamType);

                    if (sortX > sortY)
                    {
                        return -1;
                    }
                    else if (sortY > sortX)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.LanguageCode == "eng")
                        {
                            return -1;
                        }
                        else if (y.LanguageCode == "eng")
                        {
                            return 1;
                        }
                        else if (x.LanguageCode != y.LanguageCode)
                        {
                            return string.Compare(
                                x.LanguageName, y.LanguageName);
                        }
                        else if (x.PID < y.PID)
                        {
                            return -1;
                        }
                        else if (y.PID < x.PID)
                        {
                            return 1;
                        }
                        return 0;
                    }
                }
            }
        }

        public static int CompareTextStreams(
            TSTextStream x,
            TSTextStream y)
        {
            if (x == y)
            {
                return 0;
            }
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            if (x.LanguageCode == "eng")
            {
                return -1;
            }
            if (y.LanguageCode == "eng")
            {
                return 1;
            }
            if (x.LanguageCode == y.LanguageCode)
            {
                if (x.PID > y.PID)
                {
                    return 1;
                }
                if (y.PID > x.PID)
                {
                    return -1;
                }
                return 0;
            }
            return string.Compare(
                x.LanguageName, y.LanguageName);
        }

        private static int CompareGraphicsStreams(
            TSGraphicsStream x,
            TSGraphicsStream y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return -1;
            }
            else if (x != null && y == null)
            {
                return 1;
            }
            else
            {
                int sortX = GetStreamTypeSortIndex(x.StreamType);
                int sortY = GetStreamTypeSortIndex(y.StreamType);

                if (sortX > sortY)
                {
                    return -1;
                }
                else if (sortY > sortX)
                {
                    return 1;
                }
                else if (x.LanguageCode == "eng")
                {
                    return -1;
                }
                else if (y.LanguageCode == "eng")
                {
                    return 1;
                }
                else
                {
                    if (x.LanguageCode == y.LanguageCode)
                    {
                        if (x.PID > y.PID)
                        {
                            return 1;
                        }
                        else if (y.PID > x.PID)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return string.Compare(x.LanguageName, y.LanguageName);
                    }
                }
            }
        }

        public static int ComparePlaylistFiles(
            TSPlaylistFile x,
            TSPlaylistFile y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return 1;
            }
            else if (x != null && y == null)
            {
                return -1;
            }
            else
            {
                // x > y --> -1
                // y > x --> +1

                if (x.IsFeatureLength && !y.IsFeatureLength)
                    return -1;
                else if (y.IsFeatureLength && !x.IsFeatureLength)
                    return +1;

                else if (!x.HasDuplicateClips && y.HasDuplicateClips)
                    return -1;
                else if (!y.HasDuplicateClips && x.HasDuplicateClips)
                    return 1;

                else if (x.TotalLength > y.TotalLength)
                    return -1;
                else if (y.TotalLength > x.TotalLength)
                    return 1;

                else
                {
                    int xName, yName;
                    Int32.TryParse(Path.GetFileNameWithoutExtension(x.Name), out xName);
                    Int32.TryParse(Path.GetFileNameWithoutExtension(y.Name), out yName);
                    int diff = xName - yName;
                    return diff > 0 ? +1 : (diff < 0 ? -1 : 0);
                }
            }
        }

        public static List<TSPlaylistFile> Sort(List<TSPlaylistFile> playlists, Comparison<TSPlaylistFile> comparison = null)
        {
            if (comparison == null)
                comparison = ComparePlaylistFiles;

            TSPlaylistFile[] playlistArray = playlists.ToArray();
            Array.Sort(playlistArray, comparison);
            return new List<TSPlaylistFile>(playlistArray);
        }

        private static int GetStreamTypeSortIndex(TSStreamType streamType)
        {
            switch (streamType)
            {
                case TSStreamType.Unknown:
                    return 0;
                case TSStreamType.MPEG1_VIDEO:
                    return 1;
                case TSStreamType.MPEG2_VIDEO:
                    return 2;
                case TSStreamType.AVC_VIDEO:
                    return 3;
                case TSStreamType.VC1_VIDEO:
                    return 4;
                case TSStreamType.MVC_VIDEO:
                    return 5;

                case TSStreamType.MPEG1_AUDIO:
                    return 1;
                case TSStreamType.MPEG2_AUDIO:
                    return 2;
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                    return 3;
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                    return 4;
                case TSStreamType.AC3_AUDIO:
                    return 5;
                case TSStreamType.DTS_AUDIO:
                    return 6;
                case TSStreamType.AC3_PLUS_AUDIO:
                    return 7;
                case TSStreamType.DTS_HD_AUDIO:
                    return 8;
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                    return 9;
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                    return 10;
                case TSStreamType.LPCM_AUDIO:
                    return 11;

                case TSStreamType.SUBTITLE:
                    return 1;
                case TSStreamType.INTERACTIVE_GRAPHICS:
                    return 2;
                case TSStreamType.PRESENTATION_GRAPHICS:
                    return 3;

                default:
                    return 0;
            }
        }

        #endregion

        #region I/O

        protected string ReadString(
            byte[] data,
            int count,
            ref int pos)
        {
            string val =
                ASCIIEncoding.ASCII.GetString(data, pos, count);

            pos += count;

            return val;
        }

        protected int ReadInt32(
            byte[] data,
            ref int pos)
        {
            int val =
                ((int)data[pos] << 24) +
                ((int)data[pos + 1] << 16) +
                ((int)data[pos + 2] << 8) +
                ((int)data[pos + 3]);

            pos += 4;

            return val;
        }

        protected int ReadInt16(
            byte[] data,
            ref int pos)
        {
            int val =
                ((int)data[pos] << 8) +
                ((int)data[pos + 1]);

            pos += 2;

            return val;
        }

        protected byte ReadByte(
            byte[] data,
            ref int pos)
        {
            return data[pos++];
        }

        #endregion
    }

    public enum TSPlaylistRank
    {
        /// <summary>
        /// High quality, non-bogus, feature length
        /// </summary>
        MainMovieHq,

        /// <summary>
        /// Low quality, non-bogus, feature-length
        /// </summary>
        MainMovieLq,

        /// <summary>
        /// Bogus feature-length (high and low quality)
        /// </summary>
        BogusFeature,

        /// <summary>
        /// Short (non-feature-length) (high and low quality)
        /// </summary>
        Short
    }
}
