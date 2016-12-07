using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class SlicesOfTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.SlicesOf(3).ToList());
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<int> collection = new List<int>();

            ISlicedEnumerable<int> result = collection.SlicesOf(3);

            Assert.Empty(result.Slices);
            Assert.Empty(result.Remainder);
            Assert.Empty(result.ToList());
        }

        [Fact]
        public void AskedForSliceSizeOfZero_ThrowsArgumentException()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() => collection.SlicesOf(0).ToList());
        }

        [Fact]
        public void AskedForNegativeSliceSize_ThrowsArgumentException()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() => collection.SlicesOf(-1).ToList());
        }

        [Fact]
        public void GivenCollectionSizeEqualToSliceSize_RemainderIsEmpty()
        {
            var collection = new[] { 1, 2, 3 };

            var result = collection.SlicesOf(3);

            Assert.Empty(result.Remainder.ToArray());
        }

        [Fact]
        public void GivenCollectionSizeOneGreaterThanSliceSize_ReturnsRemainderOfOne()
        {
            var collection = new[] { 1, 2, 3, 4 };
            var expectedRemainder = new[] { 4 };

            var result = collection.SlicesOf(3);

            Assert.Equal(expectedRemainder, result.Remainder.ToArray());
        }

        [Fact]
        public void GivenCollectionSizeGreaterThanSliceSize_ReturnsRemainderOfCorrectSize()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var expectedRemainder = new[] { 8, 9, 0 };

            var result = collection.SlicesOf(7);

            Assert.Equal(expectedRemainder, result.Remainder.ToArray());
        }

        [Fact]
        public void GivenCollectionSizeGreaterThanMultipleSliceSizes_ReturnsRemainderOfCorrectSize()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5 };
            var expectedRemainder = new[] { 3, 4, 5 };

            var result = collection.SlicesOf(4);

            Assert.Equal(expectedRemainder, result.Remainder.ToArray());
        }

        [Fact]
        public void GivenCollectionSizeEqualToSliceSize_ReturnsCorrectSlice()
        {
            var collection = new[] { 1, 2, 3 };
            var expectedSlices = new[] { new[] { 1, 2, 3 } };

            var resultSlices = collection.SlicesOf(3)
                .Slices
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
        }

        [Fact]
        public void GivenCollectionSizeOneGreaterThanSliceSize_ReturnsCorrectSlice()
        {
            var collection = new[] { 1, 2, 3, 4 };
            var expectedSlices = new[] { new[] { 1, 2, 3 } };

            var resultSlices = collection.SlicesOf(3)
                .Slices
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
        }

        [Fact]
        public void GivenCollectionSizeGreaterThanSliceSize_ReturnsCorrectSlice()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expectedSlices = new[]
            {
                new[] { 1 ,2, 3 }
            };

            var resultSlices = collection.SlicesOf(3)
                .Slices
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
        }

        [Fact]
        public void GivenCollectionSizeGreaterThanMultipleSliceSizes_ReturnsCorrectSlices()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var expectedSlices = new[]
            {
                new[] { 1 ,2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7, 8, 9 }
            };

            var resultSlices = collection.SlicesOf(3)
                .Slices
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
        }

        [Fact]
        public void GivenEnumeratedSlices_SlicesAreTheSameOnSubsequentEvaluations()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 67, 8, 9, 0 };

            var sliced = collection.SlicesOf(3);

            var slicesFirst = sliced.Slices.ToList();
            var remainderFirst = sliced.Remainder.ToList();

            var slicesSecond = sliced.Slices.ToList();
            var remainderSecond = sliced.Remainder.ToList();

            Assert.Equal(slicesFirst, slicesSecond);
            Assert.Equal(remainderFirst, remainderSecond);
        }

        [Fact]
        public void GivenValidCollection_SlicedOutputIsEnumerable()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expectedSlices = new[]
            {
                new[] { 1 ,2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7, 8, 9 },
                new[] { 10 }
            };

            var resultSlices = collection.SlicesOf(3)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expectedSlices, resultSlices);
        }
    }
}