using System.Xml;
using ICSharpCode.TextEditor.Document;

namespace TextEditor.Resources
{
    internal class SmartResourceSyntaxModeProvider : BaseSyntaxModeProvider
    {
        public SmartResourceSyntaxModeProvider()
            : this(Resources.GetResourceNamesEndingWith(".xshd"))
        {
        }

        public SmartResourceSyntaxModeProvider(params string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                using (var stream = Resources.OpenStream(fileName))
                {
                    AddSyntaxMode(fileName, stream);
                }
            }
        }

        public override XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
        {
            return new XmlTextReader(Resources.OpenStream(syntaxMode.FileName));
        }
    }
}