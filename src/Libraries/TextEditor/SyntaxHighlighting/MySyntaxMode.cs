namespace TextEditor.SyntaxHighlighting
{
    /// <summary>
    ///     Represents a syntax highlighting definition (<c>.XSHD</c>) file's public properties.
    /// </summary>
    public class MySyntaxMode
    {
        /// <summary>
        ///     Gets the name of the <c>.XSHD</c> file.  <b>NOTE:</b> This does not have to be a real file name -- it can be anything.
        /// </summary>
        /// <example>
        ///     <c>"MarkDownModeXshd.xml"</c>.
        /// </example>
        public readonly string FileName;

        /// <summary>
        ///     Gets the name of the language represented by the syntax highlighting definition.
        /// </summary>
        /// <example>
        ///     <c>"MarkDown"</c>, <c>"JavaScript"</c>.
        /// </example>
        public readonly string Name;

        /// <summary>
        ///     Gets a list of file extensions supported by the syntax highlighting definition.
        /// </summary>
        public readonly string[] Extensions;

        /// <summary>
        ///     Constructs a new <see cref="MySyntaxMode"/> instance with the given parameters.
        /// </summary>
        /// <param name="fileName">
        ///     Name of the <c>.XSHD</c> file.  <b>NOTE:</b> This does not have to be a real file name -- it can be anything.
        /// </param>
        /// <param name="name">
        ///     Name of the language represented by the syntax highlighting definition.
        /// </param>
        /// <param name="extensions">
        ///     List of file extensions supported by the syntax highlighting definition, separated by semicolons.
        /// </param>
        public MySyntaxMode(string fileName, string name, string extensions)
        {
            FileName = fileName;
            Name = name;
            Extensions = extensions.Split(';');
        }

        /// <summary>
        ///     Constructs a new <see cref="MySyntaxMode"/> instance with the given parameters.
        /// </summary>
        /// <param name="fileName">
        ///     Name of the <c>.XSHD</c> file.  <b>NOTE:</b> This does not have to be a real file name -- it can be anything.
        /// </param>
        /// <param name="name">
        ///     Name of the language represented by the syntax highlighting definition.
        /// </param>
        /// <param name="extensions">
        ///     List of file extensions supported by the syntax highlighting definition.
        /// </param>
        public MySyntaxMode(string fileName, string name, params string[] extensions)
        {
            FileName = fileName;
            Name = name;
            Extensions = extensions;
        }
    }
}