namespace NativeAPI.Win.UXTheme
{
    /// <summary>
    ///     Enum containing the various states that a text box input control can be in.
    /// </summary>
    public enum TextBoxState
    {
        /// <summary>
        ///     Default text box that is not disabled, focused, or hot (hovered).
        /// </summary>
        Normal,

        /// <summary>
        ///     The text box does not accept user input.
        /// </summary>
        Disabled,

        /// <summary>
        ///     The mouse cursor is hovered over the text box, but the text box does not yet have focus.
        /// </summary>
        Hot,

        /// <summary>
        ///     The text box currently has focus.
        /// </summary>
        Focused,
    }
}