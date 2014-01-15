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

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TimeSpan"/> objects.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns a culture-invariant representation of the TimeSpan in <c>hh:mm:ss</c> format.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns><c>hh:mm:ss</c></returns>
        public static string ToStringShort(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// Returns a culture-invariant representation of the TimeSpan in <c>hh:mm:ss.fff</c> format.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns><c>hh:mm:ss.fff</c></returns>
        public static string ToStringMedium(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fff");
        }

        /// <summary>
        /// Returns a culture-invariant representation of the TimeSpan in <c>hh:mm:ss.fffffff</c> format.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns><c>hh:mm:ss.fffffff</c></returns>
        public static string ToStringLong(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fffffff");
        }
    }
}