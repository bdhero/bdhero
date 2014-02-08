using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LicenseUtils.Controls
{
    public partial class AuthorPanel : UserControl
    {
        public AuthorPanel(Author author)
        {
            InitializeComponent();

            labelYearRanges.Text = author.YearRanges;
            labelName.Text = author.Name ?? author.Organization;

            emailLabel.Address = emailLabel.Text = author.Email;
            emailLabel.Visible = !string.IsNullOrEmpty(author.Email);

            hyperlinkLabel.Url = hyperlinkLabel.Text = author.Url;
            hyperlinkLabel.Visible = !string.IsNullOrEmpty(author.Url);
        }
    }
}
