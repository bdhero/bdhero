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
