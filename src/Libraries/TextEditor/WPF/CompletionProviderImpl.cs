#if !__MonoCS__
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace TextEditor.WPF
{
    internal class CompletionProviderImpl
    {
        public ICompletionData[] GenerateCompletionData()
        {
            var allCompletions = AllCompletions();
            var relevantCompletions = allCompletions;

            //            var prevText = GetCharsToLeftOfCursor(textArea).Text;
            //
            //            if (prevText != "")
            //            {
            //                relevantCompletions =
            //                    allCompletions.Where(data => data.Text.StartsWith(prevText, true, CultureInfo.CurrentUICulture))
            //                                  .ToArray();
            //            }

            if (relevantCompletions.Any())
                return relevantCompletions;

            return allCompletions;
        }

        private static ICompletionData[] AllCompletions()
        {
            var completions = new List<ICompletionData>();

            var placeholders = new Dictionary<string, string>
                               {
                                   { "{title}",    "Name of the movie or TV show episode." },
                                   { "{year}",     "Year the movie was released." },
                                   { "{res}",      "Vertical resolution of the primary video track (e.g., 1080i, 720p, 480p)." },
                                   { "{channels}", "Number of audio channels (2.0, 5.1, 7.1, etc.)." },
                                   { "{vcodec}",   "Primary video track codec." },
                                   { "{acodec}",   "Primary audio track codec." },
                               };

            var placeholdersOrdered = placeholders.Select(pair => new MyCompletionData(pair.Key, pair.Value, null))
                                                  .OrderBy(data => data.Text);

            completions.AddRange(placeholdersOrdered);

            var envVars = Environment.GetEnvironmentVariables()
                                     .OfType<DictionaryEntry>()
                                     .Select(entry =>
                                             new KeyValuePair<string, string>(entry.Key as string,
                                                                              entry.Value as string))
                                     .OrderBy(pair => pair.Key)
                                     .ToArray();

            completions.AddRange(envVars.Select(pair => new MyCompletionData(string.Format("%{0}%", pair.Key), pair.Value, null)));

            return completions.ToArray();
        }

        /// Implements AvalonEdit ICompletionData interface to provide the entries in the
        /// completion drop down.
        public class MyCompletionData : ICompletionData
        {
            public MyCompletionData(string text)
            {
                Text = text;
                Description = "Description for " + Text;
            }

            public MyCompletionData(string text, string description, ImageSource image)
            {
                Text = text;
                Description = description;
                Image = image;
            }

            public ImageSource Image { get; private set; }

            public string Text { get; private set; }

            // Use this property if you want to show a fancy UIElement in the list.
            public object Content
            {
                get { return Text; }
            }

            public object Description { get; private set; }

            public double Priority { get; private set; }

            public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
            {
                // http://www.codeproject.com/Articles/42490/Using-AvalonEdit-WPF-Text-Editor
                var textCompositionEventArgs = insertionRequestEventArgs as TextCompositionEventArgs;
                var keyEventArgs = insertionRequestEventArgs as KeyEventArgs;
                var mouseEventArgs = insertionRequestEventArgs as MouseEventArgs;

                textArea.Document.Replace(completionSegment, Text);
            }
        }
    }
}
#endif
