// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using System.Threading;
using DotNetUtils.Annotations;

namespace DotNetUtils.Concurrency
{
    public struct Null
    {
    }

    public interface IInvoker
    {
        void InvokeSync(Action action);
        void InvokeAsync(Action action);
    }

    public class Invoker : IInvoker
    {
        private readonly ISynchronizeInvoke _uiContext;

        public Invoker(ISynchronizeInvoke uiContext)
        {
            _uiContext = uiContext;
        }

        public void InvokeSync(Action action)
        {
            _uiContext.Invoke(action, new object[0]);
        }

        public void InvokeAsync(Action action)
        {
            _uiContext.BeginInvoke(action, new object[0]);
        }
    }

    /// <summary>
    /// Specifies an interface for a background thread that communicates with the UI thread via events and returns a
    /// value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IPromise<TResult>
    {
        /// <summary>
        /// Gets or sets the amount of time the UI thread should wait between checks for events from the background thread.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets the last exception that was thrown by a <see cref="DoWorkPromiseHandler{TResult}"/> registered via <see cref="Work"/>.
        /// </summary>
        [CanBeNull]
        Exception LastException { get; }

        /// <summary>
        /// Gets whether cancellation has been requested via <see cref="Cancel"/>.
        /// </summary>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Gets whether the promise has been resolved.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets or sets the result value of this promise <strong>atomically</strong>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown from the getter if the result value has not yet been set.
        /// </exception>
        TResult Result { get; set; }

        /// <summary>
        /// Gets an invoker that executes actions on the main UI thread.
        /// </summary>
        IInvoker UIInvoker { get; }

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified just before 
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise<TResult> Before(BeforePromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be run in a background thread as the main unit
        /// of work.
        /// </summary>
        /// <param name="handler">Event handler to run in a background thread.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>background</strong> thread.</remarks>
        IPromise<TResult> Work(DoWorkPromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when cancellation is requested
        /// via <see cref="Cancel"/>.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks>
        /// Depending on what the background thread is doing, there may be a significant delay
        /// between when cancellation is <em>requested</em> and when the thread <em>actually exits</em>.
        /// </remarks>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise<TResult> CancellationRequested(CancellationRequestedPromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// exits due to being canceled via <see cref="Cancel"/>.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks>
        /// Depending on what the background thread is doing, there may be a significant delay
        /// between when cancellation is <em>requested</em> and when the thread <em>actually exits</em>.
        /// </remarks>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise<TResult> Canceled(CanceledPromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// completes successfully without throwing any exceptions or being canceled.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise<TResult> Done(SuccessPromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// terminates abnormally due to an exception being thrown.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        /// <seealso cref="LastException"/>
        IPromise<TResult> Fail(FailurePromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified when the background thread
        /// finishes running, regardless of why it finished.  It is always invoked <em>after</em>
        /// <see cref="Canceled"/>, <see cref="Done"/>, and <see cref="Fail"/> events.
        /// </summary>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise<TResult> Always(AlwaysPromiseHandler<TResult> handler);

        /// <summary>
        /// Registers the specified <paramref name="handler"/> to be notified whenever the background thread
        /// reports a progress update of type <typeparamref name="TState"/>.
        /// </summary>
        /// <typeparam name="TState">
        /// Datatype to listen for.  Only progress updates with this datatype will invoke the specified <paramref name="handler"/>.
        /// </typeparam>
        /// <param name="handler">Event handler to register.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        /// <remarks><paramref name="handler"/> will run in the <strong>UI</strong> thread.</remarks>
        IPromise<TResult> Progress<TState>(ProgressPromiseHandler<TResult, TState> handler) where TState : class;

        /// <summary>
        /// Report background thread progress.  Invoked by a <see cref="DoWorkPromiseHandler{TResult}"/>.
        /// </summary>
        /// <typeparam name="TState">Datatype of <paramref name="state"/>.</typeparam>
        /// <param name="state">The current state of the background thread's progress.</param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        IPromise<TResult> Progress<TState>(TState state) where TState : class;

        /// <summary>
        /// Initiates execution of the <see cref="Work"/> handlers in a background thread.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this method is called more than once.</exception>
        IPromise<TResult> Start();

        /// <summary>
        /// Requests that the background thread terminate execution.
        /// </summary>
        /// <remarks>
        /// This method merely <em>requests</em> that the background thread cancel execution; it does not (and cannot)
        /// forcefully abort the background thread.
        /// <see cref="DoWorkPromiseHandler{TResult}"/>s should frequently check the value of <see cref="IsCancellationRequested"/>
        /// to know when to stop execution.
        /// </remarks>
        IPromise<TResult> Cancel();

        /// <summary>
        /// Cancels this promise when the given <paramref name="token"/> is cancelled.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        IPromise<TResult> CancelWith(CancellationToken token);

        /// <summary>
        /// Blocks the current thread indefinitely until this promise to completes.
        /// </summary>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        IPromise<TResult> Wait();

        /// <summary>
        /// Blocks the current thread indefinitely until this promise to completes or the specified
        /// <paramref name="millisecondsTimeout"/> elapses.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/><c>(-1)</c> to wait indefinitely.
        /// </param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        IPromise<TResult> Wait(int millisecondsTimeout);

        /// <summary>
        /// Blocks the current thread indefinitely until this promise to completes or the specified
        /// <paramref name="timeout"/> elapses.
        /// </summary>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> that represents the number of milliseconds to wait,
        /// or a <see cref="TimeSpan"/> that represents <c>-1</c> milliseconds to wait indefinitely.
        /// </param>
        /// <returns>Reference to this <see cref="IPromise{TResult}"/> object.</returns>
        IPromise<TResult> Wait(TimeSpan timeout);
    }
}