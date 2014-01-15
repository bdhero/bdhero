﻿// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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

namespace OSUtils
{
    public enum OSType
    {
        /// <summary>
        /// Any version of Windows supported by .NET 4.0, from Windows XP to Windows 8.1 and beyond.
        /// </summary>
        Windows,

        /// <summary>
        /// Any version of Mac OS (a.k.a. Darwin) supported by Mono 3.2.
        /// </summary>
        Mac,

        /// <summary>
        /// Any version of Linux supported by Mono 3.2.
        /// </summary>
        Linux,

        /// <summary>
        /// Any version of UNIX (other than Linux or Mac) supported by Mono 3.2.
        /// </summary>
        Unix,

        /// <summary>
        /// Any other operating system not specified in this enum.
        /// </summary>
        Other
    }
}