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
using System.Threading;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    /// Provides atomic read/write access to a single value of type <paramtyperef name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// This class manages access to a resource, allowing multiple threads for reading or exclusive access for writing.
    /// A write operation will block (up to <see cref="MaxWait"/>) until all prior read operations have finished.
    /// Similarly, read operations will block until a write operation has finished.
    /// </remarks>
    public class AtomicValue<T>
    {
        /// <summary>
        /// Gets or sets the maximum amount of time to wait for a read or write lock before aborting the operation.
        /// The default is 1 second.  To abort immediately if a read/write lock is not available,
        /// set to <c>0</c>.  To wait indefinitely for a lock, set to <c>-1</c> milliseconds (<see cref="Timeout.Infinite"/>).
        /// </summary>
        public TimeSpan MaxWait = TimeSpan.FromSeconds(1);

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private T _value;

        /// <summary>
        /// Constructs a new <see cref="AtomicValue{T}"/> object and initializes <see cref="Value"/>
        /// to the default value for type <typeparamref name="T"/>.
        /// </summary>
        public AtomicValue()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="AtomicValue{T}"/> object with the given <paramref name="initialValue"/>.
        /// </summary>
        /// <param name="initialValue">Initial value of <see cref="Value"/>.</param>
        public AtomicValue(T initialValue)
        {
            _value = initialValue;
        }

        /// <summary>
        /// Gets or sets the value atomically.  Waits for at most <see cref="MaxWait"/> before giving up.
        /// </summary>
        public T Value
        {
            get
            {
                if (!_lock.TryEnterReadLock(MaxWait))
                {
                    return default(T);
                }

                try
                {
                    return _value;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }

            set
            {
                if (!_lock.TryEnterWriteLock(MaxWait))
                {
                    return;
                }

                try
                {
                    _value = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }
    }
}