using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using DotNetUtils.Extensions;
using ICSharpCode.TextEditor.Document;

namespace TextEditor.Resources
{
    public class T4SyntaxModeProvider : BaseSyntaxModeProvider
    {
        private static readonly Dictionary<string, string> DefaultModels
            = new Dictionary<string, string>
              {
                  { "FilePathModeXshd.tt", new FilePathModeXshd().TransformText().Trim() },
                  { "FileNameModeXshd.tt", new FileNameModeXshd().TransformText().Trim() }
              };

        private readonly IDictionary<string, string> _models;

        public T4SyntaxModeProvider()
            : this(DefaultModels)
        {
        }

        public T4SyntaxModeProvider(IDictionary<string, string> models)
        {
            _models = models;

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