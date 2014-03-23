using System.Linq;
using System.Reflection;
using System.Xml;
using TextEditor.Resources;

namespace TextEditor.SyntaxHighlighting.Providers
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
            var prefix = string.Format("{0}.SyntaxHighlighting.Definitions.", assemblyName);
            var fullFileNames = fileNames.Select(fileName => prefix + fileName).ToArray();

            foreach (var fullName in fullFileNames)
            {
                using (var stream = ResourceLoader.OpenStream(fullName))
                {
                    AddSyntaxMode(fullName, stream);
                }
            }
        }

        public override XmlTextReader GetSyntaxModeFile(MySyntaxMode syntaxMode)
        {
            return new XmlTextReader(ResourceLoader.OpenStream(syntaxMode.FileName));
        }
    }
}