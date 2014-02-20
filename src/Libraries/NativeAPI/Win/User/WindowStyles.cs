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
// ReSharper disable UnusedMember.Localx
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Localx
// ReSharper disable FieldCanBeMadeReadOnly.Globalx
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariablex
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     After the window has been created, these styles cannot be modified, except as noted.
    /// </summary>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms632600(v=vs.85).aspx"/>
    [Flags]
    public enum WindowStyles : uint
    {
        /// <summary>
        ///     The window has a thin-line border.
        /// </summary>
        WS_BORDER = 0x00800000,

        /// <summary>
        ///     The window has a title bar (includes the <see cref="WS_BORDER"/> style).
        /// </summary>
        WS_CAPTION = 0x00C00000,

        /// <summary>
        ///     The window is a child window. A window with this style cannot have a menu bar.
        ///     This style cannot be used with the <see cref="WS_POPUP"/> style.
        /// </summary>
        WS_CHILD = 0x40000000,

        /// <summary>
        ///     Same as the <see cref="WS_CHILD"/> style.
        /// </summary>
        WS_CHILDWINDOW = 0x40000000,

        /// <summary>
        ///     Excludes the area occupied by child windows when drawing occurs within the parent window.
        ///     This style is used when creating the parent window.
        /// </summary>
        WS_CLIPCHILDREN = 0x02000000,

        /// <summary>
        ///     Clips child windows relative to each other, that is, when a particular child window receives a
        ///     WM_PAINT message, the <see cref="WS_CLIPSIBLINGS"/> style clips all other overlapping child windows
        ///     out of the region of the child window to be updated. If <see cref="WS_CLIPSIBLINGS"/> is not specified
        ///     and child windows overlap, it is possible, when drawing within the client area of a child window,
        ///     to draw within the client area of a neighboring child window.
        /// </summary>
        WS_CLIPSIBLINGS = 0x04000000,

        /// <summary>
        ///     The window is initially disabled. A disabled window cannot receive input from the user.
        ///     To change this after a window has been created, use the <c>EnableWindow</c> function.
        /// </summary>
        WS_DISABLED = 0x08000000,

        /// <summary>
        ///     The window has a border of a style typically used with dialog boxes.
        ///     A window with this style cannot have a title bar.
        /// </summary>
        WS_DLGFRAME = 0x00400000,

        /// <summary>
        ///     <para>
        ///         The window is the first control of a group of controls. The group consists of this first control and
        ///         all controls defined after it, up to the next control with the <see cref="WS_GROUP"/> style.
        ///         The first control in each group usually has the <see cref="WS_TABSTOP"/> style so that the user
        ///         can move from group to group. The user can subsequently change the keyboard focus from one control
        ///         in the group to the next control in the group by using the direction keys.
        ///     </para>
        ///     <para>
        ///         You can turn this style on and off to change dialog box navigation. To change this style after
        ///         a window has been created, use the <c>SetWindowLong</c> function.
        ///     </para>
        /// </summary>
        WS_GROUP = 0x00020000,

        /// <summary>
        ///     The window has a horizontal scroll bar.
        /// </summary>
        WS_HSCROLL = 0x00100000,

        /// <summary>
        ///     The window is initially minimized. Same as the <see cref="WS_MINIMIZE"/> style.
        /// </summary>
        WS_ICONIC = 0x20000000,

        /// <summary>
        ///     The window is initially maximized.
        /// </summary>
        WS_MAXIMIZE = 0x01000000,

        /// <summary>
        ///     The window has a maximize button. Cannot be combined with the <see cref="ExtendedWindowStyles.WS_EX_CONTEXTHELP"/> style.
        ///     The <see cref="WS_SYSMENU"/> style must also be specified.
        /// </summary>
        WS_MAXIMIZEBOX = 0x00010000,

        /// <summary>
        ///     The window is initially minimized. Same as the <see cref="WS_ICONIC"/> style.
        /// </summary>
        WS_MINIMIZE = 0x20000000,

        /// <summary>
        ///     The window has a minimize button. Cannot be combined with the <see cref="ExtendedWindowStyles.WS_EX_CONTEXTHELP"/> style.
        ///     The <see cref="WS_SYSMENU"/> style must also be specified.
        /// </summary>
        WS_MINIMIZEBOX = 0x00020000,

        /// <summary>
        ///     The window is an overlapped window. An overlapped window has a title bar and a border.
        ///     Same as the <see cref="WS_TILED"/> style.
        /// </summary>
        WS_OVERLAPPED = 0x00000000,

        /// <summary>
        ///     The window is an overlapped window. Same as the <see cref="WS_TILEDWINDOW"/> style.
        /// </summary>
        WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),

        /// <summary>
        ///     The windows is a pop-up window. This style cannot be used with the <see cref="WS_CHILD"/> style.
        /// </summary>
        WS_POPUP = 0x80000000,

        /// <summary>
        ///     The window is a pop-up window. The <see cref="WS_CAPTION"/> and <see cref="WS_POPUPWINDOW"/> styles
        ///     must be combined to make the window menu visible.
        /// </summary>
        WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),

        /// <summary>
        ///     The window has a sizing border. Same as the <see cref="WS_THICKFRAME"/> style.
        /// </summary>
        WS_SIZEBOX = 0x00040000,

        /// <summary>
        ///     The window has a window menu on its title bar. The <see cref="WS_CAPTION"/> style must also be specified.
        /// </summary>
        WS_SYSMENU = 0x00080000,

        /// <summary>
        ///     <para>
        ///         The window is a control that can receive the keyboard focus when the user presses the TAB key.
        ///         Pressing the TAB key changes the keyboard focus to the next control with the <see cref="WS_TABSTOP"/> style.
        ///     </para>
        ///     <para>
        ///         You can turn this style on and off to change dialog box navigation.
        ///         To change this style after a window has been created, use the <c>SetWindowLong</c> function.
        ///         For user-created windows and modeless dialogs to work with tab stops, alter the message loop
        ///         to call the <c>IsDialogMessage</c> function.
        ///     </para>
        /// </summary>
        WS_TABSTOP = 0x00010000,

        /// <summary>
        ///     The window has a sizing border. Same as the <see cref="WS_SIZEBOX"/> style.
        /// </summary>
        WS_THICKFRAME = 0x00040000,

        /// <summary>
        ///     The window is an overlapped window. An overlapped window has a title bar and a border.
        ///     Same as the <see cref="WS_OVERLAPPED"/> style.
        /// </summary>
        WS_TILED = 0x00000000,

        /// <summary>
        ///     The window is an overlapped window. Same as the <see cref="WS_OVERLAPPEDWINDOW"/> style.
        /// </summary>
        WS_TILEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),

        /// <summary>
        ///     The window is initially visible.
        ///     This style can be turned on and off by using the ShowWindow or SetWindowPos function.
        /// </summary>
        WS_VISIBLE = 0x10000000,

        /// <summary>
        ///     The window has a vertical scroll bar.
        /// </summary>
        WS_VSCROLL = 0x00200000
    }

    public static class WindowStylesExtensions
    {
        public static Int32 ToInt32(this WindowStyles windowStyles)
        {
            return (Int32) windowStyles;
        }

        public static UInt32 ToUInt32(this WindowStyles windowStyles)
        {
            return (UInt32) windowStyles;
        }
    }
}
