using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace TextEditor.WinForms
{
    public class CodeCompletionProviderImpl : ICompletionDataProvider
    {
        private readonly ImageList imageList;

        public CodeCompletionProviderImpl(ImageList imageList)
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

        public int DefaultIndex
        {
            get { return -1; }
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
            textArea.Caret.Position = textArea.Document.OffsetToPosition(
                    Math.Min(insertionOffset, textArea.Document.TextLength)
                );
            return data.InsertAction(textArea, key);
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            var allCompletions = AllCompletions();
            var relevantCompletions = allCompletions;

            var prevText = GetCharsToLeftOfCursor(textArea);

            if (prevText != "")
            {
                relevantCompletions =
                    allCompletions.Where(data => data.Text.StartsWith(prevText, true, CultureInfo.CurrentUICulture))
                                  .ToArray();
            }

//            string name = prevNonWhitespaceTerm.Word;
//            if (name == "specification" || name == "requires" || name == "same_machine_as" || name == "@")
//            {
//                return ModulesSuggestions();
//            }
//            int temp;
//            if (name == "users_per_machine" || int.TryParse(name, out temp))
//            {
//                return NumbersSuggestions();
//            }

            if (relevantCompletions.Any())
                return relevantCompletions;

            return allCompletions;
        }

        private static string GetCharsToLeftOfCursor(TextArea textArea)
        {
            var doc = textArea.Document;
            var caret = textArea.Caret;
            var lineSegment = doc.GetLineSegment(caret.Line);

            // Caret is at the beginning of the line
            if (caret.Column == 0)
                return "";

            var curWord = lineSegment.GetWord(caret.Column);

            // Caret is at the end of the line
            if (curWord == null)
                return "";

            Console.WriteLine("Cur word: \"{0}\"", curWord.Word);

            var prevWord = GetPrevWord(caret, lineSegment, curWord);

            // Caret is at the start of the current word - get the previous word
            if (caret.Offset == curWord.Offset)
            {
                // TODO: Describe
                if (prevWord == null)
                    return "";

                var isSameSyntax = AreEqual(prevWord.SyntaxColor, curWord.SyntaxColor);

                // TODO: Describe
                if (!isSameSyntax)
                    return "";

                curWord = prevWord;
            }

            // Caret is in whitespace
            if (curWord.IsWhiteSpace)
                return "";

            var startText = doc.GetText(curWord.Offset, caret.Offset - curWord.Offset);

            if (prevWord != null)
            {
                var prevText = prevWord.Word;
                if (prevText.EndsWith("%"))
                    startText = "%" + startText;
                if (prevText.EndsWith("${"))
                    startText = "${" + startText;
            }

//            // Should never happen
//            if (string.IsNullOrWhiteSpace(prevText))
//                return "";
//
//            // Should never happen
//            if (new Regex(@"\s+$").IsMatch(prevText))
//                return "";
//
//            return new Regex(@"\S+$").Match(prevText).Value;

            Console.WriteLine("Starting text in current word: \"{0}\"", startText);

            return startText;
        }

        private static bool AreEqual(HighlightColor highlight1, HighlightColor highlight2)
        {
            if (highlight1 == null || highlight2 == null)
                return false;

            if (highlight1.Bold != highlight2.Bold)
                return false;

            if (highlight1.Italic != highlight2.Italic)
                return false;

//            if (highlight1.HasBackground != highlight2.HasBackground)
//                return false;

//            if (highlight1.HasForeground != highlight2.HasForeground)
//                return false;

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
                if (thisWord.Offset == curWord.Offset)
                    continue;

                prevWord = thisWord;
            }

            return prevWord;
        }

        private static TextWord FindPreviousWord(TextArea textArea)
        {
            LineSegment lineSegment = textArea.Document.GetLineSegment(textArea.Caret.Line);
            TextWord currentWord = lineSegment.GetWord(textArea.Caret.Column);

            if (currentWord == null && lineSegment.Words.Count > 0)
                currentWord = lineSegment.Words[lineSegment.Words.Count - 1];

            // we actually want the previous word, not the current one, in order to make decisions on it.
            int currentIndex = lineSegment.Words.IndexOf(currentWord);
            if (currentIndex == -1)
                return null;

            return lineSegment.Words.GetRange(0, currentIndex).FindLast(word => word.Word.Trim() != "");
        }

        private static ICompletionData[] AllCompletions()
        {
            var completions = new List<ICompletionData>();

            var placeholders = new Dictionary<string, string>
                               {
                                   { "${title}", "Name of the movie or TV show episode." },
                                   { "${year}", "Year the movie was released." },
                                   { "${res}", "Vertical resolution of the primary video track (e.g., 1080i, 720p, 480p)." },
                                   { "${channels}", "Number of audio channels (2.0, 5.1, 7.1, etc.)." },
                                   { "${vcodec}", "Primary video track codec." },
                                   { "${acodec}", "Primary audio track codec." },
                               };

            completions.AddRange(placeholders.Select(pair => new DefaultCompletionData(pair.Key, pair.Value, 0)));

            var envVars = Environment.GetEnvironmentVariables()
                                     .OfType<DictionaryEntry>()
                                     .Select(entry =>
                                             new KeyValuePair<string, string>(entry.Key as string,
                                                                              entry.Value as string))
                                     .ToArray();

            completions.AddRange(envVars.Select(pair => new DefaultCompletionData(string.Format("%{0}%", pair.Key), pair.Value, 1)));

            return completions.ToArray();
        }
    }
}