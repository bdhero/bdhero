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
