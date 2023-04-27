using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class AggregateBoxWhileTests
    {
        [Fact]
        public void ShouldAggregateBoxes()
        {
            var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var expected = new[] {
                new[]{ 1, 2, 3 },
                new[]{ 4, 5, 6 },
                new[]{ 7, 8, 9 },
                new[]{ 10 }
            };

            var actual = source.AggregateBoxWhile((IEnumerable<int> box) => box.Count() < 4);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldAggregateBoxesOnSum()
        {
            var source = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var expected = new[] {
                new[]{ 1, 2, 3, 4 },
                new[]{ 5, 6 },
                new[]{ 7 },
                new[]{ 8 },
                new[]{ 9 },
                new[]{ 10 },
                new[]{ 11 }
            };

            var actual = source.AggregateBoxWhile((IEnumerable<int> box) => box.Sum() < 12);

            Assert.Equal(expected, actual);
        }
    }
}