using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class SlicesTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.Slices(3).ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ThrowsInvalidOperationException()
        {
            IEnumerable<int> collection = new List<int>();

            Assert.Throws<InvalidOperationException>(() => collection.Slices(3));
        }

        [Fact]
        public void GivenCollectionSizeSmallerThanSliceSize_ThrowsInvalidOperationException()
        {
            var collection = new[] { 1, 2, 3 };

            Assert.Throws<InvalidOperationException>(() => collection.Slices(4));
        }

        [Fact]
        public void GivenCollectionSizeEqualToSliceSize_RemainderIsEmpty()
        {
            var collection = new[] { 1, 2, 3 };

            var result = collection.Slices(3);

            Assert.Equal(3, result.Slices.Count());
            Assert.Empty(result.Remainder.ToArray());
        }

        [Fact]
        public void GivenRemainderSizeEqualToSliceSize_NumberOfSlicesIsCorrect()
        {
            var collection = new[] { 1, 2, 3 };

            var expectedSlices = new[]
            {
                new[] { 1 },
                new[] { 2 }
            };

            var expectedRemainder = new[] { 3 };

            var result = collection.Slices(2);

            var resultSlices = result.Slices
                .Select(x => x.ToArray())
                .ToArray();

            var resultRemainder = result.Remainder
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
            Assert.Equal(new[] { 3 }, resultRemainder);
        }

        [Fact]
        public void GivenCollectionSizeGreaterThanSliceSize_ReturnsRemainderOfCorrectSize()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var expectedSlices = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7, 8, 9 }
            };

            var expectedRemainder = new[] { 0 };

            var result = collection.Slices(3);

            var resultSlices = result.Slices
                .Select(x => x.ToArray())
                .ToArray();

            var resultRemainder = result.Remainder
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
            Assert.Equal(expectedRemainder, resultRemainder);
        }

        [Fact]
        public void GivenCollectionSizeIntegerMultipleOfSliceSize_ReturnsCorrectSlices()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var expectedSlices = new[]
            {
                new[] { 1, 2, 3, 4, 5 },
                new[] { 6, 7, 8, 9, 0 }
            };

            var result = collection.Slices(2);

            var resultSlices = result.Slices
                .Select(x => x.ToArray())
                .ToArray();

            var resultRemainder = result.Remainder
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
            Assert.Empty(resultRemainder);
        }
    }
}