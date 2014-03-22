using System;
using System.IO;
using System.Windows.Forms;
using TextEditor.SyntaxHighlighting.Providers;

namespace TextEditor
{
    /// <summary>
    ///     Interface for a text editor that provides common features such as syntax highlighting.
    /// </summary>
    public interface ITextEditor
    {
        #region Core

        /// <summary>
        ///     Gets user-configurable options that customize the appearance and behavior of this text editor.
        /// </summary>
        ITextEditorOptions Options { get; }

        /// <summary>
        ///     Gets the Windows Forms host control that contains this text editor.
        /// </summary>
        Control Control { get; }

        #endregion

        #region Properties + Events

        /// <summary>
        ///     Gets or sets the editor's text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        ///     Triggered whenever the value of the <see cref="Text"/> property changes.
        /// </summary>
        event EventHandler TextChanged;

        /// <summary>
        ///     WPF font size.  Use the <see cref="FontSizeConverter"/> class to convert between Windows Forms and WPF font sizes.
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        ///     Triggered whenever the value of the <see cref="FontSize"/> property changes.
        /// </summary>
        event EventHandler FontSizeChanged;

        /// <summary>
        ///     Gets or sets whether the text editor should allow multiple lines of text.
        /// </summary>
        bool Multiline { get; set; }

        /// <summary>
        ///     Triggered whenever the value of the <see cref="Multiline"/> property changes.
        /// </summary>
        event EventHandler MultilineChanged;

        /// <summary>
        ///     Gets or sets whether the text editor is in "read only" mode, which prevents the user from making any changes
        ///     but still allows programmatic manipulation of the editor's text.
        /// </summary>
        bool ReadOnly { get; set; }

        /// <summary>
        ///     Triggered whenever the value of the <see cref="ReadOnly"/> property changes.
        /// </summary>
        event EventHandler ReadOnlyChanged;

        // TODO: Rename these properties to reflect how they are actually used
        int HorizontalScrollBarHeight { get; }

        int VerticalScrollBarWidth { get; }

        #endregion

        #region Load/save

        /// <summary>
        ///     Loads the text contents of the specified <paramref name="stream"/> into the editor.
        /// </summary>
        /// <param name="stream"></param>
        void Load(Stream stream);

        /// <summary>
        ///     Loads the contents of the specified <paramref name="filePath"/> into the editor.
        /// </summary>
        /// <param name="filePath">
        ///     Path to a text file.
        /// </param>
        void Load(string filePath);

        /// <summary>
        ///     Writes the contents of the editor to the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        void Save(Stream stream);

        /// <summary>
        ///     Writes the contents of the editor to the specified <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath"></param>
        void Save(string filePath);

        #endregion

        #region Syntax highlighting

        /// <summary>
        ///     Loads syntax highlighting definitions from the given <paramref name="provider"/> and makes them
        ///     available for future use via <see cref="SetSyntax"/> and <see cref="SetSyntaxFromExtension"/>.
        /// </summary>
        /// <param name="provider"></param>
        void LoadSyntaxDefinitions(ISyntaxModeProvider provider);

        /// <summary>
        ///     Sets the editor's syntax highlighter to the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        void SetSyntax(StandardSyntaxType type);

        /// <summary>
        ///     Attempts to find and use a syntax highlighter for the given <paramref name="fileNameOrExtension"/>.
        /// </summary>
        /// <param name="fileNameOrExtension"></param>
        void SetSyntaxFromExtension(string fileNameOrExtension);

        #endregion

        #region Selection

        /// <summary>
        ///     Selects all text within the editor.
        /// </summary>
        void SelectAll();

        /// <summary>
        ///     Gets or sets the contents of the current text selection.
        /// </summary>
        string SelectedText { get; set; }

        /// <summary>
        ///     Gets the number of characters currently selected in the editor.
        /// </summary>
        int SelectionLength { get; }

        #endregion

        #region History

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to undo a previous edit.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to redo a previous edit.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        ///     Undoes the previous edit.  This method may still be invoked programmatically even if <see cref="CanUndo"/> is <c>false</c>.
        /// </summary>
        void Undo();

        /// <summary>
        ///     Redoes the previous edit.  This method may still be invoked programmatically even if <see cref="CanRedo"/> is <c>false</c>.
        /// </summary>
        void Redo();

        /// <summary>
        ///     Clears the edit history so that the user cannot undo or redo any previous edits.
        /// </summary>
        void ClearHistory();

        #endregion

        #region Clear/delete

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to clear the contents of the editor.
        /// </summary>
        bool CanClear { get; }

        /// <summary>
        ///     Clears the contents of the editor, setting <see cref="Text"/> to <see cref="String.Empty"/>.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to delete the text at the current caret position.
        /// </summary>
        bool CanDelete { get; }

        /// <summary>
        ///     Deletes the selected text or, if no text is currently selected, the character immediately to the right of the
        ///     current caret position.
        /// </summary>
        void Delete();

        #endregion

        #region Clipboard

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to cut text from the editor.
        /// </summary>
        bool CanCut { get; }

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to copy text from the editor.
        /// </summary>
        bool CanCopy { get; }

        /// <summary>
        ///     Gets whether the user is both <b>able</b> and <b>allowed</b> to paste text into the editor.
        /// </summary>
        bool CanPaste { get; }

        /// <summary>
        ///     Cuts the currently selected text or line.
        /// </summary>
        void Cut();

        /// <summary>
        ///     Copies the currently selected text or line.
        /// </summary>
        void Copy();

        /// <summary>
        ///     Replaces the currently selected text with the contents of the clipboard or,
        ///     if no text is currently selected, inserts the contents of the clipboard at the current caret position.
        /// </summary>
        void Paste();

        #endregion

        #region Informational

        /// <summary>
        ///     Gets whether the user has modified the text contents of the editor since the last time <see cref="Text"/>
        ///     was set programmatically or <see cref="Load(System.IO.Stream)"/> or <see cref="Load(string)"/> was called.
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        ///     Gets the number of lines of text in the editor.
        /// </summary>
        int LineCount { get; }

        #endregion
    }
}
