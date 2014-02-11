// Copyright 2014 Andrew C. Dvorak
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
using DotNetUtils.Extensions;

namespace DotNetUtils
{
    /// <summary>
    ///     Utility class for assessing performance at a graunlate level
    ///     by timing how long a group of statements takes to execute.
    /// </summary>
    public class DebugTimer
    {
        private readonly IList<Tock> _tocks = new List<Tock>();

        /// <summary>
        ///     Starts the timer.
        /// </summary>
        /// <returns>
        ///     Reference to this DebugTimer object.
        /// </returns>
        public DebugTimer Start()
        {
            Lap("Start");
            return this;
        }

        /// <summary>
        ///     Stops the timer.
        /// </summary>
        /// <returns>
        ///     Reference to this DebugTimer object.
        /// </returns>
        public DebugTimer Stop()
        {
            Lap("Stop");
            return this;
        }

        /// <summary>
        ///     Adds a lap to the timer.  The time between this lap and the previous one will be displayed in the
        ///     <see cref="ToString"/> output along with the lap's <paramref name="name"/> (if specified).
        /// </summary>
        /// <param name="name">
        ///     Optional name to display in the <see cref="ToString"/> output along side the execution time.
        /// </param>
        /// <returns>
        ///     Reference to this DebugTimer object.
        /// </returns>
        public DebugTimer Lap(string name = null)
        {
            _tocks.Add(new Tock(name));
            return this;
        }

        public override string ToString()
        {
            if (!_tocks.Any())
                return "";

            if (_tocks.Count == 1)
                return _tocks.First().ToString();

            var diffs = new List<string>();
            var prevTock = _tocks.First();

            for (var i = 1; i < _tocks.Count; i++)
            {
                var curTock = _tocks[i];
                var diff = curTock.DateTime - prevTock.DateTime;

                if (string.IsNullOrEmpty(curTock.Name))
                    diffs.Add(diff.ToStringLong());
                else
                    diffs.Add(string.Format("{0}: {1}", curTock.Name, diff.ToStringLong()));

                prevTock = curTock;
            }

            return string.Join("  |  ", diffs);
        }

        private class Tock
        {
            public readonly string Name;
            public readonly DateTime DateTime;

            public Tock(string name)
            {
                Name = name;
                DateTime = DateTime.Now;
            }
        }
    }
}
