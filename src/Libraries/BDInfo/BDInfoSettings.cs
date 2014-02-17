// Copyright 2012-2013 Andrew C. Dvorak
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

namespace BDInfo
{
    /// <summary>
    /// Constants (ported over from BDHeroSettings)
    /// </summary>
    static class BDInfoSettings
    {
        /// <summary>
        /// Enable 3D Blu-ray support (incomplete and untested).
        /// </summary>
        public const bool EnableSSIF = false;
        public const bool KeepStreamOrder = true;
        public const bool FilterShortPlaylists = true;
        public const int FilterShortPlaylistsValue = 20;
        public const bool FilterLoopingPlaylists = true;
    }
}
