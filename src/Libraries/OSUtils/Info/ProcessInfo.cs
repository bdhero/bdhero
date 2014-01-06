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