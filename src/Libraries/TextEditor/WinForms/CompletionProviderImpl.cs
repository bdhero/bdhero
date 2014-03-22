using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace TextEditor.WinForms
{
    public class CompletionProviderImpl : ICompletionDataProvider
    {
        private readonly ImageList imageList;

        public CompletionProviderImpl(ImageList imageList)
        {
            this.imageList = imageList;
        }

        public ImageList ImageList
        {
            get { return imageList; }
        }

        public string PreSelection
        {
            get { return null; }
        }

        // -1 for "no default selected"
        public int DefaultIndex
        {
            get { return 0; }
        }

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }
            return CompletionDataProviderKeyResult.InsertionKey;
        }

        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            var prevWord = GetCharsToLeftOfCursor(textArea);
            var prevText = prevWord.Text;
            if (prevText != "")
            {
                if (data.Text.StartsWith(prevText, true, CultureInfo.CurrentUICulture))
                {
                    textArea.SelectionManager.SetSelection(
                            new TextLocation(prevWord.ColumnNumber, prevWord.LineNumber),
                            new TextLocation(prevWord.ColumnNumber + prevWord.Length, prevWord.LineNumber)
                        );
                }
            }

            return data.InsertAction(textArea, key);
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            var allCompletions = AllCompletions();
            var relevantCompletions = allCompletions;

            var prevText = GetCharsToLeftOfCursor(textArea).Text;

            if (prevText != "")
            {
                relevantCompletions =
                    allCompletions.Where(data => data.Text.StartsWith(prevText, true, CultureInfo.CurrentUICulture))
                                  .ToArray();
            }

            if (relevantCompletions.Any())
                return relevantCompletions;

            return allCompletions;
        }

        private static StartWord GetCharsToLeftOfCursor(TextArea textArea)
        {
            var doc = textArea.Document;
            var caret = textArea.Caret;
            var lineSegment = doc.GetLineSegment(caret.Line);

            var emptyWord = new StartWord("", caret.Line, caret.Column, 0, caret.Offset);

            // Caret is at the beginning of the line
            if (caret.Column == 0)
                return emptyWord;

            var curWord = lineSegment.GetWord(caret.Column);

            // Caret is at the end of the line
            if (curWord == null)
                return emptyWord;

            if (curWord.IsWhiteSpace)
                curWord = GetPrevWord(caret, lineSegment, curWord);

            Console.WriteLine("Cur word: \"{0}\"", curWord.Word);

            var prevWord = GetPrevWord(caret, lineSegment, curWord);

            // Caret is at the start of the current word - get the previous word
            if (caret.Offset == curWord.Offset)
            {
                // This should never happen
                if (prevWord == null)
                    return emptyWord;

                var isSameSyntax = AreEqual(prevWord.SyntaxColor, curWord.SyntaxColor);

                // Caret is between two different syntax highlights
                if (!isSameSyntax)
                    return emptyWord;

                curWord = prevWord;
                prevWord = GetPrevWord(caret, lineSegment, curWord);
            }

            // Caret is in whitespace
            if (curWord.IsWhiteSpace)
                return emptyWord;

            var selectionOffsetStart = curWord.Offset;
            var selectionLength = caret.Offset - curWord.Offset;

            var startText = doc.GetText(selectionOffsetStart, selectionLength);

            if (prevWord != null)
            {
                var prevText = prevWord.Word;

                const string envVar = "%";
                const string placeholder = "${";

                if (prevText.EndsWith(envVar))
                {
                    startText = envVar + startText;
                    selectionOffsetStart -= envVar.Length;
                    selectionLength += envVar.Length;
                }
                else if (prevText.EndsWith(placeholder))
                {
                    startText = placeholder + startText;
                    selectionOffsetStart -= placeholder.Length;
                    selectionLength += placeholder.Length;
                }
            }

            // Caret is in whitespace
            if (new Regex(@"\s+$").IsMatch(startText))
                return emptyWord;

            Console.WriteLine("Starting text: \"{0}\"", startText);

            var selectionColumn = caret.Column - selectionLength;

            var startWord = new StartWord(startText, caret.Line, selectionColumn, selectionLength, selectionOffsetStart);

            return startWord;
        }

        private class StartWord
        {
            public readonly string Text;
            public readonly int LineNumber;
            public readonly int ColumnNumber;
            public readonly int Length;
            public readonly int Offset;

            public StartWord(string text, int lineNumber, int columnNumber, int length, int offset)
            {
                Text = text;
                LineNumber = lineNumber;
                ColumnNumber = columnNumber;
                Length = length;
                Offset = offset;
            }
        }

        private static bool AreEqual(HighlightColor highlight1, HighlightColor highlight2)
        {
            if (highlight1 == null || highlight2 == null)
                return false;

            if (highlight1.Bold != highlight2.Bold)
                return false;

            if (highlight1.Italic != highlight2.Italic)
                return false;

            if (!highlight1.BackgroundColor.Equals(highlight2.BackgroundColor))
                return false;

            if (!highlight1.Color.Equals(highlight2.Color))
                return false;

            return true;
        }

        private static TextWord GetPrevWord(Caret caret, LineSegment lineSegment, TextWord curWord)
        {
            TextWord prevWord = null;

            var idx = caret.Column;
            while (prevWord == null && idx-- > 0)
            {
                var thisWord = lineSegment.GetWord(idx);
                if (thisWord.Offset == curWord.Offset || thisWord.IsWhiteSpace)
                    continue;

                prevWord = thisWord;
            }

            return prevWord;
        }

        private static ICompletionData[] AllCompletions()
        {
            var completions = new List<ICompletionData>();

            var placeholders = new Dictionary<string, string>
                               {
                                   { "${title}",    "Name of the movie or TV show episode." },
                                   { "${year}",     "Year the movie was released." },
                                   { "${res}",      "Vertical resolution of the primary video track (e.g., 1080i, 720p, 480p)." },
                                   { "${channels}", "Number of audio channels (2.0, 5.1, 7.1, etc.)." },
                                   { "${vcodec}",   "Primary video track codec." },
                                   { "${acodec}",   "Primary audio track codec." },
                               };

            var placeholdersOrdered = placeholders.Select(pair => new DefaultCompletionData(pair.Key, pair.Value, 0))
                                                  .OrderBy(data => data.Text);

            completions.AddRange(placeholdersOrdered);

            var envVars = Environment.GetEnvironmentVariables()
                                     .OfType<DictionaryEntry>()
                                     .Select(entry =>
                                             new KeyValuePair<string, string>(entry.Key as string,
                                                                              entry.Value as string))
                                     .OrderBy(pair => pair.Key)
                                     .ToArray();

            completions.AddRange(envVars.Select(pair => new DefaultCompletionData(string.Format("%{0}%", pair.Key), pair.Value, 1)));

            return completions.ToArray();
        }
    }
}