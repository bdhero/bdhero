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
