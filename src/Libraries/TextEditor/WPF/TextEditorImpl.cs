using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;
#if !__MonoCS__
using System.Windows.Forms.Integration;
using Control = System.Windows.Forms.Control;
#endif

namespace TextEditor.WPF
{
#if __MonoCS__

    internal class TextEditorImpl : ITextEditor
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

        public TextEditorImpl()
        {
            _editor.TextChanged += OnTextChanged;
            _elementHost.Child = _editor;
            _options = new TextEditorOptionsImpl(_editor);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(sender, e);
        }

        ITextEditorOptions ITextEditor.Options
        {
            get { return _options; }
        }

        public Control Control
        {
            get { return _elementHost; }
        }

        public event EventHandler TextChanged;

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

        public string Text
        {
            get { return _editor.Text; }
            set { _editor.Text = value; }
        }

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

        public void Print()
        {
            // TODO
        }

        public bool IsModified
        {
            get { return _editor.IsModified; }
        }

        public bool IsReadOnly
        {
            get { return _editor.IsReadOnly; }
            set { _editor.IsReadOnly = value; }
        }

        public int LineCount
        {
            get { return _editor.LineCount; }
        }

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
    }

#endif
}
