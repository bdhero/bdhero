// Copyright 2012-2014 Andrew C. Dvorak
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NativeAPI.Win.User;
using BDHero.Plugin;
using BDHero.Utils;
using DotNetUtils;
using DotNetUtils.Extensions;
using LicenseUtils;
using LicenseUtils.Controls;
using OSUtils.Info;
using UILib.Extensions;

namespace BDHeroGUI.Forms
{
    partial class AboutBox : Form
    {
        private readonly string _nameAndVersion
            = string.Format("{0} v{1}",
                            AppUtils.AppName,
                            AppUtils.AppVersion);

        private int _workY;

        public AboutBox(IPluginRepository pluginRepository)
        {
            InitializeComponent();

            SetWindowTitle();
            PopulateHeader();
            PopulateLicenses();
            PopulateSystemInfo(pluginRepository);

            creditPanel.EnableVerticalKeyboardScroll();
        }

        /// <summary>
        ///     Reduce flickering.
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/a/89125/467582"/>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (!SystemInformation.TerminalServerSession)
                    cp.ExStyle |= ExtendedWindowStyles.WS_EX_COMPOSITED.ToInt32();
                return cp;
            }
        }

        private void SetWindowTitle()
        {
            Text = String.Format("About {0}", AppUtils.AppName);
        }

        private void PopulateHeader()
        {
            labelProductName.Text = _nameAndVersion;
            labelBuildDate.Text = string.Format("Built on {0:G}", AppUtils.BuildDate);
            labelCopyright.Text = AppUtils.Copyright;
            linkLabelSourceCode.Url = AppConstants.SourceCodeUrl;
        }

        private void PopulateLicenses()
        {
            SuspendLayout();

            LicenseImporter.Works.All.ForEach(AddWork);

            ResumeLayout(false);
            PerformLayout();
        }

        private void AddWork(Work work, bool isLast)
        {
            var workPanel = CreateWorkPanel(work, _workY);
            creditPanel.Controls.Add(workPanel);
            _workY += workPanel.Height;

            if (isLast)
                return;

            var divider = CreateDivider(_workY);
            creditPanel.Controls.Add(divider);
            _workY += divider.Height;
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

        private Control CreateDivider(int top)
        {
            var margin = new Padding(2, 10, 2, 10);
            var width = creditPanel.Width
                        - creditPanel.Padding.Left
                        - creditPanel.Padding.Right
                        - margin.Left
                        - margin.Right
                ;
            return new Panel
                   {
                       Top = top,
                       Left = 0,
                       Width = width,
                       Height = 1,
                       Margin = margin,
                       BackColor = SystemColors.MenuBar,
                       Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                   };
        }

        private void PopulateSystemInfo(IPluginRepository pluginRepository)
        {
            var newline = Environment.NewLine;
            var plugins = string.Join(newline, pluginRepository.PluginsByType.Select(ToString));

            textBoxSystemInfo.Text = string.Format("{1} {0}{0}Plugins:{0}{2}{0}{0}{3}",
                                                   newline,
                                                   _nameAndVersion,
                                                   plugins,
                                                   SystemInfo.Instance);
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
