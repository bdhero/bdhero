using System;

// ReSharper disable InconsistentNaming
namespace WinAPI.CommonControls
{
    /// <summary>
    ///     Flags indicating which other <see cref="HDITEM"/> structure members contain valid data or must be filled in.
    /// </summary>
    [Flags]
    public enum HeaderItemMask
    {
        /// <summary>
        ///     The <see cref="HDITEM.fmt"/> member is valid.
        /// </summary>
        HDI_FORMAT = 0x4,
    }
}