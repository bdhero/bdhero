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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.BDROM
{
    /// <summary>
    /// Describes various optional Blu-ray features present on the disc.
    /// </summary>
    public class DiscFeatures
    {
        /// <summary>
        /// Disc is protected with BD+ encryption.
        /// </summary>
        public bool IsBDPlus;

        /// <summary>
        /// Disc contains Java menus.
        /// </summary>
        public bool IsBDJava;

        /// <summary>
        /// Disc contains 3D video.
        /// </summary>
        public bool Is3D;

        /// <summary>
        /// Disc contains D-BOX motion code.
        /// </summary>
        public bool IsDbox;

        /// <summary>
        /// Disc contains a copy-protected "Digital Copy" of the film that users can download
        /// and watch on their computers and portable devices.
        /// </summary>
        public bool IsDCopy;

        /// <summary>
        /// Disc contains a PSP-ready "Digital Copy" of the film that users can download
        /// and play on their PSPs.
        /// </summary>
        public bool IsPSP;
    }
}
