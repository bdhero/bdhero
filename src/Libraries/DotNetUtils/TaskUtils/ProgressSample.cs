// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    ///     TODO: Test pause/resume support
    /// </summary>
    public class ProgressSample
    {
        private const int LowestMinSampleSize = 2;

        private readonly ConcurrentQueue<ProgressSampleUnit> _samples = new ConcurrentQueue<ProgressSampleUnit>();

        private int _minSampleSize = 5;
        private int _maxSampleSize = 10;

        private ProgressSampleState _state;

        /// <summary>
        ///     Gets or sets the minimum number of samples required to generate a meaningful "time remaining" estimate.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is set to a value less than 2.</exception>
        public int MinSampleSize
        {
            get { return _minSampleSize; }
            set
            {
                if (value < LowestMinSampleSize)
                {
                    throw new ArgumentOutOfRangeException("MinSampleSize cannot be less than " + LowestMinSampleSize);
                }
                _minSampleSize = value;
            }
        }

        /// <summary>
        ///     Gets or sets the maximum number of samples to consider when calculating "time remaining" estimates.
        ///     Samples older than this number will be discarded.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MaxSampleSize"/> is set to a value less than 2.</exception>
        public int MaxSampleSize
        {
            get { return _maxSampleSize; }
            set
            {
                if (value < LowestMinSampleSize)
                {
                    throw new ArgumentOutOfRangeException("MaxSampleSize cannot be less than " + LowestMinSampleSize);
                }
                _maxSampleSize = value;
            }
        }

        /// <summary>
        ///     Gets the estimated time remaining.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is greater than <see cref="MaxSampleSize"/>.</exception>
        public TimeSpan EstimatedTimeRemaining
        {
            get { return Calculate(); }
        }

        private DateTime? LastSampleTime
        {
            get
            {
                var lastSample = _samples.ToArray().LastOrDefault();
                return lastSample != null ? new DateTime?(lastSample.DateSampled) : null;
            }
        }

        private double LastSamplePercent
        {
            get
            {
                var lastSample = _samples.ToArray().LastOrDefault();
                return lastSample != null ? lastSample.PercentComplete : 0.0;
            }
        }

        /// <summary>
        ///     Adds a sample to the queue and recalculates the estimated time remaining.
        /// </summary>
        /// <param name="percentComplete">From 0.0 to 100.0.</param>
        public void Add(double percentComplete)
        {
            if (_state == ProgressSampleState.Paused)
            {
                Resume(percentComplete);
            }

            _state = ProgressSampleState.Running;

            var duration = TimeSpan.Zero;
            var lastSampleTime = LastSampleTime;

            if (lastSampleTime.HasValue)
            {
                duration = DateTime.Now - lastSampleTime.Value;
            }

            Enqueue(percentComplete, duration);
        }

        private void Enqueue(double percentComplete, TimeSpan duration)
        {
            if (duration < TimeSpan.FromSeconds(0.1) && _samples.Any())
            {
                return;
            }

            _samples.Enqueue(new ProgressSampleUnit
                             {
                                 DateSampled = DateTime.Now,
                                 Duration = duration,
                                 PercentComplete = percentComplete
                             });
        }

        public void Pause()
        {
            if (_state != ProgressSampleState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to Pause: must be in {0} state, but instead found {1} state",
                                  ProgressSampleState.Running, _state));
            }

            Add(LastSamplePercent);

            _state = ProgressSampleState.Paused;
        }

        public void Resume()
        {
            Resume(LastSamplePercent);
        }

        public void Resume(double percentComplete)
        {
            if (_state != ProgressSampleState.Paused)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to Resume: must be in {0} state, but instead found {1} state",
                                  ProgressSampleState.Paused, _state));
            }

            Enqueue(percentComplete, TimeSpan.Zero);

            _state = ProgressSampleState.Running;
        }

        public void Stop()
        {
            _state = ProgressSampleState.Stopped;
        }

        public void Reset()
        {
            _state = ProgressSampleState.Stopped;

            ProgressSampleUnit result;
            while (_samples.TryDequeue(out result))
            {
            }
        }

        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is greater than <see cref="MaxSampleSize"/>.</exception>
        private bool CanCalculate
        {
            get
            {
                if (MinSampleSize > MaxSampleSize)
                {
                    throw new ArgumentOutOfRangeException("MaxSampleSize must be greater than or equal to MinSampleSize");
                }

                if (_samples.Count < MinSampleSize)
                {
                    return false;
                }

                if (!LastSampleTime.HasValue)
                {
                    return false;
                }

                return true;
            }
        }

        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is greater than <see cref="MaxSampleSize"/>.</exception>
        private TimeSpan Calculate()
        {
            if (!CanCalculate)
            {
                return TimeSpan.Zero;
            }

            var samples = new Queue<ProgressSampleUnit>(_samples);
            while (samples.Count > MaxSampleSize)
            {
                samples.Dequeue();
            }

            if (samples.Count < MinSampleSize)
            {
                return TimeSpan.Zero;
            }

            return new ProgressEstimator(samples).EstimatedTimeRemaining;
        }
    }
}
