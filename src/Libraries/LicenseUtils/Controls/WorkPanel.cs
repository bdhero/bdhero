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
using LicenseUtils.Forms;
using UILib.WinForms.Controls;

namespace LicenseUtils.Controls
{
    public partial class WorkPanel : UserControl
    {
        private readonly Work _work;

        public WorkPanel(Work work)
        {
            _work = work;

            InitializeComponent();

            labelWorkName.Text = work.Name;

            InitHyperlinkLabel(labelProject, work.Urls.Project);
            InitHyperlinkLabel(labelSource,  work.Urls.Source);
            InitHyperlinkLabel(labelPackage, work.Urls.Package);
            InitHyperlinkLabel(labelArticle, work.Urls.Article);

            PopulateAuthors(work);

            if (work.License == null)
            {
                labelLicense.Visible = false;
                return;
            }

            labelNoLicense.Visible = false;
            labelLicense.Text = string.Format("{0} license", work.License);
        }

        private void InitHyperlinkLabel(HyperlinkLabel label, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                label.Visible = false;
                return;
            }

            label.Url = url;
        }

        private void PopulateAuthors(Work work)
        {
            var top = 0;
            foreach (var author in work.Authors)
            {
                var authorPanel = CreateAuthorPanel(author, top);
                panelAuthors.Controls.Add(authorPanel);
                top += authorPanel.Height;
            }

            var oldHeight = panelAuthors.Height;
            var newHeight = top;

            Height += (newHeight - oldHeight);
        }

        private AuthorPanel CreateAuthorPanel(Author author, int top)
        {
            return new AuthorPanel(author)
                   {
                       Top = top,
                       Left = 0,
                       Width = panelAuthors.Width,
                       Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                   };
        }

        private void labelLicense_Click(object sender, System.EventArgs e)
        {
            using (var form = new LicenseForm(_work))
            {
                form.ShowDialog(this);
            }
        }
    }
}
