using System;
using System.Collections.Generic;

namespace DotNetUtils.Extensions
{
    /// <see cref="http://stackoverflow.com/a/2984664/467582"/>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds all elements in the specified enumerable to this collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="newItems">Items to add to this collection</param>
        /// <typeparam name="T"></typeparam>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Inserts the specified <paramref name="items"/> at the beginning of the list (in order) and returns the mutated list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> Prepend<T>(this IList<T> list, params T[] items)
        {
            for (var i = items.Length; i >= 0; --i)
            {
                list.Insert(0, items[i]);
            }
            return list;
        }

        /// <summary>
        /// Iterates over each item in the collection and applies the specified action to it.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="action">Operation to invoke on each item in the collection</param>
        /// <typeparam name="T"></typeparam>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Iterates over each item in the collection and applies the specified action to it.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="action">Operation to invoke on each item in the collection</param>
        /// <typeparam name="T"></typeparam>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            var i = 0;
            foreach (T item in collection)
            {
                action(item, i++);
            }
        }

        /// <summary>
        /// Converts the Enumerable to a MultiValueDictionary using the specified key provider function.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <param name="keyProvider"></param>
        /// <returns></returns>
        public static MultiValueDictionary<TKey, TValue> ToMultiValueDictionary<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keyProvider)
        {
            var dic = new MultiValueDictionary<TKey, TValue>();
            foreach (var value in values)
            {
                dic.Add(keyProvider(value), value);
            }
            return dic;
        }
    }
}