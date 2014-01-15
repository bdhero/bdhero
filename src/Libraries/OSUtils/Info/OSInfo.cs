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
using DotNetUtils;
using DotNetUtils.Annotations;

namespace OSUtils.Info
{
    public class OSInfo
    {
        /// <summary>
        /// Gets the high-level operating system type (e.g., Windows, Mac, Linux, Unix).
        /// </summary>
        [UsedImplicitly]
        public readonly OSType Type;

        /// <summary>
        /// Gets the version number of the operating system.
        /// </summary>
        [UsedImplicitly]
        public readonly Version VersionNumber;

        /// <summary>
        /// Gets a human-friendly operating system version string containing the OS's name,
        /// version number, service pack, and any other related information specified by the .NET runtime.
        /// </summary>
        [UsedImplicitly]
        public readonly string VersionString;

        /// <summary>
        /// Gets whether the operating system supports 64-bit instructions and memory addresses.
        /// </summary>
        [UsedImplicitly]
        public readonly bool Is64Bit;

        public OSInfo(OSType type)
        {
            Type = type;
            VersionNumber = Environment.OSVersion.Version;
            VersionString = Environment.OSVersion.VersionString;
            Is64Bit = Environment.Is64BitOperatingSystem;
        }

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
}
