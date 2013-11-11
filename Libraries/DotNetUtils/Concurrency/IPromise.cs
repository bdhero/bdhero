﻿using System;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    /// Specifies an interface for a background thread that communicates with the UI thread via events.
    /// </summary>
    public interface IPromise
    {
        /// <summary>
        /// Gets or sets the amount of time the UI thread should wait between checks for events from the background thread.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets the last exception that was thrown by 
        /// </summary>
        Exception LastException { get; }

        /// <summary>
        /// Gets whether cancellation has been requested via <see cref="Cancel"/>.
        /// </summary>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified just before 
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise Before(BeforePromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be run in a background thread as the main unit
        /// of work.
        /// </summary>
        /// <param name="handler">Event handler to run in a background thread.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>background</strong> thread.</remarks>
        IPromise DoWork(DoWorkPromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when cancellation is requested
        /// via <see cref="Cancel"/>.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks>
        /// Depending on what the background thread is doing, there may be a significant delay
        /// between when cancellation is <em>requested</em> and when the thread <em>actually exits</em>.
        /// </remarks>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise CancellationRequested(CancellationRequestedPromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// exits due to being canceled via <see cref="Cancel"/>.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks>
        /// Depending on what the background thread is doing, there may be a significant delay
        /// between when cancellation is <em>requested</em> and when the thread <em>actually exits</em>.
        /// </remarks>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise Canceled(CanceledPromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// completes successfully without throwing any exceptions or being canceled.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise Succeed(SuccessPromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// terminates abnormally due to an exception being thrown.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        /// <seealso cref="LastException"/>
        IPromise Fail(FailurePromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// finishes running, regardless of why it finished.  It is always invoked <em>after</em>
        /// <see cref="Canceled"/>, <see cref="Succeed"/>, and <see cref="Fail"/> events.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise Always(AlwaysPromiseHandler handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified whenever the background thread
        /// reports a progress update of type <typeparamref name="TState"/>.
        /// </summary>
        /// <typeparam name="TState">
        /// Datatype to listen for.  Only progress updates with this datatype will invoke the specified <paramref name="handler"/>.
        /// </typeparam>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise Progress<TState>(ProgressPromiseHandler<TState> handler) where TState : class;

        /// <summary>
        /// Report background thread progress.  Invoked by a <see cref="DoWorkPromiseHandler"/>.
        /// </summary>
        /// <typeparam name="TState">Datatype of <paramref name="state"/>.</typeparam>
        /// <param name="state">The current state of the background thread's progress.</param>
        /// <returns>Reference to this <see cref="IPromise"/> object.</returns>
        IPromise Progress<TState>(TState state) where TState : class;

        /// <summary>
        /// Initiates execution of the <see cref="DoWork"/> handlers in a background thread.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this method is called more than once.</exception>
        void Start();

        /// <summary>
        /// Requests that the background thread terminate execution.
        /// </summary>
        /// <remarks>
        /// This method merely <em>requests</em> that the background thread cancel execution; it does not (and cannot)
        /// forcefully abort the background thread.
        /// <see cref="DoWorkPromiseHandler"/>s should frequently check the value of <see cref="IsCancellationRequested"/>
        /// to know when to stop execution.
        /// </remarks>
        void Cancel();
    }
}