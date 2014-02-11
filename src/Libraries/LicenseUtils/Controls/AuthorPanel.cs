// Copyright 2014 Andrew C. Dvorak
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

using System.Windows.Forms;

namespace LicenseUtils.Controls
{
    public partial class AuthorPanel : UserControl
    {
        public AuthorPanel(Author author)
        {
            InitializeComponent();

            labelNotice.Text = string.Format("© {0} {1}", author.YearRanges, author.Name ?? author.Organization);

            emailLabel.Address = emailLabel.Text = author.Email;
            emailLabel.Visible = !string.IsNullOrEmpty(author.Email);

            hyperlinkLabel.Url = hyperlinkLabel.Text = author.Url;
            hyperlinkLabel.Visible = !string.IsNullOrEmpty(author.Url);

            // Visual Studio's UI editor doesn't save the value of this property for some reason,
            // so we need to explicitly set it
            labelNotice.AutoSize = true;
            emailLabel.AutoSize = true;
            hyperlinkLabel.AutoSize = true;
        }
    }
}
