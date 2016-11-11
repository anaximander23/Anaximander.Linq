using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    // ReSharper disable once InconsistentNaming
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var random = new Random();

            return source.OrderBy(x => random.Next());
        }
    }
}