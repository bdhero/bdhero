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

            labelQuickSummary.Text = string.Format("{0} {1}",
                                                   metadata.Derived.VolumeLabel,
                                                   fs.Directories.Root.FullName
                );

            discInfoMetadataPanel.SetDisc(disc);
            discInfoFeaturesPanel.SetDisc(disc);

            this.EnableSelectAll();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
