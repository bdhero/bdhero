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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using UILib.Extensions;

namespace UILib.WinForms.Controls
{
    /// <summary>
    /// A more sensible extension of the standard <see cref="ProgressBar"/> class that adds
    /// a getter/setter for <see cref="ProgressBar.Value"/> that accepts a <c>double</c>
    /// in the range 0.0 to 100.0.  It also supports custom colors to denote state.
    /// </summary>
    public class ProgressBar2 : ProgressBar
    {
        /// <summary>
        /// From <c>0.0</c> to <c>100.0</c>.
        /// </summary>
        private double _valuePercent;

        /// <summary>
        /// Gets or sets the value of the progress bar from <c>0.0</c> to <c>100.0</c>.
        /// </summary>
        public double ValuePercent
        {
            get { return _valuePercent; }
            set
            {
                _valuePercent = value;
                Value = (int) (value * Maximum / 100.0);
            }
        }

        public ProgressBarTextGenerator GenerateText;

        public bool TextOutline { get; set; }

        public Brush TextColor { get; set; }
        public Color TextOutlineColor { get; set; }
        public int TextOutlineWidth { get; set; }

        /// <summary>
        /// Gets or sets whether the progress bar should use custom background gradients
        /// to indicate its state (e.g., paused, error, success) or the standard Windows gradient (green).
        /// </summary>
        public bool UseCustomColors
        {
            get { return this.GetUserPaint(); }
            set { this.SetUserPaint(value); }
        }

        public ProgressBar2()
        {
            Maximum = 100 * 1000;

//            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetDoubleBuffered(true);

            GenerateText = valuePercent => string.Format("{0:0.00}%", valuePercent);

            TextColor = Brushes.Black;
            TextOutline = true;
            TextOutlineColor = Color.FromArgb(128, 255, 255, 255);
            TextOutlineWidth = 2;
        }

        public void SetError()
        {
            ForeColor = Color.Red;
            BackColor = Color.DarkRed;
        }

        public void SetPaused()
        {
            ForeColor = Color.LightGoldenrodYellow;
            BackColor = Color.Yellow;
        }

        public void SetInfo()
        {
            ForeColor = Color.LightSkyBlue;
            BackColor = Color.DeepSkyBlue;
        }

        public void SetSuccess()
        {
            ForeColor = Color.GreenYellow;
            BackColor = Color.Green;
        }

        public void SetMuted()
        {
            ForeColor = Color.DarkGray;
            BackColor = Color.Gray;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Helps control the flicker.
            if (UseCustomColors && VisualStyleInformation.IsEnabledByUser)
                return;

            base.OnPaintBackground(e);
        }

        /// <seealso cref="http://stackoverflow.com/a/7490884/467582"/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!UseCustomColors)
            {
                base.OnPaint(e);
                return;
            }

            const int inset = 2; // A single inset value to control the sizing of the inner rect.

            using (Image offscreenImage = new Bitmap(Width, Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, Width, Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                    rect.Width = (int)(rect.Width * ((double)Value / Maximum));
                    if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                    LinearGradientBrush brush = new LinearGradientBrush(rect, ForeColor, BackColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                    var g = e.Graphics;

                    g.DrawImage(offscreenImage, 0, 0);

                    // Use high quality rendering
                    // See http://stackoverflow.com/a/4200875/467582
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
//                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
//                    g.CompositingQuality = CompositingQuality.HighQuality;

                    if (GenerateText != null)
                    {
                        var text = GenerateText(ValuePercent);
                        var style = (int) FontStyle.Regular;
                        var emSize = Font.Size * 1.5f;

                        SizeF len = g.MeasureString(text, Font);

                        // Calculate the location of the text (the middle of progress bar)
                        Point location = new Point(Convert.ToInt32((Width / 2.0) - (len.Width / 2.0)) - TextOutlineWidth,
                                                   Convert.ToInt32((Height / 2.0) - (len.Height / 2.0)) - TextOutlineWidth);

                        // Draw the custom text
                        if (TextOutline)
                        {
                            // See http://stackoverflow.com/a/4200875/467582
                            var graphicsPath = new GraphicsPath();
                            graphicsPath.AddString(text, Font.FontFamily, style, emSize, location, StringFormat.GenericTypographic); // Brushes.Black, location);
                            e.Graphics.DrawPath(new Pen(TextOutlineColor, TextOutlineWidth), graphicsPath);

                            var graphicsPath2 = new GraphicsPath();
                            graphicsPath2.AddString(text, Font.FontFamily, style, emSize, location, StringFormat.GenericTypographic); // Brushes.Black, location);
                            g.FillPath(TextColor, graphicsPath2);
                        }
                        else
                        {
                            g.DrawString(text, Font, Brushes.Black, location);
                        }
                    }

                    offscreenImage.Dispose();
                }
            }
        }
    }

    public delegate string ProgressBarTextGenerator(double valuePercent);
}
