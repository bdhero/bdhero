using System.Windows.Forms;
using DotNetUtils.Controls;
using LicenseUtils.Forms;

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
            new LicenseForm(_work).ShowDialog(this);
        }
    }
}
