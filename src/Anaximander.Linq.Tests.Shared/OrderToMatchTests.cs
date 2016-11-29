using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class OrderToMatchTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;
            var order = new[] { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(() => collection.OrderToMatch(x => x, order).ToList());
        }

        [Fact]
        public void GivenNullOrderingCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            IEnumerable<int> order = null;

            Assert.Throws<ArgumentNullException>(() => collection.OrderToMatch(x => x, order).ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<int> collection = new List<int>();

            List<IEnumerable<int>> result = collection.Permute().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenEmptyOrderingCollection_ThrowsArgumentException()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            IEnumerable<int> order = new List<int>();

            Assert.Throws<ArgumentException>(() => collection.OrderToMatch(x => x, order).ToList());
        }

        [Fact]
        public void GivenValidCollection_ReturnsCorrectOrder()
        {
            var collection = new Dictionary<int, string>
            {
                [3] = "c",
                [2] = "a",
                [1] = "b"
            };

            var order = new[] { 2, 3, 1 };

            var expected = new Dictionary<int, string>
            {
                [2] = "a",
                [3] = "c",
                [1] = "b"
            }
            .ToArray();

            var result = collection.OrderToMatch(x => x.Key, order).ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMissingOrderKey_ReturnsCorrectOrder()
        {
            var collection = new Dictionary<int, string>
            {
                [3] = "c",
                [2] = "a",
                [1] = "b"
            };

            var order = new[] { 2, 3 };

            var expected = new Dictionary<int, string>
            {
                [2] = "a",
                [3] = "c",
                [1] = "b"
            }
            .ToArray();

            var result = collection.OrderToMatch(x => x.Key, order).ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMultipleMissingOrderKeys_ReturnsCorrectOrder()
        {
            var collection = new Dictionary<int, string>
            {
                [5] = "d",
                [3] = "c",
                [2] = "a",
                [1] = "b"
            };

            var order = new[] { 2, 3 };

            var expected = new Dictionary<int, string>
            {
                [2] = "a",
                [3] = "c",
                [5] = "d",
                [1] = "b"
            }
            .ToArray();

            var result = collection.OrderToMatch(x => x.Key, order).ToArray();

            Assert.Equal(expected, result);
        }
    }
}