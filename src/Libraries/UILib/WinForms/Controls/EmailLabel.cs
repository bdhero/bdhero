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

using System.ComponentModel;
using System.Windows.Forms;

namespace UILib.WinForms.Controls
{
    /// <summary>
    ///     <see cref="LinkLabel"/> subclass for hyperlinks to a website URL.
    ///     Provides a tooltip with the link's URL and a context menu to launch the site
    ///     in the user's browser or copy the URL to the clipboard.
    /// </summary>
    public class EmailLabel : LinkLabel2
    {
        private bool _isKeyHandled;

        private readonly EmailAddress _emailAddress;

        /// <summary>
        ///     Gets or sets the URL that 
        /// </summary>
        [DefaultValue(null)]
        public string Address
        {
            get { return _emailAddress.Address; }
            set
            {
                _emailAddress.Address = value;
                Enabled = value != null;
            }
        }

        /// <summary>
        ///     Constructs a new <see cref="HyperlinkLabel"/> instance.
        /// </summary>
        public EmailLabel()
        {
            _emailAddress = EmailAddress.MakeEmailAddress(this);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!_isKeyHandled && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space))
            {
                _isKeyHandled = true;
                e.Handled = true;
                OnClick(e);
            }

            if (IsCopyKey(e))
            {
                Clipboard.SetText(Address);
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Handled)
                return;

            _isKeyHandled = false;

            base.OnKeyUp(e);
        }
    }
}
