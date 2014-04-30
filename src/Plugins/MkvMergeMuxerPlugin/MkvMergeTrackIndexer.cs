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
using BDHero.BDROM;
using DotNetUtils.Extensions;

namespace BDHero.Plugin.MkvMergeMuxer
{
    class MkvMergeTrackIndexer
    {
        /// <summary>
        /// Map of track PIDs to their TrackIndexes.
        /// </summary>
        private readonly IDictionary<int, MkvMergeTrackIndex> _trackIndices;

        private int _curIndex;

        public MkvMergeTrackIndexer(Playlist playlist)
        {
            _trackIndices = new Dictionary<int, MkvMergeTrackIndex>(playlist.Tracks.Count * 2);
            
            playlist.Tracks.ForEach(VisitTrack);
        }

        private void VisitTrack(Track track)
        {
            var isSupported = IsSupported(track);
            _trackIndices[track.PID] = new MkvMergeTrackIndex
                                       {
                                           InputIndex = track.Index,
                                           OutputIndex = isSupported ? _curIndex++ : -1,
                                           IsSupported = isSupported
                                       };
        }

        private static bool IsSupported(Track track)
        {
            if (!track.Codec.IsKnown)
                return false;

            if (!track.Codec.IsMuxable)
                return false;

            if (track.Codec == Codec.LPCM)
                return false;

            return true;
        }

        public MkvMergeTrackIndex this[Track track]
        {
            get { return _trackIndices[track.PID]; }
        }
    }
}
