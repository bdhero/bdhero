using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using TextEditor.Extensions;

namespace TextEditor.WinForms
{
    internal class TextEditorImpl : ITextEditor
    {
        private readonly ICSharpCode.TextEditor.TextEditorControl _editor = new ICSharpCode.TextEditor.TextEditorControl();

        private readonly TextEditorOptionsImpl _options;
        private readonly MultilineHelper _multilineHelper;

        public TextEditorImpl()
        {
            _editor.TextChanged += EditorOnTextChanged;
            _editor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextAreaOnKeyEventHandler;
            _editor.ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextAreaOnDoProcessDialogKey;
            _editor.ActiveTextAreaControl.TextArea.PreviewKeyDown += TextAreaOnPreviewKeyDown;

            _options = new TextEditorOptionsImpl(_editor);

            _multilineHelper = new MultilineHelper(this, NotifyTextChanged);
        }

        ITextEditorOptions ITextEditor.Options
        {
            get { return _options; }
        }

        public Control Control
        {
            get { return _editor; }
        }

        #region Text

        public string Text
        {
            get { return _editor.Text; }
            set
            {
                var newValue = MultilineHelper.StripNewlines(value ?? "");
                if (newValue == Text)
                    return;

                _editor.Text = newValue;
            }
        }

        public event EventHandler TextChanged;

        private void EditorOnTextChanged(object sender, EventArgs e)
        {
            _multilineHelper.TextChanged();
        }

        private void NotifyTextChanged()
        {
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Input events

        /// <returns>
        ///     <c>true</c> to prevent the default editor action for the given key, or <c>false</c> to allow it.
        /// </returns>
        private bool TextAreaOnKeyEventHandler(char ch)
        {
            return ShouldPreventEnterKey(ch);
        }

        /// <returns>
        ///     <c>true</c> to prevent the default editor action for the given key, or <c>false</c> to allow it.
        /// </returns>
        private bool TextAreaOnDoProcessDialogKey(Keys data)
        {
            return ShouldPreventTabKey(data);
        }

        private void TextAreaOnPreviewKeyDown(object sender, PreviewKeyDownEventArgs args)
        {
            if (ShouldPreventTabKey(args.KeyData))
            {
                _editor.SelectNextControl(!args.Shift);
            }
        }

        private bool ShouldPreventEnterKey(char ch)
        {
            return !Multiline && ch == '\n';
        }

        private bool ShouldPreventTabKey(Keys data)
        {
            return !Multiline && (data == Keys.Tab || data == (Keys.Tab | Keys.Shift));
        }

        #endregion

        #region Font

        public double FontSize
        {
            get { return _editor.Font.Size; }
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == FontSize)
                    return;

                var font = _editor.Font;

                _editor.Font = new Font(font.FontFamily.Name,
                                        (float) GetWinFormsFontSize(value),
                                        font.Style,
                                        GraphicsUnit.Point,
                                        font.GdiCharSet,
                                        font.GdiVerticalFont);

                OnFontSizeChanged();
            }
        }

        public event EventHandler FontSizeChanged;

        private void OnFontSizeChanged()
        {
            if (FontSizeChanged != null)
                FontSizeChanged(this, EventArgs.Empty);
        }

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
        private static double GetWinFormsFontSize(double wpfFontSize)
        {
            return wpfFontSize * 72.0 / 96.0;
        }

        #endregion

        #region Multiline

        public bool Multiline
        {
            get
            {
                return _editor.HorizontalScroll.Enabled &&
                       _editor.VerticalScroll.Enabled &&

                       _editor.ActiveTextAreaControl.HScrollBar.Enabled &&
                       _editor.ActiveTextAreaControl.VScrollBar.Enabled &&

                       _editor.ActiveTextAreaControl.HScrollBar.Visible &&
                       _editor.ActiveTextAreaControl.VScrollBar.Visible;
            }
            set
            {
                var changed = value != Multiline;

                _editor.HorizontalScroll.Enabled =
                    _editor.VerticalScroll.Enabled =

                    _editor.ActiveTextAreaControl.HScrollBar.Enabled =
                    _editor.ActiveTextAreaControl.VScrollBar.Enabled =

                    _editor.ActiveTextAreaControl.HScrollBar.Visible =
                    _editor.ActiveTextAreaControl.VScrollBar.Visible = value;

                if (changed)
                {
                    OnMultilineChanged();
                }
            }
        }

        public event EventHandler MultilineChanged;

        private void OnMultilineChanged()
        {
            _multilineHelper.MultilineChanged();

            if (MultilineChanged != null)
                MultilineChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Read Only

        public bool ReadOnly
        {
            get { return _editor.IsReadOnly; }
            set
            {
                if (value == ReadOnly)
                    return;

                _editor.IsReadOnly = value;

                OnReadOnlyChanged();
            }
        }

        public event EventHandler ReadOnlyChanged;

        private void OnReadOnlyChanged()
        {
            if (ReadOnlyChanged != null)
                ReadOnlyChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Load/save

        public void Load(Stream stream)
        {
            // TODO: FIXME
            _editor.LoadFile("FIXME.md", stream, true, true);
        }

        public void Load(string filePath)
        {
            _editor.LoadFile(filePath, true, true);
        }

        public void Save(Stream stream)
        {
            _editor.SaveFile(stream);
        }

        public void Save(string filePath)
        {
            _editor.SaveFile(filePath);
        }

        #endregion

        #region Syntax highlighting

        public void SetSyntaxFromExtension(string fileNameOrExtension)
        {
            var filename = fileNameOrExtension ?? string.Empty;

            // Dot-qualified extension
            if (new Regex(@"^\.\w+$").IsMatch(filename))
                filename = "abc" + filename;

            // Extension name only (no dot)
            if (new Regex(@"^\w+$").IsMatch(filename))
                filename = "abc." + filename;

            if (string.IsNullOrEmpty(Path.GetExtension(filename)))
                throw new ArgumentException("fileNameOrExtension does not contain a valid filename or extension");

            var manager = HighlightingManager.Manager;
            var highlighter = manager.FindHighlighterForFile(filename);
            _editor.SetHighlighting(highlighter.Name);
        }

        #endregion

        #region Selection

        public void SelectAll()
        {
            _editor.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(this, EventArgs.Empty);
        }

        private List<ISelection> Selections
        {
            get { return _editor.ActiveTextAreaControl.SelectionManager.SelectionCollection; }
        }

        public string SelectedText
        {
            get { return string.Join("\n", Selections.Select(selection => selection.SelectedText)); }
            set
            {
                var selections = Selections.ToList();
                selections.Reverse();
                selections.ForEach(selection => _editor.Document.Replace(selection.Offset, selection.Length, value));
            }
        }

        public int SelectionLength
        {
            get { return Selections.Any() ? Selections.Sum(selection => selection.Length) : 0; }
        }

        #endregion

        #region History

        public bool CanUndo
        {
            get { return _editor.Document.UndoStack.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _editor.Document.UndoStack.CanRedo; }
        }

        public void Undo()
        {
            _editor.Undo();
        }

        public void Redo()
        {
            _editor.Redo();
        }

        public void ClearHistory()
        {
            _editor.Document.UndoStack.ClearAll();
        }

        #endregion

        #region Clear/delete

        public bool CanClear
        {
            get { return !ReadOnly && Text.Length > 0; }
        }

        public void Clear()
        {
            SelectAll();
            Delete();
        }

        public bool CanDelete
        {
            get { return !ReadOnly && SelectionLength > 0; }
        }

        public void Delete()
        {
            _editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(this, EventArgs.Empty);
        }

        #endregion

        #region Clipboard

        public bool CanCut
        {
            get { return !ReadOnly && SelectionLength > 0; }
        }

        public bool CanCopy
        {
            get { return SelectionLength > 0; }
        }

        public bool CanPaste
        {
            get { return !ReadOnly; }
        }

        public void Cut()
        {
            _editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(this, EventArgs.Empty);
        }

        public void Copy()
        {
            _editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(this, EventArgs.Empty);
        }

        public void Paste()
        {
            _editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(this, EventArgs.Empty);
        }

        #endregion

        #region Informational

        public bool IsModified
        {
            get { return false; }
        }

        public int LineCount
        {
            get { return _editor.ActiveTextAreaControl.Document.TotalNumberOfLines; }
        }

        #endregion
    }
}
