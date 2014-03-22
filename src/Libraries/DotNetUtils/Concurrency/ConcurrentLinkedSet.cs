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
using System.Threading;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    /// Thread-safe set that maintains the order in which items are added (like a linked list)
    /// and provides fast reads and only blocks during writes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentLinkedSet<T>
    {
        /// <summary>
        /// Gets or sets the maximum amount of time to wait for a read or write lock before aborting the operation.
        /// The default is 5 seconds.  To abort immediately if a read/write lock is not available,
        /// set to <c>0</c>.  To wait indefinitely for a lock, set to <c>-1</c> milliseconds (<see cref="Timeout.Infinite"/>).
        /// </summary>
        public TimeSpan MaxWait = TimeSpan.FromSeconds(5);

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly List<T> _list = new List<T>();
        private readonly ISet<T> _set = new HashSet<T>();

        /// <summary>
        /// Adds the given <paramref name="value"/> to the set if it is not already present.
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            if (!_lock.TryEnterWriteLock(MaxWait)) return;

            try
            {
                if (_set.Contains(value)) return;

                _list.Add(value);
                _set.Add(value);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public List<T> ToList()
        {
            if (!_lock.TryEnterReadLock(MaxWait)) return new List<T>();

            try
            {
                return _list.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
