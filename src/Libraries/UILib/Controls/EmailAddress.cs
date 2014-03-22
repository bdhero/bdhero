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
using System.Windows.Forms;
using DotNetUtils.FS;
using UILib.Properties;

namespace UILib.Controls
{
    /// <summary>
    ///     Turns a <see cref="Control"/> into a hyperlink by changing its cursor to a hand, launching the system's
    ///     default Web browser when the user clicks on the control, and adding a context menu that includes options
    ///     to open the link or copy the URL to the clipboard.
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        ///     Gets or sets the hyperlink's URL.
        /// </summary>
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    _toolTip.SetToolTip(_control, _address);
                    _control.Cursor = Cursors.Hand;
                }
                else
                {
                    _toolTip.RemoveAll();
                    _control.Cursor = Cursors.Default;
                }
            }
        }

        private readonly Control _control;
        private string _address;

        private readonly ToolTip _toolTip = new ToolTip();

        /// <summary>
        ///     Constructs a new <see cref="EmailAddress"/> object with the given parameters.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="address"></param>
        public EmailAddress(Control control, string address = null)
        {
            _control = control;

            control.Click += OnClick;
            control.ContextMenuStrip = CreateContextMenu();

            Address = address;
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add(CreateOpenMenuItem());
            menu.Items.Add(CreateCopyMenuItem());
            menu.Opening += ContextMenuStripOnOpening;
            return menu;
        }

        private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
        {
            if (string.IsNullOrEmpty(Address))
                cancelEventArgs.Cancel = true;
        }

        private ToolStripMenuItem CreateOpenMenuItem()
        {
            var bitmapIcon = Resources.mail;
            return new ToolStripMenuItem("&Compose message", bitmapIcon, OnClick);
        }

        private ToolStripMenuItem CreateCopyMenuItem()
        {
            var bitmapIcon = Resources.clipboard_arrow;
            return new ToolStripMenuItem("&Copy email address to clipboard", bitmapIcon, CopyUrlToClipboard);
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(_address)) { return; }
            FileUtils.OpenUrl("mailto:" + _address);
        }

        private void CopyUrlToClipboard(object sender, EventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(_address)) { return; }
            Clipboard.SetText(_address);
        }

        /// <summary>
        ///     Turns the given <paramref name="control"/> into a hyperlink by changing its cursor to a hand,
        ///     launching the system's default Web browser when the user clicks on the control, and adding a context menu
        ///     that includes options to open the link or copy the URL to the clipboard.
        /// </summary>
        public static EmailAddress MakeEmailAddress(Control control, string url = null)
        {
            return new EmailAddress(control, url);
        }
    }
}