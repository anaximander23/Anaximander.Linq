using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class ShuffleTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.Shuffle().ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<int> collection = new List<int>();

            var result = collection.Shuffle();

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleItem_ReturnsThatItem()
        {
            var collection = new[] { 1 };

            List<int> result = collection.Shuffle().ToList();

            Assert.Equal(collection, result);
        }

        [Fact]
        public void GivenValidCollection_TwoShufflesProduceDifferentResults()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };

            var result1 = collection.Shuffle().ToList();
            var result2 = collection.Shuffle().ToList();

            Assert.NotEqual(result1, result2);
        }
    }
}