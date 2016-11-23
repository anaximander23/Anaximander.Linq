using System;
using System.Collections.Generic;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int windowSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }

            var buffer = new List<T>();

            foreach (T item in source)
            {
                buffer.Add(item);

                if (buffer.Count == windowSize)
                {
                    yield return buffer;
                    buffer.RemoveAt(0);
                }
            }
        }
    }
}