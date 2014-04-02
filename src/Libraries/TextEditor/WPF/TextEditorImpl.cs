#if !__MonoCS__
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using DotNetUtils.Extensions;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using TextEditor.Extensions;
using TextEditor.SyntaxHighlighting.Providers;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;
using FontFamily = System.Windows.Media.FontFamily;
using HighlightingManager = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace TextEditor.WPF
{
    internal class TextEditorImpl : ITextEditor
    {
        private readonly ICSharpCode.AvalonEdit.TextEditor _editor
            = new ICSharpCode.AvalonEdit.TextEditor
              {
                  FontFamily = new FontFamily("Consolas, Courier New, Courier, monospace")
              };

        private readonly ElementHostImpl _elementHost = new ElementHostImpl { Dock = DockStyle.Fill };

        private readonly TextEditorOptionsImpl _options;
        private readonly MultilineHelper _multilineHelper;
        private readonly ICompletionController _completionController;

        private bool Singleline
        {
            get { return !Multiline; }
        }

        public TextEditorImpl()
        {
            BindEvents();

            // TODO: Dispose
            _options = new TextEditorOptionsImpl(_editor);

            // TODO: Dispose
            _multilineHelper = new MultilineHelper(this, NotifyTextChanged);

            // TODO: Dispose
            _completionController = new MockCompletionController(_editor);

            _elementHost.Child = _editor;
        }

        ~TextEditorImpl()
        {
            UnbindEvents();
        }

        // TODO: Implement IDisposable

        private void BindEvents()
        {
            _editor.TextChanged += EditorOnTextChanged;
            _editor.PreviewKeyDown += OnPreviewKeyDown;
            _editor.MouseEnter += EditorOnMouseEnter;
            _editor.MouseLeave += EditorOnMouseLeave;
        }

        private void UnbindEvents()
        {
            _editor.TextChanged -= EditorOnTextChanged;
            _editor.PreviewKeyDown -= OnPreviewKeyDown;
            _editor.MouseEnter -= EditorOnMouseEnter;
            _editor.MouseLeave -= EditorOnMouseLeave;
        }

        public void SetBackgroundColor(Color color)
        {
            _editor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void EditorOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            _elementHost.TriggerMouseEnter();
        }

        private void EditorOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            _elementHost.TriggerMouseLeave();
        }

        #region Code completion

        #endregion

        #region Core

        ITextEditorOptions ITextEditor.Options
        {
            get { return _options; }
        }

        public Control Control
        {
            get { return _elementHost; }
        }

        #endregion

        #region Text

        private string _lastTextSet;

        public string Text
        {
            get { return _editor.Text; }
            set
            {
                var newValue = (Multiline ? value : MultilineHelper.StripNewlines(value ?? "")) ?? "";
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
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Input events

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var control = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control);
            var shift = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift);

            if (e.Handled)
                return;

            if (_completionController.IgnoreTabOrEnterKey)
                return;

            if (Singleline && e.Key == Key.Enter)
                e.Handled = true;

            if ((Singleline || ReadOnly) && e.Key == Key.Enter)
            {
                _multilineHelper.SubmitForm();
                return;
            }

            if ((Singleline || ReadOnly) && e.Key == Key.Tab)
            {
                e.Handled = true;
                _elementHost.SelectNextControl(!shift);
            }

            if (ReadOnly && control && (e.Key == Key.Z || e.Key == Key.Y))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Font

        public double FontSize
        {
            get { return _editor.FontSize; }
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == FontSize)
                    return;

                _editor.FontSize = value;

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
                return _editor.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden &&
                       _editor.VerticalScrollBarVisibility != ScrollBarVisibility.Hidden;
            }
            set
            {
                var changed = value != Multiline;
                var visibility = value ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;

                _editor.HorizontalScrollBarVisibility =
                    _editor.VerticalScrollBarVisibility = visibility;

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

        public int HorizontalScrollBarHeightAllowance
        {
            get { return 0; }
        }

        public int VerticalScrollBarWidthAllowance
        {
            get { return 0; }
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
            _editor.Load(stream);
            _lastTextSet = Text;
        }

        public void Load(string filePath)
        {
            _editor.Load(filePath);
            _lastTextSet = Text;
        }

        public void Save(Stream stream)
        {
            _editor.Save(stream);
        }

        public void Save(string filePath)
        {
            _editor.Save(filePath);
        }

        #endregion

        #region Syntax highlighting

        public void LoadSyntaxDefinitions(ISyntaxModeProvider provider)
        {
            var manager = HighlightingManager.Instance;

            foreach (var syntaxMode in provider.SyntaxModes)
            {
                using (var reader = provider.GetSyntaxModeFile(syntaxMode))
                {
                    var xshdSyntaxDefinition = HighlightingLoader.LoadXshd(reader);
                    var highlightingDefinition = HighlightingLoader.Load(xshdSyntaxDefinition, manager);
                    manager.RegisterHighlighting(syntaxMode.Name, syntaxMode.Extensions, highlightingDefinition);
                }
            }
        }

        public void SetSyntax(StandardSyntaxType type)
        {
            var name = type.GetAttributeProperty<SyntaxModeNameAttribute, string>(attribute => attribute.Name) ?? "";
            _editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(name);
        }

        public void SetSyntaxFromExtension(string fileNameOrExtension)
        {
            var ext = fileNameOrExtension ?? string.Empty;

            // Filename ending w/ extension
            if (new Regex(@".\.\w+$").IsMatch(ext))
                ext = Path.GetExtension(ext);

            // Extension name only (no dot)
            if (new Regex(@"^\w+$").IsMatch(ext))
                ext = "." + ext;

            if (string.IsNullOrEmpty(ext))
                throw new ArgumentException("fileNameOrExtension does not contain a valid filename or extension");

            var highlightingManager = HighlightingManager.Instance;
            _editor.SyntaxHighlighting = highlightingManager.GetDefinitionByExtension(ext);
        }

        #endregion

        #region Selection

        public void Select(int start, int length)
        {
            _editor.Select(start, length);
        }

        public void SelectAll()
        {
            _editor.SelectAll();
        }

        public string SelectedText
        {
            get { return _editor.SelectedText; }
            set { _editor.SelectedText = value ?? ""; }
        }

        public int SelectionStart
        {
            get { return _editor.SelectionStart; }
            set { _editor.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return _editor.SelectionLength; }
            set { _editor.SelectionLength = value; }
        }

        public int CaretOffset
        {
            get { return _editor.CaretOffset; }
            set { _editor.CaretOffset = value; }
        }

        public void Select(Selection selection)
        {
            var start = new TextViewPosition(selection.StartLine, selection.StartColumn);
            var end = new TextViewPosition(selection.EndLine, selection.EndColumn);
            _editor.TextArea.Selection = new RectangleSelection(_editor.TextArea, start, end);
        }

        public void ClearSelection()
        {
            _editor.TextArea.ClearSelection();
        }

        #endregion

        #region History

        public bool CanUndo
        {
            get { return _editor.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _editor.CanRedo; }
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
            _editor.Clear();
        }

        public bool CanDelete
        {
            get { return !ReadOnly && SelectionLength > 0; }
        }

        public void Delete()
        {
            SelectedText = "";
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
            _editor.Cut();
        }

        public void Copy()
        {
            _editor.Copy();
        }

        public void Paste()
        {
            _editor.Paste();
        }

        #endregion

        #region Informational

        public bool IsModified
        {
            get { return _lastTextSet != Text; }
        }

        public int LineCount
        {
            get { return _editor.LineCount; }
        }

        #endregion

        #region Avalon-specific (no apparent 3.x equivalent)

        public void LineUp()
        {
            _editor.LineUp();
        }

        public void LineDown()
        {
            _editor.LineDown();
        }

        public void LineLeft()
        {
            _editor.LineLeft();
        }

        public void LineRight()
        {
            _editor.LineRight();
        }

        public void PageUp()
        {
            _editor.PageUp();
        }

        public void PageDown()
        {
            _editor.PageDown();
        }

        public void PageLeft()
        {
            _editor.PageLeft();
        }

        public void PageRight()
        {
            _editor.PageRight();
        }

        public void ScrollToLine(int line)
        {
            _editor.ScrollToLine(line);
        }

        public void ScrollTo(int line, int column)
        {
            _editor.ScrollTo(line, column);
        }

        public void ScrollToEnd()
        {
            _editor.ScrollToEnd();
        }

        public void ScrollToHome()
        {
            _editor.ScrollToHome();
        }

        public void ScrollToHorizontalOffset(double offset)
        {
            _editor.ScrollToHorizontalOffset(offset);
        }

        public void ScrollToVerticalOffset(double offset)
        {
            _editor.ScrollToVerticalOffset(offset);
        }

        public double VerticalOffset
        {
            get { return _editor.VerticalOffset; }
        }

        public double HorizontalOffset
        {
            get { return _editor.HorizontalOffset; }
        }

        public double ViewportHeight
        {
            get { return _editor.ViewportHeight; }
        }

        public double ViewportWidth
        {
            get { return _editor.ViewportWidth; }
        }

        #endregion
    }

}
#endif
