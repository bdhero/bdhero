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
using ICSharpCode.AvalonEdit.CodeCompletion;
using NativeAPI.Win.User;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Window = System.Windows.Window;

namespace TextEditor.WPF
{
    internal class CompletionControllerImpl
    {
        private readonly ICSharpCode.AvalonEdit.TextEditor _editor;

        private readonly CompletionProviderImpl _completionProvider;

        private CompletionWindow _completionWindow;
        
        private bool _isWindowMoveEventBound;
        private bool _isClosing;

        public CompletionControllerImpl(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _editor = editor;

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

                case Key.Left:
                case Key.Right:
                    ProxyKey(e);
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

        /// <seealso cref="http://stackoverflow.com/a/1646568/467582"/>
        private void ProxyKey(KeyEventArgs e)
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

            var data = _completionWindow.CompletionList.CompletionData;
            data.AddRange(_completionProvider.GenerateCompletionData());

            // TODO: This would be ideal, but Avalon appears to have a bug whereby the completion window is hidden (but not closed)
            // when we try to focus the text editor.
//            _completionWindow.GotKeyboardFocus += (sender, args) =>
//                                                  args.OldFocus.Focus();

            // http://stackoverflow.com/a/839806/467582
            ElementHost.EnableModelessKeyboardInterop(_completionWindow);

//            var hook = new WpfWndProcHook(_completionWindow);
//            hook.WndProcMessage += HookOnWndProcMessage;

            _completionWindow.Show();

            _completionWindow.MinWidth = _completionWindow.Width;

            BindWindowEventHandlers();
        }

        /// <seealso cref="http://stackoverflow.com/a/836713/467582"/>
        private void HookOnWndProcMessage(ref Message m, HandledEventArgs args)
        {
            WindowMessage message = m;

//            Console.WriteLine("CompletionWindow.WndProc  :  {0,-25} - {1}", message.Type, m);

            if (message.Is(WindowMessageType.WM_GETDLGCODE))
            {
                switch (message.WParamInt64Value)
                {
                    case VK_TAB:
                    case VK_RETURN:
                    case VK_LEFT:
                    case VK_UP:
                    case VK_RIGHT:
                    case VK_DOWN:
                        args.Handled = true;
                        m.Result = new IntPtr(DLGC_WANTMESSAGE);
                        break;
                }
            }
        }

        #region Win32 window message constants

        /// <summary>
        ///     Button.
        /// </summary>
        private const int DLGC_BUTTON = 0x2000;

        /// <summary>
        ///     Default push button.
        /// </summary>
        private const int DLGC_DEFPUSHBUTTON = 0x0010;

        /// <summary>
        ///     <c>EM_SETSEL</c> messages.
        /// </summary>
        private const int DLGC_HASSETSEL = 0x0008;

        /// <summary>
        ///     Radio button.
        /// </summary>
        private const int DLGC_RADIOBUTTON = 0x0040;

        /// <summary>
        ///     Static control.
        /// </summary>
        private const int DLGC_STATIC = 0x0100;

        /// <summary>
        ///     Non-default push button.
        /// </summary>
        private const int DLGC_UNDEFPUSHBUTTON = 0x0020;

        /// <summary>
        ///     All keyboard input.
        /// </summary>
        private const int DLGC_WANTALLKEYS = 0x0004;

        /// <summary>
        ///     Direction keys.
        /// </summary>
        private const int DLGC_WANTARROWS = 0x0001;

        /// <summary>
        ///     <see cref="WindowMessageType.WM_CHAR"/> messages.
        /// </summary>
        private const int DLGC_WANTCHARS = 0x0080;

        /// <summary>
        ///     All keyboard input (the application passes this message in the MSG structure to the control).
        /// </summary>
        private const int DLGC_WANTMESSAGE = 0x0004;

        /// <summary>
        ///     <kbd>TAB</kbd> key.
        /// </summary>
        private const int DLGC_WANTTAB = 0x0002;

        private const int VK_TAB = 0x09;
        private const int VK_RETURN = 0x0D;
        private const int VK_LEFT = 0x25;
        private const int VK_UP = 0x26;
        private const int VK_RIGHT = 0x27;
        private const int VK_DOWN = 0x28;

        #endregion

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
