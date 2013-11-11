using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class TrackTransformer
    {
        public static IList<Track> Transform(IEnumerable<TSStream> streams)
        {
            int numVideo = 0,
                numAudio = 0,
                numSubtitle = 0;
            // TODO: Add angle support!
            return streams.Where(IsDefaultAngle)
                          .Select((stream, i) => Transform(stream, i, ref numVideo, ref numAudio, ref numSubtitle))
                          .ToList();
        }

        private static bool IsDefaultAngle(TSStream tsStream)
        {
            return tsStream.AngleIndex == 0;
        }

        private static Track Transform(TSStream stream, int index, ref int numVideo, ref int numAudio, ref int numSubtitle)
        {
            var videoStream = stream as TSVideoStream;
            var audioStream = stream as TSAudioStream;
            var subtitleStream = stream as TSGraphicsStream;

            var indexOfType = 0;

            if (videoStream != null) indexOfType = numVideo++;
            if (audioStream != null) indexOfType = numAudio++;
            if (subtitleStream != null) indexOfType = numSubtitle++;

            return new Track
                {
                    Index = index,
                    PID = stream.PID,
                    Language = stream.Language,
                    IsHidden = stream.IsHidden,
                    Codec = CodecTransformer.CodecFromStream(stream),
                    IsVideo = stream.IsVideoStream,
                    IsAudio = stream.IsAudioStream,
                    IsSubtitle = stream.IsGraphicsStream || stream.IsTextStream,
                    ChannelCount = audioStream != null ? audioStream.ChannelCountDouble : 0,
                    BitDepth = audioStream != null ? audioStream.BitDepth : 0,
                    VideoFormat = videoStream != null ? videoStream.VideoFormat : 0,
                    FrameRate = videoStream != null ? videoStream.FrameRate : TSFrameRate.Unknown,
                    AspectRatio = videoStream != null ? videoStream.AspectRatio : TSAspectRatio.Unknown,
                    IndexOfType = indexOfType
                };
        }
    }
}
