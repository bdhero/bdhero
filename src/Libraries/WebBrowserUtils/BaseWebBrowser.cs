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

        public Icon GetIcon(int size)
        {
            if (MultiIcon != null && MultiIcon.Any())
            {
                var icons = MultiIcon.First().Where(image => image.Size.Width == size).OrderByDescending(image => image.ColorsInPalette);
                if (icons.Any())
                    return icons.First().Icon;
            }
            return null;
        }

        public Image GetIconAsBitmap(int size)
        {
            var icon = GetIcon(size);
            return icon != null ? icon.ToBitmap() : null;
        }
    }
}
