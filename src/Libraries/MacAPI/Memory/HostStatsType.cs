// ReSharper disable InconsistentNaming
namespace MacAPI.Memory
{
    /// <summary>
    ///     Defined in <c>/usr/include/mach/host_info.h</c>.
    /// </summary>
    internal enum HostStatsType : uint
    {
        #region host_statistics()

        /// <summary>
        ///     System loading statistics.
        /// </summary>
        HOST_LOAD_INFO = 1,

        /// <summary>
        ///     Virtual memory statistics.
        /// </summary>
        HOST_VM_INFO = 2,

        /// <summary>
        ///     CPU load statistics.
        /// </summary>
        HOST_CPU_LOAD_INFO = 3,

        #endregion

        #region host_statistics64()

        /// <summary>
        ///     64-bit virtual memory statistics.
        /// </summary>
        HOST_VM_INFO64 = 4,

        #endregion
    }
}