namespace TextEditor
{
    /// <summary>
    ///     Represents a single contiguous range of selected text within the editor.
    /// </summary>
    public class Selection
    {
        /// <summary>
        ///     Gets the line on which the selection begins.
        /// </summary>
        public readonly int StartLine;

        /// <summary>
        ///     Gets the column on which the selection begins.
        /// </summary>
        public readonly int StartColumn;

        /// <summary>
        ///     Gets the line on which the selection ends.
        /// </summary>
        public readonly int EndLine;

        /// <summary>
        ///     Gets the column on which the selection ends.
        /// </summary>
        public readonly int EndColumn;

        /// <summary>
        ///     Constructs a new <see cref="Selection"/> instance with the same start and end position.
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="startColumn"></param>
        public Selection(int startLine, int startColumn)
            : this(startLine, startColumn, startLine, startColumn)
        {
        }

        /// <summary>
        ///     Constructs a new <see cref="Selection"/> instance with the given start and end positions.
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="startColumn"></param>
        /// <param name="endLine"></param>
        /// <param name="endColumn"></param>
        public Selection(int startLine, int startColumn, int endLine, int endColumn)
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
        }
    }
}
