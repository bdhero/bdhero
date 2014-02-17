// Copyright 2012-2014 Andrew C. Dvorak
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

// ReSharper disable InconsistentNaming
namespace WinAPI.User
{
    /// <summary>
    ///     Enum containing the standard Windows cursors.
    /// </summary>
    public enum CursorType
    {
        /// <summary>
        ///     Standard arrow and small hourglass.
        /// </summary>
        IDC_APPSTARTING = 32650,

        /// <summary>
        ///     Standard arrow.
        /// </summary>
        IDC_ARROW = 32512,

        /// <summary>
        ///     Crosshair.
        /// </summary>
        IDC_CROSS = 32515,

        /// <summary>
        ///     Hand.
        /// </summary>
        IDC_HAND = 32649,

        /// <summary>
        ///     Arrow and question mark.
        /// </summary>
        IDC_HELP = 32651,

        /// <summary>
        ///     I-beam.
        /// </summary>
        IDC_IBEAM = 32513,

        /// <summary>
        ///     Obsolete for applications marked version 4.0 or later.
        /// </summary>
        IDC_ICON = 32641,

        /// <summary>
        ///     Slashed circle.
        /// </summary>
        IDC_NO = 32648,

        /// <summary>
        ///     Obsolete for applications marked version 4.0 or later. Use <see cref="IDC_SIZEALL"/>.
        /// </summary>
        IDC_SIZE = 32640,

        /// <summary>
        ///     Four-pointed arrow pointing north, south, east, and west.
        /// </summary>
        IDC_SIZEALL = 32646,

        /// <summary>
        ///     Double-pointed arrow pointing northeast and southwest.
        /// </summary>
        IDC_SIZENESW = 32643,

        /// <summary>
        ///     Double-pointed arrow pointing north and south.
        /// </summary>
        IDC_SIZENS = 32645,

        /// <summary>
        ///     Double-pointed arrow pointing northwest and southeast.
        /// </summary>
        IDC_SIZENWSE = 32642,

        /// <summary>
        ///     Double-pointed arrow pointing west and east.
        /// </summary>
        IDC_SIZEWE = 32644,

        /// <summary>
        ///     Vertical arrow.
        /// </summary>
        IDC_UPARROW = 32516,

        /// <summary>
        ///     Hourglass.
        /// </summary>
        IDC_WAIT = 32514,
    }
}
