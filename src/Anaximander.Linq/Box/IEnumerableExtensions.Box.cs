using System;
using System.Collections.Generic;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Groups items in a collection by adding to a box while each object meets some condition compared to the one before, and starting a new box when it does not.
        /// </summary>
        /// <param name="source">A collection of objects, in the desired order.</param>
        /// <param name="sameBoxWhile">A condition to check the next object against. When this returns false, the next object will be the first in a new box.</param>
        /// <returns>A collection of collections, where each inner collection is a box in which each consecutive pair meets the specified condition.</returns>
        public static IBoxedEnumerable<T> BoxWhile<T>(this IEnumerable<T> source, Func<T, bool> sameBoxWhile)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }

            return new BoxedEnumerable<T>(source, sameBoxWhile);
        }

        /// <summary>
        /// Groups items in a collection by adding to a box while each object meets some condition compared to the one before, and starting a new box when it does not.
        /// </summary>
        /// <param name="source">A collection of objects, in the desired order.</param>
        /// <param name="sameBoxWhile">A comparison between an object and the one following it. When this returns false, the second of the two objects will be the first in a new box.</param>
        /// <returns>A collection of collections, where each inner collection is a box in which each consecutive pair meets the specified condition.</returns>
        public static IBoxedEnumerable<T> BoxWhile<T>(this IEnumerable<T> source, Func<T, T, bool> sameBoxWhile)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }

            return new BoxedEnumerable<T>(source, sameBoxWhile);
        }
    }
}