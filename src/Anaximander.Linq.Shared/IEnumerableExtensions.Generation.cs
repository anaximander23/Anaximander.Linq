using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    // ReSharper disable once InconsistentNaming
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
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
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

            if (sourceList.Count() == 1)
            {
                return new[] { sourceList };
            }

            return sourceList
                .SelectMany(x => sourceList
                    .OrderBy(y => !x.Equals(y))
                    .Skip(1)
                    .Permute()
                    .Select(y => new[] { x }.Concat(y))
                );
        }

        /// <summary>
        /// Generates all possible subsets of a given size that can be taken from a set. Distinct orderings are counted as distinct sets.
        /// </summary>
        /// <param name="source">A set of items</param>
        /// <param name="combinationSize">The number of items per result set</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int combinationSize)
        {
            if (combinationSize < 1)
            {
                return new[] { new T[] { } };
            }

            IEnumerable<T> sourceList = source as IList<T> ?? source.ToList();
            if (sourceList.Count() == 1)
            {
                return new[] { sourceList };
            }

            return sourceList
                .SelectMany(x => sourceList
                    .OrderBy(y => !x.Equals(y))
                    .Skip(1)
                    .Combinations(combinationSize - 1)
                    .Select(y => new[] { x }.Concat(y))
                );
        }
    }
}