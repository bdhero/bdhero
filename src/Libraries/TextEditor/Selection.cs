using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor
{
    public class Selection
    {
        public int StartLine;
        public int StartColumn;
        public int EndLine;
        public int EndColumn;

        public Selection(int startLine, int startColumn)
            : this(startLine, startColumn, startLine, startColumn)
        {
        }

        public Selection(int startLine, int startColumn, int endLine, int endColumn)
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
        }
    }
}
