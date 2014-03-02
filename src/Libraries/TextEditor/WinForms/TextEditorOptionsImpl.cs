namespace TextEditor.WinForms
{
    internal class TextEditorOptionsImpl : ITextEditorOptions
    {
        private readonly ICSharpCode.TextEditor.TextEditorControl _editor;

        public TextEditorOptionsImpl(ICSharpCode.TextEditor.TextEditorControl editor)
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
            get { return _editor.ShowVRuler; }
            set { _editor.ShowVRuler = value; }
        }

        public int ColumnRulerPosition
        {
            get { return _editor.VRulerRow; }
            set { _editor.VRulerRow = value; }
        }

        public bool CutCopyWholeLine
        {
            get { return _editor.TextEditorProperties.CutCopyWholeLine; }
            set { _editor.TextEditorProperties.CutCopyWholeLine = value; }
        }

        public int IndentationSize
        {
            get { return _editor.Document.TextEditorProperties.IndentationSize; }
            set { _editor.Document.TextEditorProperties.IndentationSize = _editor.TabIndent = value; }
        }

        public bool ConvertTabsToSpaces
        {
            get { return _editor.ConvertTabsToSpaces; }
            set { _editor.ConvertTabsToSpaces = value; }
        }

        public bool ShowSpaces
        {
            get { return _editor.ShowSpaces; }
            set { _editor.ShowSpaces = value; }
        }

        public bool ShowTabs
        {
            get { return _editor.ShowTabs; }
            set { _editor.ShowTabs = value; }
        }

    }
}
