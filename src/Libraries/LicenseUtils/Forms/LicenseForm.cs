using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LicenseUtils.Forms
{
    public partial class LicenseForm : Form
    {
        public LicenseForm(License license)
        {
            InitializeComponent();

            Text = license.ToString();
            labelName.Text = string.Format("{0}", license.ToStringFull());

            hyperlinkLabelUrl.Url = license.Url;
            hyperlinkLabelUrl.Visible = !string.IsNullOrEmpty(license.Url);

            hyperlinkLabelTlDrUrl.Url = license.TlDrUrl;
            hyperlinkLabelTlDrUrl.Visible = !string.IsNullOrEmpty(license.TlDrUrl);

            var htmlProto = "<!doctype html>" +
                            "<html>" +
                            "<head>" +
                            "<title>{0}</title>" +
                            "</head>" +
                            "<body>{1}</body>" +
                            "</html>";

            webBrowser.DocumentText = string.Format(htmlProto, license, license.Html);

            textBoxPlainText.Text = license.Text;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
