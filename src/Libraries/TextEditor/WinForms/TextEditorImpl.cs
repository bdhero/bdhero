using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DotNetUtils.Extensions;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using TextEditor.Extensions;
using TextEditor.Resources;
using TextEditor.Resources.Syntax.Providers;

namespace TextEditor.WinForms
{
    internal class TextEditorImpl : ITextEditor
    {
        private readonly ICSharpCode.TextEditor.TextEditorControl _editor = new ICSharpCode.TextEditor.TextEditorControl();

        private readonly TextEditorOptionsImpl _options;
        private readonly MultilineHelper _multilineHelper;

        private bool Singleline
        {
            get { return !Multiline; }
        }

        public TextEditorImpl()
        {
            _editor.TextChanged += EditorOnTextChanged;
            _editor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextAreaOnKeyEventHandler;
            _editor.ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextAreaOnDoProcessDialogKey;
            _editor.ActiveTextAreaControl.TextArea.PreviewKeyDown += TextAreaOnPreviewKeyDown;

            _options = new TextEditorOptionsImpl(_editor);

            _multilineHelper = new MultilineHelper(this, NotifyTextChanged);

            LoadSyntaxDefinitions(new SmartResourceSyntaxModeProvider("MarkDown-Mode.xshd"));

            RegisterIntellisenseHandling();
        }

        ITextEditorOptions ITextEditor.Options
        {
            get { return _options; }
        }

        public Control Control
        {
            get { return _editor; }
        }

        #region Code completion

        private CodeCompletionWindow _codeCompletionWindow;
        private IContainer _components;
        private ImageList _intellisenseImageList;

        private void RegisterIntellisenseHandling()
        {
            _components = new Container();
            _intellisenseImageList = new ImageList(_components);
            _intellisenseImageList.Images.Add(Properties.Resources.property_blue_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.terminal_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.tag_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.property_image_16);

            var textArea = _editor.ActiveTextAreaControl.TextArea;
            textArea.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Control == false)
                    return;
                if (e.KeyCode != Keys.Space)
                    return;
                e.SuppressKeyPress = true;
                ShowCodeCompletion((char)e.KeyValue);
            };
        }

        private void ShowCodeCompletion(char value)
        {
            ICompletionDataProvider completionDataProvider = new CodeCompletionProviderImpl(_intellisenseImageList);

            _codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    Control.FindForm(),     // The parent window for the completion window
                    _editor,                // The text editor to show the window for
                    "",                     // Filename - will be passed back to the provider
                    completionDataProvider, // Provider to get the list of possible completions
                    value                   // Key pressed - will be passed to the provider
                );

            // ShowCompletionWindow can return null when the provider returns an empty list
            if (_codeCompletionWindow == null)
                return;

            _codeCompletionWindow.Closed += CloseCodeCompletionWindow;

            var completions = completionDataProvider.GenerateCompletionData("", _editor.ActiveTextAreaControl.TextArea, value) ?? new ICompletionData[0];
            
            if (!completions.Any())
                return;

            _codeCompletionWindow.MouseWheel += CodeCompletionWindowOnMouseWheel;

            using (var g = _codeCompletionWindow.CreateGraphics())
            {
                var width = (int) completions.Select(data => g.MeasureString(data.Text, _codeCompletionWindow.Font).Width).Max();

                width += 16; // Icon size
                width += SystemInformation.VerticalScrollBarWidth;

                _codeCompletionWindow.Width = width;
            }
        }

        private void CodeCompletionWindowOnMouseWheel(object sender, MouseEventArgs args)
        {
            _codeCompletionWindow.HandleMouseWheel(args);
        }

        private void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (_codeCompletionWindow != null)
            {
                _codeCompletionWindow.Closed -= CloseCodeCompletionWindow;
                _codeCompletionWindow.MouseWheel -= CodeCompletionWindowOnMouseWheel;
                _codeCompletionWindow.Dispose();
                _codeCompletionWindow = null;
            }
        }

        #endregion

        #region Text

        private string _lastTextSet;

        public string Text
        {
            get { return _editor.Text; }
            set
            {
                var newValue = MultilineHelper.StripNewlines(value ?? "");
                if (newValue == Text)
                    return;

                _editor.Text = newValue;
                _lastTextSet = newValue;
            }
        }

        public event EventHandler TextChanged;

        private void EditorOnTextChanged(object sender, EventArgs e)
        {
            _multilineHelper.TextChanged();
        }

        private void NotifyTextChanged()
        {
            ForceRepaint();

            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }

        private void ForceRepaint()
        {
            _editor.ShowInvalidLines = _editor.ShowInvalidLines;
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
            if ((Singleline || ReadOnly) && data == Keys.Enter)
            {
                _multilineHelper.SubmitForm();
                return false;
            }
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
            return Singleline && ch == '\n';
        }

        private bool ShouldPreventTabKey(Keys data)
        {
            return (Singleline || ReadOnly) && (data == Keys.Tab || data == (Keys.Tab | Keys.Shift));
        }

        #endregion

        #region Font

        public double FontSize
        {
            get { return FontSizeConverter.GetWpfFontSize(_editor.Font.Size); }
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == FontSize)
                    return;

                var font = _editor.Font;

                _editor.Font = new Font(font.FontFamily.Name,
                                        (float) FontSizeConverter.GetWinFormsFontSize(value),
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

        public int HorizontalScrollBarHeight
        {
            get { return SystemInformation.HorizontalScrollBarHeight; }
        }

        public int VerticalScrollBarWidth
        {
            get { return SystemInformation.VerticalScrollBarWidth; }
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
            _editor.LoadFile("", stream, true, true);
            _lastTextSet = Text;
        }

        public void Load(string filePath)
        {
            _editor.LoadFile(filePath, true, true);
            _lastTextSet = Text;
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

        public void LoadSyntaxDefinitions(ISyntaxModeFileProvider syntaxModeFileProvider)
        {
            var manager = HighlightingManager.Manager;
            manager.AddSyntaxModeFileProvider(syntaxModeFileProvider);
        }

        public void SetSyntax(StandardSyntaxType type)
        {
            var name = type.GetAttributeProperty<SyntaxModeNameAttribute, string>(attribute => attribute.Name) ?? "";
            _editor.SetHighlighting(name);
        }

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
            get { return _lastTextSet != Text; }
        }

        public int LineCount
        {
            get { return _editor.ActiveTextAreaControl.Document.TotalNumberOfLines; }
        }

        #endregion
    }
}
