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
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
namespace WinAPI.User
{
    public static class SystemMenuAPI
    {
        /// <summary>
        ///     Enables the application to access the window menu (also known as the system menu or the control menu)
        ///     for copying and modifying.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window that will own a copy of the window menu.
        /// </param>
        /// <param name="bRevert">
        ///     <para>The action to be taken.</para>
        ///     <para>
        ///         If this parameter is <c>false</c>, <see cref="GetSystemMenu"/> returns a handle
        ///         to the copy of the window menu currently in use. The copy is initially identical to the window menu,
        ///         but it can be modified.
        ///     </para>
        ///     <para>
        ///         If this parameter is <c>true</c>, <see cref="GetSystemMenu"/> resets the window menu back to the default state.
        ///         The previous window menu, if any, is destroyed.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         If the <paramref name="bRevert"/> parameter is <c>false</c>, the return value is a handle to a copy
        ///         of the window menu.
        ///     </para>
        ///     <para>
        ///         If the <paramref name="bRevert"/> parameter is <c>true</c>, the return value is <c>null</c>.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Any window that does not use the <see cref="GetSystemMenu"/> function to make its own copy of the
        ///         window menu receives the standard window menu.
        ///     </para>
        ///     <para>
        ///         The window menu initially contains items with various identifier values, such as
        ///         <c>SC_CLOSE</c>, <c>SC_MOVE</c>, and <c>SC_SIZE</c>.
        ///     </para>
        ///     <para>
        ///         Menu items on the window menu send <c>WM_SYSCOMMAND</c> messages.
        ///     </para>
        ///     <para>
        ///         All predefined window menu items have identifier numbers greater than <c>0xF000</c>.
        ///         If an application adds commands to the window menu, it should use identifier numbers less than
        ///         <c>0xF000</c>.
        ///     </para>
        ///     <para>
        ///         The system automatically grays items on the standard window menu, depending on the situation.
        ///         The application can perform its own checking or graying by responding to the <c>WM_INITMENU</c>
        ///         message that is sent before any menu is displayed.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        ///     Appends a new item to the end of the specified menu bar, drop-down menu, submenu, or shortcut menu.
        ///     You can use this function to specify the content, appearance, and behavior of the menu item.
        /// </summary>
        /// <param name="hMenu">
        ///     A handle to the menu bar, drop-down menu, submenu, or shortcut menu to be changed.
        /// </param>
        /// <param name="uFlags">
        ///     Controls the appearance and behavior of the new menu item. This parameter can be a combination of
        ///     <c>MF_</c> values.
        /// </param>
        /// <param name="uIDNewItem">
        ///     The identifier of the new menu item or, if the <paramref name="uFlags"/> parameter is set to
        ///     <see cref="MenuFlags.MF_POPUP"/>, a handle to the drop-down menu or submenu.
        /// </param>
        /// <param name="lpNewItem">
        ///     The content of the new menu item. The interpretation of <paramref name="lpNewItem"/> depends on whether
        ///     the <paramref name="uFlags"/> parameter includes the following values:
        ///     <see cref="MenuFlags.MF_BITMAP"/>, <see cref="MenuFlags.MF_OWNERDRAW"/>, <see cref="MenuFlags.MF_STRING"/>.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeds; <c>false</c> if it fails.
        ///     To get extended error information, use the <c>GetLastError</c> function.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The application must call the <see cref="DrawMenuBar"/> function whenever a menu changes, whether the menu is in
        ///         a displayed window.
        ///     </para>
        ///     <para>
        ///         To get keyboard accelerators to work with bitmap or owner-drawn menu items, the owner of the menu
        ///         must process the <c>WM_MENUCHAR</c> message.
        ///     </para>
        ///     <para>The following groups of flags cannot be used together:</para>
        ///     <list>
        ///         <item><see cref="MenuFlags.MF_BITMAP"/>, <see cref="MenuFlags.MF_STRING"/>, and <see cref="MenuFlags.MF_OWNERDRAW"/></item>
        ///         <item><see cref="MenuFlags.MF_CHECKED"/> and <see cref="MenuFlags.MF_UNCHECKED"/></item>
        ///         <item><see cref="MenuFlags.MF_DISABLED"/>, <see cref="MenuFlags.MF_ENABLED"/>, and <see cref="MenuFlags.MF_GRAYED"/></item>
        ///         <item><see cref="MenuFlags.MF_MENUBARBREAK"/> and <see cref="MenuFlags.MF_MENUBREAK"/></item>
        ///     </list>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AppendMenu(IntPtr hMenu, [MarshalAs(UnmanagedType.U4)] MenuFlags uFlags, uint uIDNewItem, string lpNewItem);

        /// <summary>
        ///     <para>
        ///         Inserts a new menu item into a menu, moving other items down the menu.
        ///     </para>
        ///     <para>
        ///         <b>Note</b>: The <see cref="InsertMenu"/> function has been superseded by the
        ///         <see cref="InsertMenuItem"/> function.
        ///         You can still use <see cref="InsertMenu"/>, however, if you do not need any of the extended features
        ///         of <see cref="InsertMenuItem"/>.
        ///     </para>
        /// </summary>
        /// <param name="hMenu">
        ///     A handle to the menu to be changed.
        /// </param>
        /// <param name="uPosition">
        ///     The menu item before which the new menu item is to be inserted, as determined by the
        ///     <paramref name="uFlags"/> parameter.
        /// </param>
        /// <param name="uFlags">
        ///     Controls the interpretation of the <paramref name="uPosition"/> parameter and the content, appearance,
        ///     and behavior of the new menu item. This parameter must include either
        ///     <see cref="MenuFlags.MF_BYCOMMAND"/> or <see cref="MenuFlags.MF_BYPOSITION"/>.
        /// </param>
        /// <param name="uIDNewItem">
        ///     The identifier of the new menu item or, if the <paramref name="uFlags"/> parameter has the
        ///     <see cref="MenuFlags.MF_POPUP"/> flag set, a handle to the drop-down menu or submenu.
        /// </param>
        /// <param name="lpNewItem">
        ///     The content of the new menu item. The interpretation of <paramref name="lpNewItem"/> depends on whether
        ///     the <paramref name="uFlags"/> parameter includes the
        ///     <see cref="MenuFlags.MF_BITMAP"/>, <see cref="MenuFlags.MF_OWNERDRAW"/>, or <see cref="MenuFlags.MF_STRING"/> flag.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeds; <c>false</c> if it fails.
        ///     To get extended error information, use the <c>GetLastError</c> function.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The application must call the <see cref="DrawMenuBar"/> function whenever a menu changes,
        ///         whether the menu is in a displayed window.
        ///     </para>
        ///     <para>
        ///         The following groups of flags cannot be used together:
        ///     </para>
        ///     <list>
        ///         <item><see cref="MenuFlags.MF_BYCOMMAND"/> and <see cref="MenuFlags.MF_BYPOSITION"/></item>
        ///         <item><see cref="MenuFlags.MF_DISABLED"/>, <see cref="MenuFlags.MF_ENABLED"/>, and <see cref="MenuFlags.MF_GRAYED"/></item>
        ///         <item><see cref="MenuFlags.MF_BITMAP"/>, <see cref="MenuFlags.MF_STRING"/>, <see cref="MenuFlags.MF_OWNERDRAW"/>, and <see cref="MenuFlags.MF_SEPARATOR"/></item>
        ///         <item><see cref="MenuFlags.MF_MENUBARBREAK"/> and <see cref="MenuFlags.MF_MENUBREAK"/></item>
        ///         <item><see cref="MenuFlags.MF_CHECKED"/> and <see cref="MenuFlags.MF_UNCHECKED"/></item>
        ///     </list>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InsertMenu(IntPtr hMenu, uint uPosition, [MarshalAs(UnmanagedType.U4)] MenuFlags uFlags, uint uIDNewItem, string lpNewItem);

        /// <summary>
        ///     Retrieves information about a menu item.
        /// </summary>
        /// <param name="hMenu">
        ///     A handle to the menu that contains the menu item.
        /// </param>
        /// <param name="uItem">
        ///     The identifier or position of the menu item to get information about.
        ///     The meaning of this parameter depends on the value of <see cref="fByPosition"/>.
        /// </param>
        /// <param name="fByPosition">
        ///     The meaning of <see cref="uItem"/>. If this parameter is <c>false</c>, <see cref="uItem"/> is a
        ///     menu item identifier. Otherwise, it is a menu item position.
        /// </param>
        /// <param name="lpmii">
        ///     A pointer to a <see cref="MENUITEMINFO"/> structure that specifies the information to retrieve and
        ///     receives information about the menu item. Note that you must set the <see cref="MENUITEMINFO.cbSize"/>
        ///     member to <c>sizeof(MENUITEMINFO)</c> before calling this function.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeds; <c>false</c> if it fails.
        ///     To get extended error information, use the <c>GetLastError</c> function.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         To retrieve a menu item of type <see cref="MenuItemType.MFT_STRING"/>, first find the size of the string by
        ///         setting the <see cref="MENUITEMINFO.dwTypeData"/> member of <see cref="MENUITEMINFO"/> to
        ///         <see cref="IntPtr.Zero"/> and then calling <see cref="GetMenuItemInfo"/>.
        ///         The value of <see cref="MENUITEMINFO.cch"/><c>+1</c> is the size needed.
        ///         Then allocate a buffer of this size, place the pointer to the buffer in
        ///         <see cref="MENUITEMINFO.dwTypeData"/>, increment <see cref="MENUITEMINFO.cch"/> by one, and then
        ///         call <see cref="GetMenuItemInfo"/> once again to fill the buffer with the string.
        ///     </para>
        ///     <para>
        ///         If the retrieved menu item is of some other type, then <see cref="GetMenuItemInfo"/> sets the
        ///         <see cref="MENUITEMINFO.dwTypeData"/> member to a value whose type is specified by the
        ///         <see cref="MENUITEMINFO.fType"/> member and sets <see cref="MENUITEMINFO.cch"/> to <c>0</c>.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, [In, Out] ref MENUITEMINFO lpmii);

        /// <summary>
        ///     Changes information about a menu item.
        /// </summary>
        /// <param name="hMenu">
        ///     A handle to the menu that contains the menu item.
        /// </param>
        /// <param name="uItem">
        ///     The identifier or position of the menu item to change.
        ///     The meaning of this parameter depends on the value of <paramref name="fByPosition"/>.
        /// </param>
        /// <param name="fByPosition">
        ///     The meaning of <paramref name="uItem"/>. If this parameter is <c>false</c>, <paramref name="uItem"/> is
        ///     a menu item identifier. Otherwise, it is a menu item position.
        /// </param>
        /// <param name="lpmii">
        ///     A pointer to a <see cref="MENUITEMINFO"/> structure that contains information about the menu item and
        ///     specifies which menu item attributes to change.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeds; <c>false</c> if it fails.
        ///     To get extended error information, use the <c>GetLastError</c> function.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The application must call the <see cref="DrawMenuBar"/> function whenever a menu changes, whether the menu is in
        ///         a displayed window.
        ///     </para>
        ///     <para>
        ///         To get keyboard accelerators to work with bitmap or owner-drawn menu items, the owner of the menu
        ///         must process the <c>WM_MENUCHAR</c> message.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern bool SetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        /// <summary>
        ///     Redraws the menu bar of the specified window. If the menu bar changes after the system has created the
        ///     window, this function must be called to draw the changed menu bar.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window whose menu bar is to be redrawn.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeds; <c>false</c> if it fails.
        ///     To get extended error information, use the <c>GetLastError</c> function.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern bool DrawMenuBar(IntPtr hWnd);
    }
}
