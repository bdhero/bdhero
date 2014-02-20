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
namespace NativeAPI.Win.CommonControls
{
    /// <summary>
    ///     Flags that specify a <see cref="HDITEM"/>'s format.
    /// </summary>
    [Flags]
    public enum HeaderItemFormat
    {
        /// <summary>
        ///     Version 6.00. Draws a down-arrow on this item. This is typically used to indicate that information
        ///     in the current window is sorted on this column in descending order. This flag cannot be combined with
        ///     HDF_IMAGE or HDF_BITMAP.
        /// </summary>
        HDF_SORTDOWN = 0x200,

        /// <summary>
        ///     Version 6.00. Draws an up-arrow on this item. This is typically used to indicate that information
        ///     in the current window is sorted on this column in ascending order. This flag cannot be combined with
        ///     HDF_IMAGE or HDF_BITMAP.
        /// </summary>
        HDF_SORTUP = 0x400,
    }
}