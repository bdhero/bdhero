using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using DotNetUtils.Extensions;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using TextEditor.Extensions;
using HighlightingManager = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
#if !__MonoCS__
using System.Windows.Forms.Integration;
using Control = System.Windows.Forms.Control;
#endif

namespace TextEditor.WPF
{
#if __MonoCS__

    internal class TextEditorImpl
    {
    }

#else

    internal class TextEditorImpl : ITextEditor
    {
        private readonly ICSharpCode.AvalonEdit.TextEditor _editor
            = new ICSharpCode.AvalonEdit.TextEditor
              {
                  FontFamily = new FontFamily("Consolas, Courier New, Courier, monospace")
              };

        private readonly ElementHost _elementHost = new ElementHost { Dock = DockStyle.Fill };

        private readonly TextEditorOptionsImpl _options;
        private readonly MultilineHelper _multilineHelper;

        private bool Singleline
        {
            get { return !Multiline; }
        }

        public TextEditorImpl()
        {
            _editor.TextChanged += EditorOnTextChanged;
            _editor.PreviewKeyDown += OnPreviewKeyDown;

            _options = new TextEditorOptionsImpl(_editor);

            _multilineHelper = new MultilineHelper(this, NotifyTextChanged);

            _elementHost.Child = _editor;
        }

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

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
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

                var shiftKey = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift);

                _elementHost.SelectNextControl(!shiftKey);
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

        public int HorizontalScrollBarHeight
        {
            get { return 0; }
        }

        public int VerticalScrollBarWidth
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
        }

        public void Load(string filePath)
        {
            _editor.Load(filePath);
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

        public void LoadSyntaxDefinitions(ICSharpCode.TextEditor.Document.ISyntaxModeFileProvider syntaxModeFileProvider)
        {
            var manager = HighlightingManager.Instance;

            foreach (var syntaxMode in syntaxModeFileProvider.SyntaxModes)
            {
                using (var reader = syntaxModeFileProvider.GetSyntaxModeFile(syntaxMode))
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
            set { _editor.SelectedText = value; }
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
            get { return _editor.IsModified; }
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

#endif
}
