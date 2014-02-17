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

using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.IconLib;
using System.Linq;

namespace WebBrowserUtils
{
    public abstract class BaseWebBrowser : IWebBrowser
    {
        protected MultiIcon MultiIcon;

        public string ExePath { get; protected set; }

        private readonly ConcurrentDictionary<int, IconImage> _iconImages =
            new ConcurrentDictionary<int, IconImage>();

        private readonly ConcurrentDictionary<int, Icon> _icons =
            new ConcurrentDictionary<int, Icon>();

        private readonly ConcurrentDictionary<int, Image> _images =
            new ConcurrentDictionary<int, Image>();

        private IconImage GetIconImage(int size)
        {
            return _iconImages.GetOrAdd(size, GetIconImageImpl);
        }

        private IconImage GetIconImageImpl(int size)
        {
            if (MultiIcon == null || !MultiIcon.Any())
            {
                return null;
            }
            var iconImages = MultiIcon.First()
                                 .Where(image => image.Size.Width == size)
                                 .OrderByDescending(image => image.ColorsInPalette)
                                 .ToArray();
            return iconImages.FirstOrDefault();
        }

        public Icon GetIcon(int size)
        {
            return _icons.GetOrAdd(size, GetIconImpl);
        }

        private Icon GetIconImpl(int size)
        {
            var iconImage = GetIconImage(size);
            return iconImage == null ? null : iconImage.Icon;
        }

        public Image GetIconAsBitmap(int size)
        {
            return _images.GetOrAdd(size, GetIconAsBitmapImpl);
        }

        private Image GetIconAsBitmapImpl(int size)
        {
            var iconImage = GetIconImage(size);
            return iconImage == null ? null : iconImage.Transparent;
        }
    }
}
