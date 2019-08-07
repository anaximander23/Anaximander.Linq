using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Partitions a collection into a given number of equal-sized sub-collections.
        /// </summary>
        /// <param name="source">A collection of items.</param>
        /// <param name="numberOfSlices">The number of equal-sized sub-collections to return</param>
        /// <returns>An ISlicedEnumerable, exposing the Slices of equal size, and a (possibly empty) Remainder collection when the source collection could not be evenly divided.</returns>
        public static ISlicedEnumerable<T> Slices<T>(this IEnumerable<T> source, int numberOfSlices)
        {
            IEnumerable<T> enumeratedSource = source.ToList();
            int sourceLength = enumeratedSource.Count();

            if (numberOfSlices < 1)
            {
                throw new ArgumentException("Number of slices must be 1 or greater.");
            }

            if (sourceLength < numberOfSlices)
            {
                throw new InvalidOperationException("Cannot slice a collection into fewer parts than it has elements.");
            }

            return new SlicedEnumerable<T>(source, enumeratedSource.Count() / numberOfSlices, numberOfSlices);
        }

        /// <summary>
        /// Partitions a collection into sub-collections of the desired size.
        /// </summary>
        /// <param name="source">A collection of items.</param>
        /// <param name="sliceSize">The number of elements per sub-collection</param>
        /// <returns>An ISlicedEnumerable, exposing the Slices of the desired, and a (possibly empty) Remainder collection when the source collection could not be evenly divided.</returns>
        public static ISlicedEnumerable<T> SlicesOf<T>(this IEnumerable<T> source, int sliceSize)
        {
            return new SlicedEnumerable<T>(source, sliceSize);
        }
    }
}