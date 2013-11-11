using BDHero.BDROM;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Transformer
{
    /// <summary>
    /// Translates between BDInfo and MediaInfo data types and codecs.
    /// </summary>
    static class CodecTransformer
    {
        public static Codec CodecFromStream(TSStream stream)
        {
            if (stream == null) return Codec.UnknownCodec;

            var audioStream = stream as TSAudioStream;

            switch (stream.StreamType)
            {
                case TSStreamType.MPEG1_VIDEO:
                    return Codec.MPEG1Video;
                case TSStreamType.MPEG2_VIDEO:
                    return Codec.MPEG2Video;
                case TSStreamType.AVC_VIDEO:
                    return Codec.AVC;
                case TSStreamType.MVC_VIDEO:
                    return Codec.UnknownVideo;
                case TSStreamType.VC1_VIDEO:
                    return Codec.VC1;
                case TSStreamType.MPEG1_AUDIO:
                    return Codec.MP3;
                case TSStreamType.MPEG2_AUDIO:
                    return Codec.UnknownAudio;
                case TSStreamType.LPCM_AUDIO:
                    return Codec.LPCM;
                case TSStreamType.AC3_AUDIO:
                    if (audioStream != null && audioStream.AudioMode == TSAudioMode.Extended)
                        return Codec.AC3EX;
                    if (audioStream != null && audioStream.AudioMode == TSAudioMode.Surround)
                        return Codec.ProLogic;
                    return Codec.AC3;
                case TSStreamType.AC3_PLUS_AUDIO:
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                    return Codec.EAC3;
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                    return Codec.TrueHD;
                case TSStreamType.DTS_AUDIO:
                    if (audioStream != null && audioStream.AudioMode == TSAudioMode.Extended)
                        return Codec.DTSES;
                    return Codec.DTS;
                case TSStreamType.DTS_HD_AUDIO:
                    return Codec.DTSHDHRA;
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                    return Codec.DTSExpress;
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                    return Codec.DTSHDMA;
                case TSStreamType.PRESENTATION_GRAPHICS:
                    return Codec.PGS;
                case TSStreamType.INTERACTIVE_GRAPHICS:
                    return Codec.IGS;
                case TSStreamType.SUBTITLE:
                    return Codec.UnknownSubtitle;
                default:
                    return Codec.UnknownCodec;
            }
        }
    }
}
