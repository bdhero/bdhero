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
