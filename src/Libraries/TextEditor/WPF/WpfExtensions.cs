using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace TextEditor.WPF
{
    internal static class WpfExtensions
    {
        public static void SetTimeout<T>(this T elem, Action<T> action, double interval)
            where T : UIElement
        {
            var timer = new Timer(interval) { AutoReset = false };
            timer.Elapsed += delegate
                             {

                                 if (elem.Dispatcher.CheckAccess())
                                 {
                                     // The calling thread owns the dispatcher, and hence the UI element
                                     action(elem);
                                 }
                                 else
                                 {
                                     // Invokation required
                                     elem.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => action(elem)));
                                 }
                             };
            timer.Start();
        }
    }
}
