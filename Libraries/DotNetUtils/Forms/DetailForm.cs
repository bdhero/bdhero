﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace DotNetUtils.Forms
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

        public static void ShowExceptionDetail(IWin32Window window, string title, Exception exception)
        {
            var aggregate = exception as AggregateException;
            if (aggregate != null)
            {
                exception = aggregate.GetBaseException();
            }
            new DetailForm(title, exception.Message, exception.ToString(), MessageBoxIcon.Error).ShowDialog(window);
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
