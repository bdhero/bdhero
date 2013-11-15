using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using DotNetUtils.Extensions;

namespace WindowsOSUtils.WMI
{
    /// <summary>
    /// Watches for WMI events related to the WMI class specified by <typeparamref name="T"/>
    /// and asynchronously notifies observers whenever an event occurs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WMIWatcher<T> : IDisposable
    {
        protected IList<ManagementEventWatcher> Watchers = new List<ManagementEventWatcher>();

        protected WMIWatcher()
        {
            RegisterShutdownHandlers();
        }

        ~WMIWatcher()
        {
            Dispose(false); // I am *not* calling you from Dispose, it's *not* safe to free managed resources
        }

        private void RegisterShutdownHandlers()
        {
            // Called first, but only when running in a Windows Forms application.
            // TODO: What about WPF?
            // See http://www.codeproject.com/Questions/149933/InvalidComObjectException-was-unhandled
            System.Windows.Forms.Application.ApplicationExit += (sender, args) => Dispose();

            // Called second, when the process is about to exit (but has not yet done so).
            // Throws an InvalidComObjectException when calling ManagementEventWatcher.Stop()
            // unless the Windows Forms ApplicationExit handler has been called first.
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => Dispose();
        }

        /// <summary>
        /// Frees managed and unmanaged resources.
        /// </summary>
        /// <param name="freeManagedObjectsAlso">Free managed resources.  Should only be set to <c>true</c> when called from <see cref="Dispose"/>.</param>
        /// <seealso cref="http://stackoverflow.com/a/538238/467582"/>
        protected void Dispose(bool freeManagedObjectsAlso)
        {
            // Free unmanaged resources
            // ...

            // Free managed resources too, but only if I'm being called from Dispose()
            // (If I'm being called from Finalize then the objects might not exist anymore)
            if (freeManagedObjectsAlso)
            {
                if (Watchers != null)
                {
                    Stop();
                    Watchers = null;
                }
            }
        }

        /// <seealso cref="http://stackoverflow.com/a/538238/467582"/>
        public void Dispose()
        {
            Dispose(true); // I am calling you from Dispose, it's safe
            GC.SuppressFinalize(this); // Hey, GC: don't bother calling finalize later
        }

        /// <summary>
        /// Starts listening for WMI events.
        /// </summary>
        public void Start()
        {
            Watchers.ForEach(watcher => watcher.Start());
        }

        /// <summary>
        /// Stops listening for WMI events.
        /// </summary>
        public void Stop()
        {
            Watchers.ForEach(StopWatcher);
        }

        private static void StopWatcher(ManagementEventWatcher watcher)
        {
            try
            {
                watcher.Stop();
            }
            catch (InvalidComObjectException)
            {
                // Thrown when the application is shutting down and the COM object
                // used by the WMI watcher has already been freed.
                // Ideally we should call Stop() before that happens, but
                // it may not be possible in non-Windows Forms applications.
            }
        }
    }
}
