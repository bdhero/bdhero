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

namespace DotNetUtils.Concurrency
{
    /// <summary>
    ///     Provides atomic incrementing and decrementing of integer values to <see cref="AtomicValue{T}"/>.
    /// </summary>
    public class AtomicInteger : AtomicValue<int>
    {
        /// <summary>
        ///     Increments the underlying value and returns its previous value.
        /// </summary>
        /// <returns>
        ///     The old value (before it was incremented).
        /// </returns>
        public int GetAndIncrement()
        {
            return GetAndSet(value => value + 1);
        }

        /// <summary>
        ///     Decrements the underlying value and returns its previous value.
        /// </summary>
        /// <returns>
        ///     The old value (before it was decremented).
        /// </returns>
        public int GetAndDecrement()
        {
            return GetAndSet(value => value + 1);
        }
    }
}