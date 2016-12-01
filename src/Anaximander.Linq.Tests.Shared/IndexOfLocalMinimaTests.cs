using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class IndexOfIndexOfLocalMinimaTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.IndexOfLocalMinima().ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<int> collection = new List<int>();

            var result = collection.IndexOfLocalMinima().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleItem_ReturnsThatItemsIndex()
        {
            var item = 4;
            var collection = new[] { item };
            var expected = new[] { 0 };

            List<int> result = collection.IndexOfLocalMinima().ToList();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenTwoItems_ReturnsSmallestItemsIndex()
        {
            var collection = new[] { 1, 2 };
            var expected = new[] { 0 };

            IEnumerable<int> result = collection.IndexOfLocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollectionWithSingleMinimum_ReturnsSmallestItemsIndex()
        {
            var collection = new[] { 3, 2, 1, 2, 3 };
            var expected = new[] { 2 };

            IEnumerable<int> result = collection.IndexOfLocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMinimumAtStart_ReturnsCorrectMinima()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 0 };

            IEnumerable<int> result = collection.IndexOfLocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMinimumAtEnd_ReturnsCorrectMinima()
        {
            var collection = new[] { 5, 4, 3, 2, 1 };
            var expected = new[] { 4 };

            IEnumerable<int> result = collection.IndexOfLocalMinima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMinimaAtBothEnds_ReturnsCorrectMinima()
        {
            var collection = new[] { 1, 2, 3, 2, 1 };
            var expected = new[] { 0, 4 };

            IEnumerable<int> result = collection.IndexOfLocalMinima().ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCorrectMinima()
        {
            var collection = new[] { 1, 3, 2, 1, 3, 2, 1, 2, 3, 1, 2 };
            var expected = new[] { 0, 3, 6, 9 };

            IEnumerable<int> result = collection.IndexOfLocalMinima();

            Assert.Equal(expected, result);
        }
    }
}