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
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     Values for the <c>uFlags</c> argument of various <see cref="SystemMenuAPI"/> API functions.
    /// </summary>
    [Flags]
    public enum MenuFlags : uint
    {
        #region All functions

        /// <summary>
        ///     Uses a bitmap as the menu item. The lpNewItem parameter contains a handle to the bitmap.
        /// </summary>
        MF_BITMAP = 0x00000004,

        /// <summary>
        ///     Places a check mark next to the menu item. If the application provides check-mark bitmaps
        ///     (see SetMenuItemBitmaps, this flag displays the check-mark bitmap next to the menu item.
        /// </summary>
        MF_CHECKED = 0x00000008,

        /// <summary>
        ///     Disables the menu item so that it cannot be selected, but the flag does not gray it.
        /// </summary>
        MF_DISABLED = 0x00000002,

        /// <summary>
        ///     Enables the menu item so that it can be selected, and restores it from its grayed state.
        /// </summary>
        MF_ENABLED = 0x00000000,

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected.
        /// </summary>
        MF_GRAYED = 0x00000001,

        /// <summary>
        ///     Functions the same as the MF_MENUBREAK flag for a menu bar. For a drop-down menu, submenu,
        ///     or shortcut menu, the new column is separated from the old column by a vertical line.
        /// </summary>
        MF_MENUBARBREAK = 0x00000020,

        /// <summary>
        ///     Places the item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu,
        ///     or shortcut menu) without separating columns.
        /// </summary>
        MF_MENUBREAK = 0x00000040,

        /// <summary>
        ///     Specifies that the item is an owner-drawn item. Before the menu is displayed for the first time,
        ///     the window that owns the menu receives a <c>WM_MEASUREITEM</c> message to retrieve the width and height
        ///     of the menu item. The <c>WM_DRAWITEM</c> message is then sent to the window procedure of the owner
        ///     window whenever the appearance of the menu item must be updated.
        /// </summary>
        MF_OWNERDRAW = 0x00000100,

        /// <summary>
        ///     Specifies that the menu item opens a drop-down menu or submenu. The <c>uIDNewItem</c> parameter
        ///     specifies a handle to the drop-down menu or submenu. This flag is used to add a menu name to a menu bar,
        ///     or a menu item that opens a submenu to a drop-down menu, submenu, or shortcut menu.
        /// </summary>
        MF_POPUP = 0x00000010,

        /// <summary>
        ///     Draws a horizontal dividing line. This flag is used only in a drop-down menu, submenu, or shortcut menu.
        ///     The line cannot be grayed, disabled, or highlighted. The <c>lpNewItem</c> and <c>uIDNewItem</c>
        ///     parameters are ignored.
        /// </summary>
        MF_SEPARATOR = 0x00000800,

        /// <summary>
        ///     Specifies that the menu item is a text string; the <c>lpNewItem</c> parameter is a pointer to the string.
        /// </summary>
        MF_STRING = 0x00000000,

        /// <summary>
        ///     Does not place a check mark next to the item (default). If the application supplies check-mark bitmaps
        ///     (see <c>SetMenuItemBitmaps</c>), this flag displays the clear bitmap next to the menu item.
        /// </summary>
        MF_UNCHECKED = 0x00000000,

        #endregion

        #region InsertMenu()-specific

        /// <summary>
        ///     Indicates that the <c>uPosition</c> parameter gives the identifier of the menu item.
        ///     The <see cref="MF_BYCOMMAND"/> flag is the default if neither the <see cref="MF_BYCOMMAND"/> nor
        ///     <see cref="MF_BYPOSITION"/> flag is specified.
        /// </summary>
        MF_BYCOMMAND = 0x00000000,

        /// <summary>
        ///     Indicates that the <c>uPosition</c> parameter gives the zero-based relative position of the new menu item.
        ///     If <c>uPosition</c> is <c>-1</c>, the new menu item is appended to the end of the menu.
        /// </summary>
        MF_BYPOSITION = 0x00000400,

        #endregion
    }
}
