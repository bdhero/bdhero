using System;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    ///     Interface for invoking actions synchronously (i.e., blocking the current thread) as well as
    ///     asynchronously.
    /// </summary>
    public interface IInvoker
    {
        /// <summary>
        ///     Invokes the given <paramref name="action"/> synchronously by blocking the current thread's execution
        ///     until <paramref name="action"/> completes.
        /// </summary>
        /// <param name="action">
        ///     Action to invoke synchronously on the current thread.
        /// </param>
        void InvokeSync(Action action);

        /// <summary>
        ///     Invokes the given <paramref name="action"/> asynchronously on a background thread.
        ///     Does not block the current thread's execution.
        /// </summary>
        /// <param name="action">
        ///     Action to invoke asynchronously on a background thread.
        /// </param>
        void InvokeAsync(Action action);
    }
}