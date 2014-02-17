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
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Attributes;
using DotNetUtils.Extensions;
using WinAPI.Kernel;

namespace OSUtils.Info
{
    public class HardwareInfo
    {
        /// <summary>
        /// Gets the number of logical processors on the CPU.  On Intel processors with hyperthreading,
        /// this value will be the number of cores multiplied by 2 (e.g., a quad core Intel Core i7
        /// would return 8).
        /// </summary>
        [UsedImplicitly]
        public readonly int ProcessorCount;

        /// <summary>
        /// Gets the total amount of installed physical memory in bytes.
        /// </summary>
        [FileSize]
        [UsedImplicitly]
        public readonly ulong TotalPhysicalMemory;

        /// <summary>
        /// Gets the amount of available system memory in bytes.
        /// </summary>
        [FileSize]
        [UsedImplicitly]
        public ulong AvailableMemory { get { return GetAvailableMemory(); } }

        public HardwareInfo()
        {
            ProcessorCount = Environment.ProcessorCount;
            TotalPhysicalMemory = GetTotalPhysicalMemory();
        }

        #region Native interop

        #region Available memory

        private static ulong GetAvailableMemory()
        {
            ulong free = 0;
            try { free = GetAvailableMemoryWin(); if (free > 0) { return free; } }
            catch { }
            try { free = GetAvailableMemoryOSX(); if (free > 0) { return free; } }
            catch { }
            return free;
        }

        private static ulong GetAvailableMemoryWin()
        {
            using (var counter = new PerformanceCounter("Memory", "Available Bytes"))
            {
                return (ulong)counter.NextValue();
            }
        }

        private static ulong GetAvailableMemoryOSX()
        {
            return GetTotalPhysicalMemoryOSX() - GetMemoryOSX(MemPropOSX.Used);
        }

        #endregion

        #region Total physical memory

        /// <summary>
        /// Gets the total amount of installed physical memory on the system using native Win32 interop,
        /// falling back to a Mono-specific <see cref="PerformanceCounter"/> implementation for *nix OSes.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        /// <see cref="http://stackoverflow.com/a/105109"/>
        private static ulong GetTotalPhysicalMemory()
        {
            ulong total = 0;
            try { total = GetTotalPhysicalMemoryWin(); if (total > 0) { return total; } }
            catch { }
            try { total = GetTotalPhysicalMemoryNix(); if (total > 0) { return total; } }
            catch { }
            try { total = GetTotalPhysicalMemoryOSX(); if (total > 0) { return total; } }
            catch { }
            return total;
        }

        /// <summary>
        /// On Windows systems running .NET, gets the total amount of installed physical memory.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        private static ulong GetTotalPhysicalMemoryWin()
        {
            var memStatus = new MEMORYSTATUSEX();
            return MemoryAPI.GlobalMemoryStatusEx(memStatus) ? memStatus.ullTotalPhys : 0;
        }

        /// <summary>
        /// On systemes running Mono, gets the total amount of installed physical memory.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        private static ulong GetTotalPhysicalMemoryNix()
        {
            return (ulong)new PerformanceCounter("Mono Memory", "Total Physical Memory").NextValue();
        }

        /// <summary>
        /// On Mac OS X, gets the total amount of installed physical memory.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        private static ulong GetTotalPhysicalMemoryOSX()
        {
            return GetMemoryOSX(MemPropOSX.Total);
        }

        #endregion

        #region Mac-specific memory

        private static ulong GetMemoryOSX(MemPropOSX prop)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo("sysctl", "-a")
                                    {
                                        UseShellExecute = false,
                                        RedirectStandardOutput = true,
                                        RedirectStandardError = true,
                                        CreateNoWindow = true,
                                        WindowStyle = ProcessWindowStyle.Hidden,
                                    };
                process.Start();
                var propName = prop.GetAttributeProperty<DescriptionAttribute, string>(attribute => attribute.Description);
                var output = process.StandardOutput.ReadToEnd();
                var match = new Regex(@"hw\." + propName + @"\s+?=\s+?(?<" + propName + @">\d+)", RegexOptions.Multiline).Match(output);
                return match.Success ? UInt64.Parse(match.Groups[propName].Value) : 0;
            }
        }

        #endregion

        #region Structs and enums

        private enum MemPropOSX
        {
            [Description("memsize")]
            Total,

            [Description("usermem")]
            Used,

            [Description("physmem")]
            Free
        }

        #endregion

        #endregion

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
}