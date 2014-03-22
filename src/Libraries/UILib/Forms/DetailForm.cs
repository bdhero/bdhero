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
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace UILib.Forms
{
    public partial class DetailForm : Form
    {
        private readonly string _title;
        private readonly string _summary;
        private readonly string _details;
        private readonly MessageBoxIcon _icon;

        private int _detailHeight;

        private Size _minSizeExpanded;
        private Size _maxSizeExpanded;

        private Size _minSizeCollapsed;
        private Size _maxSizeCollapsed;

        private static readonly IconMapping[] Icons =
        {
            new IconMapping(MessageBoxIcon.Asterisk,    SystemIcons.Asterisk,    SystemSounds.Asterisk),
            new IconMapping(MessageBoxIcon.Error,       SystemIcons.Error,       SystemSounds.Exclamation),
            new IconMapping(MessageBoxIcon.Exclamation, SystemIcons.Exclamation, SystemSounds.Exclamation),
            new IconMapping(MessageBoxIcon.Hand,        SystemIcons.Hand,        SystemSounds.Hand),
            new IconMapping(MessageBoxIcon.Information, SystemIcons.Information, SystemSounds.Question),
            new IconMapping(MessageBoxIcon.Question,    SystemIcons.Question,    SystemSounds.Question),
            new IconMapping(MessageBoxIcon.Stop,        SystemIcons.Error,       SystemSounds.Exclamation),
            new IconMapping(MessageBoxIcon.Warning,     SystemIcons.Warning,     SystemSounds.Exclamation)
        };

        public DetailForm(string title, string summary, string details, MessageBoxIcon icon)
        {
            _title = title;
            _summary = summary;
            _details = details;
            _icon = icon;

            InitializeComponent();

            Load += OnLoad;
        }

        #region Initialization

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            InitText();
            InitIcon();
            InitAutoSize();
            InitToggle();
            InitContextMenu();
        }

        private void InitContextMenu()
        {
            contextMenuStrip.Opening += ContextMenuStripOnOpening;
        }

        private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
        {
            copySelectedToolStripMenuItem.Visible = !string.IsNullOrEmpty(textBoxDetails.SelectedText);
        }

        private void InitText()
        {
            Text = _title;
            labelSummary.Text = _summary;
            textBoxDetails.Text = _details;
        }

        private void InitIcon()
        {
            var requested = Icons.FirstOrDefault(icon => icon.MessageBoxIcon == _icon);
            var @default = Icons.First(icon => icon.MessageBoxIcon == MessageBoxIcon.Information);
            var iconMapping = (requested ?? @default);
            systemIcon.Image = iconMapping.Bitmap;
            iconMapping.SystemSound.Play();
        }

        private void InitAutoSize()
        {
            AutoSize = false;
            textBoxDetails.AutoSize = true;
            textBoxDetails.Font = new Font(textBoxDetails.Font, FontStyle.Bold);
            textBoxDetails.AutoSize = false;
            textBoxDetails.Font = new Font(textBoxDetails.Font, FontStyle.Regular);

            _minSizeExpanded = new Size(Width, Height);
            _maxSizeExpanded = new Size();
        }

        private void InitToggle()
        {
            checkBoxShowDetails.Click += ToggleDetails;
            ToggleDetails();
        }

        #endregion

        #region Show/Hide Details

        private void HideDetails()
        {
            _detailHeight = textBoxDetails.Height;
            textBoxDetails.Visible = false;
            Height -= _detailHeight;

            MaximumSize = new Size(Screen.FromControl(this).Bounds.Width, Height);

            _minSizeCollapsed = new Size(_minSizeExpanded.Width, Height);
            _maxSizeCollapsed = new Size(Int32.MaxValue, Height);

            MinimumSize = _minSizeCollapsed;
            MaximumSize = _maxSizeCollapsed;
        }

        private void ShowDetails()
        {
            textBoxDetails.Visible = true;
            Height += _detailHeight;

            MinimumSize = _minSizeExpanded;
            MaximumSize = _maxSizeExpanded;
        }

        private void ToggleDetails(object sender = null, EventArgs args = null)
        {
            MinimumSize = new Size();
            MaximumSize = new Size();
            if (textBoxDetails.Visible)
            {
                HideDetails();
            }
            else
            {
                ShowDetails();
            }
        }

        #endregion

        public static DialogResult ShowExceptionDetail(IWin32Window window, string title, string message, string detail)
        {
            return new DetailForm(title, message, detail, MessageBoxIcon.Error).ShowDialog(window);
        }

        #region UI Events

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxDetails.SelectedText);
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxDetails.Text);
        }

        private void selectallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxDetails.SelectAll();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }

    internal class IconMapping
    {
        public readonly MessageBoxIcon MessageBoxIcon;
        public readonly Icon Icon;
        public readonly SystemSound SystemSound;

        public IconMapping(MessageBoxIcon messageBoxIcon, Icon icon, SystemSound systemSound)
        {
            MessageBoxIcon = messageBoxIcon;
            Icon = icon;
            SystemSound = systemSound;
        }

        public Bitmap Bitmap { get { return Icon.ToBitmap(); } }
    }
}
