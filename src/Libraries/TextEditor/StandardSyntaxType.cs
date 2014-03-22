using System;

namespace TextEditor
{
    /// <summary>
    ///     Enum containing common predefined syntax highlighting definitions that can be used without loading any
    ///     additional <c>.XSHD files</c>.
    /// </summary>
    public enum StandardSyntaxType
    {
        /// <summary>
        ///     No syntax highlighting.
        /// </summary>
        Default,

        /// <summary>
        ///     File names which may contain environment variables.
        /// </summary>
        [SyntaxModeName("FileName")]
        FileName,

        /// <summary>
        ///     File paths which may contain environment variables.
        /// </summary>
        [SyntaxModeName("FilePath")]
        FilePath,

        /// <summary>
        ///     Markdown syntax highlighting.
        /// </summary>
        [SyntaxModeName("MarkDown")]
        Markdown,
    }

    internal class SyntaxModeNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public SyntaxModeNameAttribute(string name)
        {
            Name = name;
        }
    }
}
