// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable MemberCanBePrivate.Local
namespace WindowsOSUtils.Win32
{
    partial class SystemMenu
    {
        #region P/Invoke constants

        /// <summary>
        ///     A window receives this message when the user chooses a command from the Window menu (formerly known
        ///     as the system or control menu) or when the user chooses the maximize button, minimize button,
        ///     restore button, or close button.
        /// </summary>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms646360(v=vs.85).aspx"/>
        private const uint WM_SYSCOMMAND = 0x00000112;

        #region uFlags - all methods

        /// <summary>
        ///     Uses a bitmap as the menu item. The lpNewItem parameter contains a handle to the bitmap.
        /// </summary>
        private const uint MF_BITMAP = 0x00000004;

        /// <summary>
        ///     Places a check mark next to the menu item. If the application provides check-mark bitmaps (see SetMenuItemBitmaps, this flag displays the check-mark bitmap next to the menu item.
        /// </summary>
        private const uint MF_CHECKED = 0x00000008;

        /// <summary>
        ///     Disables the menu item so that it cannot be selected, but the flag does not gray it.
        /// </summary>
        private const uint MF_DISABLED = 0x00000002;

        /// <summary>
        ///     Enables the menu item so that it can be selected, and restores it from its grayed state.
        /// </summary>
        private const uint MF_ENABLED = 0x00000000;

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected.
        /// </summary>
        private const uint MF_GRAYED = 0x00000001;

        /// <summary>
        ///     Functions the same as the MF_MENUBREAK flag for a menu bar. For a drop-down menu, submenu, or shortcut menu, the new column is separated from the old column by a vertical line.
        /// </summary>
        private const uint MF_MENUBARBREAK = 0x00000020;

        /// <summary>
        ///     Places the item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu, or shortcut menu) without separating columns.
        /// </summary>
        private const uint MF_MENUBREAK = 0x00000040;

        /// <summary>
        ///     Specifies that the item is an owner-drawn item. Before the menu is displayed for the first time,
        ///     the window that owns the menu receives a <c>WM_MEASUREITEM</c> message to retrieve the width and height
        ///     of the menu item. The <c>WM_DRAWITEM</c> message is then sent to the window procedure of the owner
        ///     window whenever the appearance of the menu item must be updated.
        /// </summary>
        private const uint MF_OWNERDRAW = 0x00000100;

        /// <summary>
        ///     Specifies that the menu item opens a drop-down menu or submenu. The <c>uIDNewItem</c> parameter
        ///     specifies a handle to the drop-down menu or submenu. This flag is used to add a menu name to a menu bar,
        ///     or a menu item that opens a submenu to a drop-down menu, submenu, or shortcut menu.
        /// </summary>
        private const uint MF_POPUP = 0x00000010;

        /// <summary>
        ///     Draws a horizontal dividing line. This flag is used only in a drop-down menu, submenu, or shortcut menu.
        ///     The line cannot be grayed, disabled, or highlighted. The <c>lpNewItem</c> and <c>uIDNewItem</c>
        ///     parameters are ignored.
        /// </summary>
        private const uint MF_SEPARATOR = 0x00000800;

        /// <summary>
        ///     Specifies that the menu item is a text string; the <c>lpNewItem</c> parameter is a pointer to the string.
        /// </summary>
        private const uint MF_STRING = 0x00000000;

        /// <summary>
        ///     Does not place a check mark next to the item (default). If the application supplies check-mark bitmaps
        ///     (see <c>SetMenuItemBitmaps</c>), this flag displays the clear bitmap next to the menu item.
        /// </summary>
        private const uint MF_UNCHECKED = 0x00000000;

        #endregion

        #region uFlags - InsertMenu() specific

        /// <summary>
        ///     Indicates that the <c>uPosition</c> parameter gives the identifier of the menu item.
        ///     The <see cref="MF_BYCOMMAND"/> flag is the default if neither the <see cref="MF_BYCOMMAND"/> nor
        ///     <see cref="MF_BYPOSITION"/> flag is specified.
        /// </summary>
        private const uint MF_BYCOMMAND = 0x00000000;

        /// <summary>
        ///     Indicates that the <c>uPosition</c> parameter gives the zero-based relative position of the new menu item.
        ///     If <c>uPosition</c> is <c>-1</c>, the new menu item is appended to the end of the menu.
        /// </summary>
        private const uint MF_BYPOSITION = 0x00000400;

        #endregion

        #region MenuItemInfo

        #region fMask

        /// <summary>
        ///     Retrieves or sets the <c>hbmpItem</c> member.
        /// </summary>
        private const uint MIIM_BITMAP = 0x00000080;

        /// <summary>
        ///     Retrieves or sets the <c>hbmpChecked</c> and <c>hbmpUnchecked</c> members.
        /// </summary>
        private const uint MIIM_CHECKMARKS = 0x00000008;

        /// <summary>
        ///     Retrieves or sets the <c>dwItemData</c> member.
        /// </summary>
        private const uint MIIM_DATA = 0x00000020;

        /// <summary>
        ///     Retrieves or sets the <c>fType</c> member.
        /// </summary>
        private const uint MIIM_FTYPE = 0x00000100;

        /// <summary>
        ///     Retrieves or sets the <c>wID</c> member.
        /// </summary>
        private const uint MIIM_ID = 0x00000002;

        /// <summary>
        ///     Retrieves or sets the <c>fState</c> member.
        /// </summary>
        private const uint MIIM_STATE = 0x00000001;

        /// <summary>
        ///     Retrieves or sets the <c>dwTypeData</c> member.
        /// </summary>
        private const uint MIIM_STRING = 0x00000040;

        /// <summary>
        ///     Retrieves or sets the <c>hSubMenu</c> member.
        /// </summary>
        private const uint MIIM_SUBMENU = 0x00000004;

        /// <summary>
        ///     Retrieves or sets the <c>fType</c> and <c>dwTypeData</c> members. <see cref="MIIM_TYPE"/> is replaced
        ///     by <see cref="MIIM_BITMAP"/>, <see cref="MIIM_FTYPE"/>, and <see cref="MIIM_STRING"/>.
        /// </summary>
        private const uint MIIM_TYPE = 0x00000010;

        #endregion

        #region fType

        /// <summary>
        ///     Displays the menu item using a bitmap. The low-order word of the <c>dwTypeData</c> member is the bitmap
        ///     handle, and the <c>cch</c> member is ignored.
        ///     <see cref="MFT_BITMAP"/> is replaced by <see cref="MIIM_BITMAP"/> and <c>hbmpItem</c>.
        /// </summary>
        private const uint MFT_BITMAP = 0x00000004;

        /// <summary>
        ///     Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu,
        ///     or shortcut menu). For a drop-down menu, submenu, or shortcut menu, a vertical line separates the new
        ///     column from the old.
        /// </summary>
        private const uint MFT_MENUBARBREAK = 0x00000020;

        /// <summary>
        ///     Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu,
        ///     or shortcut menu). For a drop-down menu, submenu, or shortcut menu, the columns are not separated by a
        ///     vertical line.
        /// </summary>
        private const uint MFT_MENUBREAK = 0x00000040;

        /// <summary>
        ///     Assigns responsibility for drawing the menu item to the window that owns the menu. The window receives
        ///     a <c>WM_MEASUREITEM</c> message before the menu is displayed for the first time, and a <c>WM_DRAWITEM</c>
        ///     message whenever the appearance of the menu item must be updated. If this value is specified, the
        ///     <c>dwTypeData</c> member contains an application-defined value.
        /// </summary>
        private const uint MFT_OWNERDRAW = 0x00000100;

        /// <summary>
        ///     Displays selected menu items using a radio-button mark instead of a check mark if the <c>hbmpChecked</c>
        ///     member is NULL.
        /// </summary>
        private const uint MFT_RADIOCHECK = 0x00000200;

        /// <summary>
        ///     Right-justifies the menu item and any subsequent items. This value is valid only if the menu item is in
        ///     a menu bar.
        /// </summary>
        private const uint MFT_RIGHTJUSTIFY = 0x00004000;

        /// <summary>
        ///     Specifies that menus cascade right-to-left (the default is left-to-right). This is used to support
        ///     right-to-left languages, such as Arabic and Hebrew.
        /// </summary>
        private const uint MFT_RIGHTORDER = 0x00002000;

        /// <summary>
        ///     Specifies that the menu item is a separator. A menu item separator appears as a horizontal dividing line.
        ///     The <c>dwTypeData</c> and <c>cch</c> members are ignored. This value is valid only in a drop-down menu,
        ///     submenu, or shortcut menu.
        /// </summary>
        private const uint MFT_SEPARATOR = 0x00000800;

        /// <summary>
        ///     Displays the menu item using a text string. The <c>dwTypeData</c> member is the pointer to a
        ///     null-terminated string, and the <c>cch</c> member is the length of the string.
        ///     <see cref="MFT_STRING"/> is replaced by <see cref="MIIM_STRING"/>.
        /// </summary>
        private const uint MFT_STRING = 0x00000000;

        #endregion

        #region fstate

        /// <summary>
        ///     Checks the menu item. For more information about selected menu items, see the <c>hbmpChecked</c> member.
        /// </summary>
        private const uint MFS_CHECKED = 0x00000008;

        /// <summary>
        ///     Specifies that the menu item is the default. A menu can contain only one default menu item, which is displayed in bold.
        /// </summary>
        private const uint MFS_DEFAULT = 0x00001000;

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected. This is equivalent to <see cref="MFS_GRAYED"/>.
        /// </summary>
        private const uint MFS_DISABLED = 0x00000003;

        /// <summary>
        ///     Enables the menu item so that it can be selected. This is the default state.
        /// </summary>
        private const uint MFS_ENABLED = 0x00000000;

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected. This is equivalent to <see cref="MFS_DISABLED"/>.
        /// </summary>
        private const uint MFS_GRAYED = 0x00000003;

        /// <summary>
        ///     Highlights the menu item.
        /// </summary>
        private const uint MFS_HILITE = 0x00000080;

        /// <summary>
        ///     Unchecks the menu item. For more information about clear menu items, see the <c>hbmpChecked</c> member.
        /// </summary>
        private const uint MFS_UNCHECKED = 0x00000000;

        /// <summary>
        ///     Removes the highlight from the menu item. This is the default state.
        /// </summary>
        private const uint MFS_UNHILITE = 0x00000000;

        #endregion

        #region hbmpItem

        // TODO: Value is listed as -1 on MSDN, but C# won't allow a value of -1 on a uint
        // See http://msdn.microsoft.com/en-us/library/windows/desktop/ms647578(v=vs.85).aspx
#if false
        /// <summary>
        ///     A bitmap that is drawn by the window that owns the menu. The application must process the
        ///     <c>WM_MEASUREITEM</c> and <c>WM_DRAWITEM</c> messages.
        /// </summary>
        private const uint HBMMENU_CALLBACK = uint.MaxValue; // -1
#endif

        /// <summary>
        ///     Close button for the menu bar.
        /// </summary>
        private const uint HBMMENU_MBAR_CLOSE = 5;

        /// <summary>
        ///     Disabled close button for the menu bar.
        /// </summary>
        private const uint HBMMENU_MBAR_CLOSE_D = 6;

        /// <summary>
        ///     Minimize button for the menu bar.
        /// </summary>
        private const uint HBMMENU_MBAR_MINIMIZE = 3;

        /// <summary>
        ///     Disabled minimize button for the menu bar.
        /// </summary>
        private const uint HBMMENU_MBAR_MINIMIZE_D = 7;

        /// <summary>
        ///     Restore button for the menu bar.
        /// </summary>
        private const uint HBMMENU_MBAR_RESTORE = 2;

        /// <summary>
        ///     Close button for the submenu.
        /// </summary>
        private const uint HBMMENU_POPUP_CLOSE = 8;

        /// <summary>
        ///     Maximize button for the submenu.
        /// </summary>
        private const uint HBMMENU_POPUP_MAXIMIZE = 10;

        /// <summary>
        ///     Minimize button for the submenu.
        /// </summary>
        private const uint HBMMENU_POPUP_MINIMIZE = 11;

        /// <summary>
        ///     Restore button for the submenu.
        /// </summary>
        private const uint HBMMENU_POPUP_RESTORE = 9;

        /// <summary>
        ///     Windows icon or the icon of the window specified in <c>dwItemData</c>.
        /// </summary>
        private const uint HBMMENU_SYSTEM = 1;


        #endregion

        #endregion

        #endregion

        #region P/Invoke structures

        /// <summary>
        ///     Contains information about a menu item.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The <see cref="MENUITEMINFO"/> structure is used with the <see cref="GetMenuItemInfo"/>,
        ///         <see cref="InsertMenuItem"/>, and <see cref="SetMenuItemInfo"/> functions.
        ///     </para>
        ///     <para>
        ///         The menu can display items using text, bitmaps, or both.
        ///     </para>
        /// </remarks>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms647578(v=vs.85).aspx"/>
        [StructLayout(LayoutKind.Sequential)]
        private struct MENUITEMINFO
        {
            /// <summary>
            ///     The size of the structure, in bytes. The caller must set this member to <c>sizeof(MENUITEMINFO)</c>.
            /// </summary>
            public uint cbSize;

            /// <summary>
            ///     Indicates the members to be retrieved or set. This member can be one or more of the <c>MIIM_</c> values.
            /// </summary>
            public uint fMask;

            /// <summary>
            ///     The menu item type. This member can be one or more of the <c>MFT_</c> values.
            ///     The <see cref="MFT_BITMAP"/>, <see cref="MFT_SEPARATOR"/>, and <see cref="MFT_STRING"/> values
            ///     cannot be combined with one another.
            ///     Set <see cref="fMask"/> to <see cref="MIIM_TYPE"/> to use <see cref="fType"/>.
            ///     <see cref="fType"/> is used only if <see cref="fType"/> has a value of <see cref="MIIM_FTYPE"/>.
            /// </summary>
            public uint fType;

            /// <summary>
            ///     The menu item state. This member can be one or more of the <c>MFS_</c> values.
            ///     Set <see cref="fMask"/> to <see cref="MIIM_STATE"/> to use <see cref="fState"/>.
            /// </summary>
            public uint fState;

            /// <summary>
            ///     An application-defined value that identifies the menu item. Set <see cref="fMask"/> to
            ///     <see cref="MIIM_ID"/> to use <see cref="wID"/>.
            /// </summary>
            public uint wID;

            /// <summary>
            ///     A handle to the drop-down menu or submenu associated with the menu item.
            ///     If the menu item is not an item that opens a drop-down menu or submenu, this member is
            ///     <see cref="IntPtr.Zero"/>.
            ///     Set <see cref="fMask"/> to <see cref="MIIM_SUBMENU"/> to use <see cref="hSubMenu"/>.
            /// </summary>
            public IntPtr hSubMenu;

            /// <summary>
            ///     A handle to the bitmap to display next to the item if it is selected.
            ///     If this member is <see cref="IntPtr.Zero"/>, a default bitmap is used.
            ///     If the <see cref="MFT_RADIOCHECK"/> type value is specified, the default bitmap is a bullet.
            ///     Otherwise, it is a check mark. Set <see cref="fMask"/> to <see cref="MIIM_CHECKMARKS"/> to use
            ///     <see cref="hbmpChecked"/>.
            /// </summary>
            public IntPtr hbmpChecked;

            /// <summary>
            ///     A handle to the bitmap to display next to the item if it is not selected.
            ///     If this member is <see cref="IntPtr.Zero"/>, no bitmap is used.
            ///     Set <see cref="fMask"/> to <see cref="MIIM_CHECKMARKS"/> to use <see cref="hbmpUnchecked"/>.
            /// </summary>
            public IntPtr hbmpUnchecked;

            /// <summary>
            ///     An application-defined value associated with the menu item.
            ///     Set <see cref="fMask"/> to <see cref="MIIM_DATA"/> to use <see cref="dwItemData"/>.
            /// </summary>
            public IntPtr dwItemData;

            /// <summary>
            ///     <para>
            ///         The contents of the menu item. The meaning of this member depends on the value of <see cref="fType" /> and is
            ///         used only if the <see cref="MIIM_TYPE" /> flag is set in the <see cref="fMask" /> member.
            ///     </para>
            ///     <para>
            ///         To retrieve a menu item of type <see cref="MFT_STRING" />, first find the size of the string by setting the
            ///         <see cref="dwTypeData" /> member of <see cref="MENUITEMINFO" /> to <see cref="IntPtr.Zero" /> and then calling
            ///         <see cref="GetMenuItemInfo" />. The value of <see cref="cch" /><c>+1</c> is the size needed. Then allocate a
            ///         buffer of this size, place the pointer to the buffer in
            ///         <see cref="dwTypeData" />, increment <see cref="cch" />, and call <see cref="GetMenuItemInfo" /> once again to
            ///         fill the buffer with the string. If the retrieved menu item is of some other type, then
            ///         <see cref="GetMenuItemInfo" /> sets the <see cref="dwTypeData" /> member to a value whose type is specified by
            ///         the <see cref="fType" /> member.
            ///     </para>
            ///     <para>
            ///         When using with the <see cref="SetMenuItemInfo" /> function, this member should contain a value whose type is
            ///         specified by the <see cref="fType" /> member.
            ///     </para>
            ///     <para>
            ///         <see cref="dwTypeData" /> is used only if the <see cref="MIIM_STRING" /> flag is set in the
            ///         <see cref="fMask" /> member
            ///     </para>
            /// </summary>
            public string dwTypeData;

            /// <summary>
            ///     <para>
            ///         The length of the menu item text, in characters, when information is received about a menu item of the
            ///         <see cref="MFT_STRING" /> type. However, <see cref="cch" /> is used only if the <see cref="MIIM_TYPE" /> flag
            ///         is set in the <see cref="fMask" /> member and is zero otherwise. Also, <see cref="cch" /> is ignored when the
            ///         content of a menu item is set by calling <see cref="SetMenuItemInfo" />.
            ///     </para>
            ///     <para>
            ///         Note that, before calling <see cref="GetMenuItemInfo" />, the application must set <see cref="cch" /> to the
            ///         length of the buffer pointed to by the <see cref="dwTypeData" /> member. If the retrieved menu item is of type
            ///         MFT_STRING (as indicated by the <see cref="fType" /> member), then <see cref="GetMenuItemInfo" /> changes
            ///         <see cref="cch" /> to the length of the menu item text. If the retrieved menu item is of some other type,
            ///         <see cref="GetMenuItemInfo" /> sets the <see cref="cch" /> field to zero.
            ///     </para>
            ///     <para>
            ///         The <see cref="cch" /> member is used when the <see cref="MIIM_STRING" /> flag is set in the
            ///         <see cref="fMask" /> member.
            ///     </para>
            /// </summary>
            public uint cch;

            /// <summary>
            ///     A handle to the bitmap to be displayed, or it can be one of the <c>HBMMENU_</c> values.
            ///     It is used when the <see cref="MIIM_BITMAP"/> flag is set in the <see cref="fMask"/> member.
            /// </summary>
            public IntPtr hbmpItem;
        }

        #endregion

        #region P/Invoke function declarations

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
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

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
        ///     <see cref="MF_POPUP"/>, a handle to the drop-down menu or submenu.
        /// </param>
        /// <param name="lpNewItem">
        ///     The content of the new menu item. The interpretation of <paramref name="lpNewItem"/> depends on whether
        ///     the <paramref name="uFlags"/> parameter includes the following values:
        ///     <see cref="MF_BITMAP"/>, <see cref="MF_OWNERDRAW"/>, <see cref="MF_STRING"/>.
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
        ///         <item><see cref="MF_BITMAP"/>, <see cref="MF_STRING"/>, and <see cref="MF_OWNERDRAW"/></item>
        ///         <item><see cref="MF_CHECKED"/> and <see cref="MF_UNCHECKED"/></item>
        ///         <item><see cref="MF_DISABLED"/>, <see cref="MF_ENABLED"/>, and <see cref="MF_GRAYED"/></item>
        ///         <item><see cref="MF_MENUBARBREAK"/> and <see cref="MF_MENUBREAK"/></item>
        ///     </list>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

        /// <summary>
        ///     <para>
        ///         Inserts a new menu item into a menu, moving other items down the menu.
        ///     </para>
        ///     <para>
        ///         <b>Note</b>: The <see cref="InsertMenu(System.IntPtr,int,int,int,string)"/> function has been superseded by the
        ///         <see cref="InsertMenuItem"/> function.
        ///         You can still use <see cref="InsertMenu(System.IntPtr,int,int,int,string)"/>, however, if you do not need any of the extended features
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
        ///     <see cref="MF_BYCOMMAND"/> or <see cref="MF_BYPOSITION"/>.
        /// </param>
        /// <param name="uIDNewItem">
        ///     The identifier of the new menu item or, if the <paramref name="uFlags"/> parameter has the
        ///     <see cref="MF_POPUP"/> flag set, a handle to the drop-down menu or submenu.
        /// </param>
        /// <param name="lpNewItem">
        ///     The content of the new menu item. The interpretation of <paramref name="lpNewItem"/> depends on whether
        ///     the <paramref name="uFlags"/> parameter includes the
        ///     <see cref="MF_BITMAP"/>, <see cref="MF_OWNERDRAW"/>, or <see cref="MF_STRING"/> flag.
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
        ///         <item><see cref="MF_BYCOMMAND"/> and <see cref="MF_BYPOSITION"/></item>
        ///         <item><see cref="MF_DISABLED"/>, <see cref="MF_ENABLED"/>, and <see cref="MF_GRAYED"/></item>
        ///         <item><see cref="MF_BITMAP"/>, <see cref="MF_STRING"/>, <see cref="MF_OWNERDRAW"/>, and <see cref="MF_SEPARATOR"/></item>
        ///         <item><see cref="MF_MENUBARBREAK"/> and <see cref="MF_MENUBREAK"/></item>
        ///         <item><see cref="MF_CHECKED"/> and <see cref="MF_UNCHECKED"/></item>
        ///     </list>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, uint uPosition, uint uFlags, uint uIDNewItem, string lpNewItem);

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
        ///         To retrieve a menu item of type <see cref="MFT_STRING"/>, first find the size of the string by
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
        private static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, [In, Out] ref MENUITEMINFO lpmii);

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
        private static extern bool SetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

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
        private static extern bool DrawMenuBar(IntPtr hWnd);

        #endregion
    }
}
