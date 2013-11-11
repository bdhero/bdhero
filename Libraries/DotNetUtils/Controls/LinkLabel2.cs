using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// For the latest version visit: http://wyday.com/linklabel2/

// Bugs or suggestions: http://wyday.com/forum/

namespace DotNetUtils.Controls
{
    public class LinkLabel2 : Control
    {
        private Font _hoverFont;

        private Rectangle _textRect;

        private bool _isHovered;
        private bool _keyAlreadyProcessed;

        private Image _image;
        private int _imageRightPad = 8;

        private bool _isEnabled = true;

        [DefaultValue(8)]
        public int ImageRightPad
        {
            get { return _imageRightPad; }
            set 
            { 
                _imageRightPad = value;

                RefreshTextRect();
                Invalidate();
            }
        }

        [DefaultValue(null)]
        public Image Image
        {
            get { return _image; }
            set 
            { 
                _image = value;

                RefreshTextRect();
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public bool HoverUnderline { get; set; }

        [DefaultValue(true)]
        public bool UseSystemColor { get; set; }


        public Color RegularColor { get; set; }
        public Color HoverColor { get; set; }
        public Color DisabledColor { get; set; }

        [DllImport("user32.dll")]
        public static extern int LoadCursor(int hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        public static extern int SetCursor(int hCursor);

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                RefreshTextRect();

                Invalidate();
            }
        }

        public LinkLabel2()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint
                | ControlStyles.FixedHeight
                | ControlStyles.FixedWidth, true);

            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);

            _hoverFont = new Font(Font, FontStyle.Underline);

            ForeColor = SystemColors.HotTrack;

            UseSystemColor = true;
            HoverUnderline = true;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            SetEnabledForeColor();
            base.OnEnabledChanged(e);
        }

        protected override void OnParentEnabledChanged(EventArgs e)
        {
            SetEnabledForeColor();
            base.OnParentEnabledChanged(e);
        }

        private void SetEnabledForeColor()
        {
            _isEnabled = Enabled && (Parent == null || Parent.Enabled);
            ForeColor = _isEnabled ? SystemColors.HotTrack : SystemColors.GrayText;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Focus();

            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            if (mevent.Button != MouseButtons.None)
            {
                if (!ClientRectangle.Contains(mevent.Location))
                {
                    if (_isHovered)
                    {
                        _isHovered = false;
                        Invalidate();
                    }
                }
                else if (!_isHovered)
                {
                    _isHovered = true;
                    Invalidate();
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _keyAlreadyProcessed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!_keyAlreadyProcessed && e.KeyCode == Keys.Enter)
            {
                _keyAlreadyProcessed = true;
                OnClick(e);
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _keyAlreadyProcessed = false;

            base.OnKeyUp(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isHovered && e.Clicks == 1 && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
                OnClick(e);

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.Low;

            // image
            if (_image != null)
                e.Graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height), new Rectangle(0, 0, _image.Width, _image.Height), GraphicsUnit.Pixel);

            //text
            TextRenderer.DrawText(e.Graphics, Text,
                _isHovered && HoverUnderline ? _hoverFont : Font,
                _textRect,
                UseSystemColor ? ForeColor : (!_isEnabled ? DisabledColor : _isHovered ? HoverColor : RegularColor),
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            // draw the focus rectangle.
            if (Focused && ShowFocusCues)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            _hoverFont = new Font(Font, Font.Style | FontStyle.Underline);
            RefreshTextRect();

            base.OnFontChanged(e);
        }

        private void RefreshTextRect()
        {
            _textRect = new Rectangle(Point.Empty, TextRenderer.MeasureText(Text, Font, Size, TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix));
            int width = _textRect.Width + 1,
                height = _textRect.Height + 1;

            if (_image != null)
            {
                width = _textRect.Width + 1 + _image.Width + _imageRightPad;

                //adjust the x position of the text
                _textRect.X += _image.Width + _imageRightPad;

                if (_image.Height > _textRect.Height)
                {
                    height = _image.Height + 1;

                    // adjust the y-position of the text
                    _textRect.Y += (_image.Height - _textRect.Height) / 2;
                }
            }

            Size = new Size(width, height);
        }

        protected override void WndProc(ref Message m)
        {
            //WM_SETCURSOR == 32
            if (m.Msg == 32)
            {
                try
                {
                    //IDC_HAND == 32649
                    SetCursor(LoadCursor(0, 32649));
                }
                catch (DllNotFoundException e)
                {
                    // Mono
                    Cursor = Cursors.Hand;
                }

                // The message has been handled
                m.Result = IntPtr.Zero;

                return;
            }

            base.WndProc(ref m);
        }
    }
}
