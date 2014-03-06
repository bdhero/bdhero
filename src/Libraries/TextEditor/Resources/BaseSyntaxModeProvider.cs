using System.Collections.Generic;
using System.IO;
using System.Xml;
using ICSharpCode.TextEditor.Document;

namespace TextEditor.Resources
{
    internal abstract class BaseSyntaxModeProvider : ISyntaxModeFileProvider
    {
        private readonly List<SyntaxMode> _syntaxModes = new List<SyntaxMode>();

        public ICollection<SyntaxMode> SyntaxModes
        {
            get { return _syntaxModes; }
        }

        protected void AddSyntaxMode(string fileName, Stream stream)
        {
            using (var reader = new XmlTextReader(stream))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "SyntaxDefinition":
                                    _syntaxModes.Add(new SyntaxMode(fileName,
                                                                    reader.GetAttribute("name"),
                                                                    reader.GetAttribute("extensions")));
                                    break;
                            }
                            break;
                    }
                }
                reader.Close();
            }
        }

        public abstract XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode);

        public void UpdateSyntaxModeList()
        {
            // resources don't change during runtime
        }
    }
}
