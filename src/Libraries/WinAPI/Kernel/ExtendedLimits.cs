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

using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    #region 32-bit

    /// <summary>
    ///     The JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure contains basic and extended limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ExtendedLimits32
    {
        /// <summary>
        ///     A JOBOBJECT_BASIC_LIMIT_INFORMATION structure that contains
        ///     basic limit information.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits32 BasicLimits;

        /// <summary>
        ///     Resereved.
        /// </summary>
        [FieldOffset(48)]
        public IoCounters32 IoInfo;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_PROCESS_MEMORY value, this member specifies
        ///     the limit for the virtual memory that can be committed by a process.
        ///     Otherwise, this member is ignored.
        /// </summary>
        [FieldOffset(96)]
        public uint ProcessMemoryLimit;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_JOB_MEMORY value, this member specifies the
        ///     limit for the virtual memory that can be committed for the job. Otherwise,
        ///     this member is ignored.
        /// </summary>
        [FieldOffset(100)]
        public uint JobMemoryLimit;

        /// <summary>
        ///     Peak memory used by any process ever associated with the job.
        /// </summary>
        [FieldOffset(104)]
        public uint PeakProcessMemoryUsed;

        /// <summary>
        ///     Peak memory usage of all processes currently associated with the job.
        /// </summary>
        [FieldOffset(108)]
        public uint PeakJobMemoryUsed;
    }

    #endregion

    #region 64-bit

    /// <summary>
    ///     The JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure contains basic and extended limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ExtendedLimits64
    {
        /// <summary>
        ///     A JOBOBJECT_BASIC_LIMIT_INFORMATION structure that contains
        ///     basic limit information.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits64 BasicLimits;

        /// <summary>
        ///     Resereved.
        /// </summary>
        [FieldOffset(64)]
        public IoCounters64 IoInfo;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_PROCESS_MEMORY value, this member specifies
        ///     the limit for the virtual memory that can be committed by a process.
        ///     Otherwise, this member is ignored.
        /// </summary>
        [FieldOffset(112)]
        public ulong ProcessMemoryLimit;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_JOB_MEMORY value, this member specifies the
        ///     limit for the virtual memory that can be committed for the job. Otherwise,
        ///     this member is ignored.
        /// </summary>
        [FieldOffset(120)]
        public ulong JobMemoryLimit;

        /// <summary>
        ///     Peak memory used by any process ever associated with the job.
        /// </summary>
        [FieldOffset(128)]
        public ulong PeakProcessMemoryUsed;

        /// <summary>
        ///     Peak memory usage of all processes currently associated with the job.
        /// </summary>
        [FieldOffset(136)]
        public ulong PeakJobMemoryUsed;
    }

    #endregion
}