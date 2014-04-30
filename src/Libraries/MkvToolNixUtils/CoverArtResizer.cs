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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BDHero.JobQueue;
using DotNetUtils.Annotations;
using DotNetUtils.FS;
using ProcessUtils;

namespace MkvToolNixUtils
{
    public class CoverArtResizer
    {
        private readonly ITempFileRegistrar _tempFileRegistrar;

        public CoverArtResizer(ITempFileRegistrar tempFileRegistrar)
        {
            _tempFileRegistrar = tempFileRegistrar;
        }

        public void AttachCoverArt([NotNull] ArgumentList arguments, [CanBeNull] ReleaseMedium releaseMedium)
        {
            var coverArt = releaseMedium != null ? releaseMedium.CoverArtImages.FirstOrDefault(image => image.IsSelected) : null;
            var coverArtImage = coverArt != null ? coverArt.Image : null;

            if (coverArtImage == null)
                return;

            AddCoverArt(arguments, coverArtImage);
        }

        private void AddCoverArt([NotNull] ArgumentList arguments, [CanBeNull] Image coverArt)
        {
            var coverImagePathLarge = ResizeCoverArt(coverArt, CoverArtSize.Large, "cover.jpg");
            var coverImagePathSmall = ResizeCoverArt(coverArt, CoverArtSize.Small, "small_cover.jpg");

            if (coverImagePathLarge != null)
                AddAttachment(arguments, coverImagePathLarge);

            if (coverImagePathSmall != null)
                AddAttachment(arguments, coverImagePathSmall);
        }

        private static void AddAttachment([NotNull] ArgumentList arguments, [NotNull] string attachmentFilePath)
        {
            arguments.AddAll("--attach-file", attachmentFilePath);

            // TODO: mkvpropedit
//            arguments.AddAll("--add-attachment", attachmentFilePath);
        }

        /// <summary>
        ///     Resizes the cover art <paramref name="image"/> to the appropriate dimensions and saves it to a
        ///     temporary file with the given <paramref name="filename"/>.
        /// </summary>
        /// <param name="image">
        ///     Full size cover art image from TMDb.  If <paramref name="image" /> is <c>null</c>, this method will return
        ///     <c>null</c>.
        /// </param>
        /// <param name="size">
        ///     Desired output size of the cover art image.
        /// </param>
        /// <param name="filename">
        ///     One of <c>"cover.{jpg,png}"</c> or <c>"small_cover.{jpg,png}"</c>.
        /// </param>
        /// <returns>
        ///     Full, absolute path to the resized image on disk if <paramref name="image" /> is not <c>null</c>;
        ///     otherwise <c>null</c>.
        /// </returns>
        [CanBeNull]
        private string ResizeCoverArt([CanBeNull] Image image, CoverArtSize size, [NotNull] string filename)
        {
            if (image == null)
                return null;

            var ext = Path.GetExtension(filename.ToLowerInvariant());
            var format = ext == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
            var path = _tempFileRegistrar.CreateTempFile(GetType(), filename);

            ScaleImage(image, (int) size, int.MaxValue).Save(path, format);

            return path;
        }

        [NotNull]
        private static Image ScaleImage([NotNull] Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double) maxWidth / image.Width;
            var ratioY = (double) maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int) (image.Width * ratio);
            var newHeight = (int) (image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}
