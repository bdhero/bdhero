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
using System.ComponentModel;
using System.Windows.Forms;
using DotNetUtils.FS;
using DotNetUtils.Properties;
using WebBrowserUtils;

namespace DotNetUtils.Controls
{
    /// <summary>
    ///     Turns a <see cref="Control"/> into a hyperlink by changing its cursor to a hand, launching the default browser
    ///     when the user clicks on the control, and adding a context menu that includes options to open the link
    ///     or copy the URL to the clipboard.
    /// </summary>
    public class Hyperlink
    {
        /// <summary>
        ///     Gets or sets the hyperlink's URL.
        /// </summary>
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    _toolTip.SetToolTip(_control, _url);
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
        private string _url;

        private readonly ToolTip _toolTip = new ToolTip();

        /// <summary>
        ///     Constructs a new <see cref="Hyperlink"/> object with the given parameters.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="url"></param>
        public Hyperlink(Control control, string url = null)
        {
            _control = control;

            control.Click += OnClick;
            control.ContextMenuStrip = new ContextMenuStrip();
            control.ContextMenuStrip.Items.Add(CreateOpenMenuItem());
            control.ContextMenuStrip.Items.Add(CreateCopyMenuItem());
            control.ContextMenuStrip.Opening += ContextMenuStripOnOpening;

            Url = url;
        }

        private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
        {
            if (string.IsNullOrEmpty(Url))
                cancelEventArgs.Cancel = true;
        }

        private ToolStripMenuItem CreateOpenMenuItem()
        {
            var bitmapIcon = DefaultWebBrowser.Instance.GetIconAsBitmap(16) ?? Resources.network;
            return new ToolStripMenuItem("&Open link in browser", bitmapIcon, OnClick);
        }

        private ToolStripMenuItem CreateCopyMenuItem()
        {
            var bitmapIcon = Resources.clipboard_arrow;
            return new ToolStripMenuItem("&Copy URL to clipboard", bitmapIcon, CopyUrlToClipboard);
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(_url)) { return; }
            FileUtils.OpenUrl(_url);
        }

        private void CopyUrlToClipboard(object sender, EventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(_url)) { return; }
            Clipboard.SetText(_url);
        }

        public static Hyperlink MakeHyperlink(Control control, string url = null)
        {
            return new Hyperlink(control, url);
        }
    }
}