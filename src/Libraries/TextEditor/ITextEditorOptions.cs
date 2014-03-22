namespace TextEditor
{
    /// <summary>
    ///     Defines user-configurable options that customize the appearance and behavior of an <see cref="ITextEditor"/>.
    /// </summary>
    public interface ITextEditorOptions
    {
        #region Line numbers

        /// <summary>
        ///     Gets or sets whether line numbers are shown on the left side of the editor.
        /// </summary>
        bool ShowLineNumbers { get; set; }

        #endregion

        #region Column ruler

        /// <summary>
        ///     Gets or sets whether a ruler is displayed in the editor at the position specified by <see cref="ColumnRulerPosition"/>.
        /// </summary>
        bool ShowColumnRuler { get; set; }

        /// <summary>
        ///     Gets or sets the column index at which a ruler will be displayed in the editor when <see cref="ShowColumnRuler"/> is <c>true</c>.
        /// </summary>
        int ColumnRulerPosition { get; set; }

        /// <summary>
        ///     Gets or sets whether pressing <kbd>Ctrl</kbd> + <kbd>C</kbd> in the editor when no text is selected
        ///     automatically copies the entire line.
        /// </summary>
        bool CutCopyWholeLine { get; set; }

        #endregion

        #region Indentation

        /// <summary>
        ///     Gets or sets the indentation size of tab characters, as well as the number of spaces that will be inserted
        ///     when the user presses the <kbd>Tab</kbd> key and <see cref="ConvertTabsToSpaces"/> is <c>true</c>.
        /// </summary>
        int IndentationSize { get; set; }

        /// <summary>
        ///     Gets or sets whether pressing the <kbd>Tab</kbd> key should insert a sequence of space characters instead of a hard tabstop character (<c>\t</c>).
        /// </summary>
        bool ConvertTabsToSpaces { get; set; }

        #endregion

        #region Character visualization

        /// <summary>
        ///     Gets or sets whether space characters are visualized in the editor.
        /// </summary>
        bool ShowSpaces { get; set; }

        /// <summary>
        ///     Gets or sets whether tab characters (<c>\t</c>) are visualized in the editor.
        /// </summary>
        bool ShowTabs { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="ShowSpaces"/> and <see cref="ShowTabs"/> simultaneously.
        /// </summary>
        bool ShowWhiteSpace { get; set; }

        #endregion

        #region Word wrapping

        /// <summary>
        ///     Gets whether the underlying text editor supports word wrapping.
        /// </summary>
        bool SupportsWordWrap { get; }

        /// <summary>
        ///     Gets or sets whether the text editor should wrap long lines.
        /// </summary>
        bool WordWrap { get; set; }

        /// <summary>
        ///     Gets or sets the size of the word wrap indent in pixels when <see cref="WordWrap"/> is <c>true</c>.
        /// </summary>
        double WordWrapIndent { get; set; }

        #endregion
    }
}
