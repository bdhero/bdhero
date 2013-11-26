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

namespace BDHeroGUI.Forms
{
    public partial class FormDiscInfo : Form
    {
        public FormDiscInfo(Disc disc)
        {
            InitializeComponent();

            var fs = disc.FileSystem;
            var metadata = disc.Metadata;

            labelQuickSummary.Text = GenerateQuickSummary(metadata, fs);

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
    }
}
