using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ICSharpCode.TextEditor.Document;

namespace TextEditor.Resources
{
    internal class T4SyntaxModeProvider : BaseSyntaxModeProvider
    {
        private readonly Dictionary<string, string> _models = new Dictionary<string, string>
                                                              {
                                                                  { "FilePathModeXshd.tt", new FilePathModeXshd().TransformText() },
                                                                  { "FileNameModeXshd.tt", new FileNameModeXshd().TransformText() }
                                                              };

        public T4SyntaxModeProvider()
        {
            foreach (var model in _models)
            {
                using (var stream = GetStream(model.Key))
                {
                    AddSyntaxMode(model.Key, stream);
                }
            }
        }

        public override XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
        {
            return new XmlTextReader(GetStream(syntaxMode.FileName));
        }

        private MemoryStream GetStream(string modelKey)
        {
            var modelValue = _models[modelKey];
            return new MemoryStream(Encoding.UTF8.GetBytes(modelValue));
        }
    }
}