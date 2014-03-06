using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor
{
    public enum StandardSyntaxType
    {
        Default,

        [SyntaxModeName("FileName")]
        FileName,

        [SyntaxModeName("FilePath")]
        FilePath
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
