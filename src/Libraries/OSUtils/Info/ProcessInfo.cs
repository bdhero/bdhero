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
    public class ProcessInfo
    {
        /// <summary>
        /// Gets the width of memory addresses in bits (e.g., 32, 64).
        /// </summary>
        [UsedImplicitly]
        public readonly int MemoryWidth;

        /// <summary>
        /// Gets whether the current process is using 64-bit instructions and memory addresses.
        /// </summary>
        [UsedImplicitly]
        public readonly bool Is64Bit;

        public ProcessInfo()
        {
            MemoryWidth = IntPtr.Size * 8;
            Is64Bit = Environment.Is64BitProcess;
        }

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
}