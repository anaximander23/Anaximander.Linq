using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        private static CombinationsGenerationMode[] _distinctModes = new[] { CombinationsGenerationMode.DistinctOrderSensitive, CombinationsGenerationMode.DistinctOrderInsensitive };
        private static CombinationsGenerationMode[] _orderSensitiveModes = new[] { CombinationsGenerationMode.AllowDuplicatesOrderSensitive, CombinationsGenerationMode.DistinctOrderSensitive };

        /// <summary>
        /// Produces all possible combinations of objects by taking one from each collection.
        /// From https://blogs.msdn.microsoft.com/ericlippert/2010/06/28/computing-a-cartesian-product-with-linq/
        /// </summary>
        /// <param name="firstSequence">A collection of items to combine</param>
        /// <param name="secondSequence">A second collection to combine the first set with</param>
        /// <param name="otherSequences">A set of further collections to combine with</param>
        /// <returns>A collection of collections, where each inner collection is one possible permutation.</returns>
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<T> firstSequence, IEnumerable<T> secondSequence, params IEnumerable<T>[] otherSequences)
        {
            return new[] { firstSequence, secondSequence }.Concat(otherSequences).CartesianProduct();
        }

        /// <summary>
        /// Produces all possible combinations of objects by taking one from each collection.
        /// From https://blogs.msdn.microsoft.com/ericlippert/2010/06/28/computing-a-cartesian-product-with-linq/
        /// </summary>
        /// <param name="sequences">A set of collections to combine</param>
        /// <returns>A collection of collections, where each inner collection is one possible permutation.</returns>
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException(nameof(sequences));
            }

            IEnumerable<IEnumerable<T>> emptyProduct = new IEnumerable<T>[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }))
                .Where(x => x.Any());
        }

        /// <summary>
        /// Generates all possible ways that a set of items can be reordered. WARNING: for a set of n items, this generates n! sets of n items each. Use with caution.
        /// </summary>
        /// <param name="source">A set of items</param>
        /// <returns>A set of sets, each representing a different re-ordering of the original set.</returns>
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> source)
        {
            // High-performance algorithm adapted from Eric Ouellet's method
            // https://stackoverflow.com/questions/11208446/generating-permutations-of-a-set-most-efficiently/36634935

            var items = source.ToArray();

            if (!items.Any())
            {
                yield break;
            }

            int countOfItem = items.Length;
            var indexes = new int[countOfItem];

            yield return items;

            for (int i = 1; i < countOfItem;)
            {
                if (indexes[i] < i)
                {
                    if ((i & 1) == 1)
                    {
                        Swap(ref items[i], ref items[indexes[i]]);
                    }
                    else
                    {
                        Swap(ref items[i], ref items[0]);
                    }

                    yield return items;

                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i++] = 0;
                }
            }
        }

        /// <summary>
        /// Generates all possible subsets of a given size that can be taken from a set. Distinct orderings are counted as distinct sets.
        /// </summary>
        /// <param name="source">A set of items</param>
        /// <param name="combinationSize">The number of items per result set</param>
        /// <param name="mode">The desired mode: whether to use each item from the source set only once, or allow re-use. (Note that Distinct mode requires a <paramref name="combinationSize"/> equal to or greater than the source collection length.)</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int combinationSize, CombinationsGenerationMode mode = CombinationsGenerationMode.DistinctOrderInsensitive)
        {
            if (combinationSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(combinationSize), "Combination size must be positive and non-zero");
            }

            return source.GenerateCombinations(combinationSize, mode);
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            (b, a) = (a, b);
        }

        private static IEnumerable<IEnumerable<T>> GenerateCombinations<T>(this IEnumerable<T> source, int combinationSize, CombinationsGenerationMode mode)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }

            if (combinationSize == 0)
            {
                return new[] { Array.Empty<T>() };
            }

            IEnumerable<T> sourceList = source as IList<T> ?? source.ToList();

            if (_distinctModes.Contains(mode) && (combinationSize > sourceList.Count()))
            {
                return new List<IEnumerable<T>>();
            }

            if (sourceList.Count() == 1)
            {
                return new[] { sourceList };
            }

            var indexedSource = sourceList
                .Select((x, i) => new
                {
                    Item = x,
                    Index = i
                })
                .ToList();

            return indexedSource
                .SelectMany(x => indexedSource
                    .OrderBy(y => x.Index != y.Index)
                    .Skip(_distinctModes.Contains(mode) ? 1 : 0)
                    .OrderBy(y => y.Index)
                    .Skip(_orderSensitiveModes.Contains(mode) ? 0 : x.Index)
                    .GenerateCombinations(combinationSize - 1, mode)
                    .Select(y => new[] { x }.Concat(y).Select(z => z.Item))
                );
        }
    }
}