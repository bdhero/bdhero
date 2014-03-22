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
using System.Threading;
using System.Threading.Tasks;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    /// Builds a <see cref="Task"/> object that runs an arbitrary <see cref="Action"/> in a background thread.
    /// Start/stop events and exception handlers are invoked on the UI thread specified by
    /// <see cref="OnThread"/> or <see cref="OnCurrentThread"/>.
    /// </summary>
    /// <remarks>
    /// One of the most difficult aspects of multi-threaded programming in modern languages is
    /// safely communicating with the UI thread.  This class makes such communication simple and painless
    /// by automatically invoking the <see cref="BeforeStart"/>, <see cref="Succeed"/>, <see cref="Fail"/>,
    /// and <see cref="Finally"/> actions on the UI thread while also providing a mechanism for the background
    /// <see cref="DoWork"/> action to communicate its status and progress information back to the UI.
    /// </remarks>
    public class TaskBuilder
    {
        private TaskScheduler _callbackThread;
        private CancellationToken _cancellationToken;
        private IThreadInvoker _invoker;

        private TaskStartedEventHandler _beforeStart;
        private TaskWorkHandler _work;
        private TaskSucceededEventHandler _succeed;
        private ExceptionEventHandler _fail;
        private TaskCompletedEventHandler _finally;

        /// <summary>
        /// Sets the task's execution context to that of the specified thread.
        /// </summary>
        /// <param name="callbackThread"></param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder OnThread(TaskScheduler callbackThread)
        {
            _callbackThread = callbackThread;
            return this;
        }

        /// <summary>
        /// Sets the task's execution context to that of the calling thread (typically the UI thread).
        /// </summary>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder OnCurrentThread()
        {
            // Get the calling thread's context
            _callbackThread = (SynchronizationContext.Current != null
                                  ? TaskScheduler.FromCurrentSynchronizationContext()
                                  : TaskScheduler.Default);
            return this;
        }

        /// <summary>
        /// Sets a CancellationToken that may be used to request the task to cancel is current operation.
        /// Requires that the actions specified by <see cref="BeforeStart"/> and <see cref="DoWork"/>
        /// check the value of <see cref="CancellationToken.IsCancellationRequested"/> periodically
        /// and abort their operation as soon as cancellation is requested.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder CancelWith(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread before invoking the main <see cref="DoWork"/> action.
        /// If the <c>BeforeStart </c>action fails, the <c>DoWork</c> action will not be run.
        /// </summary>
        /// <param name="beforeStart">Action to run on the UI thread</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder BeforeStart(TaskStartedEventHandler beforeStart)
        {
            _beforeStart = beforeStart;
            return this;
        }

        /// <summary>
        /// Runs in background (non-UI) thread, but may execute an action on the UI thread by calling the passed thread invoker.
        /// </summary>
        /// <param name="work">Action to run in the background thread</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder DoWork(TaskWorkHandler work)
        {
            _work = work;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and completes successfully (i.e., without throwing an exception).
        /// </summary>
        /// <param name="succeed">Action to run in the UI thread</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder Succeed(TaskSucceededEventHandler succeed)
        {
            _succeed = succeed;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread when the main <see cref="DoWork"/> action
        /// throws an exception.
        /// </summary>
        /// <param name="fail">Action to run in the UI thread</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder Fail(ExceptionEventHandler fail)
        {
            _fail = fail;
            return this;
        }

        /// <summary>
        /// Always runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and after the <see cref="Succeed"/> or <see cref="Fail"/> callbacks have ran, regardless of
        /// whether the main <c>DoWork</c> action succeeded or failed.
        /// </summary>
        /// <param name="finally">Action to run in the UI thread</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder Finally(TaskCompletedEventHandler @finally)
        {
            _finally = @finally;
            return this;
        }

        /// <summary>
        /// Builds a <see cref="Task{TResult}"/> object whose <see cref="Task{TResult}.Result"/> property returns
        /// <code>true</code> if the task's <see cref="DoWork"/> action completed without throwing an exception or
        /// <code>false</code> if it threw an exception.
        /// </summary>
        /// <returns>
        /// Task that may be run asynchronously or synchronously on the thread specified by
        /// <see cref="OnThread"/> or <see cref="OnCurrentThread"/>.
        /// </returns>
        public Task<bool> Build()
        {
            var scheduler = _callbackThread;
            var token = _cancellationToken;

            if (scheduler == null)
            {
                throw new InvalidOperationException("Missing TaskScheduler");
            }

            if (token == null)
            {
                throw new InvalidOperationException("Missing CancellationToken");
            }

            _invoker = new ThreadInvoker(scheduler, token);

            var task = new Task<bool>(delegate
                {
                    try
                    {
                        if (InvokeBeforeStart() && InvokeDoWork())
                        {
                            InvokeSucceed();
                            return true;
                        }
                    }
                    finally
                    {
                        // TODO: Log uncaught exceptions?
                        // TODO: Where to handle uncaught exceptions?
                        InvokeFinally();
                    }
                    return false;
                }, token, TaskCreationOptions.None);

            return task;
        }

        private bool InvokeBeforeStart()
        {
            if (_beforeStart == null) return true;
            return Try(() => _invoker.InvokeOnUIThreadSync(_ => _beforeStart()));
        }

        private bool InvokeDoWork()
        {
            if (_work == null) return true;
            return Try(() => _work(_invoker, _cancellationToken));
        }

        private void InvokeSucceed()
        {
            if (_succeed == null) return;
            _invoker.InvokeOnUIThreadSync(token => _succeed());
        }

        private void InvokeFail(ExceptionEventArgs args)
        {
            if (_fail == null) return;
            _invoker.InvokeOnUIThreadSync(token => _fail(args));
        }

        private void InvokeFinally()
        {
            if (_finally == null) return;
            _invoker.InvokeOnUIThreadSync(token => _finally());
        }

        private bool Try(Action action)
        {
            if (_fail != null)
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    InvokeFail(new ExceptionEventArgs(exception));
                    return false;
                }
            }
            else
            {
                action();
            }
            return true;
        }
    }
}