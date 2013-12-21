using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHeroGUI.Properties;
using DotNetUtils.FS;
using I18N;

namespace BDHeroGUI.Components
{
    public partial class DiscInfoMetadataPanel : UserControl
    {
        private const string NotFound = "(not found)";

        public DiscInfoMetadataPanel()
        {
            InitializeComponent();
        }

        public void SetDisc(Disc disc)
        {
            var fs = disc.FileSystem;
            var metadata = disc.Metadata;
            var raw = metadata.Raw;
            var der = metadata.Derived;

            // textboxes

            InitTextAndIcon(iconHardwareVolumeLabel, textBoxHardwareVolumeLabel, raw.HardwareVolumeLabel);
            InitTextAndIcon(iconAnyDVDDiscInf, textBoxAnyDVDDiscInf, raw.DiscInf != null ? raw.DiscInf.ToString() : null);
            InitTextAndIcon(iconDboxTitle, textBoxDboxTitle, raw.DboxTitle);
            InitTextAndIcon(iconVISAN, textBoxVISAN, GetIsanText(raw.V_ISAN));
            InitTextAndIcon(iconAllBdmtTitles, textBoxAllBdmtTitles, GetBdmtTitles(raw.AllBdmtTitles));

            InitText(textBoxVolumeLabelSanitized, der.VolumeLabelSanitized);
            InitText(textBoxDboxTitleSanitized, der.DboxTitleSanitized);
            InitText(textBoxIsan, GetIsanText(raw.ISAN));
            InitText(textBoxValidBdmtTitles, GetBdmtTitles(der.ValidBdmtTitles));

            // buttons

            InitButton(buttonVolumeLabelSanitized, fs.Directories.Root);
            InitButton(buttonDboxTitleSanitized, fs.Files.DBox);
            InitButton(buttonIsan, fs.Files.MCMF);
            InitButton(buttonValidBdmtTitles, fs.Directories.BDMT);
        }

        private static void InitText(TextBox textBox, string text)
        {
            var hasText = !string.IsNullOrWhiteSpace(text);
            textBox.Text = hasText ? text : NotFound;
        }

        private static void InitTextAndIcon(PictureBox icon, TextBox textBox, string text)
        {
            var hasText = !string.IsNullOrWhiteSpace(text);
            icon.Image = hasText ? Resources.tick : Resources.cross_red;
            textBox.Text = hasText ? text : NotFound;
        }

        private static void InitButton(Button button, FileSystemInfo info)
        {
            if (info == null || !info.Exists)
            {
                button.Enabled = false;
                return;
            }

            string path = info.FullName;

            var menu = new ContextMenuStrip();

            menu.Items.Add(new ToolStripMenuItem(path) { Enabled = false });
//            menu.Items.Add("-");

            if (info is FileInfo)
            {
                // TODO: Add default program association icon and name
                menu.Items.Add(new ToolStripMenuItem("&Open with default program", null,
                                                     (sender, args) => FileUtils.OpenFile(path)));
                menu.Items.Add(new ToolStripMenuItem("&Show in Explorer", Resources.folder_open,
                                                     (sender, args) => FileUtils.ShowInFolder(path)));
            }
            else if (info is DirectoryInfo)
            {
                menu.Items.Add(new ToolStripMenuItem("&Show in Explorer", Resources.folder_open,
                                                     (sender, args) => FileUtils.OpenFolder(path)));
            }

//            menu.Items.Add("-");
            menu.Items.Add(new ToolStripMenuItem("&Copy path to clipboard", Resources.clipboard_arrow,
                                                 (sender, args) => Clipboard.SetText(path)));

            button.Click += delegate
                {
                    menu.Show(button, 0, button.Height);
                };
        }

        private static string GetIsanText(Isan isan)
        {
            if (isan == null)
                return null;

            var lines = new List<string>();

            lines.Add(isan.IsSearchable ? "Valid:" : "Invalid:");
            lines.Add("");
            lines.Add(isan.NumberFormatted);
            lines.Add("");
            if (!string.IsNullOrWhiteSpace(isan.Title))
                lines.Add(string.Format("{0} ({1} - {2} min)", isan.Title, isan.Year, isan.LengthMin));
            else
                lines.Add("(no title/year/runtime found)");

            return string.Join(Environment.NewLine, lines);
        }

        private static string GetBdmtTitles(IDictionary<Language, string> bdmtTitles)
        {
            return !bdmtTitles.Any()
                       ? null
                       : string.Join(Environment.NewLine,
                                     bdmtTitles.Select(pair => string.Format("{0}: {1}", pair.Key.ISO_639_2, pair.Value)));
        }
    }
}
