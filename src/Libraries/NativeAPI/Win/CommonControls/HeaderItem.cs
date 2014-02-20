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
namespace NativeAPI.Win.CommonControls
{
    /// <summary>
    ///     Contains information about an item in a header control. This structure supersedes the HD_ITEM structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HDITEM
    {
        /// <summary>
        ///     Flags indicating which other structure members contain valid data or must be filled in.
        /// </summary>
        public HeaderItemMask mask;

        /// <summary>
        ///     The width or height of the item.
        /// </summary>
        public int cxy;

        /// <summary>
        ///     A pointer to an item string. If the text is being retrieved from the control, this member must be
        ///     initialized to point to a character buffer. If this member is set to LPSTR_TEXTCALLBACK, the control
        ///     will request text information for this item by sending an HDN_GETDISPINFO notification code.
        ///     Note that although the header control allows a string of any length to be stored as item text,
        ///     only the first 260 TCHARs are displayed.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszText;

        /// <summary>
        ///     A handle to the item bitmap.
        /// </summary>
        public IntPtr hbm;

        /// <summary>
        ///     The length of the item string, in TCHARs. If the text is being retrieved from the control, this member
        ///     must contain the number of TCHARs at the address specified by pszText.
        /// </summary>
        public int cchTextMax;

        /// <summary>
        ///     Flags that specify the item's format.
        /// </summary>
        public HeaderItemFormat fmt;

        /// <summary>
        ///     Application-defined item data.
        /// </summary>
        public IntPtr lParam;

        /// <summary>
        ///     Version 4.70. The zero-based index of an image within the image list. The specified image will be
        ///     displayed in the header item in addition to any image specified in the hbm field. If iImage is set to
        ///     I_IMAGECALLBACK, the control requests text information for this item by using an HDN_GETDISPINFO
        ///     notification code. To clear the image, set this value to I_IMAGENONE.
        /// </summary>
        public int iImage;

        /// <summary>
        ///     Version 4.70. The order in which the item appears within the header control, from left to right.
        ///     That is, the value for the far left item is 0. The value for the next item to the right is 1, and so on.
        /// </summary>
        public int iOrder;

        /// <summary>
        ///     Version 5.80. The type of filter specified by pvFilter.
        /// </summary>
        public uint type;

        /// <summary>
        ///     Version 5.80. The address of an application-defined data item. The data filter type is determined by
        ///     setting the flag value of the member. Use the HDFT_ISSTRING flag to indicate a string and HDFT_ISNUMBER
        ///     to indicate an integer. When the HDFT_ISSTRING flag is used pvFilter is a pointer to a HDTEXTFILTER structure.
        /// </summary>
        public IntPtr pvFilter;

        /// <summary>
        ///     The state. The only valid, supported value for this member is HDIS_FOCUSED.
        /// </summary>
        public uint state;
    }
}
