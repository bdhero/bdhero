using System.Drawing.IconLib;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace WebBrowserUtils
{
    /// <seealso cref="http://stackoverflow.com/a/17599201/467582"/>
    public class WindowsWebBrowser : BaseWebBrowser
    {
        private WindowsWebBrowser(string exePath, MultiIcon multiIcon)
        {
            ExePath = exePath;
            MultiIcon = multiIcon;
        }

        public static WindowsWebBrowser Default
        {
            get
            {
                return new Builder()
                    .UseDefaultProgId()
                    .GetExePath()
                    .LoadIcon()
                    .Build();
            }
        }

        private class Builder
        {
            private const string ExeSuffix = ".exe";

            private const string UserChoiceKey =
                @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";

            private readonly Regex _registryExePathRegex = new Regex(@"^(?<quote>""?)(?<path>[^""]+?\.exe)\1");

            private string _progId;
            private string _exePath;
            private MultiIcon _multiIcon;

            public Builder UseDefaultProgId()
            {
                using (var userChoiceKey = Registry.CurrentUser.OpenSubKey(UserChoiceKey))
                {
                    if (userChoiceKey == null) { return this; }
                    object progIdValue = userChoiceKey.GetValue("Progid");
                    if (progIdValue == null) { return this; }
                    _progId = progIdValue.ToString();
                }
                return this;
            }

            public Builder GetExePath()
            {
                if (_progId == null) { return this; }

                using (var pathKey = Registry.ClassesRoot.OpenSubKey(_progId + @"\shell\open\command"))
                {
                    if (pathKey == null) { return this; }

                    // Trim parameters.
                    try
                    {
                        _exePath = pathKey.GetValue(null).ToString().ToLower().Replace("\"", "");

                        if (!_exePath.EndsWith(ExeSuffix))
                        {
                            if (_registryExePathRegex.IsMatch(_exePath))
                            {
                                var match = _registryExePathRegex.Match(_exePath);
                                _exePath = match.Groups["path"].Value;
                            }
                        }
                    }
                    catch
                    {
                        // Assume the registry value is set incorrectly, or some funky browser is used which currently is unknown.
                    }
                }

                return this;
            }

            public Builder LoadIcon()
            {
                if (_progId == null) { return this; }
                if (_exePath == null) { return this; }

                _multiIcon = new MultiIcon();
                _multiIcon.Load(_exePath);

                // Fallback
//                _icon = Icon.ExtractAssociatedIcon(_exePath);

                return this;
            }

            public WindowsWebBrowser Build()
            {
                return new WindowsWebBrowser(_exePath, _multiIcon);
            }
        }
    }
}
