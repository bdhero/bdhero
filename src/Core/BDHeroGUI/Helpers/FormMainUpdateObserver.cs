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
using System.Windows.Forms;
using BDHeroGUI.Properties;
using UpdateLib;
using WebBrowserUtils;

namespace BDHeroGUI.Helpers
{
    public class FormMainUpdateObserver
    {
        private readonly ToolStripItem _menuItem;
        private readonly ToolStripItem _updateMenu;
        private readonly ToolStripItem _downloadItem;

        public FormMainUpdateObserver(ToolStripItem menuItem, ToolStripItem updateMenu, ToolStripItem downloadItem)
        {
            _menuItem = menuItem;
            _updateMenu = updateMenu;
            _updateMenu.Visible = false;
            _downloadItem = downloadItem;
        }

        public void Checking()
        {
            _menuItem.Text = "Checking for Updates...";
            _menuItem.Enabled = false;
        }

        public void UpdateFound(Update update)
        {
            _menuItem.Text = string.Format("Download Version {0}", update.Version);
            _menuItem.Enabled = true;

            _updateMenu.Visible = true;
            _downloadItem.Text = string.Format("Download v{0}...", update.Version);
            _downloadItem.Image = DefaultWebBrowser.Instance.GetIconAsBitmap(16) ?? Resources.network;
        }

        public void NoUpdateFound()
        {
            _menuItem.Text = string.Format("No Updates Available");
            _menuItem.Enabled = true;
        }

        public void Error(Exception exception)
        {
            _menuItem.Text = string.Format("Error: {0}", exception.Message);
            _menuItem.Enabled = true;
        }
    }
}