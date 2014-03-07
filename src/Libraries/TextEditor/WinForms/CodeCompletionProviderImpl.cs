using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var completions = AllCompletions();

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

            return completions;
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