using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebBrowserUtils
{
    public static class DefaultWebBrowser
    {
        public static IWebBrowser Instance
        {
            get
            {
                try
                {
                    return WindowsWebBrowser.Default;
                }
                catch
                {
                    return new UnknownWebBrowser();
                }
            }
        }
    }
}
