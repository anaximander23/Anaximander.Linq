using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class FilterTests
    {
        [Fact]
        public void CanFilterIncludeEmptyCollection()
        {
            IEnumerable<TestItem> source = Enumerable.Empty<TestItem>();

            IEnumerable<int> keys = new[] { 1, 4, 9, 17 };

            IEnumerable<TestItem> filtered = source.FilterInclude(x => x.Score, keys);

            Assert.Empty(filtered);
        }

        [Fact]
        public void CanFilterIncludeWithNoKeys()
        {
            IEnumerable<TestItem> source = GetSourceCollection(10);

            IEnumerable<int> keys = Enumerable.Empty<int>();

            IEnumerable<TestItem> filtered = source.FilterInclude(x => x.Score, keys);

            Assert.Empty(filtered);
        }

        [Fact]
        public void CanFilterIncludeCollection()
        {
            IEnumerable<TestItem> source = GetSourceCollection(20);

            IEnumerable<int> keys = new[] { 1, 4, 9, 17 };

            IEnumerable<TestItem> filtered = source.FilterInclude(x => x.Score, keys);

            Assert.Collection(filtered,
                x => Assert.Equal(1, x.Score),
                x => Assert.Equal(4, x.Score),
                x => Assert.Equal(9, x.Score),
                x => Assert.Equal(17, x.Score)
            );
        }

        [Fact]
        public void CanFilterIncludeEntireCollection()
        {
            IEnumerable<TestItem> source = GetSourceCollection(5);

            IEnumerable<int> keys = new[] { 0, 1, 2, 3, 4 };

            IEnumerable<TestItem> filtered = source.FilterInclude(x => x.Score, keys);

            Assert.Equal(source, filtered);
        }

        [Fact]
        public void CanFilterExcludeEmptyCollection()
        {
            IEnumerable<TestItem> source = Enumerable.Empty<TestItem>();

            IEnumerable<int> keys = new[] { 1, 4, 9, 17 };

            IEnumerable<TestItem> filtered = source.FilterExclude(x => x.Score, keys);
        }

        [Fact]
        public void CanFilterExcludeWithNoKeys()
        {
            IEnumerable<TestItem> source = GetSourceCollection(10);

            IEnumerable<int> keys = Enumerable.Empty<int>();

            IEnumerable<TestItem> filtered = source.FilterExclude(x => x.Score, keys);

            Assert.Equal(source, filtered);
        }

        [Fact]
        public void CanFilterExcludeCollection()
        {
            IEnumerable<TestItem> source = GetSourceCollection(10);

            IEnumerable<int> keys = new[] { 1, 3, 5, 8, 9 };

            IEnumerable<TestItem> filtered = source.FilterExclude(x => x.Score, keys);

            Assert.Collection(filtered,
                x => Assert.Equal(0, x.Score),
                x => Assert.Equal(2, x.Score),
                x => Assert.Equal(4, x.Score),
                x => Assert.Equal(6, x.Score),
                x => Assert.Equal(7, x.Score)
            );
        }

        [Fact]
        public void CanFilterExcludeEntireCollection()
        {
            IEnumerable<TestItem> source = GetSourceCollection(5);

            IEnumerable<int> keys = new[] { 0, 1, 2, 3, 4 };

            IEnumerable<TestItem> filtered = source.FilterExclude(x => x.Score, keys);

            Assert.Empty(filtered);
        }

        private static IEnumerable<TestItem> GetSourceCollection(int count)
        {
            return Enumerable.Range(0, count)
                .Select(x => new TestItem(x))
                .ToList();
        }

        private class TestItem
        {
            public TestItem(int score)
            {
                Id = Guid.NewGuid();

                Score = score;
            }

            public Guid Id { get; }

            public int Score { get; }
        }
    }
}