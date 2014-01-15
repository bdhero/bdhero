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
using System.Windows.Forms;
using System.Drawing;

namespace DotNetUtils.Controls
{
    public class SplitContainerWithDivider : SplitContainer
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Built-in method to draw a grab handle.  Draws an ugly solid bar across the entire divider.
//            ControlPaint.DrawGrabHandle(e.Graphics, SplitterRectangle, false, Enabled);

            PaintGrabHandle(this, e);
        }

        /// <summary>
        /// Fixes grab handle flickering, but causes border flickering and doesn't repaint while the user is dragging the grab handle.
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/a/89125/467582"/>
        /// <seealso cref="http://social.msdn.microsoft.com/Forums/windows/en-US/aaed00ce-4bc9-424e-8c05-c30213171c2c/flickerfree-painting"/>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                // Uncomment this line to fix grab handle flickering,
                // at the cost of causing border flickering and not repainting while the user drags the grab handle.
//                cp.ExStyle |= ExtendedWindowStyles.WS_EX_COMPOSITED;

                return cp;
            }
        }

        /// <seealso cref="http://stackoverflow.com/a/4405758/467582"/>
        private static void PaintGrabHandle(object sender, PaintEventArgs e)
        {
            var control = (SplitContainer)sender;
            var points = new Point[3];
            var w = control.Width;
            var h = control.Height;
            var d = control.SplitterDistance;
            var sW = control.SplitterWidth;

            // Calculate the position of the points
            if (control.Orientation == Orientation.Horizontal)
            {
                points[0] = new Point((w / 2), d + (sW / 2));
                points[1] = new Point(points[0].X - 10, points[0].Y);
                points[2] = new Point(points[0].X + 10, points[0].Y);
            }
            else
            {
                points[0] = new Point(d + (sW / 2), (h / 2));
                points[1] = new Point(points[0].X, points[0].Y - 10);
                points[2] = new Point(points[0].X, points[0].Y + 10);
            }

            foreach (var p in points)
            {
                p.Offset(-2, -2);
                e.Graphics.FillEllipse(SystemBrushes.ControlDark,
                    new Rectangle(p, new Size(3, 3)));

                p.Offset(1, 1);
                e.Graphics.FillEllipse(SystemBrushes.ControlLight,
                    new Rectangle(p, new Size(3, 3)));
            }
        }
    }
}
