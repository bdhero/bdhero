using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Control = System.Windows.Forms.Control;

namespace TextEditor.WinForms
{
    internal class TextEditorImpl : ITextEditor
    {
        private readonly TextEditorControl _editor = new TextEditorControl();

        private readonly TextEditorOptionsImpl _options;

        public TextEditorImpl()
        {
            _editor.TextChanged += OnTextChanged;
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
            get { return _editor; }
        }

        public event EventHandler TextChanged;

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

        public string Text
        {
            get { return _editor.Text; }
            set { _editor.Text = value; }
        }

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

        public void SelectAll()
        {
            var control = _editor.ActiveTextAreaControl;
            var manager = control.SelectionManager;
            var document = control.Document;
            var lastLine = document.TotalNumberOfLines - 1;
            manager.SetSelection(
                new TextLocation(0, 0),
                new TextLocation(lastLine, document.GetLineSegment(lastLine).TotalLength)
                );
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

        public void Cut()
        {
            Copy();
            SelectedText = "";
        }

        public void Copy()
        {
            Clipboard.SetText(string.Join("\n", Selections.Select(selection => selection.SelectedText)));
        }

        public void Paste()
        {
            var selections = Selections.ToList();
            selections.Reverse();
            selections.ForEach(selection => _editor.Document.Replace(selection.Offset, selection.Length, Clipboard.GetText()));
        }

        public void Print()
        {
            _editor.PrintDocument.Print();
        }

        public bool IsModified
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return _editor.IsReadOnly; }
            set { _editor.IsReadOnly = value; }
        }

        public int LineCount
        {
            get { return _editor.ActiveTextAreaControl.Document.TotalNumberOfLines; }
        }
    }
}
