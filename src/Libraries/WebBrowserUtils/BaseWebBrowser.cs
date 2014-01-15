// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.Drawing;
using System.Drawing.IconLib;
using System.Linq;
using System.Text;

namespace WebBrowserUtils
{
    public abstract class BaseWebBrowser : IWebBrowser
    {
        protected MultiIcon MultiIcon;

        public string ExePath { get; protected set; }

        private IconImage GetIconImage(int size)
        {
            if (MultiIcon == null || !MultiIcon.Any())
            {
                return null;
            }
            var icons = MultiIcon.First()
                                 .Where(image => image.Size.Width == size)
                                 .OrderByDescending(image => image.ColorsInPalette);
            return icons.FirstOrDefault();
        }

        public Icon GetIcon(int size)
        {
            var iconImage = GetIconImage(size);
            return iconImage == null ? null : iconImage.Icon;
        }

        public Image GetIconAsBitmap(int size)
        {
            var iconImage = GetIconImage(size);
            return iconImage == null ? null : iconImage.Transparent;
        }
    }
}
