#if !__MonoCS__
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using DotNetUtils;
using DotNetUtils.Extensions;
using DotNetUtils.Forms;
using ICSharpCode.AvalonEdit.CodeCompletion;
using NativeAPI.Win.User;
using Message = System.Windows.Forms.Message;

namespace TextEditor.WPF
{
    internal interface ICompletionController
    {
        bool IgnoreTabOrEnterKey { get; }
    }

    internal class MockCompletionController : ICompletionController
    {
        public MockCompletionController(ICSharpCode.AvalonEdit.TextEditor editor)
        {
        }

        public bool IgnoreTabOrEnterKey { get; private set; }
    }

    internal class CompletionControllerImpl : ICompletionController
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
            _editor.TextArea.MouseWheel += TextAreaOnMouseWheel;

            _completionProvider = new CompletionProviderImpl(_editor);
        }

        public bool IgnoreTabOrEnterKey
        {
            get { return _completionWindow != null; }
        }

        private bool HasCompletions
        {
            get { return WithCompletionList(list => list.ListBox.HasItems); }
        }

        #region Event Handlers - Text Editor

        private void EditorOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var control = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control);
            if (control && e.Key == Key.Space)
            {
                e.Handled = true;
                Show();
            }
        }

        private void TextAreaOnTextEntering(object sender, TextCompositionEventArgs e)
        {
            TextAreaOnTextEntering(e.Text, (EventArgs)e);

            if (e.Text == "$" || e.Text == "%" && _completionWindow == null)
            {
                Show();
            }
        }

        private void TextAreaOnTextEntering(string text, EventArgs e = null)
        {
            if (text.Length > 0 && _completionWindow != null)
            {
                if (!char.IsLetterOrDigit(text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
//                    _completionWindow.CompletionList.RequestInsertion(e ?? EventArgs.Empty);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void TextAreaOnMouseWheel(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            // Workaround for WPF/WinForms interop issue:
            // Show completion window, then focus completion window w/ mouse or scroll wheel.
            // Click back in the text editor area.  The completion window disappears, but doesn't "close".
//            Close();
        }

        #endregion

        #region Event Handlers - Parent Window

        private void BindParentWindowEventHandlers()
        {
            if (_isWindowMoveEventBound)
                return;

            var form = _editor.FindForm();
            if (form != null)
            {
                form.LocationChanged += FormOnLocationChanged;
            }

            _isWindowMoveEventBound = true;
        }

        private void FormOnLocationChanged(object sender, EventArgs eventArgs)
        {
            if (_completionWindow == null)
                return;

            RePositionCompletionWindow();
            RePositionToolTip();
        }

        private void RePositionCompletionWindow()
        {
            _completionWindow.InvokeMethod("UpdatePosition");
        }

        #endregion

        #region Event Handlers - Completion Window

        private void BindCompletionWindowEventHandlers()
        {
            _completionWindow.SizeChanged += CompletionWindowOnSizeChanged;
            _completionWindow.KeyDown     += CompletionWindowOnKeyDown;
            _completionWindow.Closing     += CompletionWindowOnClosing;
            _completionWindow.Closed      += CompletionWindowOnClosed;
        }

        private void UnbindCompletionWindowEventHandlers()
        {
            _completionWindow.SizeChanged -= CompletionWindowOnSizeChanged;
            _completionWindow.KeyDown     -= CompletionWindowOnKeyDown;
            _completionWindow.Closing     -= CompletionWindowOnClosing;
            _completionWindow.Closed      -= CompletionWindowOnClosed;
        }

        private const int RightPadding = 5;

        private bool _ignoreSizeChange;
        private int _maxHeight;
        private SizeToContent _sizeToContent;

        private void CompletionWindowOnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            var oldSize = new System.Drawing.Size((int) args.PreviousSize.Width, (int) args.PreviousSize.Height);
            var newSize = new System.Drawing.Size((int) args.NewSize.Width, (int) args.NewSize.Height);

            Logger.DebugFormat("CompletionWindow.SizeChanged({0} => {1})", oldSize, newSize);

            if (oldSize.Equals(newSize))
            {
                Logger.DebugFormat("    ignoring - size hasn't changed");

                _ignoreSizeChange = false;

                return;
            }

            if (_ignoreSizeChange)
            {
                Logger.DebugFormat("    ignoring - flag set");

                _ignoreSizeChange = false;

                // Now that we've set the final width and height of the window, we can auto-select the first completion item.
                // If we always auto-selected the first completion item every time a resize event occurred, the tooltip
                // would be incorrectly positioned because Avalon doesn't reposition it when the window size changes.
                SelectFirstItem();
                _completionWindow.SetTimeout(window => RePositionToolTip(), 10);

                return;
            }

            _ignoreSizeChange = true;
            _maxHeight = (int) _completionWindow.MaxHeight;
            _sizeToContent = _completionWindow.SizeToContent;

            _completionWindow.MaxWidth = double.PositiveInfinity;
            _completionWindow.MaxHeight = double.PositiveInfinity;
            _completionWindow.SizeToContent = SizeToContent.WidthAndHeight;

            var fullWidth = _completionWindow.Width;
            var fullHeight = _completionWindow.Height;

            Logger.DebugFormat("    full size = {0}, {1}", fullWidth, fullHeight);

            _completionWindow.MaxHeight = _maxHeight;
            _completionWindow.SizeToContent = _sizeToContent;

            if (HasCompletions)
            {
                var newWidth = fullWidth;

                if (fullHeight > _maxHeight)
                    newWidth += SystemParameters.VerticalScrollBarWidth + RightPadding;

                _completionWindow.Width = newWidth;

                RePositionToolTip();
            }
            else
            {
                _completionWindow.Width = oldSize.Width;

                HideToolTip();
            }
        }

        #region Tool Tip

        private void ShowToolTip(bool show = true)
        {
            WithToolTip(toolTip => toolTip.IsOpen = show);

            if (show && HasCompletions)
                RePositionToolTip();
        }

        private void HideToolTip()
        {
            ShowToolTip(false);
        }

        private void RePositionToolTip()
        {
            WithToolTip(RePositionToolTip);
        }

        private static void RePositionToolTip(ToolTip toolTip)
        {
            toolTip.VerticalOffset++;
            toolTip.VerticalOffset--;
        }

        private void WithToolTip(Action<ToolTip> action)
        {
            _completionWindow.WithField("toolTip", action);
        }

        #endregion

        #region Item selection

        private void SelectFirstItem()
        {
            WithCompletionList(list => list.SelectedItem = list.CompletionData.FirstOrDefault());
            RePositionToolTip();
        }

        #endregion

        #region Keyboard event handling

        private void CompletionWindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_completionWindow.IsKeyboardFocusWithin)
                return;

            if (KeyboardHelper.IsHandledByAvalon(e.Key))
                return;

            ProxyDialogKey(e);
            ProxyInputKey(e);
        }

        private void ProxyDialogKey(KeyEventArgs e)
        {
            KeyboardHelper.RaiseKeyEvent(_editor.TextArea, e);
        }

        private void ProxyInputKey(KeyEventArgs e)
        {
            var keyString = KeyboardHelper.GetPrintableString(e.Key);
            if (keyString == "")
                return;

            TextAreaOnTextEntering(keyString);
            _editor.TextArea.Document.Insert(_editor.TextArea.Caret.Offset, keyString);
        }


        #endregion

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

            if (_editor.IsReadOnly)
                return;

            // Open code completion after the user has pressed dot:
            _completionWindow = new CompletionWindow(_editor.TextArea)
                                {
                                    CloseAutomatically = true,
                                    CloseWhenCaretAtBeginning = true,
                                    ResizeMode = ResizeMode.NoResize,
                                    SizeToContent = SizeToContent.Height,
                                    Topmost = true
                                };

            // TODO:
            _completionWindow.CompletionList.InsertionRequested += CompletionListOnInsertionRequested;
            _completionWindow.CompletionList.IsFiltering = true;

            BindCompletionWindowEventHandlers();

            _completionWindow.CompletionList.CompletionData.AddRange(_completionProvider.GenerateCompletionData());

            // TODO: This would be ideal, but Avalon appears to have a bug whereby the completion window is hidden (but not closed)
            // when we try to focus the text editor.
//            _completionWindow.GotKeyboardFocus += (sender, args) =>
//                                                  _editor.TextArea.Focus();

            _completionWindow.EnableWinFormsInterop();

            BindParentWindowEventHandlers();

            _completionWindow.Show();

            WpfWndProcHook.Hook(_completionWindow, CompletionWindowOnWndProcMessage);

            // TODO: Smarter filtering
            var textArea = _completionWindow.TextArea;
            var doc = textArea.Document;
            var line = doc.GetLineByNumber(textArea.Caret.Line);
            var text = doc.GetText(line).Substring(0, textArea.Caret.Column - 1);
            _completionWindow.CompletionList.SelectItem(text);
        }

        private void CompletionWindowOnWndProcMessage(ref Message m, HandledEventArgs args)
        {
            WindowMessage msg = m;

            // Ensure that dialog keys get sent to the window
            if (msg.Is(WindowMessageType.WM_GETDLGCODE))
            {
                switch (msg.WParamInt64Value)
                {
                    case VirtualKey.VK_TAB:
                    case VirtualKey.VK_RETURN:
                    case VirtualKey.VK_ESCAPE:
                    case VirtualKey.VK_SPACE:
                    case VirtualKey.VK_LEFT:
                    case VirtualKey.VK_UP:
                    case VirtualKey.VK_RIGHT:
                    case VirtualKey.VK_DOWN:
                    {
                        // Tell Windows that we want to receive WM_KEYDOWN messages for dialog keys when the scrollbar is focused.
                        // ElementHost.EnableModelessKeyboardInterop() already handles this when all other elements are focused,
                        // but for some reason if the user clicks on the scrollbar or scrolls with the mouse wheel
                        // WM_KEYDOWN messages don't get sent.
                        args.Handled = true;
                        m.Result = new IntPtr(DialogCode.DLGC_WANTMESSAGE);
                        return;
                    }
                }
            }

            // Ensure that focus is always on something that is listening for keyboard input
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
            if (_completionWindow == null || _completionWindow.CompletionList == null)
                return;

            action(_completionWindow.CompletionList);
        }

        private T WithCompletionList<T>(Func<CompletionList, T> action)
        {
            if (_completionWindow == null || _completionWindow.CompletionList == null)
                return default(T);

            return action(_completionWindow.CompletionList);
        }

        #endregion
    }
}
#endif
