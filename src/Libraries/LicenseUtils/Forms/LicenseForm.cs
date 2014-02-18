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

#if __MonoCS__
            tabPageFormatted.Hide();
            buttonPrint.Hide();
#else
            webBrowser.DocumentText = license.Html;
#endif

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
