#if !__MonoCS__
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.Forms;
using ICSharpCode.AvalonEdit.CodeCompletion;
using NativeAPI.Win.User;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Window = System.Windows.Window;

namespace TextEditor.WPF
{
    internal class CompletionControllerImpl
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ICSharpCode.AvalonEdit.TextEditor _editor;

        private readonly CompletionProviderImpl _completionProvider;

        private CompletionWindow _completionWindow;
        
        private bool _isWindowMoveEventBound;
        private bool _isClosing;

        public CompletionControllerImpl(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _editor = editor;

            _editor.PreviewKeyDown += (s, e) =>
                    Console.WriteLine("Editor.PreviewKeyDown({0})", e.Key);

            _editor.TextArea.PreviewKeyDown += (s, e) =>
                    Console.WriteLine("Editor.TextArea.PreviewKeyDown({0})", e.Key);


            _editor.PreviewKeyDown += EditorOnPreviewKeyDown;
            _editor.TextArea.TextEntering += TextAreaOnTextEntering;
            _editor.TextArea.TextEntered += TextAreaOnTextEntered;
            _editor.TextArea.MouseWheel += TextAreaOnMouseWheel;

            _completionProvider = new CompletionProviderImpl(_editor);
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

            var parentWindow = Window.GetWindow(_editor);
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
            _completionWindow.SizeChanged += CompletionWindowOnSizeChanged;
            _completionWindow.KeyDown     += CompletionWindowOnKeyDown;
            _completionWindow.Deactivated += CompletionWindowOnDeactivated;
            _completionWindow.Closing     += CompletionWindowOnClosing;
            _completionWindow.Closed      += CompletionWindowOnClosed;
        }

        private void UnbindCompletionWindowEventHandlers()
        {
            _completionWindow.Loaded      -= CompletionWindowOnLoaded;
            _completionWindow.SizeChanged -= CompletionWindowOnSizeChanged;
            _completionWindow.KeyDown     -= CompletionWindowOnKeyDown;
            _completionWindow.Deactivated -= CompletionWindowOnDeactivated;
            _completionWindow.Closing     -= CompletionWindowOnClosing;
            _completionWindow.Closed      -= CompletionWindowOnClosed;
        }

        private void CompletionWindowOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AutoSize();
        }

        private void AutoSize()
        {
            AutoSizeToContent();
        }

        private void AutoSizeToContent()
        {
            // Disable max width/height, which will cause the window to resize itself to fit its contents.
            _completionWindow.SizeToContent = SizeToContent.WidthAndHeight;
            _completionWindow.MaxWidth = double.PositiveInfinity;
            _completionWindow.MaxHeight = double.PositiveInfinity;
        }

        private void SetManualSize(double width, double height)
        {
            _completionWindow.SizeToContent = SizeToContent.Manual;
            _completionWindow.Width = _completionWindow.MinWidth = _completionWindow.MaxWidth = width;
            _completionWindow.Height = _completionWindow.MinHeight = _completionWindow.MaxHeight = height;
        }

        private void CompletionWindowOnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            Logger.DebugFormat("CompletionWindow.SizeChanged({0} => {1})", args.PreviousSize, args.NewSize);

            // Ignore initial resize.
            if (args.PreviousSize.Equals(new Size(0, 0)))
                return;

            // Window is narrowing (or staying the same width), which means it doesn't need to be made wider to fit its contents.
            // Since we won't be resizing the window any further, we can safely auto-select the first completion item.
            // If we always auto-selected the first completion item every time a resize event occurred, the tooltip
            // would be incorrectly positioned because Avalon doesn't reposition it when the window size changes.
            if (args.NewSize.Width <= args.PreviousSize.Width)
            {
                _completionWindow.MaxHeight = args.PreviousSize.Height;
                SelectFirstItem();
                return;
            }

            // We've already found the "ideal" width and height of the window, and are currently in the middle of
            // resizing the window's height DOWN to a reasonable height so that it doesn't overflow off the screen.
            if (args.NewSize.Height <= args.PreviousSize.Height)
                return;

            var fullWidth = args.NewSize.Width + SystemParameters.VerticalScrollBarWidth;
            var oldHeight = args.PreviousSize.Height;

            SetManualSize(fullWidth, oldHeight);

            // Now that we've set the final width and height of the window, we can auto-select the first completion item.
            // If we always auto-selected the first completion item every time a resize event occurred, the tooltip
            // would be incorrectly positioned because Avalon doesn't reposition it when the window size changes.
            SelectFirstItem();
        }

        private void SelectFirstItem()
        {
            WithCompletionList(list => list.SelectedItem = list.CompletionData.FirstOrDefault());
        }

        #region Keyboard event handling

        private void CompletionWindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_completionWindow.IsKeyboardFocusWithin)
                return;

            switch (e.Key)
            {
                // Avalon already handles these
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                case Key.Down:
                case Key.Up:
                case Key.Tab:
                case Key.Enter:
                case Key.Escape:
                    return;
            }

            ProxyDialogKey(e);
            ProxyInputKey(e);
        }

        /// <seealso cref="http://stackoverflow.com/a/1646568/467582"/>
        private void ProxyDialogKey(KeyEventArgs e)
        {
            var key = e.Key;
            var target = _editor.TextArea;

            var keyboardDevice = Keyboard.PrimaryDevice;
            var inputSource = PresentationSource.FromVisual(target);
            var routedEvent = Keyboard.KeyDownEvent;

            if (inputSource == null)
                return;

            var args = new KeyEventArgs(keyboardDevice, inputSource, 0, key) { RoutedEvent = routedEvent };

            target.RaiseEvent(args);
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

        #endregion

        private void CompletionWindowOnDeactivated(object sender, EventArgs eventArgs)
        {
            // Workaround for bug in Avalon:
            // Show completion window, then focus completion window w/ mouse or scroll wheel.
            // Click back in the text editor area.  The completion window disappears, but doesn't "close".
//            Close();
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
                                    CloseAutomatically = true,
                                    CloseWhenCaretAtBeginning = true,
//                                    SizeToContent = SizeToContent.WidthAndHeight,
                                    ResizeMode = ResizeMode.NoResize
                                };


            // <---


            // TODO:
            _completionWindow.CompletionList.InsertionRequested += CompletionListOnInsertionRequested;
            _completionWindow.CompletionList.IsFiltering = true;
//            _completionWindow.CompletionList.;

            _completionWindow.PreviewKeyDown += CompletionWindowOnPreviewKeyDown;


            // --->


            BindCompletionWindowEventHandlers();

            var data = _completionWindow.CompletionList.CompletionData;
            data.AddRange(_completionProvider.GenerateCompletionData());


            // <---


            // TODO: This would be ideal, but Avalon appears to have a bug whereby the completion window is hidden (but not closed)
            // when we try to focus the text editor.
//            _completionWindow.GotKeyboardFocus += (sender, args) =>
//                                                  _editor.TextArea.Focus();

            _completionWindow.Focusable = false;


            // --->


            // http://stackoverflow.com/a/839806/467582
            ElementHost.EnableModelessKeyboardInterop(_completionWindow);

            BindWindowEventHandlers();

            _completionWindow.Show();



            // <---


            var hook = new WpfWndProcHook(_completionWindow);
            hook.WndProcMessage += HookOnWndProcMessage;


            // --->
        }

        private void HookOnWndProcMessage(ref Message m, HandledEventArgs args)
        {
//            Console.WriteLine("CompletionWindow.WndProc  -  {0}", m);

            WindowMessage msg = m;

            if (msg.Is(WindowMessageType.WM_GETDLGCODE))
            {
                switch (msg.WParamInt64Value)
                {
                    case VirtualKey.VK_TAB:
                    case VirtualKey.VK_RETURN:
                    case VirtualKey.VK_LEFT:
                    case VirtualKey.VK_UP:
                    case VirtualKey.VK_RIGHT:
                    case VirtualKey.VK_DOWN:
                    {
                        args.Handled = true;
                        m.Result = new IntPtr(DialogCode.DLGC_WANTMESSAGE);
                        return;
                    }
                }
            }

            if (msg.Is(WindowMessageType.WM_CAPTURECHANGED) ||
                msg.Is(WindowMessageType.WM_MOUSEWHEEL))
            {
                // If nothing is focused, it means the user clicked on the scrollbar or scrolled with the mouse wheel,
                // which will prevent keystrokes from being seen by Avalon.  To work around this, we need to immediately focus something.
                var focused = Keyboard.FocusedElement ?? FocusManager.GetFocusedElement(_completionWindow);
                if (focused == null)
                {
                    WithCompletionList(list => list.Focus());
                }
            }
        }

        private void CompletionWindowOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("CompletionWindow.PreviewKeyDown({0})", e.Key);
        }

        private void CompletionListOnInsertionRequested(object sender, EventArgs eventArgs)
        {
            var inputEvent = eventArgs as InputEventArgs;
            if (inputEvent != null)
            {
                inputEvent.Handled = true;
            }
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
    }
}
#endif
