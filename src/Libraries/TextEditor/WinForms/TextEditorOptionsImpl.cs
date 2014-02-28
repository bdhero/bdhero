using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace TextEditor.WinForms
{
    internal class TextEditorOptionsImpl : ITextEditorOptions
    {
        private readonly TextEditorControl _editor;

        public TextEditorOptionsImpl(TextEditorControl editor)
        {
            _editor = editor;
            _editor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextAreaOnKeyEventHandler;
        }

        private bool TextAreaOnKeyEventHandler(char ch)
        {
            return !Multiline && ch == '\n';
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
                _editor.HorizontalScroll.Enabled =
                    _editor.VerticalScroll.Enabled =

                    _editor.ActiveTextAreaControl.HScrollBar.Enabled =
                    _editor.ActiveTextAreaControl.VScrollBar.Enabled =

                    _editor.ActiveTextAreaControl.HScrollBar.Visible =
                    _editor.ActiveTextAreaControl.VScrollBar.Visible = value;
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

        public bool CutCopyWholeLine
        {
            get { return _editor.TextEditorProperties.CutCopyWholeLine; }
            set { _editor.TextEditorProperties.CutCopyWholeLine = value; }
        }

        public int IndentationSize
        {
            get { return _editor.Document.TextEditorProperties.IndentationSize; }
            set { _editor.Document.TextEditorProperties.IndentationSize = value; }
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
