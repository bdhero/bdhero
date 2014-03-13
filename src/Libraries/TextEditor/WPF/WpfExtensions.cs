using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using DotNetUtils.Annotations;
using Timer = System.Timers.Timer;

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

        /// <summary>
        ///     Attempts to retrieve the Windows form that the given WPF element is hosted in.
        /// </summary>
        /// <typeparam name="T">
        ///     Any type that extends the WPF <see cref="Visual"/> class.
        /// </typeparam>
        /// <param name="elem">
        ///     A WPF visual element.
        /// </param>
        /// <returns>
        ///     The Windows form host, or <c>null</c> if <paramref name="elem"/> is not hosted in a Windows form.
        /// </returns>
        /// <seealso cref="http://social.msdn.microsoft.com/Forums/vstudio/en-US/b0a5bfcd-db94-425d-9c56-07233441d055/how-to-get-elementhost-given-a-wpf-control?forum=wpf#92811a22-c67b-4937-8817-40d7512940b5"/>
        [CanBeNull]
        public static Form FindForm<T>(this T elem)
            where T : Visual
        {
            var wpfHandle = PresentationSource.FromVisual(elem) as HwndSource;

            // The WPF control is hosted if the wpfHandle is not null
            if (wpfHandle != null)
            {
                var host = Control.FromChildHandle(wpfHandle.Handle) as ElementHost;
                if (host != null)
                {
                    return host.FindForm();
                }
            }

            return null;
        }
    }
}
