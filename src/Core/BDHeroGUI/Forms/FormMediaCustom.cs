// Copyright 2012-2014 Andrew C. Dvorak
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
using System.Linq;
using System.Windows.Forms;
using BDHero.JobQueue;
using BDHeroGUI.Helpers;
using DotNetUtils.Dialogs.FS;
using DotNetUtils.FS;
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
            var all = new FileType
            {
                Description = "All Supported Formats",
                Extensions = AllExtensions
            };
            var jpeg = new FileType
            {
                Description = "JPEG Images",
                Extensions = JpegExtensions
            };
            var png = new FileType
            {
                Description = "PNG Images",
                Extensions = PngExtensions
            };
            var gif = new FileType
            {
                Description = "GIF Images",
                Extensions = GifExtensions
            };
            var bmp = new FileType
            {
                Description = "Bitmap Images",
                Extensions = BitmapExtensions
            };
            var extensions = new[] { all, jpeg, png, gif, bmp };
            return new OpenFileDialog2
            {
                Title = "Select Poster Image File",
                FileTypes = extensions,
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
