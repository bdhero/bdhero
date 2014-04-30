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

namespace BDHero.Plugin.MkvMergeMuxer
{
    class MkvMergeTrackIndex
    {
        /// <summary>
        /// The absolute position (index) of the track in the input MPLS, starting at 0.
        /// </summary>
        public int InputIndex { get; set; }

        /// <summary>
        /// The absolute position (index) of the track in the output MKV, starting at 0.
        /// If <see cref="IsSupported"/> is <c>false</c>, <see cref="OutputIndex"/> will be <c>-1</c>.
        /// </summary>
        public int OutputIndex { get; set; }

        /// <summary>
        /// Gets or sets whether the track's codec is supported by mkvmerge and can be muxed to MKV.
        /// </summary>
        public bool IsSupported { get; set; }
    }
}