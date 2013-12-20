using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils;
using DotNetUtils.Annotations;

namespace OSUtils
{
// ReSharper disable MemberCanBePrivate.Global
    public class SystemInfo
    {
        public static readonly SystemInfo Instance = new SystemInfo();

        /// <summary>
        /// Gets information about the operating system.
        /// </summary>
        [UsedImplicitly]
        public readonly OS OS;

        /// <summary>
        /// Gets the total amount of installed physical memory in bytes.
        /// </summary>
        [UsedImplicitly]
        public ulong TotalPhysicalMemory { get { return GetTotalPhysicalMemory(); } }

        /// <summary>
        /// Gets the amount of available system memory in bytes.
        /// </summary>
        [UsedImplicitly]
        public ulong AvailableMemory { get { return GetAvailableMemory(); } }

        /// <summary>
        /// Gets the width of memory addresses in bits (e.g., 32, 64).
        /// </summary>
        [UsedImplicitly]
        public readonly int MemoryWidth;

        /// <summary>
        /// Gets whether the current process is using 64-bit instructions and memory addresses.
        /// </summary>
        [UsedImplicitly]
        public readonly bool Is64BitProcess;

        /// <summary>
        /// Gets the number of logical processors on the CPU.  On Intel processors with hyperthreading,
        /// this value will be the number of cores multiplied by 2 (e.g., a quad core Intel Core i7
        /// would return 8).
        /// </summary>
        [UsedImplicitly]
        public readonly int ProcessorCount;

        private SystemInfo()
        {
            OS = GetOS();
            MemoryWidth = IntPtr.Size * 8;
            Is64BitProcess = Environment.Is64BitProcess;
            ProcessorCount = Environment.ProcessorCount;
        }

        private static OS GetOS()
        {
            var os = Environment.OSVersion;
            return new OS(GetOSType(), os.Version, os.VersionString, Environment.Is64BitOperatingSystem);
        }

        private static OSType GetOSType()
        {
            var id = Environment.OSVersion.Platform;
            var p = (int)id;
            if (PlatformID.Win32NT == id)
                return OSType.Windows;
            if ((p == 4) || (p == 6) || (p == 128))
                return GetNixOSType();
            return OSType.Other;
        }

        // From Managed.Windows.Forms/XplatUI
        [DllImport("libc")]
        private static extern int uname(IntPtr buf);

        /// <summary>
        /// On Unix-like systems, invokes the <c>uname()</c> function using native interop to detect the operating system type.
        /// </summary>
        /// <returns>The specific type of *Nix OS the application is running on</returns>
        /// <seealso cref="https://github.com/jpobst/Pinta/blob/master/Pinta.Core/Managers/SystemManager.cs"/>
        private static OSType GetNixOSType()
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname()
                if (uname(buf) == 0)
                {
                    var os = Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return OSType.Mac;
                    if (os == "Linux")
                        return OSType.Linux;
                }
            }
            catch
            {
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }
            return OSType.Unix;
        }

        private enum MemPropOSX
        {
            Total, Used, Free
        }

        private static ulong GetMemoryOSX(MemPropOSX prop)
        {
            var propName = prop == MemPropOSX.Free ? "physmem" :
                           prop == MemPropOSX.Used ? "usermem" :
                                                     "memsize";
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
                var output = process.StandardOutput.ReadToEnd();
                var match = new Regex(@"hw\." + propName + @"\s+?=\s+?(?<" + propName + @">\d+)", RegexOptions.Multiline).Match(output);
                return match.Success ? ulong.Parse(match.Groups[propName].Value) : 0;
            }
        }

        private static ulong GetAvailableMemory()
        {
            ulong free = 0;
            try { free = GetAvailableMemoryWin(); if (free > 0) { return free; } } catch {}
            try { free = GetAvailableMemoryOSX(); if (free > 0) { return free; } } catch {}
            return free;
        }

        private static ulong GetAvailableMemoryWin()
        {
            using (var counter = new PerformanceCounter("Memory", "Available Bytes"))
            {
                return (ulong) counter.NextValue();
            }
        }

        private static ulong GetAvailableMemoryOSX()
        {
            return GetTotalPhysicalMemoryOSX() - GetMemoryOSX(MemPropOSX.Used);
        }

        /// <summary>
        /// Gets the total amount of installed physical memory on the system using native Win32 interop,
        /// falling back to a Mono-specific <see cref="PerformanceCounter"/> implementation for *nix OSes.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        /// <see cref="http://stackoverflow.com/a/105109"/>
        private static ulong GetTotalPhysicalMemory()
        {
            ulong total = 0;
            try { total = GetTotalPhysicalMemoryWin(); if (total > 0) { return total; } } catch {}
            try { total = GetTotalPhysicalMemoryNix(); if (total > 0) { return total; } } catch {}
            try { total = GetTotalPhysicalMemoryOSX(); if (total > 0) { return total; } } catch {}
            return total;
        }

        /// <summary>
        /// On Windows systems running .NET, gets the total amount of installed physical memory.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        private static ulong GetTotalPhysicalMemoryWin()
        {
            var memStatus = new MEMORYSTATUSEX();
            return GlobalMemoryStatusEx(memStatus) ? memStatus.ullTotalPhys : 0;
        }

        /// <summary>
        /// On systemes running Mono, gets the total amount of installed physical memory.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        private static ulong GetTotalPhysicalMemoryNix()
        {
            return (ulong) new PerformanceCounter("Mono Memory", "Total Physical Memory").NextValue();
        }

        /// <summary>
        /// On Mac OS X, gets the total amount of installed physical memory.
        /// </summary>
        /// <returns>The total amount of installed physical memory on the system.</returns>
        private static ulong GetTotalPhysicalMemoryOSX()
        {
            return GetMemoryOSX(MemPropOSX.Total);
        }

        /// <summary>
        /// contains information about the current state of both physical and virtual memory, including extended memory
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            /// <summary>
            /// Size of the structure, in bytes. You must set this member before calling GlobalMemoryStatusEx.
            /// </summary>
            public uint dwLength;

            /// <summary>
            /// Number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates no memory use and 100 indicates full memory use).
            /// </summary>
            public uint dwMemoryLoad;

            /// <summary>
            /// Total size of physical memory, in bytes.
            /// </summary>
            public ulong ullTotalPhys;

            /// <summary>
            /// Size of physical memory available, in bytes.
            /// </summary>
            public ulong ullAvailPhys;

            /// <summary>
            /// Size of the committed memory limit, in bytes. This is physical memory plus the size of the page file, minus a small overhead.
            /// </summary>
            public ulong ullTotalPageFile;

            /// <summary>
            /// Size of available memory to commit, in bytes. The limit is ullTotalPageFile.
            /// </summary>
            public ulong ullAvailPageFile;

            /// <summary>
            /// Total size of the user mode portion of the virtual address space of the calling process, in bytes.
            /// </summary>
            public ulong ullTotalVirtual;

            /// <summary>
            /// Size of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling process, in bytes.
            /// </summary>
            public ulong ullAvailVirtual;

            /// <summary>
            /// Size of unreserved and uncommitted memory in the extended portion of the virtual address space of the calling process, in bytes.
            /// </summary>
            public ulong ullAvailExtendedVirtual;

            /// <summary>
            /// Initializes a new instance of the <see cref="MEMORYSTATUSEX"/> class.
            /// </summary>
            public MEMORYSTATUSEX()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
// ReSharper restore MemberCanBePrivate.Global
}
