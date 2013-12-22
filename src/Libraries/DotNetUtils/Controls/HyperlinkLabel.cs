using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotNetUtils.Extensions;
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
        private bool _isKeyHandled;

        private readonly HyperlinkAddon _hyperlink;

        /// <summary>
        ///     Gets or sets the URL that 
        /// </summary>
        [DefaultValue(null)]
        public string Url
        {
            get { return _hyperlink.Url; }
            set
            {
                _hyperlink.Url = value;
                Enabled = value != null;
            }
        }

        /// <summary>
        ///     Constructs a new <see cref="HyperlinkLabel"/> instance.
        /// </summary>
        public HyperlinkLabel()
        {
            _hyperlink = HyperlinkAddon.MakeHyperlink(this);
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
    }
}
