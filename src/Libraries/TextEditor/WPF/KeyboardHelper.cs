using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DotNetUtils.Annotations;
using NativeAPI.Win.User;
using UILib.WPF;

namespace TextEditor.WPF
{
    internal static class KeyboardHelper
    {
        /// <seealso cref="http://stackoverflow.com/a/1646568/467582"/>
        public static void RaiseKeyEvent(FrameworkElement target, KeyEventArgs e)
        {
            var keyboardDevice = Keyboard.PrimaryDevice;
            var inputSource = PresentationSource.FromVisual(target);
            var routedEvent = e.IsUp ? Keyboard.KeyUpEvent : Keyboard.KeyDownEvent;

            if (inputSource == null)
                return;

            var args = new KeyEventArgs(keyboardDevice, inputSource, e.Timestamp, e.Key) { RoutedEvent = routedEvent };

            target.RaiseEvent(args);
        }

        public static bool IsHandledByAvalon(Key key)
        {
            switch (key)
            {
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                case Key.Down:
                case Key.Up:
                case Key.Tab:
                case Key.Enter:
                case Key.Escape:
                    return true;
            }
            return false;
        }

        [NotNull]
        public static string GetPrintableString(Key key)
        {
            // Ignore meta keys
            if (IsMetaKey(key))
                return "";

            // Ignore non-printable characters
            if (!IsPrintable(key))
                return "";

            var keyChar = WpfKeyboardUtils.GetCharFromKey(key);

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
            var keyChar = WpfKeyboardUtils.GetCharFromKey(key);

            // Non-printable control character
            if (keyChar < ' ')
                return false;

            // Other non-printable character (e.g., Delete)
            if (keyChar == ' ' && key != Key.Space)
                return false;

            return true;
        }
    }
}
