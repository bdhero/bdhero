// ReSharper disable InconsistentNaming
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     Static class containing the symbolic names and hexadecimal values of dialog codes used by <see cref="WindowMessageType.WM_GETDLGCODE"/>.
    /// </summary>
    public static class DialogCode
    {
        /// <summary>
        ///     Direction keys (up, down, left, right).
        /// </summary>
        public const int DLGC_WANTARROWS = 0x0001;

        /// <summary>
        ///     <kbd>TAB</kbd> key.
        /// </summary>
        public const int DLGC_WANTTAB = 0x0002;

        /// <summary>
        ///     All keyboard input.  Alias for <see cref="DLGC_WANTMESSAGE"/>.
        /// </summary>
        /// <seealso cref="http://blogs.msdn.com/b/oldnewthing/archive/2007/06/26/3532603.aspx"/>
        public const int DLGC_WANTALLKEYS = 0x0004;

        /// <summary>
        ///     All keyboard input (the application passes this message in the <see cref="System.Windows.Forms.Message"/> structure to the control).
        ///     Alias for <see cref="DLGC_WANTALLKEYS"/>.
        /// </summary>
        /// <seealso cref="http://blogs.msdn.com/b/oldnewthing/archive/2007/06/26/3532603.aspx"/>
        public const int DLGC_WANTMESSAGE = 0x0004;

        /// <summary>
        ///     <c>EM_SETSEL</c> messages.
        /// </summary>
        public const int DLGC_HASSETSEL = 0x0008;

        /// <summary>
        ///     Default push button.
        /// </summary>
        public const int DLGC_DEFPUSHBUTTON = 0x0010;

        /// <summary>
        ///     Non-default push button.
        /// </summary>
        public const int DLGC_UNDEFPUSHBUTTON = 0x0020;

        /// <summary>
        ///     Radio button.
        /// </summary>
        public const int DLGC_RADIOBUTTON = 0x0040;

        /// <summary>
        ///     <see cref="WindowMessageType.WM_CHAR"/> messages.
        /// </summary>
        public const int DLGC_WANTCHARS = 0x0080;

        /// <summary>
        ///     Static control.
        /// </summary>
        public const int DLGC_STATIC = 0x0100;

        /// <summary>
        ///     Button.
        /// </summary>
        public const int DLGC_BUTTON = 0x2000;
    }
}
