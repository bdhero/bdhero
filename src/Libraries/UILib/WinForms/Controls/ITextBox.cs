using System;
using System.Windows.Forms;

namespace UILib.WinForms.Controls
{
    public interface ITextBox
    {
        Control Control { get; }
        string Text { get; set; }
        bool ReadOnly { get; set; }
        BorderStyle BorderStyle { get; set; }
        event EventHandler TextChanged;
        void HighlightDragDrop();
        void UnhighlightDragDrop();
    }
}