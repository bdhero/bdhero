// Copyright 2014 Andrew C. Dvorak
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

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     <b>UINT_PTR</b>.
    ///     <c>WPARAM</c> is a typedef for <c>UINT_PTR</c> which is an <c>unsigned int</c> (unsigned 32-bit) on win32
    ///     and <c>unsigned __int64</c> (unsigned 64-bit) on x86_64.
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/a/2515285/467582"/>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/aa383751%28VS.85%29.aspx"/>
    public enum WParam : uint
    {
        /// <summary>
        ///     Unknown wParam value.
        /// </summary>
        UNKNOWN = uint.MaxValue,

        /// <summary>
        ///     Null value.
        /// </summary>
        NULL = 0x0000,

        /// <summary>
        ///     A device or piece of media has been inserted and is now available.
        /// </summary>
        DBT_DEVICEARRIVAL = 0x8000,

        /// <summary>
        ///     Permission is requested to remove a device or piece of media.
        ///     Any application can deny this request and cancel the removal.
        /// </summary>
        DBT_DEVICEQUERYREMOVE = 0x8001,

        /// <summary>
        ///     A device or piece of media has been removed.
        /// </summary>
        DBT_DEVICEREMOVECOMPLETE = 0x8004,
    }


    public static class WParamExtensions
    {
        public static Int32 ToInt32(this WParam wParam)
        {
            return (Int32) wParam;
        }

        public static UInt32 ToUInt32(this WParam wParam)
        {
            return (UInt32) wParam;
        }

        public static Int64 ToInt64(this WParam wParam)
        {
            return (Int64) wParam;
        }

        public static UInt64 ToUInt64(this WParam wParam)
        {
            return (UInt64) wParam;
        }
    }
}
