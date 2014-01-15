// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
