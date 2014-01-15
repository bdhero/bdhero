// Copyright 2012, 2013, 2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace DotNetUtils.Controls
{
    /// <summary>
    ///     A better replacement for .NET's default <see cref="LinkLabel"/>.
    /// </summary>
    /// <remarks>
    ///     <para>For the latest version visit: http://wyday.com/linklabel2/ </para>
    ///     <para>Bugs or suggestions: http://wyday.com/forum/ </para>
    /// </remarks>
    public class LinkLabel2 : Control
    {
        private Font _hoverFont;

        private Image _image;
        private int _imageRightPad = 8;

        private bool _isEnabled = true;
        private bool _isHovered;
        private bool _keyAlreadyProcessed;
        private Rectangle _textRect;

        /// <summary>
        ///     Constructs a new <see cref="LinkLabel2"/> instance.
        /// </summary>
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

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;

                RefreshTextRect();

                Invalidate();
            }
        }

        [DllImport("user32.dll")]
        private static extern int LoadCursor(int hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        private static extern int SetCursor(int hCursor);

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
            {
                Focus();
            }

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
            {
                OnClick(e);
            }

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.Low;

            // image
            if (_image != null)
            {
                e.Graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height),
                                     new Rectangle(0, 0, _image.Width, _image.Height), GraphicsUnit.Pixel);
            }

            //text
            TextRenderer.DrawText(e.Graphics, Text,
                                  _isHovered && HoverUnderline ? _hoverFont : Font,
                                  _textRect,
                                  UseSystemColor
                                      ? ForeColor
                                      : (!_isEnabled ? DisabledColor : _isHovered ? HoverColor : RegularColor),
                                  TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            // draw the focus rectangle.
            if (Focused && ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            _hoverFont = new Font(Font, Font.Style | FontStyle.Underline);
            RefreshTextRect();

            base.OnFontChanged(e);
        }

        private void RefreshTextRect()
        {
            _textRect = new Rectangle(Point.Empty,
                                      TextRenderer.MeasureText(Text, Font, Size,
                                                               TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix));
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
