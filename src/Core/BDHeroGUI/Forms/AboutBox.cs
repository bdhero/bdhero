// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.Windows.Forms;
using BDHero.Utils;
using DotNetUtils;
using DotNetUtils.FS;
using OSUtils;
using OSUtils.Info;

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
