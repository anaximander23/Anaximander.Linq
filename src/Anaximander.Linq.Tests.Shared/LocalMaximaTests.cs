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
    }
}