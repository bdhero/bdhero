using System;
using System.Windows.Forms;
using UILib.Extensions;

namespace UILib.WinForms.Controls
{
    internal class StandardTextBox : ITextBox
    {
        private readonly TextBox _textBox;

        public StandardTextBox()
        {
            _textBox = new TextBox();
            _textBox.SelectVariablesOnClick();
            _textBox.TextChanged += (sender, args) => OnTextChanged(args);
            _textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
        }

        private void TextBoxOnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (PreviewKeyDown != null)
                PreviewKeyDown(sender, e);
        }

        public event PreviewKeyDownEventHandler PreviewKeyDown;

        public Control Control
        {
            get { return _textBox; }
        }

        public string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        public bool ReadOnly
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }

        public BorderStyle BorderStyle
        {
            get { return _textBox.BorderStyle; }
            set { _textBox.BorderStyle = value; }
        }

        public event EventHandler TextChanged;

        protected virtual void OnTextChanged(EventArgs args)
        {
            if (TextChanged != null)
                TextChanged(this, args);
        }

        public void HighlightDragDrop()
        {
            _textBox.Highlight();
        }

        public void UnhighlightDragDrop()
        {
            _textBox.UnHighlight();
        }
    }
}