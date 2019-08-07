using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Finds the local minima in a collection of values; that is, elements that are smaller than the elements either side.
        /// </summary>
        /// <param name="source">A collection of IComparable values or items.</param>
        /// <returns>The values of the minima.</returns>
        public static IEnumerable<T> LocalMinima<T>(this IEnumerable<T> source) where T : IComparable
        {
            return source.LocalMinima(x => x);
        }

        /// <summary>
        /// Finds the items whose comparison values are local minima in a collection of items; that is, elements whose comparison values are smaller than those of the elements either side.
        /// </summary>
        /// <param name="source">A collection of values or items.</param>
        /// <param name="comparison">A function to derive an IComparable value from each item.</param>
        /// <returns>The values of the minima.</returns>
        public static IEnumerable<TSource> LocalMinima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.FindInflectionPoints(comparison, false).Select(x => x.Item);
        }

        /// <summary>
        /// Finds the local maxima in a collection of values; that is, elements that are larger than the elements either side.
        /// </summary>
        /// <param name="source">A collection of IComparable values or items.</param>
        /// <returns>The values of the maxima.</returns>
        public static IEnumerable<T> LocalMaxima<T>(this IEnumerable<T> source) where T : IComparable
        {
            return source.LocalMaxima(x => x);
        }

        /// <summary>
        /// Finds the items whose comparison values are local maxima in a collection of items; that is, elements whose comparison values are larger than those of the elements either side.
        /// </summary>
        /// <param name="source">A collection of values or items.</param>
        /// <param name="comparison">A function to derive an IComparable value from each item.</param>
        /// <returns>The values of the maxima.</returns>
        public static IEnumerable<TSource> LocalMaxima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.FindInflectionPoints(comparison, true).Select(x => x.Item);
        }

        /// <summary>
        /// Finds the indices of the local minima in a collection of values; that is, elements that are smaller than the elements either side.
        /// </summary>
        /// <param name="source">A collection of IComparable values or items.</param>
        /// <returns>The indices of the minima within the source collection.</returns>
        public static IEnumerable<int> IndexOfLocalMinima<TSource>(this IEnumerable<TSource> source) where TSource : IComparable
        {
            return source.IndexOfLocalMinima(x => x);
        }

        /// <summary>
        /// Finds the indices of items whose comparison values are local minima in a collection of items; that is, elements whose comparison values are smaller than those of the elements either side.
        /// </summary>
        /// <param name="source">A collection of values or items.</param>
        /// <param name="comparison">A function to derive an IComparable value from each item.</param>
        /// <returns>The values of the minima.</returns>
        public static IEnumerable<int> IndexOfLocalMinima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.FindInflectionPoints(comparison, false).Select(x => x.Index);
        }

        /// <summary>
        /// Finds the indices of the local maxima in a collection of values; that is, elements that are larger than the elements either side.
        /// </summary>
        /// <param name="source">A collection of IComparable values or items.</param>
        /// <returns>The indices of the maxima within the source collection.</returns>
        public static IEnumerable<int> IndexOfLocalMaxima<TSource>(this IEnumerable<TSource> source) where TSource : IComparable
        {
            return source.IndexOfLocalMaxima(x => x);
        }

        /// <summary>
        /// Finds the items whose comparison values are local maxima in a collection of items; that is, elements whose comparison values are larger than those of the elements either side.
        /// </summary>
        /// <param name="source">A collection of values or items.</param>
        /// <param name="comparison">A function to derive an IComparable value from each item.</param>
        /// <returns>The values of the maxima.</returns>
        public static IEnumerable<int> IndexOfLocalMaxima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.FindInflectionPoints(comparison, true).Select(x => x.Index);
        }

        private static IEnumerable<IndexedItem<TSource>> FindInflectionPoints<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison, bool findMaxima) where TCompare : IComparable
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }
            if (!source.Any())
            {
                yield break;
            }

            var comparableSource = source.Select((x, i) => new ComparableItem<TSource>(i, x, comparison(x)));

            Func<IEnumerable<ComparableItem<TSource>>, IOrderedEnumerable<ComparableItem<TSource>>> order = x =>
                    (findMaxima
                        ? x.OrderByDescending(o => o.Value)
                        : x.OrderBy(o => o.Value));

            // Not actually a multiple enumeration:
            // Iff we can't start enumerating it this way, we enumerate it once the other way
            // ReSharper disable once PossibleMultipleEnumeration
            using (var windows = comparableSource
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator())
            {
                if (!windows.MoveNext())
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    yield return order(comparableSource).First();
                    yield break;
                }

                var front = windows.Current;

                if (order(front).First().Index == 0)
                {
                    yield return front.First();
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    Func<int, bool> check = x => (findMaxima ? x > 0 : x < 0);

                    if (back.Count == 3)
                    {
                        if (check(back[1].Value.CompareTo(back[0].Value)) && check(back[1].Value.CompareTo(back[2].Value)))
                        {
                            yield return back[1];
                        }
                    }
                }

                var backLargest = order(back).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last();
                }
            }
        }

        private class IndexedItem<T>
        {
            public IndexedItem(int index, T item)
            {
                Index = index;
                Item = item;
            }

            public readonly int Index;
            public readonly T Item;
        }

        private class ComparableItem<T> : IndexedItem<T>
        {
            public ComparableItem(int index, T item, IComparable comparisonValue)
                : base(index, item)
            {
                Value = comparisonValue;
            }

            public readonly IComparable Value;
        }
    }
}