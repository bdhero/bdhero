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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDHeroGUI.Forms
{
    public partial class FormResizeWindow : Form
    {
        private readonly Form _parentForm;
        private readonly Size _originalSize;

        public FormResizeWindow(Form parentForm)
        {
            InitializeComponent();

            _parentForm = parentForm;
            _originalSize = _parentForm.Size;

            var minimumSize = parentForm.MinimumSize;
            var maximumSize = parentForm.MaximumSize;

            if (!minimumSize.IsEmpty)
            {
                textBoxWidth.Minimum = minimumSize.Width;
                textBoxHeight.Minimum = minimumSize.Height;
            }

            if (!maximumSize.IsEmpty)
            {
                textBoxWidth.Maximum = maximumSize.Width;
                textBoxHeight.Maximum = maximumSize.Height;
            }

            textBoxWidth.Value = _originalSize.Width;
            textBoxHeight.Value = _originalSize.Height;

            textBoxWidth.ValueChanged += OnChanged;
            textBoxHeight.ValueChanged += OnChanged;
            Closed += OnClosed;
        }

        private void OnChanged(object sender, EventArgs eventArgs)
        {
            _parentForm.Size = new Size((int)textBoxWidth.Value, (int)textBoxHeight.Value);
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            if (DialogResult == DialogResult.OK)
                return;
            _parentForm.Size = _originalSize;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
