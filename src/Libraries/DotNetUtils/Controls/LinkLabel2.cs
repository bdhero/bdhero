// Copyright 2012-2014 Andrew C. Dvorak
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
using System.Windows.Forms;
using NativeAPI.Win.User;

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
        private const TextFormatFlags TextFlags = TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

        private Rectangle _textRect;
        private Font _hoverFont;

        private Image _image;
        private int _imageRightPad = 8;

        private bool _isEnabled = true;
        private bool _isHovered;
        private bool _isKeyHandled;

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

            _hoverFont = new Font(base.Font, FontStyle.Underline);

            base.ForeColor = SystemColors.HotTrack;

            UseSystemColor = true;
            HoverUnderline = true;
        }

        #region Editor properties

        /// <summary>
        ///     Gets or sets the amount of padding (in pixels) to place between
        ///     the text and the right side of the <see cref="Image"/>.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the image to display on the left side of the text.
        ///     A value of <c>null</c> indicates that no image should be displayed.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets whether the link should be underlined when the mouse hovers over it.
        /// </summary>
        [DefaultValue(true)]
        public bool HoverUnderline { get; set; }

        /// <summary>
        ///     Gets or sets whether the link should use standard system colors (<c>true</c>)
        ///     or custom colors (<c>false</c>).
        /// </summary>
        [DefaultValue(true)]
        public bool UseSystemColor { get; set; }

        /// <summary>
        ///     Gets or sets the color of the link when it is enabled and the mouse is not hovered over it.
        /// </summary>
        public Color RegularColor { get; set; }

        /// <summary>
        ///     Gets or sets the color of the link when it is enabled and the mouse hovers over it.
        /// </summary>
        public Color HoverColor { get; set; }

        /// <summary>
        ///     Gets or sets the color of the link when it is disabled.
        /// </summary>
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

        #endregion

        #region Color properties/methods

        private Color ForeColorAuto
        {
            get
            {
                if (UseSystemColor)
                    return ForeColor;
                if (!_isEnabled)
                    return DisabledColor;
                if (_isHovered)
                    return HoverColor;
                return RegularColor;
            }
        }

        private void SetEnabledForeColor()
        {
            _isEnabled = Enabled && (Parent == null || Parent.Enabled);
            ForeColor = _isEnabled ? SystemColors.HotTrack : SystemColors.GrayText;
        }

        #endregion

        #region Enable/disable events

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

        #endregion

        #region Mouse events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Focus();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isHovered && e.Clicks == 1 && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
            {
                OnClick(e);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SetHovered(true);

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetHovered(false);

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);

            if (mevent.Button == MouseButtons.None)
                return;

            if (!ClientRectangle.Contains(mevent.Location))
            {
                if (!_isHovered)
                    return;

                SetHovered(false);
            }
            else if (!_isHovered)
            {
                SetHovered(true);
            }
        }

        private void SetHovered(bool isHovered)
        {
            _isHovered = isHovered;
            Invalidate();
        }

        #endregion

        #region Focus events

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _isKeyHandled = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        #endregion

        #region Keyboard events

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!_isKeyHandled && e.KeyCode == Keys.Enter)
            {
                _isKeyHandled = true;
                OnClick(e);
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _isKeyHandled = false;

            base.OnKeyUp(e);
        }

        #endregion

        #region Paint events

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.Low;

            // Image
            if (_image != null)
            {
                var imageRect = new Rectangle(0, 0, _image.Width, _image.Height);
                e.Graphics.DrawImage(_image, imageRect, imageRect, GraphicsUnit.Pixel);
            }

            // Text
            var font = _isHovered && HoverUnderline ? _hoverFont : Font;
            var foreColor = ForeColorAuto;
            TextRenderer.DrawText(e.Graphics, Text, font, _textRect,
                                  foreColor,
                                  TextFlags);

            // Draw the focus rectangle.
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

            var size = TextRenderer.MeasureText(Text, Font, Size, TextFlags);
            _textRect = new Rectangle(Point.Empty, size);

            int width = _textRect.Width + 1,
                height = _textRect.Height + 1;

            if (_image != null)
            {
                width = _textRect.Width + 1 + _image.Width + _imageRightPad;

                // Adjust the X position of the text
                _textRect.X += _image.Width + _imageRightPad;

                if (_image.Height > _textRect.Height)
                {
                    height = _image.Height + 1;

                    // Adjust the Y position of the text
                    _textRect.Y += (_image.Height - _textRect.Height) / 2;
                }
            }

            Size = new Size(width, height);
        }

        #endregion

        #region Windows messages

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowMessageType.WM_SETCURSOR.ToInt32())
            {
                try
                {
                    CursorAPI.SetCursor(CursorAPI.LoadCursor(IntPtr.Zero, CursorType.IDC_HAND));
                }
                catch (DllNotFoundException)
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

        #endregion
    }
}
