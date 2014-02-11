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
