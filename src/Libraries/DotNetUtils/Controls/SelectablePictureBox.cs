using System;
using System.Windows.Forms;

namespace DotNetUtils.Controls
{
    /// <summary>
    ///     Subclass of <see cref="PictureBox"/> that can be selected (focused) with the <kbd>TAB</kbd> key.
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/a/2744485/467582"/>
    public class SelectablePictureBox : PictureBox
    {
        private bool _isLeftClick;

        /// <summary>
        ///     Constructs a new <see cref="SelectablePictureBox"/> instance.
        /// </summary>
        public SelectablePictureBox()
        {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
            _isLeftClick = e.Button == MouseButtons.Left;
        }

        protected override void OnEnter(EventArgs e)
        {
            Invalidate();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            Invalidate();
            base.OnLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (Focused)
            {
                var rc = ClientRectangle;
                rc.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
            }
        }

        private static bool IsActivateKey(Keys key)
        {
            return key == Keys.Space || key == Keys.Enter;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return IsActivateKey(keyData) || base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Handled)
                return;

            if (IsActivateKey(e.KeyCode) && e.Modifiers == Keys.None)
            {
                OnClick(e);
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (!_isLeftClick)
                return;

            base.OnClick(e);
        }
    }
}
