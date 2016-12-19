using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class CombinationsTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<object> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.Combinations(1));
        }

        [Fact]
        public void GivenZeroCombinationSize_ThrowsArgumentOutOfRangeException()
        {
            var collection = new[] { 1, 2, 3 };

            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Combinations(0));
        }

        [Fact]
        public void GivenNegativeCombinationSize_ThrowsArgumentOutOfRangeException()
        {
            var collection = new[] { 1, 2, 3 };

            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Combinations(-1));
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<object> collection = new List<object>();

            var result = collection.Combinations(1);

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleElement_ReturnsThatElementAsOnlyCombination()
        {
            IEnumerable<int> collection = new[] { 1 };
            var expected = new[] { new[] { 1 } };

            var result = collection.Combinations(1, mode: CombinationsGenerationMode.AllowDuplicatesOrderSensitive);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenCombinationSizeOfOne_ReturnsIndividualElements()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[]
            {
                new[] { 1 },
                new[] { 2 },
                new[] { 3 },
                new[] { 4 },
                new[] { 5 },
            };

            var result = collection.Combinations(1, mode: CombinationsGenerationMode.AllowDuplicatesOrderSensitive)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCombinations_Distinct()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            var expected = new[]
            {
                new[] { 1, 2 },
                new[] { 1, 3 },
                new[] { 2, 1 },
                new[] { 2, 3 },
                new[] { 3, 1 },
                new[] { 3, 2 }
            };

            var result = collection.Combinations(2, mode: CombinationsGenerationMode.DistinctOrderSensitive)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCombinations_DistinctOrderInsensitive()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            var expected = new[]
            {
                new[] { 1, 2 },
                new[] { 1, 3 },
                new[] { 2, 3 },
            };

            var result = collection.Combinations(2, mode: CombinationsGenerationMode.DistinctOrderInsensitive)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCombinations_Duplicates()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            var expected = new[]
            {
                new[] { 1, 1 },
                new[] { 1, 2 },
                new[] { 1, 3 },
                new[] { 2, 1 },
                new[] { 2, 2 },
                new[] { 2, 3 },
                new[] { 3, 1 },
                new[] { 3, 2 },
                new[] { 3, 3 }
            };

            var result = collection.Combinations(2, mode: CombinationsGenerationMode.AllowDuplicatesOrderSensitive)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenCombinationSizeLargerThanSource_InDistinctMode_ReturnsEmptySet()
        {
            IEnumerable<int> collection = new[] { 1, 2 };

            var result = collection.Combinations(3, mode: CombinationsGenerationMode.DistinctOrderSensitive);

            Assert.Empty(result);
        }

        [Fact]
        public void GivenCombinationSizeLargerThanSourceAndSourceIsSingleElement_InDistinctMode_ReturnsEmptySet()
        {
            IEnumerable<int> collection = new[] { 1 };

            var result = collection.Combinations(3, mode: CombinationsGenerationMode.DistinctOrderSensitive);

            Assert.Empty(result);
        }

        [Fact]
        public void GivenCombinationSizeLargerThanSource_InDuplicatesMode_ReturnsValidResult()
        {
            IEnumerable<int> collection = new[] { 1, 2 };
            var expected = new[]
            {
                new[] { 1, 1, 1 },
                new[] { 1, 1, 2 },
                new[] { 1, 2, 1 },
                new[] { 1, 2, 2 },
                new[] { 2, 1, 1 },
                new[] { 2, 1, 2 },
                new[] { 2, 2, 1 },
                new[] { 2, 2, 2 }
            };

            var result = collection.Combinations(3, mode: CombinationsGenerationMode.AllowDuplicatesOrderSensitive)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}