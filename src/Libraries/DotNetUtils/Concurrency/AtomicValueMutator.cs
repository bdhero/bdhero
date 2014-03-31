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
    ///     Mutates the value of an <see cref="AtomicValue{T}"/> object by inspecting its current value
    ///     and returning a new value based upon it.
    /// </summary>
    /// <param name="currentValue"></param>
    /// <typeparam name="T"></typeparam>
    public delegate T AtomicValueMutator<T>(T currentValue);
}