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
    sealed partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            Text = String.Format("About {0}", AppUtils.AppName);
            labelProductName.Text = AppUtils.ProductName;
            labelVersion.Text = String.Format("Version {0}", AppUtils.AppVersion);
            labelBuildDate.Text = String.Format("Built on {0}", AppUtils.BuildDate);
            labelCopyright.Text = AppUtils.Copyright;
            linkLabelSourceCode.Url = AppConstants.SourceCodeUrl;
            textBoxSystemInfo.Text = string.Format("{0} {1}{2}{3}", AppUtils.AppName, AppUtils.AppVersion, Environment.NewLine, SystemInfo.Instance);
        }

        private void linkLabelSourceCode_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppConstants.SourceCodeUrl);
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
