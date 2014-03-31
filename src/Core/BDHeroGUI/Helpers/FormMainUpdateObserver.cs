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
using DotNetUtils;
using DotNetUtils.Net;
using UpdateLib;
using UpdateLib.V1;
using WebBrowserUtils;

namespace BDHeroGUI.Helpers
{
    public class FormMainUpdateObserver : IUpdateObserverV1
    {
        private readonly Form _form;
        private readonly ToolStripItem _menuItem;
        private readonly ToolStripItem _updateMenu;
        private readonly ToolStripItem _downloadItem;

        public event BeforeInstallUpdateEventHandler BeforeInstallUpdate;

        public FormMainUpdateObserver(Form form, ToolStripItem menuItem, ToolStripItem updateMenu, ToolStripItem downloadItem)
        {
            _form = form;
            _menuItem = menuItem;
            _updateMenu = updateMenu;
            _updateMenu.Visible = false;
            _downloadItem = downloadItem;
        }

        public void OnBeforeCheckForUpdate()
        {
            _menuItem.Text = "Checking for Updates...";
            _menuItem.Enabled = false;
        }

        public void OnUpdateReadyToDownload(Update update)
        {
            _menuItem.Text = string.Format("Download Version {0}", update.Version);
            _menuItem.Enabled = true;

            _updateMenu.Visible = true;
            _downloadItem.Text = string.Format("Download v{0}...", update.Version);
            _downloadItem.Image = DefaultWebBrowser.Instance.GetIconAsBitmap(16) ?? Resources.network;
        }

        public void OnBeforeDownloadUpdate(Update update)
        {
            _menuItem.Text = string.Format("Downloading Version {0}...", update.Version);
            _menuItem.Enabled = false;

            _updateMenu.Visible = false;
        }

        public void OnUpdateDownloadProgressChanged(Update update, FileDownloadProgress progress)
        {
            _menuItem.Text =
                string.Format(
                    "Downloading Update: {0:P}...",
                    progress.PercentComplete / 100.0);
        }

        public void OnUpdateException(Exception exception)
        {
            _menuItem.Text = string.Format("Error: {0}", exception.Message);
            _menuItem.Enabled = true;
        }

        public void OnUpdateReadyToInstall(Update update)
        {
            _menuItem.Text = string.Format("Install Version {0}", update.Version);
            _menuItem.Enabled = true;
        }

        public void OnNoUpdateAvailable()
        {
            _menuItem.Text = string.Format("No Updates Available");
            _menuItem.Enabled = true;
        }

        public bool ShouldInstallUpdate(Update update)
        {
            const string caption = "Application restart required";
            var text =
                string.Format(
                    "To install the update, you must first close the application.\n\nClose {0} and install update?",
                    AppUtils.ProductName);

            return DialogResult.Yes ==
                   MessageBox.Show(_form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public void OnBeforeInstallUpdate(Update update)
        {
            _menuItem.Text = string.Format("Installing Version {0}...", update.Version);
            _menuItem.Enabled = false;

            if (BeforeInstallUpdate != null)
                BeforeInstallUpdate(update);
        }
    }
}