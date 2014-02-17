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
namespace WindowsOSUtils.WinAPI.User
{
    /// <summary>
    ///     Values for <see cref="MENUITEMINFO.fType"/>.
    /// </summary>
    [Flags]
    internal enum MenuItemType : uint
    {
        /// <summary>
        ///     Displays the menu item using a bitmap. The low-order word of the <c>dwTypeData</c> member is the bitmap
        ///     handle, and the <c>cch</c> member is ignored.
        ///     <see cref="MFT_BITMAP"/> is replaced by <see cref="MenuItemInfoMember.MIIM_BITMAP"/> and <c>hbmpItem</c>.
        /// </summary>
        MFT_BITMAP = 0x00000004,

        /// <summary>
        ///     Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu,
        ///     or shortcut menu). For a drop-down menu, submenu, or shortcut menu, a vertical line separates the new
        ///     column from the old.
        /// </summary>
        MFT_MENUBARBREAK = 0x00000020,

        /// <summary>
        ///     Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu,
        ///     or shortcut menu). For a drop-down menu, submenu, or shortcut menu, the columns are not separated by a
        ///     vertical line.
        /// </summary>
        MFT_MENUBREAK = 0x00000040,

        /// <summary>
        ///     Assigns responsibility for drawing the menu item to the window that owns the menu. The window receives
        ///     a <c>WM_MEASUREITEM</c> message before the menu is displayed for the first time, and a <c>WM_DRAWITEM</c>
        ///     message whenever the appearance of the menu item must be updated. If this value is specified, the
        ///     <c>dwTypeData</c> member contains an application-defined value.
        /// </summary>
        MFT_OWNERDRAW = 0x00000100,

        /// <summary>
        ///     Displays selected menu items using a radio-button mark instead of a check mark if the <c>hbmpChecked</c>
        ///     member is NULL.
        /// </summary>
        MFT_RADIOCHECK = 0x00000200,

        /// <summary>
        ///     Right-justifies the menu item and any subsequent items. This value is valid only if the menu item is in
        ///     a menu bar.
        /// </summary>
        MFT_RIGHTJUSTIFY = 0x00004000,

        /// <summary>
        ///     Specifies that menus cascade right-to-left (the default is left-to-right). This is used to support
        ///     right-to-left languages, such as Arabic and Hebrew.
        /// </summary>
        MFT_RIGHTORDER = 0x00002000,

        /// <summary>
        ///     Specifies that the menu item is a separator. A menu item separator appears as a horizontal dividing line.
        ///     The <c>dwTypeData</c> and <c>cch</c> members are ignored. This value is valid only in a drop-down menu,
        ///     submenu, or shortcut menu.
        /// </summary>
        MFT_SEPARATOR = 0x00000800,

        /// <summary>
        ///     Displays the menu item using a text string. The <c>dwTypeData</c> member is the pointer to a
        ///     null-terminated string, and the <c>cch</c> member is the length of the string.
        ///     <see cref="MFT_STRING"/> is replaced by <see cref="MenuItemInfoMember.MIIM_STRING"/>.
        /// </summary>
        MFT_STRING = 0x00000000,
    }
}
