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
using System.Collections.Generic;
using System.Linq;

namespace DotNetUtils.TaskUtils
{
    internal class ProgressEstimator
    {
        private readonly List<ProgressSampleUnit> _samples;
        private readonly ProgressSampleUnit _lastSample;

        public TimeSpan EstimatedTimeRemaining { get; private set; }

        public ProgressEstimator(IEnumerable<ProgressSampleUnit> samples)
        {
            _samples = samples.ToList();
            _lastSample = _samples.LastOrDefault();

            Calculate();
        }

        private void Calculate()
        {
            EstimatedTimeRemaining = TimeSpan.Zero;

            if (_lastSample == null) { return; }

            var percentageDeltas = new List<double>();
            var durationsInTicks = new List<long>();

            for (var i = 1; i < _samples.Count; i++)
            {
                var prevSample = _samples[i - 1];
                var curSample = _samples[i];

                // From 0.0 to 100.0
                var percentDelta = curSample.PercentComplete - prevSample.PercentComplete;

                percentageDeltas.Add(percentDelta);

                durationsInTicks.Add(curSample.Duration.Ticks);
            }

            var avgPercentageDelta = percentageDeltas.Average();
            var avgDurationInTicks = durationsInTicks.Average();

            if (avgDurationInTicks <= 0)
            {
                // TODO: Is there a better way to avoid dividing by zero?
                avgDurationInTicks = 1;
            }

            // PercentageDelta / Duration.Ticks
            var avgVelocity = avgPercentageDelta / avgDurationInTicks;

            var percentageRemaining = 100.0 - _lastSample.PercentComplete;

            var timeRemaining = TimeSpan.Zero;

            if (avgVelocity > 0)
            {
                var timeRemainingInTicks = percentageRemaining / avgVelocity;
                var secondsRemaining = TimeSpan.FromTicks((long)timeRemainingInTicks).TotalSeconds;
                secondsRemaining = Math.Floor(secondsRemaining);
                secondsRemaining = secondsRemaining < 0 ? 0 : secondsRemaining;
                timeRemaining = TimeSpan.FromSeconds(secondsRemaining);
            }

            _lastSample.EstimatedTimeRemaining = timeRemaining;

            EstimatedTimeRemaining = timeRemaining;
        }
    }
}
