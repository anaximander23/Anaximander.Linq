using System;
using System.Collections.Generic;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Presents a moving window over a source collection, by returning a collection of sub-collections where each sub-collection is shifted one element along from the previous.
        /// </summary>
        /// <param name="source">A collection of items.</param>
        /// <param name="windowSize">The number of items to include in each sub-collection.</param>
        /// <returns>A collection of sub-collections taken in sequence from the source collection, with each sub-collection shifted one element along from the previous.</returns>
        /// <example>new[] { 1, 2, 3, 4, 5 }.Window(3); would return {{ 1, 2, 3 }, { 2, 3, 4 }, { 3, 4, 5 }} </example>
        public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int windowSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }

            var buffer = new List<T>();

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                for (var i = 0; i < windowSize; i++)
                {
                    if (enumerator.MoveNext())
                    {
                        buffer.Add(enumerator.Current);
                    }
                }

                yield return buffer;

                while (enumerator.MoveNext())
                {
                    buffer.RemoveAt(0);
                    buffer.Add(enumerator.Current);

                    if (buffer.Count == windowSize)
                    {
                        yield return buffer;
                    }
                }
            }
        }
    }
}