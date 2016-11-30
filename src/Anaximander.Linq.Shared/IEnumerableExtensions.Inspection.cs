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
            if (!source.Any())
            {
                throw new InvalidOperationException("Source collection is empty");
            }

            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator();

            if (windows.MoveNext())
            {
                var front = windows.Current;

                if (front.OrderBy(x => x.Value).First().Index == 0)
                {
                    yield return front.First().Item;
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    if (back.Count == 3)
                    {
                        if ((back[1].Value.CompareTo(back[0].Value) < 0) && (back[1].Value.CompareTo(back[2].Value) < 0))
                        {
                            yield return back[1].Item;
                        }
                    }
                }

                var backLargest = back.OrderBy(x => x.Value).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last().Item;
                }
            }
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
            if (!source.Any())
            {
                throw new InvalidOperationException("Source collection is empty");
            }

            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator();

            if (windows.MoveNext())
            {
                var front = windows.Current;

                if (front.OrderByDescending(x => x.Value).First().Index == 0)
                {
                    yield return front.First().Item;
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    if (back.Count == 3)
                    {
                        if ((back[1].Value.CompareTo(back[0].Value) > 0) && (back[1].Value.CompareTo(back[2].Value) > 0))
                        {
                            yield return back[1].Item;
                        }
                    }
                }

                var backLargest = back.OrderByDescending(x => x.Value).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last().Item;
                }
            }
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
            if (!source.Any())
            {
                throw new InvalidOperationException("Source collection is empty");
            }

            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator();

            if (windows.MoveNext())
            {
                var front = windows.Current;

                if (front.OrderBy(x => x.Value).First().Index == 0)
                {
                    yield return front.First().Index;
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    if (back.Count == 3)
                    {
                        if ((back[1].Value.CompareTo(back[0].Value) < 0) && (back[1].Value.CompareTo(back[2].Value) < 0))
                        {
                            yield return back[1].Index;
                        }
                    }
                }

                var backLargest = back.OrderBy(x => x.Value).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last().Index;
                }
            }
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
            if (!source.Any())
            {
                throw new InvalidOperationException("Source collection is empty");
            }

            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator();

            if (windows.MoveNext())
            {
                var front = windows.Current;

                if (front.OrderByDescending(x => x.Value).First().Index == 0)
                {
                    yield return front.First().Index;
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    if (back.Count == 3)
                    {
                        if ((back[1].Value.CompareTo(back[0].Value) > 0) && (back[1].Value.CompareTo(back[2].Value) > 0))
                        {
                            yield return back[1].Index;
                        }
                    }
                }

                var backLargest = back.OrderByDescending(x => x.Value).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last().Index;
                }
            }
        }
    }
}