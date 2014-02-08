using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;

namespace LicenseUtils.Controls
{
    public partial class WorkPanel : UserControl
    {
        public WorkPanel(Work work)
        {
            InitializeComponent();

            labelWorkName.Text = work.Name;

            InitHyperlinkLabel(labelProject, work.Urls.Project);
            InitHyperlinkLabel(labelSource,  work.Urls.Source);
            InitHyperlinkLabel(labelPackage, work.Urls.Package);
            InitHyperlinkLabel(labelArticle, work.Urls.Article);

            var authors = work.Authors.Select(author => "(C) " + author.ToStringDescriptive()).ToArray();
            textBoxAuthors.Text = string.Join(Environment.NewLine, authors);
            var delta = textBoxAuthors.GetAutoSizeDelta();
            Height += delta.Height;

            if (work.License == null)
            {
                labelLicense.Visible = false;
                return;
            }

            labelNoLicense.Visible = false;
            labelLicense.Text = work.License.ToStringDescriptive();
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
    }
}
