using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        public static ISlicedEnumerable<T> Slices<T>(this IEnumerable<T> source, int numberOfSlices)
        {
            IEnumerable<T> enumeratedSource = source.ToList();
            int sliceSize = enumeratedSource.Count() / numberOfSlices;
            return new SlicedEnumerable<T>(enumeratedSource, sliceSize);
        }

        public static ISlicedEnumerable<T> SlicesOf<T>(this IEnumerable<T> source, int sliceSize)
        {
            return new SlicedEnumerable<T>(source, sliceSize);
        }
    }
}