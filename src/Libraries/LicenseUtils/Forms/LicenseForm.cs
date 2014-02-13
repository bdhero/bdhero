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
        public LicenseForm(Work work)
        {
            InitializeComponent();

            var license = work.License;

            Text = string.Format("{0}: {1} license", work.Name, license);

            labelWorkName.Text = string.Format("{0}", work.Name);
            labelLicenseName.Text = string.Format("{0}", license.ToStringFull());

            hyperlinkLabelUrl.Url = license.Url;
            hyperlinkLabelUrl.Visible = !string.IsNullOrEmpty(license.Url);

            hyperlinkLabelTlDrUrl.Url = license.TlDrUrl;
            hyperlinkLabelTlDrUrl.Visible = !string.IsNullOrEmpty(license.TlDrUrl);

            webBrowser.DocumentText = license.Html;

            textBoxPlainText.Text = license.Text;
        }

        private void ShowPrintPreview()
        {
            webBrowser.ShowPrintPreviewDialog();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            ShowPrintPreview();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                ShowPrintPreview();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
