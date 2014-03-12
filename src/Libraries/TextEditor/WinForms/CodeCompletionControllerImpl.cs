using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace TextEditor.WinForms
{
    internal class CodeCompletionControllerImpl
    {
        private readonly ICSharpCode.TextEditor.TextEditorControl _editor;

        private CodeCompletionWindow _codeCompletionWindow;
        private ImageList _intellisenseImageList;

        public CodeCompletionControllerImpl(ICSharpCode.TextEditor.TextEditorControl editor)
        {
            _editor = editor;
            _editor.ActiveTextAreaControl.TextArea.KeyPress += TextAreaOnKeyPress;

            RegisterIntellisenseHandling();
        }

        public bool HandleTabKey()
        {
            if (_codeCompletionWindow == null)
                return false;

            SendKeys.Send("\n");

            return true;
        }

        private void RegisterIntellisenseHandling()
        {
            _intellisenseImageList = new ImageList(new Container());
            _intellisenseImageList.Images.Add(Properties.Resources.property_blue_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.terminal_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.tag_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.property_image_16);

            var textArea = _editor.ActiveTextAreaControl.TextArea;
            textArea.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                var isShortcut = e.Control && e.KeyCode == Keys.Space;
                if (!isShortcut)
                    return;

                e.SuppressKeyPress = true;
                ShowCodeCompletion((char)e.KeyValue);
            };

            _editor.ActiveTextAreaControl.VScrollBar.ValueChanged += ScrollBarOnValueChanged;
            _editor.ActiveTextAreaControl.HScrollBar.ValueChanged += ScrollBarOnValueChanged;
        }

        private void ScrollBarOnValueChanged(object sender, EventArgs eventArgs)
        {
            if (_codeCompletionWindow == null)
                return;

            _codeCompletionWindow.Close();
        }

        private void ShowCodeCompletion(char value)
        {
            if (_editor.IsReadOnly || !_editor.Enabled)
                return;

            if (_codeCompletionWindow != null)
                _codeCompletionWindow.Close();

            ICompletionDataProvider completionDataProvider = new CodeCompletionProviderImpl(_intellisenseImageList);

            _codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    _editor.FindForm(),     // The parent window for the completion window
                    _editor,                // The text editor to show the window for
                    "",                     // Filename - will be passed back to the provider
                    completionDataProvider, // Provider to get the list of possible completions
                    value                   // Key pressed - will be passed to the provider
                );

            // ShowCompletionWindow can return null when the provider returns an empty list
            if (_codeCompletionWindow == null)
                return;

            _codeCompletionWindow.Closed += CloseCodeCompletionWindow;

            var completions = completionDataProvider.GenerateCompletionData("", _editor.ActiveTextAreaControl.TextArea, value) ?? new ICompletionData[0];

            if (!completions.Any())
                return;

            _codeCompletionWindow.MouseWheel += CodeCompletionWindowOnMouseWheel;

            using (var g = _codeCompletionWindow.CreateGraphics())
            {
                var width = (int)completions.Select(data => g.MeasureString(data.Text, _codeCompletionWindow.Font).Width).Max();

                width += 16; // Icon size
                width += SystemInformation.VerticalScrollBarWidth;

                if (width > _codeCompletionWindow.Width)
                    _codeCompletionWindow.Width = width;
            }
        }

        private void CodeCompletionWindowOnMouseWheel(object sender, MouseEventArgs args)
        {
            _codeCompletionWindow.HandleMouseWheel(args);
        }

        private void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (_codeCompletionWindow != null)
            {
                _codeCompletionWindow.Closed -= CloseCodeCompletionWindow;
                _codeCompletionWindow.MouseWheel -= CodeCompletionWindowOnMouseWheel;
                _codeCompletionWindow.Dispose();
                _codeCompletionWindow = null;
            }
        }

        private void TextAreaOnKeyPress(object sender, KeyPressEventArgs args)
        {
            if (args.KeyChar == '$' || args.KeyChar == '%' || _codeCompletionWindow != null)
            {
                ShowCodeCompletionAsync(args.KeyChar);
            }
        }

        private void ShowCodeCompletionAsync(char keyPressed)
        {
            var timer = new System.Timers.Timer(100) { AutoReset = false };
            timer.Elapsed += (s, e) => _editor.Invoke(new Action(() => ShowCodeCompletion(keyPressed)));
            timer.Start();
        }
    }
}
