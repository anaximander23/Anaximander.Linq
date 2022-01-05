using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class PermuteTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.Permute().ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<int> collection = new List<int>();

            List<IEnumerable<int>> result = collection.Permute().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleItem_ReturnsThatItemAsOnlyPermutation()
        {
            IEnumerable<int> collection = new[] { 1 };
            var expected = new[] { new[] { 1 } };

            int[][] result = collection.Permute()
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollectionOfTwo_ReturnsCorrectPermutations()
        {
            var collection = new[] { 1, 2 };
            var expected = new[]
            {
                new[] { 1, 2 },
                new[] { 2, 1 }
            };

            int[][] result = collection.Permute()
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollectionOfThree_ReturnsCorrectPermutations()
        {
            var collection = new[] { 1, 2, 3 };
            var expected = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 1, 3, 2 },
                new[] { 2, 1, 3 },
                new[] { 2, 3, 1 },
                new[] { 3, 1, 2 },
                new[] { 3, 2, 1 }
            };

            int[][] result = collection.Permute()
                .Select(x => x.ToArray())
                .OrderBy(x => x[0])
                .ThenBy(x => x[1])
                .ThenBy(x => x[2])
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}