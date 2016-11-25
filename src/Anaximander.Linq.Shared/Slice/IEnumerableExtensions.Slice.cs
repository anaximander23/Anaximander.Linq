using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
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

        public static ISlicedEnumerable<T> SlicesOf<T>(this IEnumerable<T> source, int sliceSize)
        {
            return new SlicedEnumerable<T>(source, sliceSize);
        }
    }
}