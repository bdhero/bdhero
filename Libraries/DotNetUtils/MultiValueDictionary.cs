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