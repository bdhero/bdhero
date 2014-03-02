using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;

namespace TextEditor
{
    internal class NewlineStripper
    {
        private static readonly Regex NewlineRegex = new Regex(@"[\n\r\f]+");

        private readonly Control _control;
        private readonly Func<bool> _getMultiline;
        private readonly Func<string> _getText;
        private readonly Action<string> _setText;
        private readonly Action _forceRepaint;

        public bool IgnoreTextChanged { get; private set; }

        public NewlineStripper(Control control, Func<bool> getMultiline, Func<string> getText, Action<string> setText, Action forceRepaint)
        {
            _control = control;
            _getMultiline = getMultiline;
            _getText = getText;
            _setText = setText;
            _forceRepaint = forceRepaint;
        }

        private bool Multiline
        {
            get { return _getMultiline(); }
        }

        private string Text
        {
            get { return _getText(); }
            set
            {
                var newValue = SanitizeText(value);
                if (newValue == Text)
                    return;

                _setText(newValue);
            }
        }

        public void SanitizeTextAsync()
        {
            IgnoreTextChanged = true;

            var timer = new System.Timers.Timer(10) { AutoReset = true };
            timer.Elapsed += delegate(object sender, ElapsedEventArgs args)
                             {
                                 if (_control.IsHandleCreated)
                                 {
                                     timer.AutoReset = false;
                                     SanitizeTextOnBackgroundThread();
                                 }
                             };
            timer.Start();
        }

        private void SanitizeTextOnBackgroundThread()
        {
            _control.Invoke(new Action(SanitizeTextOnUIThread));
        }

        private void SanitizeTextOnUIThread()
        {
            Text = Text;
            ForceRepaint();
            IgnoreTextChanged = false;
        }

        private void ForceRepaint()
        {
            _forceRepaint();
//            _control.PerformLayout();
        }

        public string SanitizeText(string text)
        {
            return Multiline ? text : NewlineRegex.Replace(text, "");
        }
    }
}
