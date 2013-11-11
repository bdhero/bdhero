using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetUtils.TaskUtils
{
    internal class ProgressSampleUnit
    {
        /// <summary>
        /// The date and time the unit was sampled.
        /// </summary>
        public DateTime DateSampled;

        /// <summary>
        /// Amount of time that elapsed between when this unit was sampled and when the previous unit was sampled.
        /// </summary>
        public TimeSpan Duration;

        /// <summary>
        /// From 0.0 to 100.0.
        /// </summary>
        public double PercentComplete;

        /// <summary>
        /// The estimated amount of time remaining, calculated when this unit was the most recently sampled unit.
        /// </summary>
        public TimeSpan? EstimatedTimeRemaining;
    }
}
