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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinAPI.User;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for classes derived from the <see cref="Control"/> base class.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        ///     <kbd>CTRL</kbd> + <kbd>A</kbd> character.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const char CTRL_A = '\x1';

        /// <summary>
        ///     Gets the difference between the given <paramref name="control"/>'s "ideal" size
        ///     (i.e., the minimum width and height required to fit all text without scrollbars)
        ///     and its actual current size.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="numPaddingChars">
        ///     Number of characters of padding to add to the end of the <paramref name="control"/>'s text
        ///     when computing its "ideal" size.
        /// </param>
        /// <returns>
        ///     The "ideal" size of the given <paramref name="control"/>.
        /// </returns>
        public static Size GetAutoSizeDelta(this Control control, int numPaddingChars = 0)
        {
            var maxWidth = Screen.FromControl(control).WorkingArea.Width / 2;
            var maxHeight = Screen.FromControl(control).WorkingArea.Height / 2;

            var widthBefore = control.Width;
            var heightBefore = control.Height;

            var text = control.Text + new string('M', numPaddingChars);

            SizeF size;

            using (Graphics g = control.CreateGraphics())
            {
                size = g.MeasureString(text, control.Font);
            }

            var widthAfter = (int)Math.Ceiling(size.Width);
            var heightAfter = (int)Math.Ceiling(size.Height);

            var widthAfterBounded = Math.Min(widthAfter, maxWidth);
            var heightAfterBounded = Math.Min(heightAfter, maxHeight);

            var widthDelta = widthAfterBounded - widthBefore;
            var heightDelta = heightAfterBounded - heightBefore;

            return new Size(widthDelta, heightDelta);
        }

        /// <summary>
        /// Recursively iterates over the control's child controls and returns a collection of all descendant controls.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        /// <seealso cref="http://stackoverflow.com/a/2735242/467582"/>
        public static IEnumerable<Control> Descendants(this Control control)
        {
            return Descendants<Control>(control);
        }

        /// <summary>
        /// Recursively iterates over the control's child controls and returns a collection of all descendant controls
        /// that are instances of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        /// <seealso cref="http://stackoverflow.com/a/2735242/467582"/>
        public static IEnumerable<T> Descendants<T>(this Control control) where T : class
        {
            foreach (Control child in control.Controls)
            {
                var childOfT = child as T;

                if (childOfT != null)
                {
                    yield return childOfT;
                }

                if (!child.HasChildren) continue;

                foreach (var descendant in Descendants<T>(child))
                {
                    yield return descendant;
                }
            }
        }

        /// <summary>
        /// Iterates through the Control's child Controls recursively and attaches an event handler
        /// to each TextBox and ListItem which allows the user to press CTRL + A to select all text or items.
        /// </summary>
        /// <param name="control"></param>
        public static void EnableSelectAll(this Control control)
        {
            var controls = control.Descendants<Control>().ToList();
            foreach (var textBox in controls.OfType<TextBox>())
            {
                textBox.EnableSelectAll();
            }
            foreach (var listView in controls.OfType<ListView>())
            {
                listView.EnableSelectAll();
            }
        }

        /// <summary>
        /// Attaches a KeyPress event handler to the TextBox that allows the user to press CTRL + A to select all text.
        /// </summary>
        /// <param name="textBox"></param>
        public static void EnableSelectAll(this TextBox textBox)
        {
            textBox.KeyPress += (sender, args) => SelectAll<TextBox>(sender, args, box => box.SelectAll());
        }

        /// <summary>
        /// Attaches a KeyPress event handler to the ListView that allows the user to press CTRL + A to select all ListViewItems.
        /// </summary>
        /// <param name="listView"></param>
        public static void EnableSelectAll(this ListView listView)
        {
            var lastSelectedIndex = 0;
            listView.SelectedIndexChanged += delegate
                {
                    if (listView.SelectedIndices.Count > 0)
                        lastSelectedIndex = listView.SelectedIndices[0];
                };
            listView.KeyPress += (sender, args) => SelectAll<ListView>(sender, args, delegate
                {
                    if (listView.MultiSelect)
                        listView.SelectAll();
                    else if (listView.SelectedIndices.Count == 0 && lastSelectedIndex < listView.Items.Count)
                        listView.Items[lastSelectedIndex].Selected = true;
                });
        }

        private static void SelectAll<T>(object sender, KeyPressEventArgs e, Action<T> action) where T : Control
        {
            if (e.KeyChar != CTRL_A) return;

            var control = sender as T;
            if (control == null) return;

            action(control);

            e.Handled = true;
        }

        /// <summary>
        /// Sets whether the control is first drawn to a buffer rather than directly to the screen,
        /// which can reduce flicker.  If this method is called while the user is in a Terminal session,
        /// double buffering will be automatically disabled to improve performance.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="doubleBuffered">
        /// If <c>true</c>, the control is first drawn to a buffer rather than directly to the screen,
        /// which can reduce flicker.  If you set this property to <c>true</c>, you should also set
        /// the <see cref="ControlStyles.AllPaintingInWmPaint"/> flag to true.
        /// </param>
        /// <seealso cref="http://stackoverflow.com/a/77233/467582"/>
        public static void SetDoubleBuffered(this Control control, bool doubleBuffered)
        {
            PropertyInfo prop =
                typeof(Control).GetProperty(
                    "DoubleBuffered",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

            // Taxes: Remote Desktop Connection and painting
            // http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            var @value = doubleBuffered && !SystemInformation.TerminalServerSession;

            prop.SetValue(control, value, null);

            // Alternative:
//            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// Sets whether the control paints itself rather than the operating system doing so.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="userPaint">
        /// If <c>true</c>, the control paints itself rather than the operating system doing so.
        /// If <c>false</c>, the <see cref="Control.Paint"/> event is not raised.
        /// </param>
        public static void SetUserPaint(this Control control, bool userPaint)
        {
            MethodInfo method =
                typeof(Control).GetMethod(
                    "SetStyle",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);
            var @params = new object[] { ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, userPaint };
            method.Invoke(control, @params);
        }

        /// <summary>
        /// Gets whether the control paints itself rather than the operating system doing so.
        /// </summary>
        /// <param name="control"></param>
        /// <returns><c>true</c> if the control paints itself; otherwise <c>false</c></returns>
        public static bool GetUserPaint(this Control control)
        {
            MethodInfo method =
                typeof(Control).GetMethod(
                    "GetStyle",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);
            var @params = new object[] { ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint };
            return (bool) method.Invoke(control, @params);
        }

        #region OS-specific extensions

#if __MonoCS__

        /// <summary>
        /// Does nothing on this platform.
        /// </summary>
        /// <param name="parent"></param>
        public static void SuspendDrawing(this Control parent)
        {
        }

        /// <summary>
        /// Does nothing on this platform.
        /// </summary>
        /// <param name="parent"></param>
        public static void ResumeDrawing(this Control parent)
        {
        }

#else


        // ReSharper disable once InconsistentNaming
        private const int WM_SETREDRAW = 11;

        /// <summary>
        /// Prevents controls from repainting.
        /// </summary>
        /// <param name="parent"></param>
        /// <seealso cref="http://stackoverflow.com/a/487757/467582"/>
        public static void SuspendDrawing(this Control parent)
        {
            var falsePtr = IntPtr.Zero;
            WindowAPI.SendMessage(parent.Handle, WM_SETREDRAW, falsePtr, IntPtr.Zero);
        }

        /// <summary>
        /// Allows controls to repaint.
        /// </summary>
        /// <param name="parent"></param>
        /// <seealso cref="http://stackoverflow.com/a/487757/467582"/>
        public static void ResumeDrawing(this Control parent)
        {
            var truePtr = new IntPtr(1);
            WindowAPI.SendMessage(parent.Handle, WM_SETREDRAW, truePtr, IntPtr.Zero);
            parent.Refresh();
        }

#endif

        #endregion
    }
}