namespace TextEditor
{
    /// <summary>
    ///     Static utility class to convert between Windows Forms and WPF font sizes.
    /// </summary>
    internal static class FontSizeConverter
    {
        private const double Ratio = 72.0 / 96.0;

        /// <summary>
        ///     Converts a WPF font size to its Windows Forms equivalent.
        /// </summary>
        /// <param name="wpfFontSize">
        ///     WPF font size measured in points.
        /// </param>
        /// <returns>
        ///     Windows Forms equivalent of <paramref name="wpfFontSize"/>.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Font size in WPF is expressed as one ninety-sixth of an inch, and in Windows Forms as one seventy-second of an inch.
        ///     </para>
        ///     <para>
        ///         The corresponding conversion is:
        ///     </para>
        ///     <code>
        ///         Windows Forms font size = WPF font size * 72.0 / 96.0
        ///     </code>
        /// </remarks>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms751565(v=vs.100).aspx"/>
        public static double GetWinFormsFontSize(double wpfFontSize)
        {
            return wpfFontSize * Ratio;
        }

        /// <summary>
        ///     Converts a Windows Forms font size to its WPF equivalent.
        /// </summary>
        /// <param name="winFormsFontSize">
        ///     WPF font size measured in points.
        /// </param>
        /// <returns>
        ///     WPF equivalent of <paramref name="winFormsFontSize"/>.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Font size in WPF is expressed as one ninety-sixth of an inch, and in Windows Forms as one seventy-second of an inch.
        ///     </para>
        ///     <para>
        ///         The corresponding conversion is:
        ///     </para>
        ///     <code>
        ///         Windows Forms font size = WPF font size * 72.0 / 96.0
        ///     </code>
        /// </remarks>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms751565(v=vs.100).aspx"/>
        public static double GetWpfFontSize(double winFormsFontSize)
        {
            return winFormsFontSize / Ratio;
        }
    }
}
