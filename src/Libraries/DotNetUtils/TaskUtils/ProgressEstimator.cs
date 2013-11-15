using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
