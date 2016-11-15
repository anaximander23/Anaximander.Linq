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

            Assert.Throws<ArgumentNullException>(() => collection.BoxWhile((a, b) => true));
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsSourceCollectionAsOnlyElement()
        {
            var collection = new object[] { };

            object[][] result = collection.BoxWhile((a, b) => true)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(new[] { collection }, result);
        }

        [Fact]
        public void GivenSingleElement_ReturnsSourceCollectionAsOnlyElement()
        {
            var collection = new[] { 0 };

            int[][] result = collection.BoxWhile((a, b) => true)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(new[] { collection }, result);
        }

        [Fact]
        public void GivenUnchangingCondition_ReturnsSingleCollection()
        {
            var collection = new[] { 0, 0, 0 };

            int[][] result = collection.BoxWhile((a, b) => true)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(new[] { collection }, result);
        }

        [Fact]
        public void GivenSingleChangeInCondition_ReturnsTwoCollections()
        {
            var collection = new[] { 0, 0, 1, 1 };
            var expected = new[] { new[] { 0, 0 }, new[] { 1, 1 } };

            int[][] result = collection.BoxWhile((a, b) => a == b)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenOddSizedCollection_ReturnsCorrectNumber()
        {
            var collection = new[] { 0, 0, 1, 1, 1 };
            var expected = new[] { new[] { 0, 0 }, new[] { 1, 1, 1 } };

            int[][] result = collection.BoxWhile((a, b) => a == b)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMultipleChangesInCondition_ReturnsMultipleCollections()
        {
            var collection = new[] { 0, 0, 1, 1, 0, 0 };
            var expected = new[] { new[] { 0, 0 }, new[] { 1, 1 }, new[] { 0, 0 } };

            int[][] result = collection.BoxWhile((a, b) => a == b)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}