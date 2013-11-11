using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHeroGUI.Properties;

namespace BDHeroGUI.Components
{
    public partial class DiscInfoFeaturesPanel : UserControl
    {
        public DiscInfoFeaturesPanel()
        {
            InitializeComponent();
        }

        public void SetDisc(Disc disc)
        {
            var features = disc.Features;

            SetFeature(iconBDPlus, labelBDPlus, features.IsBDPlus);
            SetFeature(iconBDJava, labelBDJava, features.IsBDJava);
            SetFeature(icon3D, label3D, features.Is3D);
            SetFeature(iconDbox, labelDbox, features.IsDbox);
            SetFeature(iconDCopy, labelDCopy, features.IsDCopy);
            SetFeature(iconPSP, labelPSP, features.IsPSP);
        }

        private void SetFeature(PictureBox icon, Label label, bool enabled)
        {
            icon.Image = enabled ? Resources.tick : Resources.cross_red;
            label.Text = enabled ? "Yes" : "No";
        }
    }
}
