using System;
using System.Windows.Forms;
using DotNetUtils.FS;
using DotNetUtils.Properties;
using WebBrowserUtils;

namespace DotNetUtils.Extensions
{
    internal class HyperlinkAddon
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
                _toolTip.SetToolTip(_control, _url);
            }
        }

        private readonly Control _control;
        private string _url;

        private readonly ToolTip _toolTip = new ToolTip();

        private HyperlinkAddon(Control control, string url = null)
        {
            _control = control;

            control.Cursor = Cursors.Hand;
            control.Click += OnClick;
            control.ContextMenuStrip = new ContextMenuStrip();
            control.ContextMenuStrip.Items.Add(CreateOpenMenuItem());
            control.ContextMenuStrip.Items.Add(CreateCopyMenuItem());

            Url = url;
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

        public static HyperlinkAddon MakeHyperlink(Control control, string url = null)
        {
            return new HyperlinkAddon(control, url);
        }
    }
}