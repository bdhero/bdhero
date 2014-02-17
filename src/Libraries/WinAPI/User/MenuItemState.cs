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
    ///     Values for <see cref="MENUITEMINFO.fState"/>.
    /// </summary>
    [Flags]
    public enum MenuItemState : uint
    {
        /// <summary>
        ///     Checks the menu item. For more information about selected menu items, see the <c>hbmpChecked</c> member.
        /// </summary>
        MFS_CHECKED = 0x00000008,

        /// <summary>
        ///     Specifies that the menu item is the default. A menu can contain only one default menu item, which is displayed in bold.
        /// </summary>
        MFS_DEFAULT = 0x00001000,

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected. This is equivalent to <see cref="MFS_GRAYED"/>.
        /// </summary>
        MFS_DISABLED = 0x00000003,

        /// <summary>
        ///     Enables the menu item so that it can be selected. This is the default state.
        /// </summary>
        MFS_ENABLED = 0x00000000,

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected. This is equivalent to <see cref="MFS_DISABLED"/>.
        /// </summary>
        MFS_GRAYED = 0x00000003,

        /// <summary>
        ///     Highlights the menu item.
        /// </summary>
        MFS_HILITE = 0x00000080,

        /// <summary>
        ///     Unchecks the menu item. For more information about clear menu items, see the <c>hbmpChecked</c> member.
        /// </summary>
        MFS_UNCHECKED = 0x00000000,

        /// <summary>
        ///     Removes the highlight from the menu item. This is the default state.
        /// </summary>
        MFS_UNHILITE = 0x00000000,
    }
}
