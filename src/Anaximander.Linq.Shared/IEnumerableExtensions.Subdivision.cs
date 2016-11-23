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

            using (var enumerator = source.GetEnumerator())
            {
                for (int i = 0; i < windowSize; i++)
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