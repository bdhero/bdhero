// Copyright 2013-2014 Andrew C. Dvorak
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
namespace WinAPI.User
{
    /// <summary>
    ///     Values for the <c>fMask</c> argument of various <see cref="SystemMenuAPI"/> API functions.
    /// </summary>
    [Flags]
    public enum MenuItemInfoMember : uint
    {
        /// <summary>
        ///     Retrieves or sets the <c>hbmpItem</c> member.
        /// </summary>
        MIIM_BITMAP = 0x00000080,

        /// <summary>
        ///     Retrieves or sets the <c>hbmpChecked</c> and <c>hbmpUnchecked</c> members.
        /// </summary>
        MIIM_CHECKMARKS = 0x00000008,

        /// <summary>
        ///     Retrieves or sets the <c>dwItemData</c> member.
        /// </summary>
        MIIM_DATA = 0x00000020,

        /// <summary>
        ///     Retrieves or sets the <c>fType</c> member.
        /// </summary>
        MIIM_FTYPE = 0x00000100,

        /// <summary>
        ///     Retrieves or sets the <c>wID</c> member.
        /// </summary>
        MIIM_ID = 0x00000002,

        /// <summary>
        ///     Retrieves or sets the <c>fState</c> member.
        /// </summary>
        MIIM_STATE = 0x00000001,

        /// <summary>
        ///     Retrieves or sets the <c>dwTypeData</c> member.
        /// </summary>
        MIIM_STRING = 0x00000040,

        /// <summary>
        ///     Retrieves or sets the <c>hSubMenu</c> member.
        /// </summary>
        MIIM_SUBMENU = 0x00000004,

        /// <summary>
        ///     Retrieves or sets the <c>fType</c> and <c>dwTypeData</c> members. <see cref="MIIM_TYPE"/> is replaced
        ///     by <see cref="MIIM_BITMAP"/>, <see cref="MIIM_FTYPE"/>, and <see cref="MIIM_STRING"/>.
        /// </summary>
        MIIM_TYPE = 0x00000010,
    }
}
