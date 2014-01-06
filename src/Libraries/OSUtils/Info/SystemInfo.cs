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

        private SystemInfo()
        {
            OS = new OSInfo(GetOSType());
            Hardware = new HardwareInfo();
            Process = new ProcessInfo();
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
