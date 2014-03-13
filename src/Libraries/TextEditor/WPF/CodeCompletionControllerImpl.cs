using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using NativeAPI.Win.User;

namespace TextEditor.WPF
{
    internal class CodeCompletionControllerImpl
    {
        private readonly ICSharpCode.AvalonEdit.TextEditor _editor;

        private CompletionWindow _completionWindow;
        
        private bool _isWindowMoveEventBound;
        private bool _isClosing;

        public CodeCompletionControllerImpl(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _editor = editor;

            _editor.PreviewKeyDown += EditorOnPreviewKeyDown;
            _editor.TextArea.TextEntering += TextAreaOnTextEntering;
            _editor.TextArea.TextEntered += TextAreaOnTextEntered;
            _editor.TextArea.MouseWheel += TextAreaOnMouseWheel;
        }

        public bool IgnoreTabOrEnterKey
        {
            get { return _completionWindow != null; }
        }

        #region Event Handlers - Text Editor

        private void EditorOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true;
                Show();
            }
        }

        private void TextAreaOnTextEntering(object sender, TextCompositionEventArgs e)
        {
            TextAreaOnTextEntering(e.Text, (EventArgs) e);
        }

        private void TextAreaOnTextEntering(string text, EventArgs e = null)
        {
            if (text.Length > 0 && _completionWindow != null)
            {
                if (!char.IsLetterOrDigit(text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    _completionWindow.CompletionList.RequestInsertion(e ?? EventArgs.Empty);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void TextAreaOnTextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "$" || e.Text == "%" || _completionWindow != null)
            {
                Show();
            }
        }

        private void TextAreaOnMouseWheel(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            // Workaround for bug in Avalon:
            // Show completion window, then focus completion window w/ mouse or scroll wheel.
            // Click back in the text editor area.  The completion window disappears, but doesn't "close".
            Close();
        }

        #endregion

        #region Event Handlers - Parent Window

        private void BindWindowEventHandlers()
        {
            if (_isWindowMoveEventBound)
                return;

            Window parentWindow = Window.GetWindow(_editor);
            if (parentWindow != null)
            {
                parentWindow.LocationChanged += Close;
                parentWindow.SizeChanged += Close;
                parentWindow.StateChanged += Close;
            }
            else
            {
                var form = _editor.FindForm();
                if (form != null)
                {
                    form.Move += Close;
                    form.LocationChanged += Close;
                    form.SizeChanged += Close;
                }
            }

            _isWindowMoveEventBound = true;
        }

        #endregion

        #region Event Handlers - Completion Window

        private void BindCompletionWindowEventHandlers()
        {
            _completionWindow.Loaded      += CompletionWindowOnLoaded;
            _completionWindow.KeyDown     += CompletionWindowOnKeyDown;
            _completionWindow.KeyUp       += CompletionWindowOnKeyUp;
            _completionWindow.Deactivated += CompletionWindowOnDeactivated;
            _completionWindow.Closing     += CompletionWindowOnClosing;
            _completionWindow.Closed      += CompletionWindowOnClosed;
        }

        private void UnbindCompletionWindowEventHandlers()
        {
            _completionWindow.Loaded      -= CompletionWindowOnLoaded;
            _completionWindow.KeyDown     -= CompletionWindowOnKeyDown;
            _completionWindow.KeyUp       -= CompletionWindowOnKeyUp;
            _completionWindow.Deactivated -= CompletionWindowOnDeactivated;
            _completionWindow.Closing     -= CompletionWindowOnClosing;
            _completionWindow.Closed      -= CompletionWindowOnClosed;
        }

        private void CompletionWindowOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            WithCompletionList(list => list.SelectedItem = list.CompletionData.FirstOrDefault());
        }

        private void CompletionWindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_completionWindow.IsKeyboardFocusWithin)
                return;

            switch (e.Key)
            {
                // Avalon already handles these, even when the completion window is focused
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                    break;

                // Handled by CompletionWindowOnKeyUp
                case Key.Down:
                case Key.Up:
                case Key.Tab:
                case Key.Enter:
                case Key.Escape:
                    break;

                // Close the completion window
                case Key.Back:
                case Key.Delete:
                    Close();
                    break;

                // All other keys get inserted as text input (if applicable)
                default:
                    ProxyInputKey(e);
                    break;
            }
        }

        private void CompletionWindowOnKeyUp(object sender, KeyEventArgs e)
        {
            if (!_completionWindow.IsKeyboardFocusWithin)
                return;

            switch (e.Key)
            {
                // Proxy key events that Avalon doesn't handle when the completion window is focused
                case Key.Down:
                case Key.Up:
                case Key.Tab:
                case Key.Enter:
                    ProxyDialogKey(e);
                    break;

                // Close the completion window
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void ProxyDialogKey(KeyEventArgs e)
        {
            WithCompletionList(list => list.HandleKey(e));
        }

        private void ProxyInputKey(KeyEventArgs e)
        {
            var keyString = GetPrintableString(e.Key);
            if (keyString == "")
                return;

            TextAreaOnTextEntering(keyString);
            _editor.TextArea.Document.Insert(_editor.TextArea.Caret.Offset, keyString);
        }

        [NotNull]
        private static string GetPrintableString(Key key)
        {
            // Ignore meta keys
            if (IsMetaKey(key))
                return "";

            // Ignore non-printable characters
            if (!IsPrintable(key))
                return "";

            var keyChar = KeyboardAPI.GetCharFromKey(key);

            return string.Format("{0}", keyChar);
        }

        private static bool IsMetaKey(Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LWin:
                case Key.RWin:
                case Key.System:
                    return true;
            }
            return false;
        }

        private static bool IsPrintable(Key key)
        {
            var keyChar = KeyboardAPI.GetCharFromKey(key);

            // Non-printable control character
            if (keyChar < ' ')
                return false;

            // Other non-printable character (e.g., Delete)
            if (keyChar == ' ' && key != Key.Space)
                return false;

            return true;
        }

        private void CompletionWindowOnDeactivated(object sender, EventArgs eventArgs)
        {
            // Workaround for bug in Avalon:
            // Show completion window, then focus completion window w/ mouse or scroll wheel.
            // Click back in the text editor area.  The completion window disappears, but doesn't "close".
            Close();
        }

        private void CompletionWindowOnClosing(object sender, CancelEventArgs e)
        {
            _isClosing = true;
        }

        private void CompletionWindowOnClosed(object sender, EventArgs eventArgs)
        {
            UnbindCompletionWindowEventHandlers();
            _completionWindow = null;
            _isClosing = false;
        }

        #endregion

        #region Show/close

        private void Show()
        {
            Close();

            // Open code completion after the user has pressed dot:
            _completionWindow = new CompletionWindow(_editor.TextArea)
                                {
                                    CloseWhenCaretAtBeginning = true,
                                    SizeToContent = SizeToContent.WidthAndHeight
                                };

            BindCompletionWindowEventHandlers();

            IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;
            data.AddRange(GenerateCompletionData());

            // TODO: This would be ideal, but Avalon appears to have a bug whereby the completion window is hidden (but not closed)
            // when we try to focus the text editor.
//            _completionWindow.GotKeyboardFocus += (sender, args) =>
//                                                  args.OldFocus.Focus();

            _completionWindow.Show();

            _completionWindow.MinWidth = _completionWindow.Width;

            BindWindowEventHandlers();
        }

        private void Close()
        {
            if (_isClosing)
                return;

            if (_completionWindow != null)
                _completionWindow.Close();
        }

        private void Close(object sender, EventArgs eventArgs)
        {
            Close();
        }

        #endregion

        #region WPF element accessors

        private void WithCompletionList(Action<CompletionList> action)
        {
            var list = _completionWindow.Content as CompletionList;
            if (list != null)
            {
                action(list);
            }
        }

        #endregion

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
                                   { "${title}",    "Name of the movie or TV show episode." },
                                   { "${year}",     "Year the movie was released." },
                                   { "${res}",      "Vertical resolution of the primary video track (e.g., 1080i, 720p, 480p)." },
                                   { "${channels}", "Number of audio channels (2.0, 5.1, 7.1, etc.)." },
                                   { "${vcodec}",   "Primary video track codec." },
                                   { "${acodec}",   "Primary audio track codec." },
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
