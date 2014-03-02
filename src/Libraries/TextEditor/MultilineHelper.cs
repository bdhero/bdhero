using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TextEditor
{
    internal class MultilineHelper
    {
        private static readonly Regex NewlineRegex = new Regex(@"[\n\r\f]+");

        public static string StripNewlines(string text)
        {
            return NewlineRegex.Replace(text, "");
        }

        private readonly ITextEditor _editor;
        private readonly Action _notifyTextSanitized;

        private bool _ignoreTextChanged;

        public MultilineHelper(ITextEditor editor, Action notifyTextSanitized)
        {
            _editor = editor;
            _notifyTextSanitized = notifyTextSanitized;
        }

        public void TextChanged()
        {
            if (_ignoreTextChanged)
                return;

            if (!_editor.Multiline)
            {
                var sanitized = StripNewlines(_editor.Text);
                if (sanitized != _editor.Text)
                {
                    _ignoreTextChanged = true;

                    var timer = new System.Timers.Timer(10) { AutoReset = false };
                    timer.Elapsed += (sender, args) => TimerOnElapsed(sanitized);
                    timer.Start();

                    return;
                }
            }

            _notifyTextSanitized();
        }

        private void TimerOnElapsed(string sanitized)
        {
            _editor.Control.Invoke(new Action(() => SetSanitizedText(sanitized)));
        }

        private void SetSanitizedText(string sanitized)
        {
            if (_editor.CanUndo)
            {
                if (string.IsNullOrEmpty(sanitized))
                {
                    _editor.Text = "";
                    return;
                }

                var clipboardText = Clipboard.GetText();

                Clipboard.SetText(sanitized);

                _editor.Undo();
                _editor.SelectAll();
                _editor.Paste();

                Clipboard.SetText(clipboardText);
            }
            else
            {
                _editor.Text = sanitized;
            }

            _notifyTextSanitized();

            _ignoreTextChanged = false;
        }
    }
}
