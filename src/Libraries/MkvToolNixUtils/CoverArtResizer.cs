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
    internal delegate void AttachFileDelegate(string attachmentFilePath);

    public class CoverArtResizer
    {
        private readonly ArgumentList _arguments;
        private readonly ITempFileRegistrar _tempFileRegistrar;

        public CoverArtResizer([NotNull] ArgumentList arguments, ITempFileRegistrar tempFileRegistrar)
        {
            _arguments = arguments;
            _tempFileRegistrar = tempFileRegistrar;
        }

        /// <summary>
        ///     Used by mkvmerge.
        /// </summary>
        /// <param name="releaseMedium"></param>
        public void AttachCoverArt([CanBeNull] ReleaseMedium releaseMedium)
        {
            AttachCoverArt(releaseMedium, AttachFile);
        }

        /// <summary>
        ///     Used by mkvpropedit.
        /// </summary>
        /// <param name="releaseMedium"></param>
        public void AddCoverArt([CanBeNull] ReleaseMedium releaseMedium)
        {
            AttachCoverArt(releaseMedium, AddAttachment);
        }

        private void AttachCoverArt([CanBeNull] ReleaseMedium releaseMedium, AttachFileDelegate @delegate)
        {
            var coverArt = releaseMedium != null ? releaseMedium.CoverArtImages.FirstOrDefault(image => image.IsSelected) : null;
            var coverArtImage = coverArt != null ? coverArt.Image : null;

            if (coverArtImage == null)
                return;

            AttachCoverArt(coverArtImage, @delegate);
        }

        private void AttachCoverArt([CanBeNull] Image coverArt, AttachFileDelegate @delegate)
        {
            var coverImagePathLarge = ResizeCoverArt(coverArt, CoverArtSize.Large, "cover.jpg");
            var coverImagePathSmall = ResizeCoverArt(coverArt, CoverArtSize.Small, "small_cover.jpg");

            if (coverImagePathLarge != null)
                @delegate(coverImagePathLarge);

            if (coverImagePathSmall != null)
                @delegate(coverImagePathSmall);
        }

        /// <summary>
        ///     Used by mkvmerge.
        /// </summary>
        /// <param name="attachmentFilePath"></param>
        private void AttachFile([NotNull] string attachmentFilePath)
        {
            _arguments.AddAll("--attach-file", attachmentFilePath);
        }

        /// <summary>
        ///     Used by mkvpropedit.
        /// </summary>
        /// <param name="attachmentFilePath"></param>
        private void AddAttachment([NotNull] string attachmentFilePath)
        {
            _arguments.AddAll("--add-attachment", attachmentFilePath);
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
