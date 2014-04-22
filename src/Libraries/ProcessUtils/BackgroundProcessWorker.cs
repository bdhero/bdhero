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
using System.Timers;
using OSUtils.JobObjects;

namespace ProcessUtils
{
    /// <summary>
    /// Threaded version of <see cref="NonInteractiveProcess"/>.  Allows a process to run in the background
    /// on a separate thread while reporting its status and progress information to the UI.
    /// </summary>
    public class BackgroundProcessWorker : NonInteractiveProcess
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Timer _timer = new Timer(1000);

        private readonly BackgroundWorker _worker = new BackgroundWorker
                                                        {
                                                            WorkerReportsProgress = true,
                                                            WorkerSupportsCancellation = true
                                                        };

        /// <summary>
        /// From 0.0 to 100.0.
        /// </summary>
        protected double _progress;

        private DateTime _lastTick = DateTime.Now;

        private TimeSpan _timeRemaining = TimeSpan.Zero;

        /// <summary>
        /// Accessed ONLY from UI thread.
        /// </summary>
        private ProgressState _uiProgressState = new ProgressState();

        /// <summary>
        /// Invoked approximately once every second with the process's current state, progress percentage, and time estimates.
        /// </summary>
        public event BackgroundProgressHandler ProgressUpdated;

        /// <summary>
        ///     Constructs a new <see cref="BackgroundProcessWorker"/> object that uses the given
        ///     <paramref name="jobObjectManager"/> to ensure that child processes are terminated
        ///     if the parent process exits prematurely.
        /// </summary>
        /// <param name="jobObjectManager"></param>
        public BackgroundProcessWorker(IJobObjectManager jobObjectManager)
            : base(jobObjectManager)
        {
            PropertyChanged += OnPropertyChanged;
            _timer.Elapsed += TimerOnTick;
            _worker.DoWork += (sender, args) => Start();
            _worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs args)
                {
                    if (args.Error != null)
                    {
                        Logger.Error("Error occurred while running NonInteractiveProcess in BackgroundWorker", args.Error);
                    }
                };
        }

        public BackgroundProcessWorker StartAsync()
        {
            _worker.RunWorkerAsync();
            return this;
        }

        /// <summary>
        /// Runs in UI and/or BackgroundWorker threads.
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (State == NonInteractiveProcessState.Running)
                _timer.Start();
            else
                _timer.Elapsed += StopTimer;
        }

        private void StopTimer(object sender, ElapsedEventArgs args)
        {
            _timer.Stop();
            _timer.Elapsed -= StopTimer;
        }

        /// <summary>
        /// Runs in UI thread.
        /// </summary>
        private void TimerOnTick(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CalculateTimeRemaining();
            if (ProgressUpdated != null)
                ProgressUpdated(_uiProgressState);
            _lastTick = DateTime.Now;
            Logger.Debug("Timer tick");
        }
        
        /// <summary>
        /// Runs in UI thread.
        /// </summary>
        private void CalculateTimeRemaining()
        {
            // Thread safety :-)
            var progress = _progress;

            // 0.0 to 1.0
            var pct = progress / 100;

            // Previously calculated estimate from the last tick
            var oldEstimate = _timeRemaining;

            // Fresh estimate (might be jerky)
            var newEstimate = TimeSpan.Zero;

            // The final estimate value that will actually be used
            var finalEstimate = oldEstimate;

            // Make sure progress percentage is within legal bounds (0%, 100%)
            if (pct > 0 && pct < 1)
                newEstimate = new TimeSpan((long)(RunTime.Ticks / pct) - RunTime.Ticks);

            // Make sure the user gets fresh calculations when the process is first started and when it's nearly finished.
            var criticalThreshold = TimeSpan.FromMinutes(1.25);
            var isCriticalPeriod = progress < .5 || progress > 95 || oldEstimate < criticalThreshold || newEstimate < criticalThreshold;

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
                finalEstimate -= (DateTime.Now - _lastTick);
            }

            // If the estimate dips below 0, set it to 30 seconds
            if (finalEstimate.TotalMilliseconds < 0)
                finalEstimate = TimeSpan.FromSeconds(30);

            // Truncate (remove) milliseconds from estimate
            finalEstimate = TimeSpan.FromSeconds(Math.Floor(finalEstimate.TotalSeconds));

            _timeRemaining = finalEstimate;
            _uiProgressState = new ProgressState
                                   {
                                       ProcessState = State,
                                       PercentComplete = progress,
                                       TimeElapsed = RunTime,
                                       TimeRemaining = finalEstimate
                                   };
        }
    }

    public class ProgressState
    {
        /// <summary>
        /// Internal state of the process.
        /// </summary>
        public NonInteractiveProcessState ProcessState = NonInteractiveProcessState.Ready;

        /// <summary>
        /// From 0.0 to 100.0.
        /// </summary>
        public double PercentComplete;

        /// <summary>
        /// Total amount of time the process has been running (excluding any time in which it was paused).
        /// </summary>
        public TimeSpan TimeElapsed = TimeSpan.Zero;

        /// <summary>
        /// Estimated length of time remaining before the process completes.
        /// </summary>
        public TimeSpan TimeRemaining = TimeSpan.Zero;

        public override string ToString()
        {
            const string timeSpanFormat = @"hh\:mm\:ss";
            return string.Format("{0}: {1}% complete - {2} elapsed, {3} remaining",
                                 ProcessState,
                                 PercentComplete.ToString("0.000"),
                                 TimeElapsed.ToString(timeSpanFormat),
                                 TimeRemaining.ToString(timeSpanFormat));
        }
    }

    public delegate void BackgroundProgressHandler(ProgressState progressState);
}
