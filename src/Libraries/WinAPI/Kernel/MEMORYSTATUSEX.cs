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

using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace WinAPI.Kernel
{
    /// <summary>
    ///     Contains information about the current state of both physical and virtual memory, including extended memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MEMORYSTATUSEX
    {
        /// <summary>
        ///     Size of the structure, in bytes. You must set this member before calling GlobalMemoryStatusEx.
        /// </summary>
        public uint dwLength;

        /// <summary>
        ///     Number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates
        ///     no memory use and 100 indicates full memory use).
        /// </summary>
        public uint dwMemoryLoad;

        /// <summary>
        ///     Total size of physical memory, in bytes.
        /// </summary>
        public ulong ullTotalPhys;

        /// <summary>
        ///     Size of physical memory available, in bytes.
        /// </summary>
        public ulong ullAvailPhys;

        /// <summary>
        ///     Size of the committed memory limit, in bytes. This is physical memory plus the size of the page file, minus a small
        ///     overhead.
        /// </summary>
        public ulong ullTotalPageFile;

        /// <summary>
        ///     Size of available memory to commit, in bytes. The limit is ullTotalPageFile.
        /// </summary>
        public ulong ullAvailPageFile;

        /// <summary>
        ///     Total size of the user mode portion of the virtual address space of the calling process, in bytes.
        /// </summary>
        public ulong ullTotalVirtual;

        /// <summary>
        ///     Size of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling
        ///     process, in bytes.
        /// </summary>
        public ulong ullAvailVirtual;

        /// <summary>
        ///     Size of unreserved and uncommitted memory in the extended portion of the virtual address space of the calling
        ///     process, in bytes.
        /// </summary>
        public ulong ullAvailExtendedVirtual;

        /// <summary>
        ///     Initializes a new instance of the <c>MEMORYSTATUSEX</c> class.
        /// </summary>
        public MEMORYSTATUSEX()
        {
            dwLength = (uint) Marshal.SizeOf(typeof (MEMORYSTATUSEX));
        }
    }
}
