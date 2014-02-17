using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    /// <summary>
    ///     Union of different limit data structures that may be passed
    ///     to SetInformationJobObject / from QueryInformationJobObject.
    ///     This union also contains separate 32 and 64 bit versions of
    ///     each structure.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct JobObjectInfo
    {
        #region 32 bit structures

        /// <summary>
        ///     The BasicLimits32 structure contains basic limit information
        ///     for a job object on a 32bit platform.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits32 basicLimits32;

        /// <summary>
        ///     The ExtendedLimits32 structure contains extended limit information
        ///     for a job object on a 32bit platform.
        /// </summary>
        [FieldOffset(0)]
        public ExtendedLimits32 extendedLimits32;

        #endregion

        #region 64 bit structures

        /// <summary>
        ///     The BasicLimits64 structure contains basic limit information
        ///     for a job object on a 64bit platform.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits64 basicLimits64;

        /// <summary>
        ///     The ExtendedLimits64 structure contains extended limit information
        ///     for a job object on a 64bit platform.
        /// </summary>
        [FieldOffset(0)]
        public ExtendedLimits64 extendedLimits64;

        #endregion
    }
}