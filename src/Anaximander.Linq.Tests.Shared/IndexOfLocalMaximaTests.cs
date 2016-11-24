using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class IndexOfIndexOfLocalMaximaTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.IndexOfLocalMaxima().ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ThrowsInvalidOperationException()
        {
            IEnumerable<int> collection = new List<int>();

            Assert.Throws<InvalidOperationException>(() => collection.IndexOfLocalMaxima().ToList());
        }

        [Fact]
        public void GivenSingleItem_ReturnsThatItemsIndex()
        {
            var item = 4;
            var collection = new[] { item };
            var expected = new[] { 0 };

            List<int> result = collection.IndexOfLocalMaxima().ToList();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenTwoItems_ReturnsLargestItemsIndex()
        {
            var collection = new[] { 1, 2 };
            var expected = new[] { 1 };

            IEnumerable<int> result = collection.IndexOfLocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollectionWithSingleMaximum_ReturnsLargestItemIndex()
        {
            var collection = new[] { 1, 2, 3, 2, 1 };
            var expected = new[] { 2 };

            IEnumerable<int> result = collection.IndexOfLocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMaximumAtStart_ReturnsCorrectMaxima()
        {
            var collection = new[] { 5, 4, 3, 2, 1 };
            var expected = new[] { 0 };

            IEnumerable<int> result = collection.IndexOfLocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMaximumAtEnd_ReturnsCorrectMaxima()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 4 };

            IEnumerable<int> result = collection.IndexOfLocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMaximaAtBothEnds_ReturnsCorrectMaxima()
        {
            var collection = new[] { 3, 2, 1, 2, 3 };
            var expected = new[] { 0, 4 };

            IEnumerable<int> result = collection.IndexOfLocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCorrectMaxima()
        {
            var collection = new[] { 3, 1, 2, 3, 1, 2, 3, 2, 1, 3, 2 };
            var expected = new[] { 0, 3, 6, 9 };

            IEnumerable<int> result = collection.IndexOfLocalMaxima();

            Assert.Equal(expected, result);
        }
    }
}