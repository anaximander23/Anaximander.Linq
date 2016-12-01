using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class CartesianProductTests
    {
        [Fact]
        public void GivenNullOuterCollection_ThrowsArgumentNullException()
        {
            IEnumerable<IEnumerable<object>> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.CartesianProduct());
        }

        [Fact]
        public void GivenEmptyOuterCollection_ReturnsEmptyCollection()
        {
            var collection = new List<List<object>>();

            var result = collection.CartesianProduct();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleCollection_ReturnsOriginalCollectionAsIndividuals()
        {
            var collection = new[] { new[] { 0, 1, 2 } };

            var expected = new[]
            {
                new[] { 0 },
                new[] { 1 },
                new[] { 2 }
            };

            IEnumerable<IEnumerable<int>> result = collection.CartesianProduct()
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenTwoCollections_ReturnsCorrectCombinations()
        {
            var collection = new[]
            {
                new[] { 0, 1, 2 },
                new[] { 7, 8, 9 }
            };

            var expected = new[]
            {
                new[] { 0, 7 },
                new[] { 0, 8 },
                new[] { 0, 9 },
                new[] { 1, 7 },
                new[] { 1, 8 },
                new[] { 1, 9 },
                new[] { 2, 7 },
                new[] { 2, 8 },
                new[] { 2, 9 }
            };

            IEnumerable<IEnumerable<int>> result = collection.CartesianProduct()
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenNCollections_ReturnsCorrectCombinations()
        {
            var collection = new[]
            {
                new[] { 0, 1, 2 },
                new[] { 3, 4, 5 },
                new[] { 6, 7, 8 }
            };

            var expected = new[]
            {
                new[] { 0, 3, 6 },
                new[] { 0, 3, 7 },
                new[] { 0, 3, 8 },
                new[] { 0, 4, 6 },
                new[] { 0, 4, 7 },
                new[] { 0, 4, 8 },
                new[] { 0, 5, 6 },
                new[] { 0, 5, 7 },
                new[] { 0, 5, 8 },
                new[] { 1, 3, 6 },
                new[] { 1, 3, 7 },
                new[] { 1, 3, 8 },
                new[] { 1, 4, 6 },
                new[] { 1, 4, 7 },
                new[] { 1, 4, 8 },
                new[] { 1, 5, 6 },
                new[] { 1, 5, 7 },
                new[] { 1, 5, 8 },
                new[] { 2, 3, 6 },
                new[] { 2, 3, 7 },
                new[] { 2, 3, 8 },
                new[] { 2, 4, 6 },
                new[] { 2, 4, 7 },
                new[] { 2, 4, 8 },
                new[] { 2, 5, 6 },
                new[] { 2, 5, 7 },
                new[] { 2, 5, 8 },
            };

            IEnumerable<IEnumerable<int>> result = collection.CartesianProduct()
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}