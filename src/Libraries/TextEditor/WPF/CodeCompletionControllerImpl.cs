using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
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

        public CodeCompletionControllerImpl(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _editor = editor;

            _editor.PreviewKeyDown += OnPreviewKeyDown;
            _editor.TextArea.TextEntering += OnTextEntering;
            _editor.TextArea.TextEntered += OnTextEntered;

            _editor.TextArea.MouseWheel += TextAreaOnMouseWheel;
        }

        public bool IgnoreTabOrEnterKey
        {
            get { return _completionWindow != null; }
        }

        #region Event handlers

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true;
                ShowSync();
            }
        }

        private void OnTextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "$" || e.Text == "%" || _completionWindow != null)
            {
                ShowSync();
            }
        }

        private void OnTextEntering(object sender, TextCompositionEventArgs e)
        {
            OnTextEntering(e.Text, (EventArgs) e);
        }

        private void OnTextEntering(string text, EventArgs e = null)
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

        private void TextAreaOnMouseWheel(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            // Workaround for bug in Avalon:
            // Show completion window, then focus completion window w/ mouse or scroll wheel.
            // Click back in the text editor area.  The completion window disappears, but doesn't "close".
            Close();
        }

        #endregion

        #region Show/close

        private void ShowSync()
        {
            Close();

            // Open code completion after the user has pressed dot:
            _completionWindow = new CompletionWindow(_editor.TextArea);
            _completionWindow.CloseWhenCaretAtBeginning = true;
            _completionWindow.SizeToContent = SizeToContent.WidthAndHeight;

            IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;
            data.AddRange(GenerateCompletionData());

//            _completionWindow.GotKeyboardFocus += (sender, args) =>
//                                                  args.OldFocus.Focus();

            BindCompletionWindowEventHandlers();

            _closing = false;

            _completionWindow.Show();
        }

        private void Close()
        {
            if (_closing)
                return;

            if (_completionWindow != null)
                _completionWindow.Close();
        }

        #endregion

        #region Event handlers

        private void BindCompletionWindowEventHandlers()
        {
            _completionWindow.Loaded      += CompletionWindowOnLoaded;
            _completionWindow.KeyUp       += CompletionWindowOnKeyUp;
            _completionWindow.Deactivated += CompletionWindowOnDeactivated;
            _completionWindow.Closing     += CompletionWindowOnClosing;
            _completionWindow.Closed      += CompletionWindowOnClosed;
        }

        private void UnbindCompletionWindowEventHandlers()
        {
            _completionWindow.Loaded      -= CompletionWindowOnLoaded;
            _completionWindow.KeyUp       -= CompletionWindowOnKeyUp;
            _completionWindow.Deactivated -= CompletionWindowOnDeactivated;
            _completionWindow.Closing     -= CompletionWindowOnClosing;
            _completionWindow.Closed      -= CompletionWindowOnClosed;
        }

        private void CompletionWindowOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            WithCompletionList(list => list.SelectedItem = list.CompletionData.FirstOrDefault());
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
                case Key.Home:
                case Key.End:
                case Key.Tab:
                case Key.Enter:
                    WithCompletionList(list => list.HandleKey(e));
                    break;
                case Key.Escape:
                    Close();
                    break;

                // Avalon already handles these, even when the completion window is focused
                case Key.PageUp:
                case Key.PageDown:
                    break;

                // All other keys get passed on 
                default:
                    var keyChar = KeyboardAPI.GetCharFromKey(e.Key);
                    var keyString = string.Format("{0}", keyChar);

                    OnTextEntering(keyString);

                    if (keyString.Any())
                    {
                        _editor.TextArea.Document.Insert(_editor.TextArea.Caret.Offset, keyString);
                    }

                    break;
            }
        }

        private void CompletionWindowOnDeactivated(object sender, EventArgs eventArgs)
        {
            // Workaround for bug in Avalon:
            // Show completion window, then focus completion window w/ mouse or scroll wheel.
            // Click back in the text editor area.  The completion window disappears, but doesn't "close".
            Close();
        }

        private bool _closing;

        private void CompletionWindowOnClosing(object sender, CancelEventArgs e)
        {
            _closing = true;
        }

        private void CompletionWindowOnClosed(object sender, EventArgs eventArgs)
        {
            UnbindCompletionWindowEventHandlers();
            _completionWindow = null;
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

        private void WithCompletionListBox(Action<CompletionListBox> action)
        {
            var list = _completionWindow.Content as CompletionList;
            if (list != null)
            {
                var box = list.ListBox;
                if (box != null)
                {
                    action(box);
                }
            }
        }

        #endregion

        #region Event bindings (TESTING/DEBUGGING ONLY)

        private static int _evtIdx;

        private void BindToAllEvents(CompletionWindow completionWindow)
        {
            var name = completionWindow.GetType().Name;
            completionWindow.Activated += (sender, args) => Console.WriteLine("{0} - {1}.Activated()", _evtIdx++, name);
            completionWindow.Closed += (sender, args) => Console.WriteLine("{0} - {1}.Closed()", _evtIdx++, name);
            completionWindow.Closing += (sender, args) => Console.WriteLine("{0} - {1}.Closing()", _evtIdx++, name);
            completionWindow.ContentRendered += (sender, args) => Console.WriteLine("{0} - {1}.ContentRendered()", _evtIdx++, name);
            completionWindow.ContextMenuClosing += (sender, args) => Console.WriteLine("{0} - {1}.ContextMenuClosing()", _evtIdx++, name);
            completionWindow.ContextMenuOpening += (sender, args) => Console.WriteLine("{0} - {1}.ContextMenuOpening()", _evtIdx++, name);
            completionWindow.DataContextChanged += (sender, args) => Console.WriteLine("{0} - {1}.DataContextChanged()", _evtIdx++, name);
            completionWindow.Deactivated += (sender, args) => Console.WriteLine("{0} - {1}.Deactivated()", _evtIdx++, name);
            completionWindow.DragEnter += (sender, args) => Console.WriteLine("{0} - {1}.DragEnter()", _evtIdx++, name);
            completionWindow.DragLeave += (sender, args) => Console.WriteLine("{0} - {1}.DragLeave()", _evtIdx++, name);
            completionWindow.DragOver += (sender, args) => Console.WriteLine("{0} - {1}.DragOver()", _evtIdx++, name);
            completionWindow.Drop += (sender, args) => Console.WriteLine("{0} - {1}.Drop()", _evtIdx++, name);
            completionWindow.FocusableChanged += (sender, args) => Console.WriteLine("{0} - {1}.FocusableChanged()", _evtIdx++, name);
            completionWindow.GiveFeedback += (sender, args) => Console.WriteLine("{0} - {1}.GiveFeedback()", _evtIdx++, name);
            completionWindow.GotFocus += (sender, args) => Console.WriteLine("{0} - {1}.GotFocus()", _evtIdx++, name);
            completionWindow.GotKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.GotKeyboardFocus()", _evtIdx++, name);
            completionWindow.GotMouseCapture += (sender, args) => Console.WriteLine("{0} - {1}.GotMouseCapture()", _evtIdx++, name);
            completionWindow.GotStylusCapture += (sender, args) => Console.WriteLine("{0} - {1}.GotStylusCapture()", _evtIdx++, name);
            completionWindow.GotTouchCapture += (sender, args) => Console.WriteLine("{0} - {1}.GotTouchCapture()", _evtIdx++, name);
            completionWindow.Initialized += (sender, args) => Console.WriteLine("{0} - {1}.Initialized()", _evtIdx++, name);
            completionWindow.IsEnabledChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsEnabledChanged()", _evtIdx++, name);
            completionWindow.IsHitTestVisibleChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsHitTestVisibleChanged()", _evtIdx++, name);
            completionWindow.IsKeyboardFocusWithinChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsKeyboardFocusWithinChanged()", _evtIdx++, name);
            completionWindow.IsKeyboardFocusedChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsKeyboardFocusedChanged()", _evtIdx++, name);
            completionWindow.IsMouseCaptureWithinChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsMouseCaptureWithinChanged()", _evtIdx++, name);
            completionWindow.IsMouseCapturedChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsMouseCapturedChanged()", _evtIdx++, name);
            completionWindow.IsMouseDirectlyOverChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsMouseDirectlyOverChanged()", _evtIdx++, name);
            completionWindow.IsStylusCaptureWithinChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsStylusCaptureWithinChanged()", _evtIdx++, name);
            completionWindow.IsStylusCapturedChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsStylusCapturedChanged()", _evtIdx++, name);
            completionWindow.IsStylusDirectlyOverChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsStylusDirectlyOverChanged()", _evtIdx++, name);
            completionWindow.IsVisibleChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsVisibleChanged()", _evtIdx++, name);
            completionWindow.KeyDown += (sender, args) => Console.WriteLine("{0} - {1}.KeyDown({2})", _evtIdx++, name, args.Key);
            completionWindow.KeyUp += (sender, args) => Console.WriteLine("{0} - {1}.KeyUp({2})", _evtIdx++, name, args.Key);
//            completionWindow.LayoutUpdated += (sender, args) => Console.WriteLine("{0} - {1}.LayoutUpdated()", _evtIdx++, name);
            completionWindow.Loaded += (sender, args) => Console.WriteLine("{0} - {1}.Loaded()", _evtIdx++, name);
            completionWindow.LocationChanged += (sender, args) => Console.WriteLine("{0} - {1}.LocationChanged()", _evtIdx++, name);
            completionWindow.LostFocus += (sender, args) => Console.WriteLine("{0} - {1}.LostFocus()", _evtIdx++, name);
            completionWindow.LostKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.LostKeyboardFocus()", _evtIdx++, name);
            completionWindow.LostMouseCapture += (sender, args) => Console.WriteLine("{0} - {1}.LostMouseCapture()", _evtIdx++, name);
            completionWindow.LostStylusCapture += (sender, args) => Console.WriteLine("{0} - {1}.LostStylusCapture()", _evtIdx++, name);
            completionWindow.LostTouchCapture += (sender, args) => Console.WriteLine("{0} - {1}.LostTouchCapture()", _evtIdx++, name);
            completionWindow.ManipulationBoundaryFeedback += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationBoundaryFeedback()", _evtIdx++, name);
            completionWindow.ManipulationCompleted += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationCompleted()", _evtIdx++, name);
            completionWindow.ManipulationDelta += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationDelta()", _evtIdx++, name);
            completionWindow.ManipulationInertiaStarting += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationInertiaStarting()", _evtIdx++, name);
            completionWindow.ManipulationStarted += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationStarted()", _evtIdx++, name);
            completionWindow.ManipulationStarting += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationStarting()", _evtIdx++, name);
            completionWindow.MouseDoubleClick += (sender, args) => Console.WriteLine("{0} - {1}.MouseDoubleClick()", _evtIdx++, name);
            completionWindow.MouseDown += (sender, args) => Console.WriteLine("{0} - {1}.MouseDown()", _evtIdx++, name);
            completionWindow.MouseEnter += (sender, args) => Console.WriteLine("{0} - {1}.MouseEnter()", _evtIdx++, name);
            completionWindow.MouseLeave += (sender, args) => Console.WriteLine("{0} - {1}.MouseLeave()", _evtIdx++, name);
            completionWindow.MouseLeftButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.MouseLeftButtonDown()", _evtIdx++, name);
            completionWindow.MouseLeftButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.MouseLeftButtonUp()", _evtIdx++, name);
            completionWindow.MouseMove += (sender, args) => Console.WriteLine("{0} - {1}.MouseMove()", _evtIdx++, name);
            completionWindow.MouseRightButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.MouseRightButtonDown()", _evtIdx++, name);
            completionWindow.MouseRightButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.MouseRightButtonUp()", _evtIdx++, name);
            completionWindow.MouseUp += (sender, args) => Console.WriteLine("{0} - {1}.MouseUp()", _evtIdx++, name);
            completionWindow.MouseWheel += (sender, args) => Console.WriteLine("{0} - {1}.MouseWheel()", _evtIdx++, name);
            completionWindow.PreviewDragEnter += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDragEnter()", _evtIdx++, name);
            completionWindow.PreviewDragLeave += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDragLeave()", _evtIdx++, name);
            completionWindow.PreviewDragOver += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDragOver()", _evtIdx++, name);
            completionWindow.PreviewDrop += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDrop()", _evtIdx++, name);
            completionWindow.PreviewGiveFeedback += (sender, args) => Console.WriteLine("{0} - {1}.PreviewGiveFeedback()", _evtIdx++, name);
            completionWindow.PreviewGotKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.PreviewGotKeyboardFocus()", _evtIdx++, name);
            completionWindow.PreviewKeyDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewKeyDown({2})", _evtIdx++, name, args.Key);
            completionWindow.PreviewKeyUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewKeyUp({2})", _evtIdx++, name, args.Key);
            completionWindow.PreviewLostKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.PreviewLostKeyboardFocus()", _evtIdx++, name);
            completionWindow.PreviewMouseDoubleClick += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseDoubleClick()", _evtIdx++, name);
            completionWindow.PreviewMouseDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseDown()", _evtIdx++, name);
            completionWindow.PreviewMouseLeftButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseLeftButtonDown()", _evtIdx++, name);
            completionWindow.PreviewMouseLeftButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseLeftButtonUp()", _evtIdx++, name);
            completionWindow.PreviewMouseMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseMove()", _evtIdx++, name);
            completionWindow.PreviewMouseRightButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseRightButtonDown()", _evtIdx++, name);
            completionWindow.PreviewMouseRightButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseRightButtonUp()", _evtIdx++, name);
            completionWindow.PreviewMouseUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseUp()", _evtIdx++, name);
            completionWindow.PreviewMouseWheel += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseWheel()", _evtIdx++, name);
            completionWindow.PreviewQueryContinueDrag += (sender, args) => Console.WriteLine("{0} - {1}.PreviewQueryContinueDrag()", _evtIdx++, name);
            completionWindow.PreviewStylusButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusButtonDown()", _evtIdx++, name);
            completionWindow.PreviewStylusButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusButtonUp()", _evtIdx++, name);
            completionWindow.PreviewStylusDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusDown()", _evtIdx++, name);
            completionWindow.PreviewStylusInAirMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusInAirMove()", _evtIdx++, name);
            completionWindow.PreviewStylusInRange += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusInRange()", _evtIdx++, name);
            completionWindow.PreviewStylusMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusMove()", _evtIdx++, name);
            completionWindow.PreviewStylusOutOfRange += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusOutOfRange()", _evtIdx++, name);
            completionWindow.PreviewStylusSystemGesture += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusSystemGesture()", _evtIdx++, name);
            completionWindow.PreviewStylusUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusUp()", _evtIdx++, name);
            completionWindow.PreviewTextInput += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTextInput()", _evtIdx++, name);
            completionWindow.PreviewTouchDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTouchDown()", _evtIdx++, name);
            completionWindow.PreviewTouchMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTouchMove()", _evtIdx++, name);
            completionWindow.PreviewTouchUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTouchUp()", _evtIdx++, name);
            completionWindow.QueryContinueDrag += (sender, args) => Console.WriteLine("{0} - {1}.QueryContinueDrag()", _evtIdx++, name);
            completionWindow.QueryCursor += (sender, args) => Console.WriteLine("{0} - {1}.QueryCursor()", _evtIdx++, name);
            completionWindow.RequestBringIntoView += (sender, args) => Console.WriteLine("{0} - {1}.RequestBringIntoView()", _evtIdx++, name);
            completionWindow.SizeChanged += (sender, args) => Console.WriteLine("{0} - {1}.SizeChanged()", _evtIdx++, name);
            completionWindow.SourceInitialized += (sender, args) => Console.WriteLine("{0} - {1}.SourceInitialized()", _evtIdx++, name);
            completionWindow.SourceUpdated += (sender, args) => Console.WriteLine("{0} - {1}.SourceUpdated()", _evtIdx++, name);
            completionWindow.StateChanged += (sender, args) => Console.WriteLine("{0} - {1}.StateChanged()", _evtIdx++, name);
            completionWindow.StylusButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.StylusButtonDown()", _evtIdx++, name);
            completionWindow.StylusButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.StylusButtonUp()", _evtIdx++, name);
            completionWindow.StylusDown += (sender, args) => Console.WriteLine("{0} - {1}.StylusDown()", _evtIdx++, name);
            completionWindow.StylusEnter += (sender, args) => Console.WriteLine("{0} - {1}.StylusEnter()", _evtIdx++, name);
            completionWindow.StylusInAirMove += (sender, args) => Console.WriteLine("{0} - {1}.StylusInAirMove()", _evtIdx++, name);
            completionWindow.StylusInRange += (sender, args) => Console.WriteLine("{0} - {1}.StylusInRange()", _evtIdx++, name);
            completionWindow.StylusLeave += (sender, args) => Console.WriteLine("{0} - {1}.StylusLeave()", _evtIdx++, name);
            completionWindow.StylusMove += (sender, args) => Console.WriteLine("{0} - {1}.StylusMove()", _evtIdx++, name);
            completionWindow.StylusOutOfRange += (sender, args) => Console.WriteLine("{0} - {1}.StylusOutOfRange()", _evtIdx++, name);
            completionWindow.StylusSystemGesture += (sender, args) => Console.WriteLine("{0} - {1}.StylusSystemGesture()", _evtIdx++, name);
            completionWindow.StylusUp += (sender, args) => Console.WriteLine("{0} - {1}.StylusUp()", _evtIdx++, name);
            completionWindow.TargetUpdated += (sender, args) => Console.WriteLine("{0} - {1}.TargetUpdated()", _evtIdx++, name);
            completionWindow.TextInput += (sender, args) => Console.WriteLine("{0} - {1}.TextInput()", _evtIdx++, name);
            completionWindow.ToolTipClosing += (sender, args) => Console.WriteLine("{0} - {1}.ToolTipClosing()", _evtIdx++, name);
            completionWindow.ToolTipOpening += (sender, args) => Console.WriteLine("{0} - {1}.ToolTipOpening()", _evtIdx++, name);
            completionWindow.TouchDown += (sender, args) => Console.WriteLine("{0} - {1}.TouchDown()", _evtIdx++, name);
            completionWindow.TouchEnter += (sender, args) => Console.WriteLine("{0} - {1}.TouchEnter()", _evtIdx++, name);
            completionWindow.TouchLeave += (sender, args) => Console.WriteLine("{0} - {1}.TouchLeave()", _evtIdx++, name);
            completionWindow.TouchMove += (sender, args) => Console.WriteLine("{0} - {1}.TouchMove()", _evtIdx++, name);
            completionWindow.TouchUp += (sender, args) => Console.WriteLine("{0} - {1}.TouchUp()", _evtIdx++, name);
            completionWindow.Unloaded += (sender, args) => Console.WriteLine("{0} - {1}.Unloaded()", _evtIdx++, name);
        }

        private void BindToAllEvents(UIElement uiElement)
        {
            var name = uiElement.GetType().Name;
            uiElement.DragEnter += (sender, args) => Console.WriteLine("{0} - {1}.DragEnter()", _evtIdx++, name);
            uiElement.DragLeave += (sender, args) => Console.WriteLine("{0} - {1}.DragLeave()", _evtIdx++, name);
            uiElement.DragOver += (sender, args) => Console.WriteLine("{0} - {1}.DragOver()", _evtIdx++, name);
            uiElement.Drop += (sender, args) => Console.WriteLine("{0} - {1}.Drop()", _evtIdx++, name);
            uiElement.FocusableChanged += (sender, args) => Console.WriteLine("{0} - {1}.FocusableChanged()", _evtIdx++, name);
            uiElement.GiveFeedback += (sender, args) => Console.WriteLine("{0} - {1}.GiveFeedback()", _evtIdx++, name);
            uiElement.GotFocus += (sender, args) => Console.WriteLine("{0} - {1}.GotFocus()", _evtIdx++, name);
            uiElement.GotKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.GotKeyboardFocus()", _evtIdx++, name);
            uiElement.GotMouseCapture += (sender, args) => Console.WriteLine("{0} - {1}.GotMouseCapture()", _evtIdx++, name);
            uiElement.GotStylusCapture += (sender, args) => Console.WriteLine("{0} - {1}.GotStylusCapture()", _evtIdx++, name);
            uiElement.GotTouchCapture += (sender, args) => Console.WriteLine("{0} - {1}.GotTouchCapture()", _evtIdx++, name);
            uiElement.IsEnabledChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsEnabledChanged()", _evtIdx++, name);
            uiElement.IsHitTestVisibleChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsHitTestVisibleChanged()", _evtIdx++, name);
            uiElement.IsKeyboardFocusWithinChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsKeyboardFocusWithinChanged()", _evtIdx++, name);
            uiElement.IsKeyboardFocusedChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsKeyboardFocusedChanged()", _evtIdx++, name);
            uiElement.IsMouseCaptureWithinChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsMouseCaptureWithinChanged()", _evtIdx++, name);
            uiElement.IsMouseCapturedChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsMouseCapturedChanged()", _evtIdx++, name);
            uiElement.IsMouseDirectlyOverChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsMouseDirectlyOverChanged()", _evtIdx++, name);
            uiElement.IsStylusCaptureWithinChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsStylusCaptureWithinChanged()", _evtIdx++, name);
            uiElement.IsStylusCapturedChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsStylusCapturedChanged()", _evtIdx++, name);
            uiElement.IsStylusDirectlyOverChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsStylusDirectlyOverChanged()", _evtIdx++, name);
            uiElement.IsVisibleChanged += (sender, args) => Console.WriteLine("{0} - {1}.IsVisibleChanged()", _evtIdx++, name);
            uiElement.KeyDown += (sender, args) => Console.WriteLine("{0} - {1}.KeyDown({2})", _evtIdx++, name, args.Key);
            uiElement.KeyUp += (sender, args) => Console.WriteLine("{0} - {1}.KeyUp({2})", _evtIdx++, name, args.Key);
            uiElement.LostFocus += (sender, args) => Console.WriteLine("{0} - {1}.LostFocus()", _evtIdx++, name);
            uiElement.LostKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.LostKeyboardFocus()", _evtIdx++, name);
            uiElement.LostMouseCapture += (sender, args) => Console.WriteLine("{0} - {1}.LostMouseCapture()", _evtIdx++, name);
            uiElement.LostStylusCapture += (sender, args) => Console.WriteLine("{0} - {1}.LostStylusCapture()", _evtIdx++, name);
            uiElement.LostTouchCapture += (sender, args) => Console.WriteLine("{0} - {1}.LostTouchCapture()", _evtIdx++, name);
            uiElement.ManipulationBoundaryFeedback += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationBoundaryFeedback()", _evtIdx++, name);
            uiElement.ManipulationCompleted += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationCompleted()", _evtIdx++, name);
            uiElement.ManipulationDelta += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationDelta()", _evtIdx++, name);
            uiElement.ManipulationInertiaStarting += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationInertiaStarting()", _evtIdx++, name);
            uiElement.ManipulationStarted += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationStarted()", _evtIdx++, name);
            uiElement.ManipulationStarting += (sender, args) => Console.WriteLine("{0} - {1}.ManipulationStarting()", _evtIdx++, name);
            uiElement.MouseDown += (sender, args) => Console.WriteLine("{0} - {1}.MouseDown()", _evtIdx++, name);
            uiElement.MouseEnter += (sender, args) => Console.WriteLine("{0} - {1}.MouseEnter()", _evtIdx++, name);
            uiElement.MouseLeave += (sender, args) => Console.WriteLine("{0} - {1}.MouseLeave()", _evtIdx++, name);
            uiElement.MouseLeftButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.MouseLeftButtonDown()", _evtIdx++, name);
            uiElement.MouseLeftButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.MouseLeftButtonUp()", _evtIdx++, name);
            uiElement.MouseMove += (sender, args) => Console.WriteLine("{0} - {1}.MouseMove()", _evtIdx++, name);
            uiElement.MouseRightButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.MouseRightButtonDown()", _evtIdx++, name);
            uiElement.MouseRightButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.MouseRightButtonUp()", _evtIdx++, name);
            uiElement.MouseUp += (sender, args) => Console.WriteLine("{0} - {1}.MouseUp()", _evtIdx++, name);
            uiElement.MouseWheel += (sender, args) => Console.WriteLine("{0} - {1}.MouseWheel()", _evtIdx++, name);
            uiElement.PreviewDragEnter += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDragEnter()", _evtIdx++, name);
            uiElement.PreviewDragLeave += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDragLeave()", _evtIdx++, name);
            uiElement.PreviewDragOver += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDragOver()", _evtIdx++, name);
            uiElement.PreviewDrop += (sender, args) => Console.WriteLine("{0} - {1}.PreviewDrop()", _evtIdx++, name);
            uiElement.PreviewGiveFeedback += (sender, args) => Console.WriteLine("{0} - {1}.PreviewGiveFeedback()", _evtIdx++, name);
            uiElement.PreviewGotKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.PreviewGotKeyboardFocus()", _evtIdx++, name);
            uiElement.PreviewKeyDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewKeyDown({2})", _evtIdx++, name, args.Key);
            uiElement.PreviewKeyUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewKeyUp({2})", _evtIdx++, name, args.Key);
            uiElement.PreviewLostKeyboardFocus += (sender, args) => Console.WriteLine("{0} - {1}.PreviewLostKeyboardFocus()", _evtIdx++, name);
            uiElement.PreviewMouseDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseDown()", _evtIdx++, name);
            uiElement.PreviewMouseLeftButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseLeftButtonDown()", _evtIdx++, name);
            uiElement.PreviewMouseLeftButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseLeftButtonUp()", _evtIdx++, name);
            uiElement.PreviewMouseMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseMove()", _evtIdx++, name);
            uiElement.PreviewMouseRightButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseRightButtonDown()", _evtIdx++, name);
            uiElement.PreviewMouseRightButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseRightButtonUp()", _evtIdx++, name);
            uiElement.PreviewMouseUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseUp()", _evtIdx++, name);
            uiElement.PreviewMouseWheel += (sender, args) => Console.WriteLine("{0} - {1}.PreviewMouseWheel()", _evtIdx++, name);
            uiElement.PreviewQueryContinueDrag += (sender, args) => Console.WriteLine("{0} - {1}.PreviewQueryContinueDrag()", _evtIdx++, name);
            uiElement.PreviewStylusButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusButtonDown()", _evtIdx++, name);
            uiElement.PreviewStylusButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusButtonUp()", _evtIdx++, name);
            uiElement.PreviewStylusDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusDown()", _evtIdx++, name);
            uiElement.PreviewStylusInAirMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusInAirMove()", _evtIdx++, name);
            uiElement.PreviewStylusInRange += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusInRange()", _evtIdx++, name);
            uiElement.PreviewStylusMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusMove()", _evtIdx++, name);
            uiElement.PreviewStylusOutOfRange += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusOutOfRange()", _evtIdx++, name);
            uiElement.PreviewStylusSystemGesture += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusSystemGesture()", _evtIdx++, name);
            uiElement.PreviewStylusUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewStylusUp()", _evtIdx++, name);
            uiElement.PreviewTextInput += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTextInput()", _evtIdx++, name);
            uiElement.PreviewTouchDown += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTouchDown()", _evtIdx++, name);
            uiElement.PreviewTouchMove += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTouchMove()", _evtIdx++, name);
            uiElement.PreviewTouchUp += (sender, args) => Console.WriteLine("{0} - {1}.PreviewTouchUp()", _evtIdx++, name);
            uiElement.QueryContinueDrag += (sender, args) => Console.WriteLine("{0} - {1}.QueryContinueDrag()", _evtIdx++, name);
            uiElement.QueryCursor += (sender, args) => Console.WriteLine("{0} - {1}.QueryCursor()", _evtIdx++, name);
            uiElement.StylusButtonDown += (sender, args) => Console.WriteLine("{0} - {1}.StylusButtonDown()", _evtIdx++, name);
            uiElement.StylusButtonUp += (sender, args) => Console.WriteLine("{0} - {1}.StylusButtonUp()", _evtIdx++, name);
            uiElement.StylusDown += (sender, args) => Console.WriteLine("{0} - {1}.StylusDown()", _evtIdx++, name);
            uiElement.StylusEnter += (sender, args) => Console.WriteLine("{0} - {1}.StylusEnter()", _evtIdx++, name);
            uiElement.StylusInAirMove += (sender, args) => Console.WriteLine("{0} - {1}.StylusInAirMove()", _evtIdx++, name);
            uiElement.StylusInRange += (sender, args) => Console.WriteLine("{0} - {1}.StylusInRange()", _evtIdx++, name);
            uiElement.StylusLeave += (sender, args) => Console.WriteLine("{0} - {1}.StylusLeave()", _evtIdx++, name);
            uiElement.StylusMove += (sender, args) => Console.WriteLine("{0} - {1}.StylusMove()", _evtIdx++, name);
            uiElement.StylusOutOfRange += (sender, args) => Console.WriteLine("{0} - {1}.StylusOutOfRange()", _evtIdx++, name);
            uiElement.StylusSystemGesture += (sender, args) => Console.WriteLine("{0} - {1}.StylusSystemGesture()", _evtIdx++, name);
            uiElement.StylusUp += (sender, args) => Console.WriteLine("{0} - {1}.StylusUp()", _evtIdx++, name);
            uiElement.TextInput += (sender, args) => Console.WriteLine("{0} - {1}.TextInput()", _evtIdx++, name);
            uiElement.TouchDown += (sender, args) => Console.WriteLine("{0} - {1}.TouchDown()", _evtIdx++, name);
            uiElement.TouchEnter += (sender, args) => Console.WriteLine("{0} - {1}.TouchEnter()", _evtIdx++, name);
            uiElement.TouchLeave += (sender, args) => Console.WriteLine("{0} - {1}.TouchLeave()", _evtIdx++, name);
            uiElement.TouchMove += (sender, args) => Console.WriteLine("{0} - {1}.TouchMove()", _evtIdx++, name);
            uiElement.TouchUp += (sender, args) => Console.WriteLine("{0} - {1}.TouchUp()", _evtIdx++, name);
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
