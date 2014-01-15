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
using System.Diagnostics;
using System.Linq;
using System.Text;
using BDHero.BDROM;

namespace BDHero.Plugin.FFmpegMuxer
{
    /// <summary>
    /// MPEG-2 Transport Streams allow each stream (what BDHero refers to as a "track") to have an embedded inner stream
    /// for backward compatibility with older A/V equipment.
    /// 
    /// On Blu-ray discs, Dolby TrueHD streams contain a separate, independent embedded inner AC-3 (Dolby Digital) stream
    /// for players that don't support TrueHD.  In effect, a Blu-ray TrueHD track is really a FULL TrueHD stream + a separate FULL AC-3 stream.
    /// 
    /// DTS-HD tracks, on the other hand, contain a standard DTS core stream + the difference between the lossy DTS data and the lossless DTS-HD data.
    /// This means DTS-HD doesn't require an embedded stream because older players will simply decode the standard DTS core and ignore the extra HD data,
    /// whereas newer players will see the HD diff and combine it with the standard DTS core to recreate the full lossless DTS-HD recording.
    /// 
    /// FFmpeg counts TrueHD and its embedded AC-3 stream as two completely separate streams/tracks, which means we have to increment our
    /// track indexes whenever we encounter a TrueHD track to make sure our numbers match up with FFmpeg.
    /// </summary>
    internal class FFmpegTrackIndexer
    {
        private readonly Playlist _playlist;

        /// <summary>
        /// Map of track PIDs to their TrackIndexes.
        /// </summary>
        private readonly IDictionary<int, FFmpegTrackIndex> _trackIndices; 

        public FFmpegTrackIndexer(Playlist playlist)
        {
            _playlist = playlist;
            _trackIndices = new Dictionary<int, FFmpegTrackIndex>(playlist.Tracks.Count * 2);

            // Input indexes
            int ii = 0, // absolute index
                vi = 0, // video index
                ai = 0, // audio index
                si = 0; // subtitle index

            // Input tracks in FFmpeg order (by PID ascending)
            foreach (var track in _playlist.Tracks.OrderBy(track => track.PID))
            {
                var inputIndex = ii++;
                var inputIndexOfType = -1;

                if (track.IsVideo) inputIndexOfType = vi++;
                if (track.IsAudio) inputIndexOfType = ai++;
                if (track.IsSubtitle) inputIndexOfType = si++;

                Debug.Assert(inputIndexOfType != -1, "Track must be video, audio, or subtitle");

                _trackIndices[track.PID] = new FFmpegTrackIndex
                    {
                        InputIndex = inputIndex,
                        InputIndexOfType = inputIndexOfType,
                        OutputIndex = -1,
                        OutputIndexOfType = -1
                    };

                // Count TrueHD tracks twice to match FFmpeg's numbering system
                // in which inner embedded AC-3 streams are counted as separate tracks
                if (track.Codec is CodecTrueHD)
                {
                    ii++;
                    ai++;
                }
            }

            // Output indexes
            int io = 0, // absolute index
                vo = 0, // video index
                ao = 0, // audio index
                so = 0; // subtitle index

            // Output tracks in original BDInfo order
            foreach (var track in _playlist.Tracks.Where(track => track.Keep))
            {
                var outputIndex = io++;
                var outputIndexOfType = -1;

                if (track.IsVideo) outputIndexOfType = vo++;
                if (track.IsAudio) outputIndexOfType = ao++;
                if (track.IsSubtitle) outputIndexOfType = so++;

                Debug.Assert(outputIndexOfType != -1, "Track must be video, audio, or subtitle");

                _trackIndices[track.PID].OutputIndex = outputIndex;
                _trackIndices[track.PID].OutputIndexOfType = outputIndexOfType;
            }
        }

        public FFmpegTrackIndex this[Track track]
        {
            get { return _trackIndices[track.PID]; }
        }
    }
}
