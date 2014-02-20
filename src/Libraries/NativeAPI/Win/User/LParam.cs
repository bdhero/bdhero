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
    ///     <b>LONG_PTR</b>.
    ///     <c>LPARAM</c> is a typedef for <c>LONG_PTR</c> which is a <c>long</c> (signed 32-bit) on win32
    ///     and <c>__int64</c> (signed 64-bit) on x86_64.
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/a/2515285/467582"/>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/aa383751%28VS.85%29.aspx"/>
    public enum LParam
    {
        /// <summary>
        ///     Unknown lParam value.
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        ///     Null value.
        /// </summary>
        NULL = 0x0000,
        
        /// <summary>
        ///     Drive type is logical volume.
        /// </summary>
        DBT_DEVTYP_VOLUME = 0x0002,
    }

    public static class LParamExtensions
    {
        public static Int32 ToInt32(this LParam lParam)
        {
            return (Int32) lParam;
        }

        public static UInt32 ToUInt32(this LParam lParam)
        {
            return (UInt32) lParam;
        }

        public static Int64 ToInt64(this LParam lParam)
        {
            return (Int64) lParam;
        }

        public static UInt64 ToUInt64(this LParam lParam)
        {
            return (UInt64) lParam;
        }
    }
}
