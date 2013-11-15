using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotNetUtils.Annotations;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    /// Thread-safe multi-value dictionary.  Values are stored in FIFO queues.
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public class ConcurrentMultiValueDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, Queue<TValue>> _dictionary = new Dictionary<TKey, Queue<TValue>>();

        public bool TryDequeue(TKey key, out TValue value)
        {
            lock (_dictionary)
            {
                if (!IsQueuedNonLocking(key))
                {
                    value = default(TValue);
                    return false;
                }

                value = _dictionary[key].Dequeue();
                return true;
            }
        }

        public void Enqueue(TKey key, TValue value)
        {
            lock (_dictionary)
            {
                if (!_dictionary.ContainsKey(key))
                {
                    _dictionary[key] = new Queue<TValue>();
                }
                _dictionary[key].Enqueue(value);
            }
        }

        /// <summary>
        /// Gets the values for the specified <paramref name="key"/>.  If the dictionary does not contain
        /// <paramref name="key"/>, an empty collection will be returned.  This method never returns <c>null</c>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>
        /// Values for the specified <paramref name="key"/>, or an empty collection if <paramref name="key"/>
        /// does not exist in the collection.
        /// </returns>
        /// <remarks>
        /// The returned value is a <em>copy</em> of the underlying queue for the given <paramref name="key"/>.
        /// Mutating the returned list will not affect the dictionary's queue.
        /// </remarks>
        [NotNull]
        public List<TValue> GetValues(TKey key)
        {
            var values = new List<TValue>();

            lock (_dictionary)
            {
                if (IsQueuedNonLocking(key))
                {
                    values = _dictionary[key].ToList();
                }
            }

            return values;
        }

        /// <summary>
        /// Gets all keys that have at least one value in their queue.
        /// </summary>
        /// <returns>List of keys whose queues are non-empty (i.e., have at least one value).</returns>
        public List<TKey> GetKeys()
        {
            lock (_dictionary)
            {
                return _dictionary.Keys.Where(IsQueuedNonLocking).ToList();
            }
        }

        private bool IsQueuedNonLocking(TKey key)
        {
            return _dictionary.ContainsKey(key) && _dictionary[key].Any();
        }
    }
}