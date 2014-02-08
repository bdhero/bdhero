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
            var startTime = DateTime.Now;

            InitializeComponent();

            var endTime = DateTime.Now;
            var time = endTime - startTime;
//            Console.WriteLine("{0}: {1}", time.ToStringLong(), work);

            return;

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
