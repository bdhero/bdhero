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
            var meta = disc.Metadata;
            var raw = meta.Raw;
            var der = meta.Derived;
            var inf = raw.DiscInf;

            string discInfVolumeLabel = null;
            string regionName = null;
            string regionDesc = null;

            if (inf != null)
            {
                discInfVolumeLabel = inf.VolumeLabel;
                regionName = inf.Region.GetLongName();
                regionDesc = inf.Region.GetDescription();
            }

            new ToolTip().SetToolTip(textBoxAnyDVDDiscInfSanitized, regionDesc);

            // textboxes

            InitTextAndIcon(iconHardwareVolumeLabel, textBoxHardwareVolumeLabel, raw.HardwareVolumeLabel);
            InitTextAndIcon(iconAnyDVDDiscInf, textBoxAnyDVDDiscInf, discInfVolumeLabel);
            InitTextAndIcon(iconDboxTitle, textBoxDboxTitle, raw.DboxTitle);
            InitTextAndIcon(iconVISAN, textBoxVISAN, GetIsanText(raw.V_ISAN));
            InitTextAndIcon(iconAllBdmtTitles, textBoxAllBdmtTitles, GetBdmtTitles(raw.AllBdmtTitles));

            InitText(textBoxVolumeLabelSanitized, der.VolumeLabelSanitized);
            InitText(textBoxAnyDVDDiscInfSanitized, regionName);
            InitText(textBoxDboxTitleSanitized, der.DboxTitleSanitized);
            InitText(textBoxIsan, GetIsanText(raw.ISAN));
            InitText(textBoxValidBdmtTitles, GetBdmtTitles(der.ValidBdmtTitles));

            // buttons

            InitButton(buttonVolumeLabelSanitized, fs.Directories.Root);
            InitButton(buttonAnyDVDDiscInfSanitized, fs.Files.AnyDVDDiscInf);
            InitButton(buttonDboxTitleSanitized, fs.Files.DBox);
            InitButton(buttonIsan, fs.Files.MCMF);
            InitButton(buttonValidBdmtTitles, fs.Directories.BDMT);
        }

        private static void InitText(TextBox textBox, string text)
        {
            var hasText = !string.IsNullOrWhiteSpace(text);
            textBox.Text = hasText ? text : NotFound;
            textBox.Enabled = hasText;
        }

        private static void InitTextAndIcon(PictureBox icon, TextBox textBox, string text)
        {
            var hasText = !string.IsNullOrWhiteSpace(text);
            icon.Image = hasText ? Resources.tick : Resources.cross_red;
            InitText(textBox, text);
        }

        private static void InitButton(Button button, FileSystemInfo info)
        {
            if (info == null || !info.Exists)
            {
                button.Enabled = false;
                return;
            }

            FileSystemContextMenuStrip menu = null;

            if (info is FileInfo)
                menu = new FileContextMenuStrip(info);

            if (info is DirectoryInfo)
                menu = new DirectoryContextMenuStrip(info);

            if (menu == null)
                return;

            button.Click += (sender, args) => menu.Show(button, 0, button.Height);
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

    abstract class FileSystemContextMenuStrip : ContextMenuStrip
    {
        protected readonly FileSystemInfo Info;
        protected readonly string Path;

        protected FileSystemContextMenuStrip(FileSystemInfo info)
        {
            Info = info;
            Path = Info.FullName;
        }

        protected ToolStripMenuItem CreatePath()
        {
            return new ToolStripMenuItem(Path) { Enabled = false };
        }

        protected ToolStripMenuItem CreateCopyToClipboard()
        {
            return new ToolStripMenuItem("&Copy path to clipboard", Resources.clipboard_arrow, CopyToClipboard);
        }

        private void CopyToClipboard(object sender, EventArgs args)
        {
            Clipboard.SetText(Path);
        }
    }

    sealed class FileContextMenuStrip : FileSystemContextMenuStrip
    {
        private readonly FileAssociation _association;

        public FileContextMenuStrip(FileSystemInfo info)
            : base(info)
        {
            _association = new FileAssociation(Path);

            // Common
            Items.Add(CreatePath());

            // File-specific
            Items.Add(CreateOpenFile());
            Items.Add(CreateShowInFolder());

            // Common
            Items.Add(CreateCopyToClipboard());
        }

        private ToolStripMenuItem CreateOpenFile()
        {
            var text = _association.HasAssociation
                           ? string.Format("&Open with {0}", _association.AppName)
                           : "&Open with...";
            return new ToolStripMenuItem(text, _association.GetProgramImage(16), OpenFile);
        }

        private ToolStripMenuItem CreateShowInFolder()
        {
            return new ToolStripMenuItem("&Show in folder", Resources.folder_open, ShowInExplorer);
        }

        private void OpenFile(object sender, EventArgs args)
        {
            FileUtils.OpenFile(Path);
        }

        private void ShowInExplorer(object sender, EventArgs args)
        {
            FileUtils.ShowInFolder(Path);
        }
    }

    sealed class DirectoryContextMenuStrip : FileSystemContextMenuStrip
    {
        public DirectoryContextMenuStrip(FileSystemInfo info)
            : base(info)
        {
            // Common
            Items.Add(CreatePath());

            // Directory-specific
            Items.Add(CreateOpenFolder());

            // Common
            Items.Add(CreateCopyToClipboard());
        }

        private ToolStripMenuItem CreateOpenFolder()
        {
            return new ToolStripMenuItem("&Open folder", Resources.folder_open, Explore);
        }

        private void Explore(object sender, EventArgs args)
        {
            FileUtils.OpenFolder(Path);
        }
    }
}
