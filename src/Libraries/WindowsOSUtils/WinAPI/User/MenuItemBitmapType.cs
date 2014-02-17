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
    ///     Values for <see cref="MENUITEMINFO.hbmpItem"/>.
    /// </summary>
    public enum MenuItemBitmapType : uint
    {
        /// <summary>
        ///     A bitmap that is drawn by the window that owns the menu. The application must process the
        ///     <c>WM_MEASUREITEM</c> and <c>WM_DRAWITEM</c> messages.
        /// </summary>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms647578(v=vs.85).aspx"/>
        HBMMENU_CALLBACK = uint.MaxValue, // -1

        /// <summary>
        ///     Close button for the menu bar.
        /// </summary>
        HBMMENU_MBAR_CLOSE = 5,

        /// <summary>
        ///     Disabled close button for the menu bar.
        /// </summary>
        HBMMENU_MBAR_CLOSE_D = 6,

        /// <summary>
        ///     Minimize button for the menu bar.
        /// </summary>
        HBMMENU_MBAR_MINIMIZE = 3,

        /// <summary>
        ///     Disabled minimize button for the menu bar.
        /// </summary>
        HBMMENU_MBAR_MINIMIZE_D = 7,

        /// <summary>
        ///     Restore button for the menu bar.
        /// </summary>
        HBMMENU_MBAR_RESTORE = 2,

        /// <summary>
        ///     Close button for the submenu.
        /// </summary>
        HBMMENU_POPUP_CLOSE = 8,

        /// <summary>
        ///     Maximize button for the submenu.
        /// </summary>
        HBMMENU_POPUP_MAXIMIZE = 10,

        /// <summary>
        ///     Minimize button for the submenu.
        /// </summary>
        HBMMENU_POPUP_MINIMIZE = 11,

        /// <summary>
        ///     Restore button for the submenu.
        /// </summary>
        HBMMENU_POPUP_RESTORE = 9,

        /// <summary>
        ///     Windows icon or the icon of the window specified in <c>dwItemData</c>.
        /// </summary>
        HBMMENU_SYSTEM = 1,
    }

    public static class MenuItemBitmapTypeExtensions
    {
        public static IntPtr ToIntPtr(this MenuItemBitmapType type)
        {
            return new IntPtr((uint) type);
        }
    }
}
