using System;

namespace TextEditor
{
    public enum StandardSyntaxType
    {
        Default,

        [SyntaxModeName("FileName")]
        FileName,

        [SyntaxModeName("FilePath")]
        FilePath,

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
