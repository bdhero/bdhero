using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace TextEditor.WinForms
{
    internal class CompletionControllerImpl
    {
        private readonly ICSharpCode.TextEditor.TextEditorControl _editor;

        private readonly ImageList _intellisenseImageList;

        private CodeCompletionWindow _codeCompletionWindow;

        public CompletionControllerImpl(ICSharpCode.TextEditor.TextEditorControl editor)
        {
            _editor = editor;

            _intellisenseImageList = new ImageList(new Container());
            _intellisenseImageList.Images.Add(Properties.Resources.property_blue_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.terminal_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.tag_image_16);
            _intellisenseImageList.Images.Add(Properties.Resources.property_image_16);

            _editor.ActiveTextAreaControl.TextArea.KeyDown += TextAreaOnKeyDown;
            _editor.ActiveTextAreaControl.TextArea.KeyPress += TextAreaOnKeyPress;
            _editor.ActiveTextAreaControl.VScrollBar.ValueChanged += ScrollBarOnValueChanged;
            _editor.ActiveTextAreaControl.HScrollBar.ValueChanged += ScrollBarOnValueChanged;
        }

        #region Public API

        public bool HandleTabKey()
        {
            if (_codeCompletionWindow == null)
                return false;

            SendKeys.Send("\n");

            return true;
        }

        #endregion

        #region Events

        private void TextAreaOnKeyDown(object sender, KeyEventArgs e)
        {
            var isShortcut = e.Control && e.KeyCode == Keys.Space;
            if (!isShortcut)
                return;

            e.SuppressKeyPress = true;
            ShowSync((char)e.KeyValue);
        }

        private void TextAreaOnKeyPress(object sender, KeyPressEventArgs args)
        {
            if (args.KeyChar == '$' || args.KeyChar == '%' || _codeCompletionWindow != null)
            {
                ShowAsync(args.KeyChar);
            }
        }

        private void CodeCompletionWindowOnMouseWheel(object sender, MouseEventArgs args)
        {
            _codeCompletionWindow.HandleMouseWheel(args);
        }

        private void ScrollBarOnValueChanged(object sender, EventArgs eventArgs)
        {
            if (_codeCompletionWindow == null)
                return;

            _codeCompletionWindow.Close();
        }

        private void OnClose(object sender, EventArgs e)
        {
            if (_codeCompletionWindow != null)
            {
                _codeCompletionWindow.Closed -= OnClose;
                _codeCompletionWindow.MouseWheel -= CodeCompletionWindowOnMouseWheel;
                _codeCompletionWindow.Dispose();
                _codeCompletionWindow = null;
            }
        }

        #endregion

        #region Show

        private void ShowAsync(char keyPressed)
        {
            var timer = new System.Timers.Timer(100) { AutoReset = false };
            timer.Elapsed += (s, e) => _editor.Invoke(new Action(() => ShowSync(keyPressed)));
            timer.Start();
        }

        private void ShowSync(char value)
        {
            if (_editor.IsReadOnly || !_editor.Enabled)
                return;

            if (_codeCompletionWindow != null)
                _codeCompletionWindow.Close();

            ICompletionDataProvider completionDataProvider = new CompletionProviderImpl(_intellisenseImageList);

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

            _codeCompletionWindow.Closed += OnClose;

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

        #endregion
    }
}
