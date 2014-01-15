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

using System.Collections.Generic;

namespace DotNetUtils
{
    /// <summary>
    /// Dictionary that supports keys with a list of values (one-to-many).
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, IList<TValue>>
    {
        /// <summary>
        /// Adds the specified value to the list at the specified key.
        /// If the key is not already present in the dictionary, it is added automatically.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                this[key] = new List<TValue>();
            this[key].Add(value);
        }
    }
}