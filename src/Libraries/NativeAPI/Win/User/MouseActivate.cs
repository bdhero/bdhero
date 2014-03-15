// ReSharper disable InconsistentNaming
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     Static class containing values that specify whether the window should be activated and whether the identifier of the mouse message should be discarded
    ///     in response to a <see cref="WindowMessageType.WM_MOUSEACTIVATE"/> message.
    /// </summary>
    public static class MouseActivate
    {
        /// <summary>
        ///     Activates the window, and does not discard the mouse message.
        /// </summary>
        public const int MA_ACTIVATE = 1;

        /// <summary>
        ///     Activates the window, and discards the mouse message.
        /// </summary>
        public const int MA_ACTIVATEANDEAT = 2;

        /// <summary>
        ///     Does not activate the window, and does not discard the mouse message.
        /// </summary>
        public const int MA_NOACTIVATE = 3;

        /// <summary>
        ///     Does not activate the window, but discards the mouse message.
        /// </summary>
        public const int MA_NOACTIVATEANDEAT = 4;

    }
}
