using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
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
        /// Generates all possible ways that a set of items can be reordered.
        /// WARNING: for a set of n items, this generates n! sets of n items each. Use with caution.
        /// </summary>
        /// <param name="source">A set of items</param>
        /// <returns>A set of sets, each representing a different re-ordering of the original set.</returns>
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> source)
        {
            IEnumerable<T> sourceList = source as IList<T> ?? source.ToList();

            var sourceCount = sourceList.Count();

            if (sourceCount == 0)
            {
                return Enumerable.Empty<IEnumerable<T>>();
            }

            if (sourceCount == 1)
            {
                return new[] { sourceList };
            }

            return GenerateCombinations(source, source.Count(), CombinationsGenerationMode.DistinctOrderSensitive);
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

        private static IEnumerable<IEnumerable<T>> GenerateCombinations<T>(this IEnumerable<T> source, int combinationSize, CombinationsGenerationMode mode)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source collection is null");
            }

            if (combinationSize == 0)
            {
                return new[] { new T[] { } };
            }

            IEnumerable<T> sourceList = source as IList<T> ?? source.ToList();

            var distinctModes = new[] { CombinationsGenerationMode.DistinctOrderSensitive, CombinationsGenerationMode.DistinctOrderInsensitive };
            var orderSensitiveModes = new[] { CombinationsGenerationMode.AllowDuplicatesOrderSensitive, CombinationsGenerationMode.DistinctOrderSensitive };

            if (distinctModes.Contains(mode) && (combinationSize > sourceList.Count()))
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
                        .Skip(distinctModes.Contains(mode) ? 1 : 0)
                        .OrderBy(y => y.Index)
                        .Skip(orderSensitiveModes.Contains(mode) ? 0 : x.Index)
                        .GenerateCombinations(combinationSize - 1, mode)
                        .Select(y => new[] { x }.Concat(y).Select(z => z.Item))
                );
        }
    }

    /// <summary>
    /// How to handle duplication when generating combinations.
    /// </summary>
    public enum CombinationsGenerationMode
    {
        /// <summary>
        /// Use each source item only once, treating a different order of the same elements as a new combination. Note: Requires combinationSize to be equal to or greater than the source collection size.
        /// </summary>
        DistinctOrderSensitive,

        /// <summary>
        /// Use each source item only once, treating a different order of the same elements as the same combination. Note: Requires combinationSize to be equal to or greater than the source collection size.
        /// </summary>
        DistinctOrderInsensitive,

        /// <summary>
        /// Allow source items to be used multiple times, treating a different order of the same elements as a new combination.
        /// </summary>
        AllowDuplicatesOrderSensitive,

        /// <summary>
        /// Allow source items to be used multiple times, treating a different order of the same elements as the same combination.
        /// </summary>
        AllowDuplicatesOrderInsensitive,
    }
}