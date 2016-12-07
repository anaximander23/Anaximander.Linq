using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class WindowTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.Window(1).ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            var collection = new List<object>();

            var result = collection.Window(3).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenValidCollection_CanProduceWindowSizeOfOne()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };

            var expected = new[]
            {
                new[] { 1 },
                new[] { 2 },
                new[] { 3 },
                new[] { 4 },
                new[] { 5 }
            };

            int[][] actual = collection.Window(1)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GivenValidCollection_CanProduceWindowSizeOfTwo()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };

            var expected = new[]
            {
                new[] { 1, 2 },
                new[] { 2, 3 },
                new[] { 3, 4 },
                new[] { 4, 5 }
            };

            int[][] actual = collection.Window(2)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GivenValidCollection_CanProduceWindowSizeOfThree()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };

            var expected = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 2, 3, 4 },
                new[] { 3, 4, 5 }
            };

            int[][] actual = collection.Window(3)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenCollectionSizeIsSmallerThanWindowSize_ReturnsEmptySet()
        {
            var collection = new[] { 1, 2, 3 };

            var result = collection.Window(4);

            Assert.Empty(result);
        }

        [Fact]
        public void WhenCollectionSizeIsEqualToWindowSize_ReturnsSingleWindow()
        {
            var collection = new[] { 1, 2, 3 };
            var expected = new[] { new[] { 1, 2, 3 } };

            var result = collection.Window(3)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}