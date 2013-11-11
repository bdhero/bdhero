using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using BDHero.Utils;
using DotNetUtils;
using OSUtils;

namespace BDHeroGUI.Forms
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AppUtils.AppName);
            this.labelProductName.Text = AppUtils.ProductName;
            this.labelVersion.Text = String.Format("Version {0}", AppUtils.AppVersion);
            this.labelBuildDate.Text = String.Format("Built on {0}", AppUtils.BuildDate);
            this.labelCopyright.Text = AppUtils.Copyright;
            this.textBoxSystemInfo.Text = string.Format("{0} {1}{2}{3}", AppUtils.AppName, AppUtils.AppVersion, Environment.NewLine, SystemInfo.Instance);
            new ToolTip().SetToolTip(linkLabelSourceCode, AppConstants.SourceCodeUrl);
        }

        private void linkLabelSourceCode_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppConstants.SourceCodeUrl);
        }
    }
}
