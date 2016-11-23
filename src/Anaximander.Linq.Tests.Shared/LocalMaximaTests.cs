using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class LocalMaximaTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.LocalMaxima().ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ThrowsInvalidOperationException()
        {
            IEnumerable<int> collection = new List<int>();

            Assert.Throws<InvalidOperationException>(() => collection.LocalMaxima().ToList());
        }

        [Fact]
        public void GivenSingleItem_ReturnsThatItem()
        {
            var item = 4;
            var collection = new[] { item };

            var result = collection.LocalMaxima().ToList();

            Assert.Equal(result.Count, 1);
            Assert.True(collection.SequenceEqual(result));
        }

        [Fact]
        public void GivenTwoItems_ReturnsLargestItem()
        {
            var collection = new[] { 1, 2 };
            var expected = new[] { 2 };

            var result = collection.LocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollectionWithSingleMaximum_ReturnsLargestItem()
        {
            var collection = new[] { 1, 2, 3, 2, 1 };
            var expected = new[] { 3 };

            var result = collection.LocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMaximumAtStart_ReturnsCorrectMaxima()
        {
            var collection = new[] { 5, 4, 3, 2, 1 };
            var expected = new[] { 5 };

            var result = collection.LocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMaximumAtEnd_ReturnsCorrectMaxima()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 5 };

            var result = collection.LocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenMaximaAtBothEnds_ReturnsCorrectMaxima()
        {
            var collection = new[] { 3, 2, 1, 2, 3 };
            var expected = new[] { 3, 3 };

            var result = collection.LocalMaxima();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCorrectMaxima()
        {
            var collection = new[] { 3, 1, 2, 3, 1, 2, 3, 2, 1, 3, 2 };
            var expected = new[] { 3, 3, 3, 3 };

            var result = collection.LocalMaxima();

            Assert.Equal(expected, result);
        }
    }
}