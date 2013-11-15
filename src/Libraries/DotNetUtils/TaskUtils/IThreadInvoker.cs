using System;
using System.Threading;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    /// Defines methods to invoke actions on a specific thread.
    /// </summary>
    public interface IThreadInvoker
    {
        /// <summary>
        /// Invokes the given action on the UI thread and blocks until the action completes executing.
        /// </summary>
        /// <param name="action">Action to perform on the UI thread</param>
        void InvokeOnUIThreadSync(ThreadAction action);

        /// <summary>
        /// Invokes the given action on the UI thread and returns immediately without waiting for the action to complete executing.
        /// </summary>
        /// <param name="action">Action to perform on the UI thread</param>
        void InvokeOnUIThreadAsync(ThreadAction action);
    }
}