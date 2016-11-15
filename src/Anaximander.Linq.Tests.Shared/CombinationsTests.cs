﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests.Shared
{
    public class CombinationsTests
    {
        [Fact]
        public void GivenNullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<object> collection = null;

            Assert.Throws<ArgumentNullException>(() => collection.Combinations(1));
        }

        [Fact]
        public void GivenEmptyCollection_ReturnsEmptyCollection()
        {
            IEnumerable<object> collection = new List<object>();

            var result = collection.Combinations(1);

            Assert.Empty(result);
        }

        [Fact]
        public void GivenSingleElement_ReturnsThatElementAsOnlyCombination()
        {
            IEnumerable<int> collection = new[] { 1 };
            var expected = new[] { new[] { 1 } };

            var result = collection.Combinations(1);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenCombinationSizeOfOne_ReturnsIndividualElements()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[]
            {
                new[] { 1 },
                new[] { 2 },
                new[] { 3 },
                new[] { 4 },
                new[] { 5 },
            };

            var result = collection.Combinations(1)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenValidCollection_ReturnsCombinations()
        {
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            var expected = new[]
            {
                new[] { 1, 2 },
                new[] { 1, 3 },
                new[] { 2, 1 },
                new[] { 2, 3 },
                new[] { 3, 1 },
                new[] { 3, 2 },
            };

            var result = collection.Combinations(2)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.Equal(expected, result);
        }
    }
}