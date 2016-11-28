using System.Collections.Generic;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Returns a shuffling enumerable, which will enumerate its contents in a random order each time it is enumerated.
        /// </summary>
        public static IShufflingEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return new ShufflingEnumerable<T>(source);
        }
    }
}