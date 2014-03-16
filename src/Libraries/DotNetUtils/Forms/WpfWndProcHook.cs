using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Hooks into the <c>WindowProc</c> message handler to allow classes outside of a <see cref="FrameworkElement"/>
    ///     to receive, handle, and suppress native window messages.
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/a/836713/467582"/>
    public class WpfWndProcHook
    {
        private readonly FrameworkElement _elem;

        /// <summary>
        ///     Invoked whenever the hooked control receives a native window message.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public class MyElementHost : ElementHost
        ///         {
        ///             public void ShowWindow()
        ///             {
        ///                 var window = new Window();
        ///                 var hook = new WpfWndProcHook(window);
        ///                 hook.WndProcMessage += HookOnWndProcMessage;
        ///                 window.Show();
        ///             }
        ///
        ///             private void HookOnWndProcMessage(ref Message m, HandledEventArgs args)
        ///             {
        ///                 WindowMessage message = m;
        ///
        ///                 if (message.Is(WindowMessageType.WM_GETDLGCODE))
        ///                 {
        ///                     switch (message.WParamInt64Value)
        ///                     {
        ///                         case VirtualKey.VK_TAB:
        ///                         case VirtualKey.VK_RETURN:
        ///                         case VirtualKey.VK_LEFT:
        ///                         case VirtualKey.VK_UP:
        ///                         case VirtualKey.VK_RIGHT:
        ///                         case VirtualKey.VK_DOWN:
        ///                             args.Handled = true;
        ///                             m.Result = new IntPtr(DialogCode.DLGC_WANTMESSAGE);
        ///                             break;
        ///                     }
        ///                 }
        ///             }
        ///         }
        /// </code>
        /// </example>
        public event WndProcEventHandler WndProcMessage;

        /// <summary>
        ///     Constructs a new <see cref="WpfWndProcHook"/> instance that listens for window messages
        ///     sent to the given <paramref name="elem"/>.
        /// </summary>
        /// <param name="elem">
        ///     A WPF framework element to listen to for window messages.
        /// </param>
        public WpfWndProcHook(FrameworkElement elem)
        {
            _elem = elem;

            if (_elem == null)
                return;

            if (_elem.IsLoaded)
                AddHook();
            else
                _elem.Loaded += (sender, args) => AddHook();
        }

        /// <summary>
        ///     Hooks the specified <paramref name="handler"/> into the given <paramref name="elem"/>'s
        ///     window message system.
        /// </summary>
        /// <param name="elem">
        ///     A WPF framework element to listen to for window messages.
        /// </param>
        /// <param name="handler">
        ///     Event handler that will be called whenever <paramref name="elem"/> receives a window message.
        /// </param>
        public static void Hook(FrameworkElement elem, WndProcEventHandler handler)
        {
            new WpfWndProcHook(elem).WndProcMessage += handler;
        }

        private void AddHook()
        {
            var hwndSource = PresentationSource.FromVisual(_elem) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(Hook);
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var m = new Message
                    {
                        HWnd = hwnd,
                        Msg = msg,
                        WParam = wParam,
                        LParam = lParam
                    };

            if (WndProcMessage != null)
            {
                var args = new HandledEventArgs();
                WndProcMessage(ref m, args);
                handled = args.Handled;
                return m.Result;
            }

            return IntPtr.Zero;
        }
    }
}
