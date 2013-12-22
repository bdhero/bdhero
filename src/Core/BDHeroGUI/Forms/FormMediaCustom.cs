using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BDHero.JobQueue;
using BDHeroGUI.Helpers;
using DotNetUtils.Controls;
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

        private void SetPosterImage(string imageUri)
        {
            pictureBoxPoster.ImageLocation = imageUri;

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
            var data = new MediaDragData(e, AllExtensions);
            if (data.AcceptDrop)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void FormMediaCustom_DragDrop(object sender, DragEventArgs e)
        {
            var data = new MediaDragData(e, AllExtensions);
            if (data.HasImageFile)
            {
                SetPosterImage(data.ImageFile);
            }
            if (data.HasImageUri)
            {
                SetPosterImage(data.ImageUri);
            }
            if (data.HasMovieTitle)
            {
                textBoxTitle.Text = data.MovieTitle;
            }
            if (data.HasReleaseYear)
            {
                textBoxYear.Text = data.ReleaseYear;
            }
        }
    }
}
