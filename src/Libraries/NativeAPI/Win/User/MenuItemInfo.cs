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
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     Contains information about a menu item.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <see cref="MENUITEMINFO"/> structure is used with the <see cref="SystemMenuAPI.GetMenuItemInfo"/>,
    ///         <see cref="SystemMenuAPI.InsertMenuItem"/>, and <see cref="SystemMenuAPI.SetMenuItemInfo"/> functions.
    ///     </para>
    ///     <para>
    ///         The menu can display items using text, bitmaps, or both.
    ///     </para>
    /// </remarks>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms647578(v=vs.85).aspx"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct MENUITEMINFO
    {
        /// <summary>
        ///     The size of the structure, in bytes. The caller must set this member to <c>sizeof(MENUITEMINFO)</c>.
        /// </summary>
        public uint cbSize;

        /// <summary>
        ///     Indicates the members to be retrieved or set. This member can be one or more of the <c>MIIM_</c> values.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public MenuItemInfoMember fMask;

        /// <summary>
        ///     The menu item type. This member can be one or more of the <see cref="MenuItemType"/> values.
        ///     The <see cref="MenuItemType.MFT_BITMAP"/>, <see cref="MenuItemType.MFT_SEPARATOR"/>, and <see cref="MenuItemType.MFT_STRING"/> values
        ///     cannot be combined with one another.
        ///     Set <see cref="fMask"/> and <see cref="MenuItemInfoMember.MIIM_TYPE"/> to use <see cref="fType"/>.
        ///     <see cref="fType"/> is used only if <see cref="fType"/> has a value of <see cref="MenuItemInfoMember.MIIM_FTYPE"/>.
        /// </summary>
        public uint fType;

        /// <summary>
        ///     The menu item state. This member can be one or more of the <see cref="MenuItemState"/> values.
        ///     Set <see cref="fMask"/> to <see cref="MenuItemInfoMember.MIIM_STATE"/> to use <see cref="fState"/>.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public MenuItemState fState;

        /// <summary>
        ///     An application-defined value that identifies the menu item. Set <see cref="fMask"/> to
        ///     <see cref="MenuItemInfoMember.MIIM_ID"/> to use <see cref="wID"/>.
        /// </summary>
        public uint wID;

        /// <summary>
        ///     A handle to the drop-down menu or submenu associated with the menu item.
        ///     If the menu item is not an item that opens a drop-down menu or submenu, this member is
        ///     <see cref="IntPtr.Zero"/>.
        ///     Set <see cref="fMask"/> to <see cref="MenuItemInfoMember.MIIM_SUBMENU"/> to use <see cref="hSubMenu"/>.
        /// </summary>
        public IntPtr hSubMenu;

        /// <summary>
        ///     A handle to the bitmap to display next to the item if it is selected.
        ///     If this member is <see cref="IntPtr.Zero"/>, a default bitmap is used.
        ///     If the <see cref="MenuItemType.MFT_RADIOCHECK"/> type value is specified, the default bitmap is a bullet.
        ///     Otherwise, it is a check mark. Set <see cref="fMask"/> to <see cref="MenuItemInfoMember.MIIM_CHECKMARKS"/> to use
        ///     <see cref="hbmpChecked"/>.
        /// </summary>
        public IntPtr hbmpChecked;

        /// <summary>
        ///     A handle to the bitmap to display next to the item if it is not selected.
        ///     If this member is <see cref="IntPtr.Zero"/>, no bitmap is used.
        ///     Set <see cref="fMask"/> to <see cref="MenuItemInfoMember.MIIM_CHECKMARKS"/> to use <see cref="hbmpUnchecked"/>.
        /// </summary>
        public IntPtr hbmpUnchecked;

        /// <summary>
        ///     An application-defined value associated with the menu item.
        ///     Set <see cref="fMask"/> to <see cref="MenuItemInfoMember.MIIM_DATA"/> to use <see cref="dwItemData"/>.
        /// </summary>
        public IntPtr dwItemData;

        /// <summary>
        ///     <para>
        ///         The contents of the menu item. The meaning of this member depends on the value of <see cref="fType" /> and is
        ///         used only if the <see cref="MenuItemInfoMember.MIIM_TYPE" /> flag is set in the <see cref="fMask" /> member.
        ///     </para>
        ///     <para>
        ///         To retrieve a menu item of type <see cref="MenuItemType.MFT_STRING" />, first find the size of the string by setting the
        ///         <see cref="dwTypeData" /> member of <see cref="MENUITEMINFO" /> to <see cref="IntPtr.Zero" /> and then calling
        ///         <see cref="SystemMenuAPI.GetMenuItemInfo" />. The value of <see cref="cch" /><c>+1</c> is the size needed. Then allocate a
        ///         buffer of this size, place the pointer to the buffer in
        ///         <see cref="dwTypeData" />, increment <see cref="cch" />, and call <see cref="SystemMenuAPI.GetMenuItemInfo" /> once again to
        ///         fill the buffer with the string. If the retrieved menu item is of some other type, then
        ///         <see cref="SystemMenuAPI.GetMenuItemInfo" /> sets the <see cref="dwTypeData" /> member to a value whose type is specified by
        ///         the <see cref="fType" /> member.
        ///     </para>
        ///     <para>
        ///         When using with the <see cref="SystemMenuAPI.SetMenuItemInfo" /> function, this member should contain a value whose type is
        ///         specified by the <see cref="fType" /> member.
        ///     </para>
        ///     <para>
        ///         <see cref="dwTypeData" /> is used only if the <see cref="MenuItemInfoMember.MIIM_STRING" /> flag is set in the
        ///         <see cref="fMask" /> member
        ///     </para>
        /// </summary>
        public string dwTypeData;

        /// <summary>
        ///     <para>
        ///         The length of the menu item text, in characters, when information is received about a menu item of the
        ///         <see cref="MenuItemType.MFT_STRING" /> type. However, <see cref="cch" /> is used only if the <see cref="MenuItemInfoMember.MIIM_TYPE" /> flag
        ///         is set in the <see cref="fMask" /> member and is zero otherwise. Also, <see cref="cch" /> is ignored when the
        ///         content of a menu item is set by calling <see cref="SystemMenuAPI.SetMenuItemInfo" />.
        ///     </para>
        ///     <para>
        ///         Note that, before calling <see cref="SystemMenuAPI.GetMenuItemInfo" />, the application must set <see cref="cch" /> to the
        ///         length of the buffer pointed to by the <see cref="dwTypeData" /> member. If the retrieved menu item is of type
        ///         <see cref="MenuItemType.MFT_STRING"/> (as indicated by the <see cref="fType" /> member), then <see cref="SystemMenuAPI.GetMenuItemInfo" /> changes
        ///         <see cref="cch" /> to the length of the menu item text. If the retrieved menu item is of some other type,
        ///         <see cref="SystemMenuAPI.GetMenuItemInfo" /> sets the <see cref="cch" /> field to zero.
        ///     </para>
        ///     <para>
        ///         The <see cref="cch" /> member is used when the <see cref="MenuItemInfoMember.MIIM_STRING" /> flag is set in the
        ///         <see cref="fMask" /> member.
        ///     </para>
        /// </summary>
        public uint cch;

        /// <summary>
        ///     A handle to the bitmap to be displayed, or it can be one of the <see cref="MenuItemBitmapType"/> values.
        ///     It is used when the <see cref="MenuItemInfoMember.MIIM_BITMAP"/> flag is set in the <see cref="fMask"/> member.
        /// </summary>
        public IntPtr hbmpItem;

        // ReSharper disable once UnusedParameter.Local
        public MENUITEMINFO(bool? dummy)
            : this()
        {
            cbSize = (uint) Marshal.SizeOf(this);
        }
    }
}
