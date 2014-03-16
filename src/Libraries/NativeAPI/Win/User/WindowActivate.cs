// ReSharper disable InconsistentNaming
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     Static class containing values for the WPARAM field of <see cref="WindowMessageType.WM_ACTIVATE"/> messages.
    /// </summary>
    public static class WindowActivate
    {
        /// <summary>
        ///     Activated by some method other than a mouse click (for example, by a call to the SetActiveWindow function or by use of the keyboard interface to select the window).
        /// </summary>
        private const int WA_ACTIVE = 1;

        /// <summary>
        ///     Activated by a mouse click.
        /// </summary>
        private const int WA_CLICKACTIVE = 2;

        /// <summary>
        ///     Deactivated.
        /// </summary>
        private const int WA_INACTIVE = 0;

    }
}
