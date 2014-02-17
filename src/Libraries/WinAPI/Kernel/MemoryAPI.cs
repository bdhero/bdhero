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

namespace WinAPI.Kernel
{
    /// <summary>
    ///     Windows API functions for memory management.
    /// </summary>
    public static class MemoryAPI
    {
        /// <summary>
        ///     Retrieves information about the system's current usage of both physical and virtual memory.
        /// </summary>
        /// <param name="lpBuffer">
        ///     A pointer to a <see cref="MEMORYSTATUSEX"/> structure that receives information about current memory availability.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         You can use the <c>GlobalMemoryStatusEx</c> function to determine how much memory your application
        ///         can allocate without severely impacting other applications.
        ///     </para>
        ///     <para>
        ///         The information returned by the <c>GlobalMemoryStatusEx</c> function is volatile. There is no guarantee
        ///         that two sequential calls to this function will return the same information.
        ///     </para>
        ///     <para>
        ///         The <see cref="MEMORYSTATUSEX.ullAvailPhys"/> member of the <see cref="MEMORYSTATUSEX"/> structure
        ///         at <paramref name="lpBuffer"/> includes memory for all NUMA nodes.
        ///     </para>
        /// </remarks>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
    }
}
