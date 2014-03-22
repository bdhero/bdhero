namespace TextEditor.WPF
{
#if __MonoCS__

    internal class TextEditorOptionsImpl
    {
    }

#else

    internal class TextEditorOptionsImpl : ITextEditorOptions
    {
        private readonly ICSharpCode.AvalonEdit.TextEditor _editor;

        public TextEditorOptionsImpl(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _editor = editor;
        }

        public bool ShowLineNumbers
        {
            get { return _editor.ShowLineNumbers; }
            set { _editor.ShowLineNumbers = value; }
        }

        public bool ShowColumnRuler
        {
            get { return _editor.Options.ShowColumnRuler; }
            set { _editor.Options.ShowColumnRuler = value; }
        }

        public int ColumnRulerPosition
        {
            get { return _editor.Options.ColumnRulerPosition; }
            set { _editor.Options.ColumnRulerPosition = value; }
        }

        public bool CutCopyWholeLine
        {
            get { return _editor.Options.CutCopyWholeLine; }
            set { _editor.Options.CutCopyWholeLine = value; }
        }

        public bool EnableRectangularSelection
        {
            get { return _editor.Options.EnableRectangularSelection; }
            set { _editor.Options.EnableRectangularSelection = value; }
        }

        public bool EnableTextDragDrop
        {
            get { return _editor.Options.EnableTextDragDrop; }
            set { _editor.Options.EnableTextDragDrop = value; }
        }

        public int IndentationSize
        {
            get { return _editor.Options.IndentationSize; }
            set { _editor.Options.IndentationSize = value; }
        }

        public bool ConvertTabsToSpaces
        {
            get { return _editor.Options.ConvertTabsToSpaces; }
            set { _editor.Options.ConvertTabsToSpaces = value; }
        }

        public bool ShowBoxForControlCharacters
        {
            get { return _editor.Options.ShowBoxForControlCharacters; }
            set { _editor.Options.ShowBoxForControlCharacters = value; }
        }

        public bool ShowSpaces
        {
            get { return _editor.Options.ShowSpaces; }
            set { _editor.Options.ShowSpaces = value; }
        }

        public bool ShowTabs
        {
            get { return _editor.Options.ShowTabs; }
            set { _editor.Options.ShowTabs = value; }
        }

        public bool SupportsWordWrap
        {
            get { return true; }
        }

        public bool WordWrap
        {
            get { return _editor.WordWrap; }
            set { _editor.WordWrap = value; }
        }

        public double WordWrapIndent
        {
            get { return _editor.Options.WordWrapIndentation; }
            set { _editor.Options.WordWrapIndentation = value; }
        }
    }

#endif
}