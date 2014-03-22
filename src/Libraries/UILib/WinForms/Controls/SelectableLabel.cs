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
using System.Windows.Forms;
using UILib.Extensions;

namespace UILib.WinForms.Controls
{
    /// <summary>
    ///     Emulates a <see cref="Label"/> control that allows its text to be selected with a mouse or keyboard.
    /// </summary>
    public class SelectableLabel : TextBox
    {
        /// <summary>
        ///     Gets or sets whether the label should resize itself to fit its text.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        public override bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
                ResizeAuto();
            }
        }

        private bool _autoSize;

        /// <summary>
        ///     Constructs a new <see cref="SelectableLabel"/> control.
        /// </summary>
        public SelectableLabel()
        {
            ReadOnly = true;
            BorderStyle = BorderStyle.None;
            this.EnableSelectAll();
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs eventArgs)
        {
            ResizeAuto();
        }

        private void ResizeAuto()
        {
            if (!AutoSize)
                return;

            Width += this.GetAutoSizeDelta().Width;
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor()
        {
            UpdateBackgroundColor(Parent);
        }

        private void UpdateBackgroundColor(Control parent)
        {
            if (parent == null)
                return;

            if (parent.BackColor == Color.Transparent)
            {
                if (parent is TabPage)
                {
                    BackColor = SystemColors.Window;
                    return;
                }

                UpdateBackgroundColor(parent.Parent);
                return;
            }

            try
            {
                BackColor = parent.BackColor;
            }
            catch
            {
                UpdateBackgroundColor(parent.Parent);
            }
        }
    }
}
