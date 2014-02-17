using System;

// ReSharper disable InconsistentNaming
namespace WinAPI.CommonControls
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