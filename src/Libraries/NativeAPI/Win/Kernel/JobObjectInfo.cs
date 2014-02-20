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
namespace NativeAPI.Win.Kernel
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