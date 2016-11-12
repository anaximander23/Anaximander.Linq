using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        public static IOrderedEnumerable<T> OrderToMatch<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IEnumerable<T> ordering)
        {
            var orderArray = ordering.ToArray();
            return source.OrderBy<T, int>(x => Array.IndexOf(orderArray, selector(x)));
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
            }
            return source;
        }

        public static IEnumerable<T> ForEachLazy<T>(this IEnumerable<T> source, Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
                yield return item;
            }
        }
    }
}