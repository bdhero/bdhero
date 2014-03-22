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

namespace BDHero.BDROM
{
    /// <summary>
    /// Represents the <c>disc.inf</c> file created by AnyDVD HD in the root of the BD-ROM.
    /// </summary>
    public class AnyDVDDiscInf
    {
        /// <summary>
        /// Version string reported by AnyDVD HD.
        /// </summary>
        /// <example><code>AnyDVD HD 7.1.3.0 (BDPHash.bin 12-11-23)</code></example>
        /// <example><code>AnyDVD HD 7.1.7.0 (BDPHash.bin 13-03-04-A)</code></example>
        public string AnyDVDVersion;

        /// <summary>
        /// Volume label of the BD-ROM detected by AnyDVD HD.
        /// May differ from the reported hardware volume label for some discs
        /// (e.g., <c>49123204_BLACK_HAWK_DOWN</c>, <c>09488088_COURAGE_UNDER_FIRE</c>, <c>69791692_BROKEN_ARROW</c>).
        /// </summary>
        /// <example><code>49123204_BLACK_HAWK_DOWN</code></example>
        /// <example><code>DIEANOTHERDAY</code></example>
        /// <example><code>EXPENDABLES_2</code></example>
        /// <example><code>THE_PHANTOM_MENACE</code></example>
        /// <example><code>WEST_SIDE_STORY</code></example>
        public string VolumeLabel;

        /// <summary>
        /// Blu-ray region code.
        /// </summary>
        public RegionCode Region;

        public override string ToString()
        {
            var regionName = Region == RegionCode.Free ? "region-free" : "region " + Region.GetName();
            return string.Format("{0} ({1})", VolumeLabel, regionName);
        }
    }
}
