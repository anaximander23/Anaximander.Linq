using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class LocalMinimaTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.LocalMinima().ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<int> collection = new List<int>();

            var result = collection.LocalMinima().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleItem_ReturnsThatItem()
        {
            var item = 4;
            var collection = new[] { item };

            List<int> result = collection.LocalMinima().ToList();

            Assert.Single(result);
            Assert.True(collection.SequenceEqual(result));
        }

        [Fact]
        public void GivenTwoItems_ReturnsSmallestItem()
        {
            var collection = new[] { 1, 2 };
            var expected = new[] { 1 };

            IEnumerable<int> result = collection.LocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollectionWithSingleMinimum_ReturnsSmallestItem()
        {
            var collection = new[] { 3, 2, 1, 2, 3 };
            var expected = new[] { 1 };

            IEnumerable<int> result = collection.LocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMinimumAtStart_ReturnsCorrectMinima()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 1 };

            IEnumerable<int> result = collection.LocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMinimumAtEnd_ReturnsCorrectMinima()
        {
            var collection = new[] { 5, 4, 3, 2, 1 };
            var expected = new[] { 1 };

            IEnumerable<int> result = collection.LocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMinimaAtBothEnds_ReturnsCorrectMinima()
        {
            var collection = new[] { 1, 2, 3, 2, 1 };
            var expected = new[] { 1, 1 };

            var result = collection.LocalMinima().ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCorrectMinima()
        {
            var collection = new[] { 1, 3, 2, 1, 3, 2, 1, 2, 3, 1, 2 };
            var expected = new[] { 1, 1, 1, 1 };

            IEnumerable<int> result = collection.LocalMinima();

            Assert.Equal(expected, result);
        }
    }
}