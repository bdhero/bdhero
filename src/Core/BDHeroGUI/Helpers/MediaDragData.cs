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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using DotNetUtils.Forms;
using DotNetUtils.FS;

namespace BDHeroGUI.Helpers
{
    public class MediaDragData
    {
        private static readonly Regex ReleaseYearRegex = new Regex(@"\((?<year>\d{4})\)$");

        private readonly string[] _imageExtensions;

        public bool AcceptDrop { get; private set; }

        #region Image file

        public IList<string> ImageFiles = new List<string>();

        [CanBeNull]
        public string ImageFile
        {
            get { return ImageFiles.FirstOrDefault(); }
        }

        public bool HasImageFile
        {
            get { return ImageFiles.Any(); }
        }

        #endregion

        #region Image URI

        /// <summary>
        ///     Local file path or remote HTTP address.
        /// </summary>
        /// <example>
        ///     <code>"C:\some\image.jpg"</code>
        ///     <code>"http://example.com/image.jpg"</code>
        /// </example>
        [CanBeNull]
        public string ImageUri;

        public bool HasImageUri
        {
            get { return !string.IsNullOrWhiteSpace(ImageUri); }
        }

        #endregion

        #region Movie title

        [CanBeNull]
        public string MovieTitle;

        public bool HasMovieTitle
        {
            get { return !string.IsNullOrWhiteSpace(MovieTitle); }
        }

        #endregion

        #region Release year

        [CanBeNull]
        public string ReleaseYear;

        public bool HasReleaseYear
        {
            get { return !string.IsNullOrWhiteSpace(ReleaseYear); }
        }

        #endregion

        public MediaDragData(DragEventArgs e, string[] imageExtensions)
        {
            _imageExtensions = imageExtensions;

            // ReSharper disable CSharpWarnings::CS0665
            if (AcceptDrop |= InitImageFiles(e)) return;
            if (AcceptDrop |= InitImageUri(e)) return;
            // ReSharper restore CSharpWarnings::CS0665

            AcceptDrop |= InitMovieTitle(e);
            AcceptDrop |= InitReleaseYear(e);
        }

        private bool InitImageFiles(DragEventArgs e)
        {
            ImageFiles = DragUtils.GetFilesWithExtension(e, _imageExtensions);
            return ImageFiles.Any();
        }

        private bool InitImageUri(DragEventArgs e)
        {
            if (HasImageFile)
                return false;

            var imageUri = DragUtils.GetUnicodeText(e);

            if (IsImageFilePath(imageUri))
            {
                ImageUri = imageUri;
                return true;
            }

            if (Uri.IsWellFormedUriString(imageUri, UriKind.Absolute))
            {
                ImageUri = imageUri;
                return true;
            }

            return false;
        }

        private bool InitMovieTitle(DragEventArgs e)
        {
            if (HasImageFile || HasImageUri)
                return false;

            var movieTitle = (DragUtils.GetUnicodeText(e) ?? "").Trim();

            if (string.IsNullOrWhiteSpace(movieTitle))
                return false;

            MovieTitle = movieTitle;
            return true;
        }

        private bool InitReleaseYear(DragEventArgs e)
        {
            if (HasImageFile || HasImageUri || MovieTitle == null || !HasMovieTitle)
                return false;

            var match = ReleaseYearRegex.Match(MovieTitle);

            if (!match.Success)
                return false;

            ReleaseYear = match.Groups["year"].Value;
            MovieTitle = ReleaseYearRegex.Replace(MovieTitle, "").Trim();
            return true;
        }

        private bool IsImageFilePath(string path)
        {
            try
            {
                return File.Exists(path) && FileUtils.FileHasExtension(path, _imageExtensions);
            }
            catch { }
            return false;
        }
    }
}
