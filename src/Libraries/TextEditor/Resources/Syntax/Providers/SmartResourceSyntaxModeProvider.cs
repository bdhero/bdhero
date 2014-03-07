using System.Linq;
using System.Reflection;
using System.Xml;
using ICSharpCode.TextEditor.Document;

namespace TextEditor.Resources.Syntax.Providers
{
    internal class SmartResourceSyntaxModeProvider : BaseSyntaxModeProvider
    {
        public SmartResourceSyntaxModeProvider()
            : this(ResourceLoader.GetResourceNamesEndingWith(".xshd"))
        {
        }

        public SmartResourceSyntaxModeProvider(params string[] fileNames)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;
            var prefix = string.Format("{0}.Resources.Syntax.Definitions.", assemblyName);
            var fullFileNames = fileNames.Select(fileName => prefix + fileName).ToArray();

            foreach (var fullName in fullFileNames)
            {
                using (var stream = ResourceLoader.OpenStream(fullName))
                {
                    AddSyntaxMode(fullName, stream);
                }
            }
        }

        public override XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
        {
            return new XmlTextReader(ResourceLoader.OpenStream(syntaxMode.FileName));
        }
    }
}