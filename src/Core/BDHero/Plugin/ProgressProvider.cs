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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using DotNetUtils.TaskUtils;

namespace BDHero.Plugin
{
    /// <summary>
    /// Provides information about the overall state, status, and progress of a plugin.
    /// </summary>
    public class ProgressProvider
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constants

        /// <summary>
        /// Default timer interval between updates in milliseconds.
        /// </summary>
        public const double DefaultIntervalMs = 250;

        /// <summary>
        /// Shortest amount of time allowed between progress updates when the underlying Plugin
        /// is generating extremely frequent updates.
        /// </summary>
        public const double MinIntervalMs = 100;

        #endregion

        #region Public getters and setters

        /// <summary>
        /// The plugin that reports progress to this provider.
        /// </summary>
        public IPlugin Plugin;

        /// <summary>
        /// Gets or sets the percentage of the provider's work that has been completed, from <c>0.0</c> to <c>100.0</c>.
        /// </summary>
        public double PercentComplete
        {
            get { return _percentComplete; }
            set
            {
                _percentComplete = value;
                _progressSample.Add(_percentComplete);
            }
        }

        /// <summary>
        /// DO NOT MODIFY DIRECTLY!  Used internally by <see cref="PercentComplete"/>.
        /// </summary>
        private double _percentComplete;

        #endregion

        #region Public getters

        /// <summary>
        /// Gets the current state of the process.
        /// </summary>
        public ProgressProviderState State { get; protected set; }

        /// <summary>
        /// Gets a short human-readable description of what the provider is currently doing.  E.G., "Parsing 00800.MPLS", "Querying TMDb", "Muxing to MKV".
        /// </summary>
        public string ShortStatus { get; private set; }

        /// <summary>
        /// Gets a detailed human-readable description of what the provider is currently doing.  E.G., "Parsing 00800.MPLS", "Querying TMDb", "Muxing to MKV".
        /// </summary>
        public string LongStatus { get; private set; }

        /// <summary>
        /// Gest the total amount of time spent actively running (i.e., not paused).
        /// This property may be accessed at any time and in any state.
        /// </summary>
        public TimeSpan RunTime { get { return _stopwatch.Elapsed; } }

        /// <summary>
        /// Gets the estimated amount of time remaining before the process completes.
        /// </summary>
        public TimeSpan TimeRemaining { get; protected set; }

        /// <summary>
        /// Gets the last exception that was thrown or <c>null</c> if no exceptions have been thrown.
        /// </summary>
        public Exception Exception { get; protected set; }

        #endregion

        #region Public events

        /// <summary>
        /// Invoked approximately once every second and whenever the <see cref="State"/> or <see cref="PercentComplete"/> changes.
        /// </summary>
        public event ProgressUpdateHandler Updated;

        /// <summary>
        /// Invoked when the process starts.
        /// </summary>
        public event ProgressUpdateHandler Started;

        /// <summary>
        /// Invoked when the user pauses the process.
        /// </summary>
        public event ProgressUpdateHandler Paused;

        /// <summary>
        /// Invoked when the user resumes the process.
        /// </summary>
        public event ProgressUpdateHandler Resumed;

        /// <summary>
        /// Invoked when the user terminates the process.
        /// </summary>
        public event ProgressUpdateHandler Canceled;

        /// <summary>
        /// Invoked when the process terminates due to an error.
        /// </summary>
        public event ProgressUpdateHandler Errored;

        /// <summary>
        /// Invoked only when the process completes successfully with no errors.
        /// </summary>
        public event ProgressUpdateHandler Successful;

        /// <summary>
        /// Invoked when the process finishes with any status (canceled, errored, or successful).
        /// </summary>
        public event ProgressUpdateHandler Completed;

        #endregion

        #region Private members

        /// <summary>
        /// Synchronization primitive for multi-threading.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Last time the <see cref="_timer"/> interval elapsed.  Used to calculate <see cref="RunTime"/>.
        /// </summary>
        private DateTime _lastTick;

        private TimeSpan _lastEstimate;

        /// <summary>
        /// The value of <see cref="_lastTick"/> the last time <see cref="CalculateTimeRemaining"/> was called.
        /// </summary>
        private DateTime _lastCalculationTick;

        /// <summary>
        /// Calculates time remaining approximately once per second.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Used to calculate elapsed time.  Allows for pause/resume.
        /// </summary>
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// The last time <see cref="Updated"/> event handlers were notified of a progress update.
        /// </summary>
        private DateTime _lastNotified;

        /// <summary>
        /// The previous state of the provider
        /// </summary>
        private ProgressProviderState _lastState;

        private readonly ProgressSample _progressSample = new ProgressSample();

        #endregion

        #region Constructor

        public ProgressProvider()
        {
            _timer = new Timer(DefaultIntervalMs) { AutoReset = true };
            _timer.Elapsed += Tick;

            Reset();
        }

        #endregion

        #region Logging

        private enum MethodEntry
        {
            Entering, Exiting
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LogMethod(MethodBase method, MethodEntry entryType)
        {
#if DEBUG_CONCURRENCY
            var @params = method.GetParameters();
            var strMethod = string.Format("{0}({1})", method.Name, string.Join(", ", @params.Select(info => info.ParameterType.Name + " " + info.Name)));
            var name = Plugin != null ? Plugin.Name : "??? UNINITIALIZED ???";
            Logger.DebugFormat("ProgressProvider for \"{0}\" plugin - {1} {2} method", name, entryType, strMethod);
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LogMethodEntry()
        {
            var method = new StackFrame(1, true).GetMethod();
            LogMethod(method, MethodEntry.Entering);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LogMethodExit()
        {
            var method = new StackFrame(1, true).GetMethod();
            LogMethod(method, MethodEntry.Exiting);
        }

        #endregion

        #region Update notification

        private void NotifyObservers()
        {
            if (!CanUpdate) return;

            if (Updated != null)
                Updated(this);

            _lastNotified = DateTime.Now;
            _lastState = State;
        }

        private bool CanUpdate
        {
            get { return HasStateChanged || IsNearCompletion || HasMinIntervalElapsed; }
        }

        private bool HasStateChanged
        {
            get { return State != _lastState; }
        }

        private bool IsNearCompletion
        {
            get { return PercentComplete > 95; }
        }

        private bool HasMinIntervalElapsed
        {
            get { return (DateTime.Now - _lastNotified).TotalMilliseconds >= MinIntervalMs; }
        }

        #endregion

        #region Timer methods

        /// <summary>
        /// Called by <see cref="_timer"/> whenever its interval elapses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void Tick(object sender = null, ElapsedEventArgs elapsedEventArgs = null)
        {
            LogMethodEntry();

            CalculateTimeRemaining();
            NotifyObservers();
            SetLastTick();

            LogMethodExit();
        }

        private void SetLastTick()
        {
            lock (_lock)
            {
                _lastTick = DateTime.Now;
            }
        }

        // TODO: Allow caller to choose estimation algorithm at runtime
        // depending on whether the task is likely to have stable progress
        // or change frequently.
        private void CalculateTimeRemaining()
        {
            lock (_lock)
            {
                var lastTick = _lastTick;

                if (_lastCalculationTick == lastTick)
                    return;

                LogMethodEntry();

                var ticks = RunTime.Ticks;

                // Thread safety :-)
                var percentComplete = PercentComplete;

                // 0.0 to 1.0
                var pct = percentComplete / 100;

                // Previously calculated estimate from the last tick
                var oldEstimate = TimeRemaining;

                // Fresh estimate (might be jerky)
                var newEstimate = TimeSpan.Zero;

                // The final estimate value that will actually be used
                var finalEstimate = oldEstimate;

                // Make sure progress percentage is within legal bounds (0%, 100%)
                if (pct > 0 && pct < 1)
                    newEstimate = new TimeSpan((long)(ticks / pct) - ticks);

                // Make sure the user gets fresh calculations when the process is first started and when it's nearly finished.
                var criticalThreshold = TimeSpan.FromMinutes(1.25);
                var isCriticalPeriod = percentComplete < .5 || percentComplete > 95 || oldEstimate < criticalThreshold || newEstimate < criticalThreshold;

                // If the new estimate differs from the previous estimate by more than 2 minutes, use the new estimate.
                var changeThreshold = pct < 0.2 ? TimeSpan.FromMinutes(2) : TimeSpan.FromSeconds(20);
                var isLargeChange = Math.Abs(newEstimate.TotalSeconds - oldEstimate.TotalSeconds) > changeThreshold.TotalSeconds;

                // If the previous estimate was less than 1 second remaining, use the new estimate.
                var lowPreviousEstimate = oldEstimate.TotalMilliseconds < 1000;

                // Use the new estimate
                if (isCriticalPeriod || isLargeChange || lowPreviousEstimate)
                {
                    finalEstimate = newEstimate;
                }
                // Decrement the previous estimate by roughly 1 second
                else
                {
                    finalEstimate -= (DateTime.Now - lastTick);
                }

                // If the estimate dips below 0, set it to 30 seconds
                if (finalEstimate.TotalMilliseconds < 0)
                    finalEstimate = TimeSpan.FromSeconds(30);

                TimeRemaining = finalEstimate;

                _lastCalculationTick = lastTick;

                LogMethodExit();
            }
        }

        private void CalculateTimeRemainingV2()
        {
            lock (_lock)
            {
                var lastTick = _lastTick;

                if (_lastCalculationTick == lastTick)
                    return;

                LogMethodEntry();

                TimeRemaining = _progressSample.EstimatedTimeRemaining;

                _lastCalculationTick = lastTick;

                LogMethodExit();
            }
        }

        private void CalculateTimeRemainingV3()
        {
            lock (_lock)
            {
                var lastTick = _lastTick;

                if (_lastCalculationTick == lastTick)
                    return;

                LogMethodEntry();

                // Thread safety :-)
                var percentComplete = PercentComplete;

                // 0.0 to 1.0
                var pct = percentComplete / 100;

                // Previously calculated estimate from the last tick
                var oldEstimate = TimeRemaining;

                // Fresh estimate (might be jerky)
                var newEstimate = TimeSpan.Zero;

                // The final estimate value that will actually be used
                var finalEstimate = oldEstimate;

                // Make sure progress percentage is within legal bounds (0%, 100%)
                if (pct > 0 && pct < 1)
                    newEstimate = _progressSample.EstimatedTimeRemaining;

                // Make sure the user gets fresh calculations when the process is first started and when it's nearly finished.
                var criticalThreshold = TimeSpan.FromSeconds(10);
                var isCriticalPeriod = percentComplete < 0.5 || percentComplete > 95 || oldEstimate < criticalThreshold || newEstimate < criticalThreshold;

                // If the new estimate differs from the previous estimate by more than 2 minutes, use the new estimate.
                var changeThreshold = pct < 0.2 ? TimeSpan.FromMinutes(2) : TimeSpan.FromSeconds(20);
                var isLargeChange = Math.Abs(newEstimate.TotalSeconds - oldEstimate.TotalSeconds) > changeThreshold.TotalSeconds;

                // If the previous estimate was less than 1 second remaining, use the new estimate.
                var lowPreviousEstimate = oldEstimate < TimeSpan.FromSeconds(1);

                // Use the new estimate
                if (isCriticalPeriod || isLargeChange || lowPreviousEstimate)
                {
                    finalEstimate = newEstimate;
                }
                // Decrement the previous estimate by roughly 1 second
                else
                {
                    finalEstimate -= (DateTime.Now - lastTick);
                }

                // If the estimate dips below 0, set it to 30 seconds
                if (finalEstimate.TotalMilliseconds < 0)
                    finalEstimate = TimeSpan.FromSeconds(30);

                TimeRemaining = finalEstimate;

                if (finalEstimate == TimeSpan.Zero && percentComplete > 60)
                {
//                    Debugger.Break();
                }

                _lastCalculationTick = lastTick;

                LogMethodExit();
            }
        }

        private void CalculateTimeRemainingV4()
        {
            lock (_lock)
            {
                var lastTick = _lastTick;

                if (_lastCalculationTick == lastTick)
                    return;

                LogMethodEntry();

                var oldEstimate = _lastEstimate;
                var newEstimate = _progressSample.EstimatedTimeRemaining;

                if (oldEstimate == newEstimate)
                {
                    TimeRemaining -= (DateTime.Now - lastTick);
                }
                else
                {
                    TimeRemaining = newEstimate;
                }

                _lastEstimate = newEstimate;
                _lastCalculationTick = lastTick;

                LogMethodExit();
            }
        }

        #endregion

        #region State change methods (start, stop, reset, etc.)

        /// <summary>
        /// Resets the provider's state to its initial values.
        /// </summary>
        public void Reset()
        {
            LogMethodEntry();

            if (State == ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to reset: object is in {0} state", State));
            }

            _timer.Stop();
            _stopwatch.Reset();
            _progressSample.Reset();

            State = ProgressProviderState.Ready;
            TimeRemaining = TimeSpan.Zero;
            PercentComplete = 0.0;
            Exception = null;

            LogMethodExit();
        }

        public void Start()
        {
            LogMethodEntry();

            if (State != ProgressProviderState.Ready)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to start: must be in {0}, but object is in {1} state",
                                  ProgressProviderState.Ready, State));
            }

            SetLastTick();

            State = ProgressProviderState.Running;
            Tick();

            _timer.Start();
            _stopwatch.Start();

            if (Started != null)
                Started(this);

            NotifyObservers();

            LogMethodExit();
        }

        public void Resume()
        {
            LogMethodEntry();

            if (State != ProgressProviderState.Paused)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to resume: must be in {0} state, but object is in {1} state",
                                  ProgressProviderState.Paused, State));
            }

            SetLastTick();

            State = ProgressProviderState.Running;
            Tick();

            _timer.Start();
            _stopwatch.Start();
            _progressSample.Resume();

            if (Resumed != null)
                Resumed(this);

            NotifyObservers();

            LogMethodExit();
        }

        public void Pause()
        {
            LogMethodEntry();

            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to pause: must be in {0} state, but object is in {1} state",
                                  ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();
            _progressSample.Pause();

            State = ProgressProviderState.Paused;
            Tick();

            if (Paused != null)
                Paused(this);

            NotifyObservers();

            LogMethodExit();
        }

        public void Cancel()
        {
            LogMethodEntry();

            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to cancel: must be in {0} state, but object is in {1} state",
                                  ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();
            _progressSample.Stop();

            State = ProgressProviderState.Canceled;
            Tick();

            if (Canceled != null)
                Canceled(this);

            if (Completed != null)
                Completed(this);

            NotifyObservers();

            LogMethodExit();
        }

        public void Error(Exception exception)
        {
            LogMethodEntry();

            if (State != ProgressProviderState.Ready && State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to switch to {0} state: must be in {1} or {2} state, but object is in {3} state",
                                  ProgressProviderState.Error, ProgressProviderState.Ready,
                                  ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();
            _progressSample.Stop();

            State = ProgressProviderState.Error;
            Exception = exception;
            Tick();

            if (Errored != null)
                Errored(this);

            if (Completed != null)
                Completed(this);

            NotifyObservers();

            LogMethodExit();
        }

        public void Succeed()
        {
            LogMethodEntry();

            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to switch to {0} state: must be in {1} state, but object is in {2} state",
                                  ProgressProviderState.Success, ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();
            _progressSample.Stop();

            State = ProgressProviderState.Success;
            PercentComplete = 100.0;
            TimeRemaining = TimeSpan.Zero;

            if (Successful != null)
                Successful(this);

            if (Completed != null)
                Completed(this);

            NotifyObservers();

            LogMethodExit();
        }

        #endregion

        /// <summary>
        /// Called by <see cref="IPluginHost"/> whenever an <see cref="IPlugin"/> reports a progress update.
        /// </summary>
        /// <param name="percentComplete">0.0 to 100.0</param>
        /// <param name="shortStatus">Short description of what the plugin is currently doing</param>
        /// <param name="longStatus">Detailed description of what the plugin is currently doing</param>
        public void Update(double percentComplete, string shortStatus, string longStatus = null)
        {
            LogMethodEntry();

            PercentComplete = percentComplete;
            ShortStatus = shortStatus;
            LongStatus = longStatus ?? shortStatus;

            NotifyObservers();

            LogMethodExit();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TimeRemaining.GetHashCode();
                hashCode = (hashCode*397) ^ (int) State;
                hashCode = (hashCode*397) ^ (LongStatus != null ? LongStatus.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ShortStatus != null ? ShortStatus.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ PercentComplete.GetHashCode();
                return hashCode;
            }
        }
    }

    public delegate void ProgressUpdateHandler(ProgressProvider progressProvider);

    /// <summary>
    /// Describes the state of a <see cref="ProgressProvider"/>.
    /// States are mutually exclusive; a <see cref="ProgressProvider"/> can only have one state at a time.
    /// </summary>
    public enum ProgressProviderState
    {
        /// <summary>
        /// Process has not yet started.
        /// </summary>
        Ready,

        /// <summary>
        /// Process has started and is currently running.
        /// </summary>
        Running,

        /// <summary>
        /// Process has started but is paused (suspended).
        /// </summary>
        Paused,

        /// <summary>
        /// Process was canceled manually by the user.
        /// </summary>
        Canceled,

        /// <summary>
        /// Process terminated due to an unrecoverable error.
        /// </summary>
        Error,

        /// <summary>
        /// Process finished running and completed successfully.
        /// </summary>
        Success
    }
}
