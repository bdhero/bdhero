using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    #region All architectures

    [StructLayout(LayoutKind.Sequential)]
    internal struct IoCounters
    {
        public readonly UInt64 ReadOperationCount;
        public readonly UInt64 WriteOperationCount;
        public readonly UInt64 OtherOperationCount;
        public readonly UInt64 ReadTransferCount;
        public readonly UInt64 WriteTransferCount;
        public readonly UInt64 OtherTransferCount;
    }

    #endregion

    #region 32-bit

    /// <summary>
    ///     Various counters for different types of IO operations.  Don't believe
    ///     this is currently implemented by Windows.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IoCounters32
    {
        /// <summary>
        ///     The number of read operations.
        /// </summary>
        [FieldOffset(0)]
        public ulong ReadOperationCount;

        /// <summary>
        ///     The number of write operations.
        /// </summary>
        [FieldOffset(8)]
        public ulong WriteOperationCount;

        /// <summary>
        ///     The number of other operations.
        /// </summary>
        [FieldOffset(16)]
        public ulong OtherOperationCount;

        /// <summary>
        ///     The number of read transfers.
        /// </summary>
        [FieldOffset(24)]
        public ulong ReadTransferCount;

        /// <summary>
        ///     The number of write transfers.
        /// </summary>
        [FieldOffset(32)]
        public ulong WriteTransferCount;

        /// <summary>
        ///     The number of other transfers.
        /// </summary>
        [FieldOffset(40)]
        public ulong OtherTransferCount;
    }

    #endregion

    #region 64-bit

    /// <summary>
    ///     Various counters for different types of IO operations.  Don't believe
    ///     this is currently implemented by Windows.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IoCounters64
    {
        /// <summary>
        ///     The number of read operations.
        /// </summary>
        [FieldOffset(0)]
        public ulong ReadOperationCount;

        /// <summary>
        ///     The number of write operations.
        /// </summary>
        [FieldOffset(8)]
        public ulong WriteOperationCount;

        /// <summary>
        ///     The number of other operations.
        /// </summary>
        [FieldOffset(16)]
        public ulong OtherOperationCount;

        /// <summary>
        ///     The number of read transfers.
        /// </summary>
        [FieldOffset(24)]
        public ulong ReadTransferCount;

        /// <summary>
        ///     The number of write transfers.
        /// </summary>
        [FieldOffset(32)]
        public ulong WriteTransferCount;

        /// <summary>
        ///     The number of other transfers.
        /// </summary>
        [FieldOffset(40)]
        public ulong OtherTransferCount;
    }

    #endregion
}