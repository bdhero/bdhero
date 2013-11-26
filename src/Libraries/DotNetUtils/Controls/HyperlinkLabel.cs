using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotNetUtils.FS;
using DotNetUtils.Properties;
using WebBrowserUtils;

namespace DotNetUtils.Controls
{
    /// <summary>
    ///     <see cref="LinkLabel"/> subclass for hyperlinks to a website URL.
    ///     Provides a tooltip with the link's URL and a context menu to launch the site
    ///     in the user's browser or copy the URL to the clipboard.
    /// </summary>
    public class HyperlinkLabel : LinkLabel2
    {
        private string _url;

        private readonly ToolTip _toolTip = new ToolTip();

        private bool _isKeyHandled;

        /// <summary>
        ///     Gets or sets the URL that 
        /// </summary>
        [DefaultValue(null)]
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                _toolTip.SetToolTip(this, _url);
                Enabled = _url != null;
            }
        }

        /// <summary>
        ///     Constructs a new <see cref="HyperlinkLabel"/> instance.
        /// </summary>
        public HyperlinkLabel()
        {
            Url = null;
            Click += OnClick;
            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add(CreateOpenMenuItem());
            ContextMenuStrip.Items.Add(CreateCopyMenuItem());
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!_isKeyHandled && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space))
            {
                _isKeyHandled = true;
                e.Handled = true;
                OnClick(e);
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _isKeyHandled = false;
            base.OnKeyDown(e);
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
    }
}
