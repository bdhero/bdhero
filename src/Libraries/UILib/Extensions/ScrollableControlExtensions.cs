// Copyright 2014 Andrew C. Dvorak
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

namespace UILib.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="ScrollableControl"/> objects.
    /// </summary>
    public static class ScrollableControlExtensions
    {
        /// <summary>
        ///     Enables scrolling the given <paramref name="control"/> vertically with the
        ///     <kbd>Up</kbd>, <kbd>Down</kbd>, <kbd>Page Up</kbd>, and <kbd>Page Down</kbd> keys.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control that requires keyboard support for vertical scrolling.
        /// </param>
        public static void EnableVerticalKeyboardScroll(this ScrollableControl control)
        {
            KeyEventHandler onKeyDown = (sender, args) => OnKeyDown(control, args);

            var form = control.FindForm();
            if (form == null)
            {
                control.KeyDown += onKeyDown;
                return;
            }

            form.KeyPreview = true;
            form.KeyDown += onKeyDown;
        }

        private static void OnKeyDown(this ScrollableControl control, KeyEventArgs e)
        {
            if (!control.ContainsFocus)
                return;

            if (e.KeyCode == Keys.PageUp)
                control.PageUp();

            if (e.KeyCode == Keys.PageDown)
                control.PageDown();

            if (e.KeyCode == Keys.Up)
                control.ScrollUp();

            if (e.KeyCode == Keys.Down)
                control.ScrollDown();
        }

        /// <summary>
        ///     Scrolls the given <paramref name="control"/> up by the height of the <paramref name="control"/> if possible.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control to scroll up by one page.
        /// </param>
        public static void PageUp(this ScrollableControl control)
        {
            control.ScrollVerticallyBy(-control.Height);
        }

        /// <summary>
        ///     Scrolls the given <paramref name="control"/> down by the height of the <paramref name="control"/> if possible.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control to scroll down by one page.
        /// </param>
        public static void PageDown(this ScrollableControl control)
        {
            control.ScrollVerticallyBy(+control.Height);
        }

        /// <summary>
        ///     Scrolls the given <paramref name="control"/> up by the size of the <paramref name="control"/>'s font if possible.
        ///     This method is equivalent to the user pressing the "Up" arrow on the <paramref name="control"/>'s scrollbar.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control to scroll up.
        /// </param>
        public static void ScrollUp(this ScrollableControl control)
        {
            control.ScrollVerticallyBy(-(int)control.Font.Size);
        }

        /// <summary>
        ///     Scrolls the given <paramref name="control"/> down by the size of the <paramref name="control"/>'s font if possible.
        ///     This method is equivalent to the user pressing the "Down" arrow on the <paramref name="control"/>'s scrollbar.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control to scroll down.
        /// </param>
        public static void ScrollDown(this ScrollableControl control)
        {
            control.ScrollVerticallyBy(+(int)control.Font.Size);
        }

        /// <summary>
        ///     Scrolls the given <paramref name="control"/> vertically by the specified <paramref name="amount"/>,
        ///     ensuring that the new scroll value does not exceed the <paramref name="control"/>'s minimum or maximum
        ///     scroll values.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control.
        /// </param>
        /// <param name="amount">
        ///     Number of pixels (positive or negative) to scroll.
        /// </param>
        public static void ScrollVerticallyBy(this ScrollableControl control, int amount)
        {
            control.SetVerticalScroll(control.VerticalScroll.Value + amount);
        }

        /// <summary>
        ///     Sets the given <paramref name="control"/>'s vertical scroll value to the specified <paramref name="value"/>,
        ///     ensuring that the new value does not exceed the <paramref name="control"/>'s minimum or maximum scroll values.
        /// </summary>
        /// <param name="control">
        ///     A scrollable control.
        /// </param>
        /// <param name="value">
        ///     New vertical scroll value (in pixels).
        /// </param>
        public static void SetVerticalScroll(this ScrollableControl control, int value)
        {
            value = Math.Min(control.VerticalScroll.Maximum, value);
            value = Math.Max(control.VerticalScroll.Minimum, value);
            control.VerticalScroll.Value = value;
            control.VerticalScroll.Value = value; // need to set value twice to force the scrollbar to redraw...
        }
    }
}
