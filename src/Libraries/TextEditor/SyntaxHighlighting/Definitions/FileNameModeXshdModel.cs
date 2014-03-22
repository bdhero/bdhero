using System.IO;
using System.Linq;
using DotNetUtils;

namespace TextEditor.SyntaxHighlighting.Definitions
{
    partial class FileNameModeXshd
    {
        private static char[] InvalidNameChars
        {
            get { return Path.GetInvalidFileNameChars(); }
        }

        private static string[] InvalidNameCharsXmlEscaped
        {
            get
            {
                return InvalidNameChars.Select(Escape).Where(IsNotNullOrEmpty).ToArray();
            }
        }

        private static string Escape(char ch)
        {
            return XmlTextEncoder.Encode(new string(ch, 1));
        }

        private static bool IsNotNullOrEmpty(string s)
        {
            return !string.IsNullOrEmpty(s);
        }
    }
}
