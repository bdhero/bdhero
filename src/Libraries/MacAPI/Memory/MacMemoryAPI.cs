using System;
using System.Runtime.InteropServices;
using NativeAPI;

// ReSharper disable InconsistentNaming
namespace MacAPI.Memory
{
    public static class MacMemoryAPI
    {
        #region Public API

        /// <summary>
        ///     Gets the total amount of physical memory installed on the system.
        /// </summary>
        /// <returns></returns>
        public static ulong GetPhysicalMemory()
        {
            int[] name = { CTL_HW, HW_MEMSIZE };
            uint nameLen = (uint)name.Length;
            long physicalMemory = 0;
            uint physicalMemorySize = sizeof(long);

            PInvokeUtils.Try(() => sysctl(name, nameLen, ref physicalMemory, ref physicalMemorySize, IntPtr.Zero, 0), retVal => retVal == SysctlSuccess);

            return (ulong)physicalMemory;
        }

        /// <summary>
        ///     Gets the amount of available (free) memory on the system.
        /// </summary>
        /// <returns></returns>
        public static ulong GetAvailableMemory()
        {
            uint hostInfoCount = HOST_VM_INFO_COUNT;
            var vmstat = new vm_statistics_data_t();
            uint host_priv = mach_host_self();

            PInvokeUtils.Try(() => host_statistics(host_priv, HostStatsType.HOST_VM_INFO, ref vmstat, ref hostInfoCount), result => result != KERN_SUCCESS);

            ulong physicalMemory = GetPhysicalMemory();

            double pagesTotal = vmstat.wire_count + vmstat.active_count + vmstat.inactive_count + vmstat.free_count;
            double pagesFreePct = vmstat.free_count / pagesTotal;
            double bytesFree = pagesFreePct * physicalMemory;

            return (ulong) bytesFree;
        }

        #endregion

        #region Datatype reference

        // See http://fxr.watson.org/fxr/source/osfmk/mach/i386/kern_return.h?v=xnu-1228;im=bigexcerpts#L71
        // kern_return_t = int

        // See /usr/include/mach/mach_types.h
        // host_priv_t = mach_port_t = uint

        // host_flavor_t = uint
        // host_info_t = IntPtr (ref vm_statistics_data_t)

        // See /usr/include/mach/i386/vm_types.h
        // mach_msg_type_number_t = natural_t = __darwin_natural_t = HOST_VM_INFO_COUNT = uint

        // mach_port_t = uint

        #endregion

        #region Constants

        const int SysctlSuccess = 0;
        const int SysctlError = -1;

        /// <summary>
        ///     Generic CPU, I/O.
        /// </summary>
        const int CTL_HW = 6;

        /// <summary>
        ///     Total amount of physical memory.
        /// </summary>
        const int HW_MEMSIZE = 24;

        /// <seealso cref="http://www.opensource.apple.com/source/xnu/xnu-1456.1.26/osfmk/mach/kern_return.h"/>
        const int KERN_SUCCESS = 0;

        /// <summary>
        ///     Defined in <c>/usr/include/mach/host_info.h</c>.
        /// </summary>
        static readonly uint HOST_VM_INFO_COUNT =
            (uint) (Marshal.SizeOf(typeof (vm_statistics_data_t)) / sizeof (int));

        #endregion

        #region Functions

        /// <summary>
        /// 	Retrieves system information and allows processes with appropriate privileges to set system information.
        /// </summary>
        /// <param name="name">
        ///     A "Management Information Base" (MIB) style name in the form of an array of <c>int</c>s that describes the requested system information.
        /// </param>
        /// <param name="nlen">
        ///     Length of the <paramref name="name"/> array.
        /// </param>
        /// <param name="oldval">
        ///     <c>NULL</c> or the address where to store the old (current/existing) value.
        /// </param>
        /// <param name="oldlenp">
        ///     Size of the <paramref name="oldval"/> parameter in bytes.
        /// </param>
        /// <param name="newval">
        ///     <c>NULL</c> or the address of the new value.
        /// </param>
        /// <param name="newlen">
        ///     Size of the <paramref name="newval"/> parameter in bytes.
        /// </param>
        /// <returns>
        /// 	<c>0</c> if successful; otherwise <c>-1</c> and <c>errno</c> is set to indicate the error.
        /// </returns>
        /// <remarks>
        ///     C function signature:
        ///     <code>
        ///         int    *name;    // integer vector describing variable
        ///         int     nlen;    // length of this vector
        ///         void   *oldval;  // 0 or address where to store old value
        ///         size_t *oldlenp; // available room for old value overwritten by actual size of old value
        ///         void   *newval;  // 0 or address of new value
        ///         size_t  newlen;  // size of new value
        ///     </code>
        /// </remarks>
        /// <seealso cref="http://www.freebsd.org/cgi/man.cgi?sysctl(3)"/>
        /// <seealso cref="http://man7.org/linux/man-pages/man2/sysctl.2.html"/>
        [DllImport("libc", SetLastError = true)]
        static extern int sysctl(
            [In, Out] int[] name,       // int mib[] = { CTL_HW, HW_MEMSIZE }
            [In, Out] uint nlen,        // ARRAY_SIZE(mib) = 2
            [In, Out] ref long oldval,  // &mem_phys
            [In, Out] ref uint oldlenp, // &sizeof(mem_phys)
            [In, Out] IntPtr newval,
            [In, Out] int newlen
            );

        /// <summary>
        ///     <b>Function</b> - Return statistics for a host.
        /// </summary>
        /// <param name="host_priv">
        ///     [in host-control send right] The control port for the host for which information is to be obtained.
        /// </param>
        /// <param name="flavor">
        ///     [in scalar] The type of statistics desired.
        /// </param>
        /// <param name="host_info">
        ///     [out structure] Statistics about the specified host.
        /// </param>
        /// <param name="host_info_count">
        ///     [in/out scalar] On input, the maximum size of the buffer; on output, the size returned (in natural-sized units).
        /// </param>
        /// <returns>
        ///     Only generic errors apply.
        /// </returns>
        /// <remarks>
        ///     The <c>host_statistics</c> function returns scheduling and virtual memory statistics concerning the host as specified by flavor.
        /// </remarks>
        /// <seealso cref="http://web.mit.edu/darwin/src/modules/xnu/osfmk/man/host_statistics.html"/>
        /// <seealso cref="http://www.gnu.org/software/hurd/gnumach-doc/Message-Format.html"/>
        [DllImport("libc", SetLastError = true)]
        static extern int host_statistics(
            [In] uint host_priv,
            [In] [MarshalAs(UnmanagedType.SysUInt)] HostStatsType flavor,
            [In, Out] ref vm_statistics_data_t host_info,
            [In, Out] ref uint host_info_count
            );

        /// <summary>
        ///     <b>System Trap</b> - Returns the host self port.
        /// </summary>
        /// <returns>
        ///     [host-self send right] Send rights to the host's name port.
        /// </returns>
        /// <remarks>
        ///     The <c>mach_host_self</c> function returns send rights to the task's host self port.
        ///     By default, this is the name port for the current host but can be a different value if so set.
        /// </remarks>
        /// <seealso cref="http://web.mit.edu/darwin/src/modules/xnu/osfmk/man/mach_host_self.html"/>
        [DllImport("libc")]
        static extern uint mach_host_self();

        #endregion
    }
}
