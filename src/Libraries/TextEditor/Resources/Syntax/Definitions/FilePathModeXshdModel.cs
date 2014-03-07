using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetUtils;

namespace TextEditor.Resources.Syntax.Definitions
{
    partial class FilePathModeXshd
    {
        private static char[] DriveLetters
        {
            get
            {
                var driveLetters = new List<char>();
                for (var letter = 'A'; letter <= 'Z'; letter++)
                {
                    driveLetters.Add(letter);
                }
                return driveLetters.ToArray();
            }
        }

        private static char[] InvalidPathChars
        {
            get { return Path.GetInvalidPathChars(); }
        }

        private static string[] InvalidPathCharsXmlEscaped
        {
            get
            {
                return InvalidPathChars.Select(Escape).Where(IsNotNullOrEmpty).ToArray();
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
