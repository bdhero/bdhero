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
