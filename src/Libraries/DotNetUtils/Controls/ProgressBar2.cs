using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Extensions;

namespace DotNetUtils.Controls
{
    /// <summary>
    /// A more sensible extension of the standard <see cref="ProgressBar"/> class that adds
    /// a getter/setter for <see cref="ProgressBar.Value"/> that accepts a <c>double</c>
    /// in the range 0.0 to 100.0.  It also supports custom colors to denote state.
    /// </summary>
    public class ProgressBar2 : ProgressBar
    {
        /// <summary>
        /// Gets or sets the value of the progress bar from 0.0 to 100.0.
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

        private double _valuePercent;

        public Func<double, string> GenerateText;

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

            GenerateText = d => string.Format("{0:0.00}%", d);

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
            if (!UseCustomColors)
            {
                base.OnPaintBackground(e);
            }
            // None... Helps control the flicker.
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
}
