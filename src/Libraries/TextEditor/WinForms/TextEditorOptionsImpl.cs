using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;

namespace TextEditor.WinForms
{
    internal class TextEditorOptionsImpl : ITextEditorOptions
    {
        private readonly TextEditorControl _editor;

        public TextEditorOptionsImpl(TextEditorControl editor)
        {
            _editor = editor;
        }

        public double FontSize
        {
            get { return _editor.Font.Size; }
            set
            {
                var f = _editor.Font;
                _editor.Font = new Font(f.FontFamily.Name, (float)value, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
            }
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

        public int IndentationSize
        {
            // TODO
//            get { return _editor.IndentationSize; }
//            set { _editor.IndentationSize = value; }
            get { return 0; }
            set {  }
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
