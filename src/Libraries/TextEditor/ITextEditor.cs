using System;
using System.IO;
using System.Windows.Forms;
using TextEditor.SyntaxHighlighting.Providers;

namespace TextEditor
{
    public interface ITextEditor
    {
        #region Core

        ITextEditorOptions Options { get; }

        Control Control { get; }

        #endregion

        #region Properties + Events

        string Text { get; set; }

        event EventHandler TextChanged;

        /// <summary>
        ///     WPF font size.  Use the <see cref="FontSizeConverter"/> class to convert between Windows Forms and WPF font sizes.
        /// </summary>
        double FontSize { get; set; }

        event EventHandler FontSizeChanged;

        bool Multiline { get; set; }

        event EventHandler MultilineChanged;

        bool ReadOnly { get; set; }

        event EventHandler ReadOnlyChanged;

        // TODO: Rename these properties to reflect how they are actually used
        int HorizontalScrollBarHeight { get; }

        int VerticalScrollBarWidth { get; }

        #endregion

        #region Load/save

        void Load(Stream stream);

        void Load(string filePath);

        void Save(Stream stream);

        void Save(string filePath);

        #endregion

        #region Syntax highlighting

        void LoadSyntaxDefinitions(ISyntaxModeProvider syntaxModeFileProvider);

        void SetSyntax(StandardSyntaxType type);

        void SetSyntaxFromExtension(string fileNameOrExtension);

        #endregion

        #region Selection

        void SelectAll();

        string SelectedText { get; set; }

        int SelectionLength { get; }

        #endregion

        #region History

        bool CanUndo { get; }

        bool CanRedo { get; }

        void Undo();

        void Redo();

        void ClearHistory();

        #endregion

        #region Clear/delete

        bool CanClear { get; }

        void Clear();

        bool CanDelete { get; }

        void Delete();

        #endregion

        #region Clipboard

        bool CanCut { get; }
        
        bool CanCopy { get; }

        bool CanPaste { get; }

        void Cut();

        void Copy();

        void Paste();

        #endregion

        #region Informational

        bool IsModified { get; }

        int LineCount { get; }

        #endregion
    }
}
