using System;
using System.Linq;
using System.Windows.Forms;
using BDHero.JobQueue;
using DotNetUtils.Controls;
using DotNetUtils.Forms;
using I18N;

namespace BDHeroGUI.Forms
{
    public partial class FormMediaCustom : Form
    {
        private readonly Movie _movie;
        private readonly Movie _movieClone;

        private readonly OpenFileDialog2 _openFileDialog = CreateOpenFileDialog();

        public FormMediaCustom(Movie movie)
        {
            InitializeComponent();

            _movie = movie;
            _movieClone = movie.Clone();

            textBoxTitle.Text = _movie.Title ?? "";
            textBoxYear.Text = _movie.ReleaseYearDisplayable ?? "";

            if (_movie.CoverArtImages.Any())
            {
                pictureBoxPoster.Image = _movie.CoverArtImages.First().Image;
            }
        }

        private void pictureBoxPoster_Click(object sender, EventArgs e)
        {
            BrowseCoverArt();
        }

        private void linkLabelBrowse_Click(object sender, EventArgs e)
        {
            BrowseCoverArt();
        }

        private void BrowseCoverArt()
        {
            if (_openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            SetPosterImage(_openFileDialog.SelectedPath);
        }

        private void SetPosterImage(string imageFilePath)
        {
            pictureBoxPoster.ImageLocation = imageFilePath;

            _movieClone.CoverArtImages.Clear();
            _movieClone.CoverArtImages.Add(new InMemoryCoverArt
                                           {
                                               IsSelected = true,
                                               Language = Language.Undetermined,
                                               Image = pictureBoxPoster.Image
                                           });
        }

        private static readonly string[] JpegExtensions = { ".jpg", ".jpeg" };
        private static readonly string[] PngExtensions = { ".png" };
        private static readonly string[] GifExtensions = { ".gif" };
        private static readonly string[] BitmapExtensions = { ".bmp" };
        private static readonly string[] AllExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        private static OpenFileDialog2 CreateOpenFileDialog()
        {
            var all = new FileExtension
            {
                Description = "All Supported Formats",
                Extensions = AllExtensions,
                IsDefault = true
            };
            var jpeg = new FileExtension
            {
                Description = "JPEG Images",
                Extensions = JpegExtensions
            };
            var png = new FileExtension
            {
                Description = "PNG Images",
                Extensions = PngExtensions
            };
            var gif = new FileExtension
            {
                Description = "GIF Images",
                Extensions = GifExtensions
            };
            var bmp = new FileExtension
            {
                Description = "Bitmap Images",
                Extensions = BitmapExtensions
            };
            var extensions = new[] { all, jpeg, png, gif, bmp };
            return new OpenFileDialog2
            {
                Title = "Select Poster Image File",
                FileExtensions = extensions,
            };
        }

        private void FormMediaCustom_DragEnter(object sender, DragEventArgs e)
        {
            var imageFiles = DragUtils.GetFilesWithExtension(e, AllExtensions);
            if (imageFiles.Any())
            {
                e.Effect = DragDropEffects.All;
            }

            // TODO: Accept drag and drop from shortcuts and image URLs from Web browsers
            var imageUrl = DragUtils.GetUnicodeText(e);
            Console.WriteLine(imageUrl);
        }

        private void FormMediaCustom_DragDrop(object sender, DragEventArgs e)
        {
            var imageFiles = DragUtils.GetFilesWithExtension(e, AllExtensions);
            if (!imageFiles.Any())
                return;
            SetPosterImage(imageFiles.First());
        }
    }
}
