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
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DotNetUtils.Concurrency
{
    public class TimerPromise : IPromise
    {
        private delegate void ProgressPromiseHandler(object state);

        private static readonly TimeSpan DefaultInterval = TimeSpan.FromSeconds(1/10);

        private readonly AtomicValue<Exception> _lastException = new AtomicValue<Exception>();

        #region Event handlers

        private readonly ConcurrentLinkedSet<BeforePromiseHandler> _beforeHandlers =
            new ConcurrentLinkedSet<BeforePromiseHandler>();

        private readonly ConcurrentLinkedSet<DoWorkPromiseHandler> _doWorkHandlers =
            new ConcurrentLinkedSet<DoWorkPromiseHandler>();

        private readonly ConcurrentLinkedSet<CancellationRequestedPromiseHandler> _cancellationRequestedHandlers =
            new ConcurrentLinkedSet<CancellationRequestedPromiseHandler>();

        private readonly ConcurrentLinkedSet<CanceledPromiseHandler> _canceledHandlers =
            new ConcurrentLinkedSet<CanceledPromiseHandler>();

        private readonly ConcurrentLinkedSet<SuccessPromiseHandler> _successHandlers =
            new ConcurrentLinkedSet<SuccessPromiseHandler>();

        private readonly ConcurrentLinkedSet<FailurePromiseHandler> _failureHandlers =
            new ConcurrentLinkedSet<FailurePromiseHandler>();

        private readonly ConcurrentLinkedSet<AlwaysPromiseHandler> _alwaysHandlers =
            new ConcurrentLinkedSet<AlwaysPromiseHandler>();

        private readonly ConcurrentMultiValueDictionary<Type, ProgressPromiseHandler> _progressHandlers =
            new ConcurrentMultiValueDictionary<Type, ProgressPromiseHandler>();

        #endregion

        #region Event queues

        private readonly ConcurrentMultiValueDictionary<Type, object> _progressEventStates =
            new ConcurrentMultiValueDictionary<Type, object>();

        private readonly ConcurrentQueue<DateTime> _cancellationRequestQueue = new ConcurrentQueue<DateTime>();
        private readonly ConcurrentQueue<DateTime> _successQueue = new ConcurrentQueue<DateTime>();
        private readonly ConcurrentQueue<DateTime> _finishedQueue = new ConcurrentQueue<DateTime>();

        #endregion

        private volatile bool _hasStarted;

        private readonly Timer _timer = new Timer(DefaultInterval.TotalMilliseconds) { AutoReset = true };

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public TimerPromise(ISynchronizeInvoke synchronizingObject)
        {
            _timer.SynchronizingObject = synchronizingObject;
            _timer.Elapsed += OnTimerElapsed;
            _cancellationTokenSource.Token.Register(OnCancellationRequested);
        }

        #region Public properties

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        public Exception LastException
        {
            get { return _lastException.Value; }
            private set { _lastException.Value = value; }
        }

        public bool IsCancellationRequested
        {
            get { return _cancellationTokenSource.IsCancellationRequested; }
        }

        #endregion

        #region Public event handlers

        public IPromise Before(BeforePromiseHandler handler)
        {
            _beforeHandlers.Add(handler);
            return this;
        }

        public IPromise DoWork(DoWorkPromiseHandler handler)
        {
            _doWorkHandlers.Add(handler);
            return this;
        }

        public IPromise CancellationRequested(CancellationRequestedPromiseHandler handler)
        {
            _cancellationRequestedHandlers.Add(handler);
            return this;
        }

        public IPromise Canceled(CanceledPromiseHandler handler)
        {
            _canceledHandlers.Add(handler);
            return this;
        }

        public IPromise Fail(FailurePromiseHandler handler)
        {
            _failureHandlers.Add(handler);
            return this;
        }

        public IPromise Succeed(SuccessPromiseHandler handler)
        {
            _successHandlers.Add(handler);
            return this;
        }

        public IPromise Always(AlwaysPromiseHandler handler)
        {
            _alwaysHandlers.Add(handler);
            return this;
        }

        #endregion

        #region Public progress handlers

        public IPromise Progress<TState>(ProgressPromiseHandler<TState> handler) where TState : class
        {
            var handlerWrapper = new ProgressPromiseHandler(o => handler(this, o as TState));
            _progressHandlers.Enqueue(typeof(TState), handlerWrapper);
            return this;
        }

        public IPromise Progress<TState>(TState state) where TState : class
        {
            _progressEventStates.Enqueue(typeof(TState), state);
            return this;
        }

        #endregion

        #region Public control methods

        public void Start()
        {
            PreventMultipleStarts();
            InvokeBeforeHandlers();
            StartTimer();
            StartTask();
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        #endregion

        #region Private methods

        private void PreventMultipleStarts()
        {
            if (_hasStarted)
            {
                throw new InvalidOperationException("TimerPromise.Start() cannot be called more than once");
            }

            _hasStarted = true;
        }

        private void InvokeBeforeHandlers()
        {
            foreach (var beforeHandler in _beforeHandlers.ToList())
            {
                beforeHandler(this);

                if (IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void StartTimer()
        {
            _timer.Start();
        }

        private void StartTask()
        {
            Task.Factory.StartNew(TryDoWork, _cancellationTokenSource.Token);
        }

        /// <remarks>
        /// Background thread.
        /// </remarks>
        private void TryDoWork()
        {
            try
            {
                DoWork();
            }
            catch (Exception exception)
            {
                LastException = exception;
            }
            finally
            {
                _finishedQueue.Enqueue(DateTime.Now);
            }
        }

        /// <remarks>
        /// Background thread.
        /// </remarks>
        private void DoWork()
        {
            foreach (var doWorkHandler in _doWorkHandlers.ToList())
            {
                doWorkHandler(this);

                if (IsCancellationRequested)
                {
                    return;
                }
            }

            _successQueue.Enqueue(DateTime.Now);
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DispatchEvents();
        }

        private void OnCancellationRequested()
        {
            _cancellationRequestQueue.Enqueue(DateTime.Now);
        }

        private void DispatchEvents()
        {
            DispatchProgressEvents();
            DispatchCancellationRequestedEvents();
            DispatchCompletionEvents();
        }

        private void DispatchProgressEvents()
        {
            var handlerTypes = _progressHandlers.GetKeys();
            var eventTypes = _progressEventStates.GetKeys();

            foreach (var eventType in eventTypes)
            {
                object state;
                while (_progressEventStates.TryDequeue(eventType, out state))
                {
                    foreach (var handlerType in handlerTypes)
                    {
                        var handlers = _progressHandlers.GetValues(handlerType);
                        foreach (var handler in handlers)
                        {
                            handler(state);
                        }
                    }
                }
            }
        }

        private void DispatchCancellationRequestedEvents()
        {
            DateTime dateTime;
            while (_cancellationRequestQueue.TryDequeue(out dateTime))
            {
                var handlers = _cancellationRequestedHandlers.ToList();
                foreach (var handler in handlers)
                {
                    handler(this);
                }
            }
        }

        private void DispatchCompletionEvents()
        {
            DateTime dateTime;
            if (!_finishedQueue.TryDequeue(out dateTime)) return;

            DispatchCanceledEvents();
            DispatchFailEvents();
            DispatchSuccessEvents();
            DispatchAlwaysEvents();
        }

        private void DispatchCanceledEvents()
        {
            if (!IsCancellationRequested) return;

            var handlers = _canceledHandlers.ToList();
            foreach (var handler in handlers)
            {
                handler(this);
            }
        }

        private void DispatchFailEvents()
        {
            if (LastException == null) return;

            var handlers = _failureHandlers.ToList();
            foreach (var handler in handlers)
            {
                handler(this);
            }
        }

        private void DispatchSuccessEvents()
        {
            if (IsCancellationRequested || LastException != null) return;

            var handlers = _successHandlers.ToList();
            foreach (var handler in handlers)
            {
                handler(this);
            }
        }

        private void DispatchAlwaysEvents()
        {
            var handlers = _alwaysHandlers.ToList();
            foreach (var handler in handlers)
            {
                handler(this);
            }
        }

        #endregion
    }
}
