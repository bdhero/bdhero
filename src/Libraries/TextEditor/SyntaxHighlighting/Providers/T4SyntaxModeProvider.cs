using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using TextEditor.SyntaxHighlighting.Definitions;

namespace TextEditor.SyntaxHighlighting.Providers
{
    /// <summary>
    ///     Provides syntax modes from compiled T4 templates.
    /// </summary>
    public class T4SyntaxModeProvider : BaseSyntaxModeProvider
    {
        private static readonly Dictionary<string, string> DefaultModels
            = new Dictionary<string, string>
              {
                  { "FilePathModeXshd.tt", new FilePathModeXshd().TransformText().Trim() },
                  { "FileNameModeXshd.tt", new FileNameModeXshd().TransformText().Trim() }
              };

        private readonly IDictionary<string, string> _models;

        /// <summary>
        ///     Constructs a new <see cref="T4SyntaxModeProvider"/> instance with the default list of syntax modes
        ///     stored in the <see cref="TextEditor.Resources.Syntax.Definitions"/> namespace.
        /// </summary>
        public T4SyntaxModeProvider()
            : this(DefaultModels)
        {
        }

        /// <summary>
        ///     Constructs a new <see cref="T4SyntaxModeProvider"/> instance with the the given list of <paramref name="models"/>.
        /// </summary>
        /// <param name="models">
        ///     Map of key value pairs where the <b>key</b> is the syntax mode's file name and
        ///     the <b>value</b> is the text contents of the <c>.XSHD</c> file.
        /// </param>
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

        public override XmlTextReader GetSyntaxModeFile(MySyntaxMode syntaxMode)
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