using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TextEditor.Resources.Syntax.Providers
{
    /// <summary>
    ///     Provides common base functionality for <see cref="ISyntaxModeProvider"/>.
    /// </summary>
    public abstract class BaseSyntaxModeProvider : ISyntaxModeProvider
    {
        private readonly List<MySyntaxMode> _syntaxModes = new List<MySyntaxMode>();

        public ICollection<MySyntaxMode> SyntaxModes
        {
            get { return _syntaxModes; }
        }

        /// <summary>
        ///     Reads the given <paramref name="stream"/> as an XML document in <c>.XSHD</c> format and extracts the
        ///     name and list of extensions from the syntax definition contained therein.
        /// </summary>
        /// <param name="fileName">
        ///     Name of the <c>.XSHD</c> file.  <b>NOTE:</b> This does not have to be a real file name -- it can be whatever you want.
        /// </param>
        /// <param name="stream">
        ///     Stream containing the contents of the <c>.XSHD</c> file.
        /// </param>
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
                                    _syntaxModes.Add(new MySyntaxMode(fileName,
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

        public abstract XmlTextReader GetSyntaxModeFile(MySyntaxMode syntaxMode);
    }
}
