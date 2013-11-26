using System.Drawing;

namespace WebBrowserUtils
{
    public interface IWebBrowser
    {
        string ExePath { get; }

        Icon GetIcon(int size);

        Image GetIconAsBitmap(int size);
    }
}