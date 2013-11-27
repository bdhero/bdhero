using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BDHero.BDROM
{
    #region Abstract Codecs

    public abstract class Codec
    {
        #region Individual Codecs

        #region Video

        public static readonly CodecAVC AVC = new CodecAVC();
        public static readonly CodecVC1 VC1 = new CodecVC1();
        public static readonly CodecMPEG1Video MPEG1Video = new CodecMPEG1Video();
        public static readonly CodecMPEG2Video MPEG2Video = new CodecMPEG2Video();
        public static readonly CodecUnknownVideo UnknownVideo = new CodecUnknownVideo();

        #endregion

        #region Audio

        public static readonly CodecProLogic ProLogic = new CodecProLogic();
        public static readonly CodecAC3 AC3 = new CodecAC3();
        public static readonly CodecAC3EX AC3EX = new CodecAC3EX();
        public static readonly CodecEAC3 EAC3 = new CodecEAC3();
        public static readonly CodecTrueHD TrueHD = new CodecTrueHD();

        public static readonly CodecDTS DTS = new CodecDTS();
        public static readonly CodecDTSES DTSES = new CodecDTSES();
        public static readonly CodecDTSExpress DTSExpress = new CodecDTSExpress();
        public static readonly CodecDTSHDHRA DTSHDHRA = new CodecDTSHDHRA();
        public static readonly CodecDTSHDMA DTSHDMA = new CodecDTSHDMA();

        public static readonly CodecMP3 MP3 = new CodecMP3();
        public static readonly CodecAAC AAC = new CodecAAC();

        public static readonly CodecVorbis Vorbis = new CodecVorbis();
        public static readonly CodecFLAC FLAC = new CodecFLAC();

        public static readonly CodecLPCM LPCM = new CodecLPCM();

        public static readonly CodecUnknownAudio UnknownAudio = new CodecUnknownAudio();

        #endregion

        #region Subtitle

        public static readonly CodecPGS PGS = new CodecPGS();
        public static readonly CodecVobSub VobSub = new CodecVobSub();
        public static readonly CodecSRT SRT = new CodecSRT();
        public static readonly CodecIGS IGS = new CodecIGS();

        public static readonly CodecUnknownSubtitle UnknownSubtitle = new CodecUnknownSubtitle();

        #endregion

        #region Unknown

        public static readonly UnknownCodec UnknownCodec = new UnknownCodec();

        #endregion

        #endregion

        #region Codec Lists

        public static readonly List<VideoCodec> VideoCodecs =
            new List<VideoCodec>
                {
                    AVC,
                    VC1,
                    MPEG1Video,
                    MPEG2Video
                };

        public static readonly List<AudioCodec> AudioCodecs =
            new List<AudioCodec>
                {
                    // LPCM
                    LPCM,
                    // DTS
                    DTSHDMA,
                    DTSHDHRA,
                    DTSES,
                    DTS,
                    DTSExpress,
                    // Dolby
                    TrueHD,
                    EAC3,
                    AC3EX,
                    AC3,
                    ProLogic,
                    // MPEG
                    AAC,
                    MP3,
                    // sc
                    FLAC,
                    Vorbis
                };

        public static readonly List<Codec> MuxableBDCodecs =
            new List<Codec>
                {
                    // Video
                    MPEG1Video,
                    MPEG2Video,
                    AVC,
                    VC1,

                    // Audio
                    LPCM,
                    DTSHDMA,
                    DTSHDHRA,
                    DTSES,
                    DTS,
                    TrueHD,
                    EAC3,
                    AC3EX,
                    AC3,
                    ProLogic,

                    // Subtitles
                    PGS
                };

        public static readonly List<AudioCodec> MuxableBDAudioCodecs =
            new List<AudioCodec>
                {
                    // Lossless - All
                    LPCM,
                    DTSHDMA,
                    TrueHD,
                    // Lossy - DTS
                    DTSHDHRA,
                    DTS,
                    // Lossy - Dolby
                    EAC3,
                    AC3
                };

        public static readonly List<SubtitleCodec> SubtitleCodecs =
            new List<SubtitleCodec>
                {
                    PGS,
                    VobSub,
                    SRT,
                    IGS
                };

        public static readonly List<Codec> AllCodecs =
            new List<Codec>
                {
                    AVC,
                    VC1,
                    MPEG1Video,
                    MPEG2Video,
                    ProLogic,
                    AC3,
                    AC3EX,
                    EAC3,
                    TrueHD,
                    DTS,
                    DTSES,
                    DTSExpress,
                    DTSHDHRA,
                    DTSHDMA,
                    MP3,
                    AAC,
                    Vorbis,
                    FLAC,
                    LPCM,
                    PGS,
                    IGS
                };

        #endregion

        public abstract bool IsAudio { get; }
        public abstract bool IsVideo { get; }
        public abstract bool IsSubtitle { get; }

        public virtual bool IsKnown { get { return true; } }

        public bool IsOfficialBlurayCodec { get { return IsRequiredBlurayCodec || IsOptionalBlurayCodec; } }
        public virtual bool IsRequiredBlurayCodec { get { return false; } }
        public virtual bool IsOptionalBlurayCodec { get { return false; } }

        public bool IsOfficialDVDCodec { get { return IsRequiredDVDCodec || IsOptionalDVDCodec; } }
        public virtual bool IsRequiredDVDCodec { get { return false; } }
        public virtual bool IsOptionalDVDCodec { get { return false; } }

        /// <summary>
        /// Serializable codec ID in all uppercase (e.g., "V_H264")
        /// </summary>
        public abstract string SerializableName { get; }

        /// <summary>
        /// Shown in MediaInfo.  Also stored in MKV headers.
        /// </summary>
        /// <example>A_AC3</example>
        /// <example>A_DTS</example>
        /// <example>V_MPEG4/ISO/AVC</example>
        public abstract string CodecId { get; }

        public abstract string FullName { get; }
        public abstract string ShortName { get; }
        public abstract string MicroName { get; }

        public virtual string AltFullName { get { return null; } }
        public virtual string AltShortName { get { return null; } }
        public virtual string AltMicroName { get { return null; } }

        public abstract string Description { get; }

        public virtual string CommonName { get { return ShortName; } }

        public virtual string DisplayName { get { return ShortName; } }

        public abstract string FileName { get; }

        public virtual IEnumerable<string> AltDisplayNames
        {
            get
            {
                return new List<string>(new[] { AltFullName, AltShortName }.Where(s => s != null));
            }
        }

        /// <summary>
        /// Can this codec be muxed by standard, freely available consumer software?
        /// </summary>
        public virtual bool IsMuxable { get { return true; } }

        /// <summary>
        /// The full name of the codec plus any alternate names to avoid confusion (e.g., "Dolby Digital (AC-3)").
        /// </summary>
        public virtual string FullNameDisambig { get { return FullName; } }

        /// <summary>
        /// "Audio", "Video", "Subtitle", or "Unknown"
        /// </summary>
        public abstract string TypeDisplay { get; }

//        public abstract Image Logo { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(CodecId);
            sb.AppendFormat(" - {0}", FullName);
            if (!string.IsNullOrWhiteSpace(AltFullName))
                sb.AppendFormat(" ({0})", AltFullName);
            if (!IsMuxable)
                sb.Append(" (NOT muxable)");
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return SerializableName.GetHashCode();
        }

        #region Serializing / Deserializing

        public static Codec FromSerializableName(string serializedName)
        {
            serializedName = serializedName.ToUpperInvariant();
            return AllCodecs.FirstOrDefault(codec => codec.SerializableName == serializedName) ?? UnknownCodec;
        }

        public static string SerializeCodecs<T>(ICollection<T> codecs) where T : Codec
        {
            return string.Join(";", new HashSet<T>(codecs).Select(codec => codec.SerializableName));
        }

        public static ISet<T> DeserializeCodecs<T>(string codecs) where T : Codec
        {
            var codecNames = (codecs ?? "").Trim().Split(';').Where(serializedName => !string.IsNullOrWhiteSpace(serializedName));
            return new HashSet<T>(codecNames.Select(DeserializeCodec<T>));
        }

        public static T DeserializeCodec<T>(string serializableName) where T : Codec
        {
            serializableName = (serializableName ?? "").Trim().ToUpperInvariant();
            var matchingCodec = AllCodecs.FirstOrDefault(codec => codec.SerializableName == serializableName);
            return (matchingCodec ?? UnknownCodec) as T;
        }

        #endregion
    }

    public abstract class AudioCodec : Codec
    {
        public override bool IsAudio
        {
            get { return true; }
        }
        public override bool IsVideo
        {
            get { return false; }
        }
        public override bool IsSubtitle
        {
            get { return false; }
        }

        public override string TypeDisplay
        {
            get { return "Audio"; }
        }

        public virtual AudioCodec Core { get { return null; } }

        public bool Lossy { get { return !Lossless; } }
        public abstract bool Lossless { get; }
    }

    public abstract class VideoCodec : Codec
    {
        public override bool IsAudio
        {
            get { return false; }
        }
        public override bool IsVideo
        {
            get { return true; }
        }
        public override bool IsSubtitle
        {
            get { return false; }
        }
        public override string TypeDisplay
        {
            get { return "Video"; }
        }
    }

    public abstract class SubtitleCodec : Codec
    {
        public override bool IsAudio
        {
            get { return false; }
        }
        public override bool IsVideo
        {
            get { return false; }
        }
        public override bool IsSubtitle
        {
            get { return true; }
        }
        public override string TypeDisplay
        {
            get { return "Subtitle"; }
        }
    }

    #endregion

    #region Video Codecs

    /// <summary>
    /// H.264/MPEG-4 AVC
    /// </summary>
    public class CodecAVC : VideoCodec
    {
        public override string SerializableName
        {
            get { return "V_H264"; }
        }

        public override string CodecId
        {
            get { return "V_MPEG4/ISO/AVC"; }
        }

        public override string FullName
        {
            get { return "H.264/MPEG-4 AVC"; }
        }

        public override string ShortName
        {
            get { return "H.264/AVC"; }
        }

        public override string MicroName
        {
            get { return "H.264"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-4 Part 10"; }
        }

        public override string AltMicroName
        {
            get { return "AVC"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override string DisplayName
        {
            get { return MicroName; }
        }

        public override string FileName
        {
            get { return "H.264"; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { AltMicroName, ShortName, AltFullName }; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "The de facto standard for high quality HD video at reasonable file sizes.  Enjoys wide player support and is HandBrake's default video codec."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_h264; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "AVC" || format.CodecID == "V_MPEG4/ISO/AVC";
//        }
    }

    /// <summary>
    /// VC-1
    /// </summary>
    public class CodecVC1 : VideoCodec
    {
        public override string SerializableName
        {
            get { return "V_VC1"; }
        }

        public override string CodecId
        {
            // TODO: Or is it "V_MS/VFW/WVC1"?
            get { return "V_VC1"; }
        }

        public override string FullName
        {
            get { return "VC-1"; }
        }

        public override string ShortName
        {
            get { return "VC-1"; }
        }

        public override string MicroName
        {
            get { return "VC-1"; }
        }

        public override string FileName
        {
            get { return "VC-1"; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "crosoft's Blu-ray video codec.  Much less common than H.264/AVC."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_vc1; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "VC-1";
//        }
    }

    /// <summary>
    /// MPEG-1 Video (non-Blu-ray)
    /// </summary>
    public class CodecMPEG1Video : VideoCodec
    {
        public override string SerializableName
        {
            get { return "V_MPEG1"; }
        }

        public override string CodecId
        {
            get { return "V_MPEG1"; }
        }

        public override string FullName
        {
            get { return "MPEG-1 Video"; }
        }

        public override string ShortName
        {
            get { return "MPEG-1 Video"; }
        }

        public override string MicroName
        {
            get { return "M1V"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-1 Part 2"; }
        }

        public override string CommonName
        {
            get { return "MPEG"; }
        }

        public override string FileName
        {
            get { return "MPEG-1"; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Video portion of the MPEG-1 standard.  Most commonly found on DVDs and standalone video files (e.g., .mpg, .mpeg)."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_mpeg1_video; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return (format.Id == "MPEG Video" && format.Version == "Version 1") || (format.CodecID == "V_MPEG1");
//        }
    }

    /// <summary>
    /// MPEG-2 Video
    /// </summary>
    public class CodecMPEG2Video : VideoCodec
    {
        public override string SerializableName
        {
            get { return "V_MPEG2"; }
        }

        public override string CodecId
        {
            get { return "V_MPEG2"; }
        }

        public override string FullName
        {
            get { return "H.262/MPEG-2 Part 2"; }
        }

        public override string ShortName
        {
            get { return "MPEG-2 Video"; }
        }

        public override string MicroName
        {
            get { return "M2V"; }
        }

        public override string AltShortName
        {
            get { return "MPEG-2 Part 2"; }
        }

        public override string AltMicroName
        {
            get { return "H.262"; }
        }

        public override string FileName
        {
            get { return "MPEG-2"; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { AltMicroName, AltShortName }; }
        }

        public override string Description
        {
            get { return "Video portion of the MPEG-2 standard."; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_mpeg2_video; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return (format.Id == "MPEG Video" && format.Version == "Version 2") || (format.CodecID == "V_MPEG2");
//        }
    }

    public class CodecUnknownVideo : SubtitleCodec
    {
        public override string SerializableName
        {
            get { return "V_UNKNOWN"; }
        }

        public override string CodecId
        {
            get { return "V_UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Video Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown Video"; }
        }

        public override string MicroName
        {
            get { return "Unknown Video"; }
        }

        public override string FileName
        {
            get { return "UNKNOWN"; }
        }

        public override string Description
        {
            get { return "Unknown video format."; }
        }

//        public override Image Logo
//        {
//            get { return null; }
//        }

        public override bool IsKnown
        {
            get { return false; }
        }
    }

    #endregion

    #region Audio Codecs

    #region Dolby

    /// <summary>
    /// Dolby Pro Logic
    /// </summary>
    public class CodecProLogic : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3_PL"; }
        }

        public override string CodecId
        {
            get { return "A_AC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Pro Logic"; }
        }

        public override string ShortName
        {
            get { return "Pro Logic"; }
        }

        public override string MicroName
        {
            get { return "PL"; }
        }

        /// <summary>
        /// "Dolby Pro Logic" is actually the name of the Decoder; "Dolby Surround" is the name of the Encoder.
        /// </summary>
        public override string AltFullName
        {
            get { return "Dolby Surround"; }
        }

        public override string FileName
        {
            get { return "Pro Logic"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override string Description
        {
            get { return "Dolby Stereo + 2 matrixed channels (front center and rear center), resulting in 4.0 channel output.  Backwards compatible with older stereo systems."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dolby_pro_logic; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "AC-3" && format.Profile == "Dolby Digital";
//        }
    }

    /// <summary>
    /// Dolby Digital
    /// </summary>
    public class CodecAC3 : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3"; }
        }

        public override string CodecId
        {
            get { return "A_AC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Digital"; }
        }

        public override string ShortName
        {
            get { return "AC-3"; }
        }

        public override string MicroName
        {
            get { return "AC3"; }
        }

        public override string AltMicroName
        {
            get { return "DD"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1})", base.FullNameDisambig, ShortName); }
        }

        public override string DisplayName
        {
            get { return FullName; }
        }

        public override string FileName
        {
            get { return "AC3"; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { MicroName }; }
        }

        public override string Description
        {
            get { return "Standard Dolby Digital.  One of the most common audio codecs for consumer video (Blu-ray, DVD, and TV)."; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dolby_digital; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "AC-3" && string.IsNullOrEmpty(format.Profile);
//        }
    }

    /// <summary>
    /// Dolby Digital EX
    /// </summary>
    public class CodecAC3EX : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3_EX"; }
        }

        public override string CodecId
        {
            get { return "A_AC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Digital EX"; }
        }

        public override string ShortName
        {
            get { return "AC-3 EX"; }
        }

        public override string MicroName
        {
            get { return "AC3 EX"; }
        }

        public override string AltMicroName
        {
            get { return "DD EX"; }
        }

        public override string CommonName
        {
            get { return FullName; }
        }

        public override string DisplayName
        {
            get { return FullName; }
        }

        public override string FileName
        {
            get { return "AC3-EX"; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { MicroName }; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override AudioCodec Core
        {
            get { return AC3; }
        }

        public override string Description
        {
            get { return "Extension of AC-3 (Dolby Digital) that adds 1 or 2 matrixed rear channels, creating 6.1 or 7.1 channel output.  Backwards compatible with regular AC-3."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dolby_digital_ex; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "AC-3" && format.Profile == "MA";
//        }
    }

    /// <summary>
    /// Dolby Digital Plus
    /// </summary>
    public class CodecEAC3 : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3_PLUS"; }
        }

        public override string CodecId
        {
            get { return "A_EAC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Digital Plus"; }
        }

        public override string ShortName
        {
            get { return "E-AC-3"; }
        }

        public override string MicroName
        {
            get { return "EAC3"; }
        }

        public override string AltMicroName
        {
            get { return "DD+"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1} / {2})", base.FullNameDisambig, AltMicroName, ShortName); }
        }

        public override string DisplayName
        {
            get { return FullName; }
        }

        public override string FileName
        {
            get { return "EAC3"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Enhanced version of AC-3.  Not backwards compatible with regular AC-3.  Typically used for secondary audio (commentary) tracks on Blu-ray discs."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dolby_digital_plus; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "E-AC-3";
//        }
    }

    /// <summary>
    /// Dolby TrueHD
    /// </summary>
    public class CodecTrueHD : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_TRUEHD"; }
        }

        public override string CodecId
        {
            get { return "A_TRUEHD"; }
        }

        public override string FullName
        {
            get { return "Dolby TrueHD"; }
        }

        public override string ShortName
        {
            get { return "TrueHD"; }
        }

        public override string MicroName
        {
            get { return "TrueHD"; }
        }

        public override string DisplayName
        {
            get { return FullName; }
        }

        public override string FileName
        {
            get { return "TrueHD"; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override AudioCodec Core
        {
            get { return AC3; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Lossless audio encoding with a core AC-3 (Dolby Digital) stream for backwards compatibility with existing AC-3 hardware."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dolby_truehd; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id.StartsWith("TrueHD");
//        }
    }

    #endregion

    #region DTS

    /// <summary>
    /// Standard DTS
    /// </summary>
    public class CodecDTS : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS"; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS Digital Surround"; }
        }

        public override string ShortName
        {
            get { return "DTS"; }
        }

        public override string MicroName
        {
            get { return "DTS"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1})", base.FullNameDisambig, ShortName); }
        }

        public override string FileName
        {
            get { return "DTS"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsOptionalDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Standard DTS.  One of the most common audio codecs for consumer video (Blu-ray and DVD)."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dts; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "DTS" && string.IsNullOrEmpty(format.Profile);
//        }
    }

    /// <summary>
    /// DTS Extended Surround
    /// </summary>
    public class CodecDTSES : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_ES"; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS Extended Surround"; }
        }

        public override string ShortName
        {
            get { return "DTS-ES"; }
        }

        public override string MicroName
        {
            get { return "DTS-ES"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1})", base.FullNameDisambig, ShortName); }
        }

        public override string FileName
        {
            get { return "DTS-ES"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalDVDCodec
        {
            get { return true; }
        }

        public override AudioCodec Core
        {
            get { return DTS; }
        }

        public override string Description
        {
            get { return "Regular DTS Digital Surround plus an additional discrete or matrix-encoded rear channel.  Backwards compatible with regular DTS."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dts_es; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "DTS" && format.Profile == "ES";
//        }
    }

    /// <summary>
    /// DTS Express
    /// </summary>
    public class CodecDTSExpress : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_EXPRESS"; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS Express"; }
        }

        public override string ShortName
        {
            get { return "DTS Express"; }
        }

        public override string MicroName
        {
            get { return "DTS Express"; }
        }

        public override string FileName
        {
            get { return "DTS-Express"; }
        }

        public override string Description
        {
            get { return "Low bit-rate audio codec used for Blu-ray secondary audio and BD Live.  ght not be muxable with current freely available software."; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override bool IsMuxable
        {
            get { return false; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dts_express; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "DTS" && format.Profile == "Express";
//        }
    }

    /// <summary>
    /// DTS-HD High Resolution Audio
    /// </summary>
    public class CodecDTSHDHRA : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_HD_HRA"; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS-HD High Resolution Audio"; }
        }

        public override string ShortName
        {
            get { return "DTS-HD Hi-Res"; }
        }

        public override string MicroName
        {
            get { return "DTS-HD HR"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override string FileName
        {
            get { return "DTS-HD HR"; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { ShortName }; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override AudioCodec Core
        {
            get { return DTS; }
        }

        public override string Description
        {
            get { return "Extension of regular DTS Digital Surround with higher quality.  Contains backwards compatible DTS Digital Surround core."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dts_hd_hra; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "DTS" && format.Profile == "HRA / Core";
//        }
    }

    /// <summary>
    /// DTS-HD Master Audio
    /// </summary>
    public class CodecDTSHDMA : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_HD_MA"; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS-HD Master Audio"; }
        }

        public override string ShortName
        {
            get { return "DTS-HD Master"; }
        }

        public override string MicroName
        {
            get { return "DTS-HD MA"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override string DisplayName
        {
            get { return FullName; }
        }

        public override string FileName
        {
            get { return "DTS-HD MA"; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override AudioCodec Core
        {
            get { return DTS; }
        }

        public override string Description
        {
            get { return "Lossless extension to regular DTS Digital Surround.  Contains backwards compatible DTS Digital Surround core."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_dts_hd_ma; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "DTS" && format.Profile == "MA / Core";
//        }
    }

    #endregion

    #region Misc Audio

    public class CodecVorbis : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_VORBIS"; }
        }

        public override string CodecId
        {
            get { return "A_VORBIS"; }
        }

        public override string FullName
        {
            get { return "Ogg Vorbis"; }
        }

        public override string ShortName
        {
            get { return "Vorbis"; }
        }

        public override string MicroName
        {
            get { return "OGA"; }
        }

        public override string FileName
        {
            get { return "Vorbis"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override string Description
        {
            get { return "Free / open source audio codec."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_vorbis; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "Vorbis";
//        }
    }

    public class CodecFLAC : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_FLAC"; }
        }

        public override string CodecId
        {
            get { return "A_FLAC"; }
        }

        public override string FullName
        {
            get { return "Free Lossless Audio Codec"; }
        }

        public override string ShortName
        {
            get { return "FLAC"; }
        }

        public override string MicroName
        {
            get { return "FLAC"; }
        }

        public override string FileName
        {
            get { return "FLAC"; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Open audio format with royalty-free licensing and a free software reference implementation."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_flac; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "FLAC";
//        }
    }

    #endregion

    #region MPEG Audio

    // TODO: Investigate "MPEG-1 Audio Layer I", "MPEG-1 Audio Layer II", and "MPEG-2 Audio Layer II (MP2)"

    public class CodecMP3 : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_MP3"; }
        }

        public override string CodecId
        {
            get { return "A_MPEG/L3"; }
        }

        public override string FullName
        {
            get { return "MPEG Audio Layer III"; }
        }

        public override string ShortName
        {
            get { return "MPEG Layer 3"; }
        }

        public override string MicroName
        {
            get { return "MP3"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-1 Part 3"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override string FileName
        {
            get { return "MP3"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override string Description
        {
            get { return "Audio portion of the MPEG-1 and MPEG-2 standards."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_mp3; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return (format.Id == "MPEG Audio" && format.Profile == "Layer 3") || (format.CodecID == "A_MPEG/L3");
//        }
    }

    public class CodecAAC : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AAC"; }
        }

        public override string CodecId
        {
            get { return "A_AAC"; }
        }

        public override string FullName
        {
            get { return "Advanced Audio Coding"; }
        }

        public override string ShortName
        {
            get { return "AAC"; }
        }

        public override string MicroName
        {
            get { return "AAC"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-2 Part 7"; }
        }

        public override string FileName
        {
            get { return "AAC"; }
        }

        public override string Description
        {
            get { return "Successor to MP3 with better sound quality at similar bit rates."; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_aac; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "AAC";
//        }
    }

    #endregion

    #region LPCM

    public class CodecLPCM : AudioCodec
    {
        public override string SerializableName
        {
            get { return "A_LPCM"; }
        }

        public override string CodecId
        {
            // TODO: throw new NotImplementedException("Could be either A_PCM/INT/LIT, A_PCM/INT/BIG, or A_PCM/FLOAT/IEEE");
            get { return "A_PCM"; }
        }

        public override string FullName
        {
            get { return "LPCM"; }
        }

        public override string ShortName
        {
            get { return "LPCM"; }
        }

        public override string MicroName
        {
            get { return "PCM"; }
        }

        public override string AltFullName
        {
            get { return "Linear pulse-code modulation"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} (uncompressed)", base.FullNameDisambig); }
        }

        public override string FileName
        {
            get { return "LPCM"; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Uncompressed studio-quality audio.  Not directly supported as input from M2TS containers by mkvmerge; must first be demuxed to .WAV files with eac3to or tsMuxeR (see https://trac.bunkus.org/ticket/763)."; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_lpcm; }
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "PCM";
//        }
    }

    #endregion

    #region Unknown

    public class CodecUnknownAudio : SubtitleCodec
    {
        public override string SerializableName
        {
            get { return "A_UNKNOWN"; }
        }

        public override string CodecId
        {
            get { return "A_UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Audio Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown Audio"; }
        }

        public override string MicroName
        {
            get { return "Unknown Audio"; }
        }

        public override string FileName
        {
            get { return "UNKNOWN"; }
        }

        public override string Description
        {
            get { return "Unknown audio format."; }
        }

//        public override Image Logo
//        {
//            get { return null; }
//        }

        public override bool IsKnown
        {
            get { return false; }
        }
    }

    #endregion

    #endregion

    #region Subtitle Codecs

    /*
    .sup = S_HDMV/PGS  - Blu-ray
    .idx = S_VOBSUB    - DVD      (.sub = companion file)
    .srt = S_TEXT/UTF8 - Matroska
     */

    public class CodecPGS : SubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_PGS"; }
        }

        public override string CodecId
        {
            get { return "S_HDMV/PGS"; }
        }

        public override string FullName
        {
            get { return "Presentation Graphics Stream"; }
        }

        public override string ShortName
        {
            get { return "PGS"; }
        }

        public override string MicroName
        {
            get { return "PGS"; }
        }

        public override string FileName
        {
            get { return "PGS"; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "The official subtitle format for Blu-ray discs.  Stored as bitmap images rather than plain text to reduce strain on player hardware.  Can be converted to plain text with OCR software (e.g., BDSup2Sub)."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_pgs; }
//        }
//
//        public static bool Matches(string filename)
//        {
//            return new Regex(@"(^|\.)sup$", RegexOptions.IgnoreCase).IsMatch(filename);
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "PGS";
//        }
    }

    public class CodecVobSub : SubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_VOBSUB"; }
        }

        public override string CodecId
        {
            get { return "S_VOBSUB"; }
        }

        public override string FullName
        {
            get { return "VobSub"; }
        }

        public override string ShortName
        {
            get { return "VobSub"; }
        }

        public override string MicroName
        {
            get { return "VobSub"; }
        }

        public override string FileName
        {
            get { return "VobSub"; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Official DVD subtitle format.  *.sub file = subtitle bitmaps (images); *.idx file = timecodes (plain text)"; }
        }

//        public override Image Logo
//        {
//            get { return null; }
//        }
//
//        public static bool Matches(string filename)
//        {
//            return new Regex(@"(^|\.)(idx|sup)$", RegexOptions.IgnoreCase).IsMatch(filename);
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "VobSub";
//        }
    }

    public class CodecSRT : SubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_SRT"; }
        }

        public override string CodecId
        {
            get { return "S_TEXT/UTF"; }
        }

        public override string FullName
        {
            get { return "SRT"; }
        }

        public override string ShortName
        {
            get { return "SRT"; }
        }

        public override string MicroName
        {
            get { return "SRT"; }
        }

        public override string FileName
        {
            get { return "SRT"; }
        }

        public override string Description
        {
            get { return "Plain text Matroska subtitle format."; }
        }

//        public override Image Logo
//        {
//            get { return null; }
//        }
//
//        public static bool Matches(string filename)
//        {
//            return new Regex(@"(^|\.)(srt)$", RegexOptions.IgnoreCase).IsMatch(filename);
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "UTF-8";
//        }
    }

    public class CodecIGS : SubtitleCodec
    {
        public override bool IsMuxable
        {
            get { return false; }
        }

        public override string SerializableName
        {
            get { return "M_IGS"; }
        }

        public override string CodecId
        {
            get { return "M_HDMV/IGS"; }
        }

        public override string FullName
        {
            get { return "Interactive Graphics Stream"; }
        }

        public override string ShortName
        {
            get { return "IGS"; }
        }

        public override string MicroName
        {
            get { return "IGS"; }
        }

        public override string FileName
        {
            get { return "IGS"; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Pop-up menu displayed during playback on some Blu-ray Discs."; }
        }

//        public override Image Logo
//        {
//            get { return Logos.logo_igs; }
//        }
//
//        public static bool Matches(string filename)
//        {
//            return new Regex(@"(^|\.)sup$", RegexOptions.IgnoreCase).IsMatch(filename);
//        }
//
//        public static bool Matches(Format format)
//        {
//            return format.Id == "PGS";
//        }
    }

    public class CodecUnknownSubtitle : SubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_UNKNOWN"; }
        }

        public override string CodecId
        {
            get { return "S_UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Subtitle Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown Sub"; }
        }

        public override string MicroName
        {
            get { return "Unknown Sub"; }
        }

        public override string FileName
        {
            get { return "UNKNOWN"; }
        }

        public override string Description
        {
            get { return "Unknown subtitle format."; }
        }

//        public override Image Logo
//        {
//            get { return null; }
//        }

        public override bool IsKnown
        {
            get { return false; }
        }
    }

    #endregion

    #region Unknown Codec

    public class UnknownCodec : Codec
    {
        public override bool IsAudio
        {
            get { return false; }
        }

        public override bool IsVideo
        {
            get { return false; }
        }

        public override bool IsSubtitle
        {
            get { return false; }
        }

        public override bool IsKnown
        {
            get { return false; }
        }

        public override string TypeDisplay
        {
            get { return "Unknown"; }
        }

        public override string SerializableName
        {
            get { return "UNKNOWN"; }
        }

        public override string CodecId
        {
            get { return "UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown"; }
        }

        public override string MicroName
        {
            get { return "Unknown"; }
        }

        public override string FileName
        {
            get { return "UNKNOWN"; }
        }

        public override string Description
        {
            get { return "Unknown format."; }
        }

//        public override Image Logo
//        {
//            get { return null; }
//        }
    }

    #endregion

}
