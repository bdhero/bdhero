using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;
using DotNetUtils.FS;

namespace BDHeroGUI.Forms
{
    public partial class FormDiscInfo : Form
    {
        private readonly PictureBoxSizeMode _defaultJacketSizeMode;

        private readonly bool _hasSmallJacketImage;
        private readonly string _smallJacketImagePath;

        public FormDiscInfo(Disc disc)
        {
            InitializeComponent();

            _defaultJacketSizeMode = pictureBoxJacket.SizeMode;

            var fs = disc.FileSystem;
            var metadata = disc.Metadata;

            labelQuickSummary.Text = GenerateQuickSummary(metadata, fs);

            if (fs.Files.JacketImageSmall != null)
            {
                _hasSmallJacketImage = true;
                _smallJacketImagePath = fs.Files.JacketImageSmall.FullName;

                pictureBoxJacket.ImageLocation = _smallJacketImagePath;
                toolStripMenuItemJacketImagePath.Text = _smallJacketImagePath;
            }

            groupBoxJacket.Visible = _hasSmallJacketImage;

            discInfoMetadataPanel.SetDisc(disc);
            discInfoFeaturesPanel.SetDisc(disc);

            this.EnableSelectAll();
        }

        private static string GenerateQuickSummary(DiscMetadata metadata, DiscFileSystem fs)
        {
            var volumeLabel = metadata.Derived.VolumeLabel;
            var fullPath = fs.Directories.Root.FullName;
            if (fullPath.EndsWith(volumeLabel))
            {
                return fullPath;
            }
            return string.Format("{0} ({1})", fullPath, volumeLabel);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ExpandJacketImage()
        {
            if (!_hasSmallJacketImage)
            {
                return;
            }

            pictureBoxJacket.SizeMode = PictureBoxSizeMode.AutoSize;
            groupBoxJacket.AutoSize = true;
        }

        private void CollapseJacketImage()
        {
            if (!_hasSmallJacketImage)
            {
                return;
            }

            pictureBoxJacket.SizeMode = _defaultJacketSizeMode;
            groupBoxJacket.AutoSize = false;
        }

        private void pictureBoxJacket_MouseEnter(object sender, EventArgs e)
        {
            ExpandJacketImage();
        }

        private void pictureBoxJacket_MouseLeave(object sender, EventArgs e)
        {
            if (!_isJacketContextMenuOpen)
                CollapseJacketImage();
        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.ShowInFolder(_smallJacketImagePath, this);
        }

        private void copyPathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_smallJacketImagePath);
        }

        private bool _isJacketContextMenuOpen;

        private void contextMenuStripJacket_Opened(object sender, EventArgs e)
        {
            ExpandJacketImage();
            _isJacketContextMenuOpen = true;
        }

        private void contextMenuStripJacket_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            CollapseJacketImage();
            _isJacketContextMenuOpen = false;
        }
    }
}
