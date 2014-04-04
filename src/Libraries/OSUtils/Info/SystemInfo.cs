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
using System.Runtime.InteropServices;
using DotNetUtils;
using DotNetUtils.Annotations;

namespace OSUtils.Info
{
    public class SystemInfo
    {
        public static readonly SystemInfo Instance = new SystemInfo();

        /// <summary>
        /// Gets information about the operating system.
        /// </summary>
        [UsedImplicitly]
        public readonly OSInfo OS;

        /// <summary>
        /// Gets information about the physical hardware.
        /// </summary>
        [UsedImplicitly]
        public readonly HardwareInfo Hardware;

        /// <summary>
        /// Gets information about the current process.
        /// </summary>
        [UsedImplicitly]
        public readonly ProcessInfo Process;

        /// <summary>
        /// Gets information about the current thread's culture (a.k.a. locale).
        /// </summary>
        [UsedImplicitly]
        public readonly CultureInfos Culture;

        private SystemInfo()
        {
            OS = new OSInfo(GetOSType());
            Hardware = new HardwareInfo();
            Process = new ProcessInfo();
            Culture = new CultureInfos();
        }

        #region Native interop

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

        #endregion

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
}
