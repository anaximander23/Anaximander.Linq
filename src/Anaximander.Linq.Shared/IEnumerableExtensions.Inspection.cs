using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<T> LocalMinima<T>(this IEnumerable<T> source) where T : IComparable
        {
            return source.LocalMaxima(x => x);
        }

        public static IEnumerable<TSource> LocalMinima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            var windows = source
                .Select(x => new
                {
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList());

            var front = windows.First();
            if (front[0].Value.CompareTo(front[1].Value) < 0)
            {
                yield return front[0].Item;
            }

            var back = front;
            foreach (var x in windows)
            {
                back = x;
                if ((x[1].Value.CompareTo(x[0].Value) < 0) && (x[1].Value.CompareTo(x[2].Value) < 0))
                {
                    yield return x[1].Item;
                }
            }

            if (back[2].Value.CompareTo(back[1].Value) < 0)
            {
                yield return back[2].Item;
            }
        }

        public static IEnumerable<T> LocalMaxima<T>(this IEnumerable<T> source) where T : IComparable
        {
            return source.LocalMaxima(x => x);
        }

        public static IEnumerable<TSource> LocalMaxima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            var windows = source
                .Select(x => new
                {
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList());

            var front = windows.First();
            if (front[0].Value.CompareTo(front[1].Value) > 0)
            {
                yield return front[0].Item;
            }

            var back = front;
            foreach (var x in windows)
            {
                back = x;
                if ((x[1].Value.CompareTo(x[0].Value) > 0) && (x[1].Value.CompareTo(x[2].Value) > 0))
                {
                    yield return x[1].Item;
                }
            }

            if (back[2].Value.CompareTo(back[1].Value) > 0)
            {
                yield return back[2].Item;
            }
        }

        public static IEnumerable<int> IndexOfLocalMinima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.Select(comparison).IndexOfLocalMinima();
        }

        public static IEnumerable<int> IndexOfLocalMinima<T>(this IEnumerable<T> source) where T : IComparable
        {
            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Value = x
                })
                .Window(3)
                .Select(x => x.ToList());

            var front = windows.First();
            if (front[0].Value.CompareTo(front[1].Value) < 0)
            {
                yield return front[0].Index;
            }

            var back = front;
            foreach (var x in windows)
            {
                back = x;
                if ((x[1].Value.CompareTo(x[0].Value) < 0) && (x[1].Value.CompareTo(x[2].Value) < 0))
                {
                    yield return x[1].Index;
                }
            }

            if (back[2].Value.CompareTo(back[1].Value) < 0)
            {
                yield return back[2].Index;
            }
        }

        public static IEnumerable<int> IndexOfLocalMaxima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.Select(comparison).IndexOfLocalMaxima();
        }

        public static IEnumerable<int> IndexOfLocalMaxima<T>(this IEnumerable<T> source) where T : IComparable
        {
            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Value = x
                })
                .Window(3)
                .Select(x => x.ToList());

            var front = windows.First();
            if (front[0].Value.CompareTo(front[1].Value) > 0)
            {
                yield return front[0].Index;
            }

            var back = front;
            foreach (var x in windows)
            {
                back = x;
                if ((x[1].Value.CompareTo(x[0].Value) > 0) && (x[1].Value.CompareTo(x[2].Value) > 0))
                {
                    yield return x[1].Index;
                }
            }

            if (back[2].Value.CompareTo(back[1].Value) > 0)
            {
                yield return back[2].Index;
            }
        }
    }
}