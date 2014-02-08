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
using System.Linq;
using System.Windows.Forms;
using BDHero.Plugin;
using BDHero.Utils;
using DotNetUtils;
using LicenseUtils;
using LicenseUtils.Controls;
using OSUtils.Info;

namespace BDHeroGUI.Forms
{
    partial class AboutBox : Form
    {
        private readonly string _nameVersionDate
            = string.Format("{0} v{1} ({2})",
                            AppUtils.AppName,
                            AppUtils.AppVersion,
                            AppUtils.BuildDate);

        public AboutBox(IPluginRepository pluginRepository)
        {
            InitializeComponent();

            SetWindowTitle();
            PopulateHeader();
            PopulateLicenses();
            PopulateSystemInfo(pluginRepository);
        }

        private void SetWindowTitle()
        {
            Text = String.Format("About {0}", AppUtils.AppName);
        }

        private void PopulateHeader()
        {
            labelProductName.Text = _nameVersionDate;
            labelCopyright.Text = AppUtils.Copyright;
            linkLabelSourceCode.Url = AppConstants.SourceCodeUrl;
        }

        private void PopulateLicenses()
        {
            var top = 0;

            SuspendLayout();

            foreach (var work in LicenseImporter.Works.All)
            {
                var workPanel = CreateWorkPanel(work, top);
                creditPanel.Controls.Add(workPanel);
                top += workPanel.Height;
            }

            ResumeLayout(false);
            PerformLayout();
        }

        private void PopulateSystemInfo(IPluginRepository pluginRepository)
        {
            var newline = Environment.NewLine;
            var plugins = string.Join(newline, pluginRepository.PluginsByType.Select(ToString));

            textBoxSystemInfo.Text = string.Format("{1} {0}{0}Plugins:{0}{2}{0}{0}{3}",
                                                   newline,
                                                   _nameVersionDate,
                                                   plugins,
                                                   SystemInfo.Instance);
        }

        private WorkPanel CreateWorkPanel(Work work, int top)
        {
            return new WorkPanel(work)
                   {
                       Top = top,
                       Left = 0,
                       Width = creditPanel.Width,
                       Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                   };
        }

        private static string ToString(IPlugin plugin)
        {
            return string.Format("    {0}: {1} v{2}{3}",
                                 plugin.RunOrder,
                                 plugin.Name,
                                 plugin.AssemblyInfo.Version,
                                 plugin.Enabled ? "" : " (disabled)");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
