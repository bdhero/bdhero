using System;
using System.Windows.Forms;
using DotNetUtils;
using DotNetUtils.Net;
using UpdateLib;

namespace BDHeroGUI.Helpers
{
    public class FormMainUpdateObserver : IUpdateObserver
    {
        private readonly Form _form;
        private readonly ToolStripItem _menuItem;
        private readonly Control _button;

        public event BeforeInstallUpdateEventHandler BeforeInstallUpdate;

        public FormMainUpdateObserver(Form form, ToolStripItem menuItem, Control button)
        {
            _form = form;
            _menuItem = menuItem;
            _button = button;
        }

        public void OnBeforeCheckForUpdate()
        {
            _menuItem.Text = "Checking for Updates...";
            _menuItem.Enabled = false;
        }

        public void OnBeforeDownloadUpdate(Update update)
        {
            _menuItem.Text = string.Format("Downloading Version {0}...", update.Version);
            _menuItem.Enabled = false;
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