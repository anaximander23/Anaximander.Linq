using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class BoxWhileTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<IEnumerable<object>> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.BoxWhile((a, b) => true).ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            var collection = new object[] { };

            var result = collection.BoxWhile((a, b) => true).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleElement_ReturnsSourceCollectionAsOnlyElement()
        {
            var collection = new[] { 3 };

            int[][] result = collection.BoxWhile((a, b) => true)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(new[] { collection }, result);
        }

        [Fact]
        public void GivenUnchangingCondition_ReturnsSingleCollection()
        {
            var collection = new[] { 0, 1, 2 };

            int[][] result = collection.BoxWhile((a, b) => true)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(new[] { collection }, result);
        }

        [Fact]
        public void GivenSingleChangeInCondition_ReturnsTwoCollections()
        {
            var collection = new[] { 2, 4, 3, 6 };
            var expected = new[] { new[] { 2, 4 }, new[] { 3, 6 } };

            int[][] result = collection.BoxWhile((a, b) => b % a == 0)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenOddSizedCollection_ReturnsCorrectNumber()
        {
            var collection = new[] { 2, 4, 3, 6, 12 };
            var expected = new[] { new[] { 2,  4}, new[] { 3, 6, 12 } };

            int[][] result = collection.BoxWhile((a, b) => b % a == 0)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMultipleChangesInCondition_ReturnsMultipleCollections()
        {
            var collection = new[] { 2, 4, 3, 6, 5, 10 };
            var expected = new[] { new[] { 2, 4 }, new[] { 3, 6 }, new[] { 5, 10 } };

            int[][] result = collection.BoxWhile((a, b) => b % a == 0)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}