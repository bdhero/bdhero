using System.Collections.Generic;
using System.Xml;
using ICSharpCode.TextEditor.Document;

namespace TextEditor.Resources.Syntax.Providers
{
    internal class SyntaxModeProviderAdapter : ISyntaxModeFileProvider
    {
        private readonly ISyntaxModeProvider _provider;
        private readonly List<SyntaxMode> _syntaxModes = new List<SyntaxMode>();

        public ICollection<SyntaxMode> SyntaxModes
        {
            get { return _syntaxModes; }
        }

        public SyntaxModeProviderAdapter(ISyntaxModeProvider provider)
        {
            _provider = provider;
        }

        public XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
        {
            return _provider.GetSyntaxModeFile(new MySyntaxMode(syntaxMode.FileName, syntaxMode.Name, syntaxMode.Extensions));
        }

        public void UpdateSyntaxModeList()
        {
            // resources don't change during runtime
        }
    }
}