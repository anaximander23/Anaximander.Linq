using System;
using System.Collections;

namespace Anaximander.Linq
{
    internal abstract class ShufflingEnumerable : IEnumerable
    {
        protected static readonly Random Random = new Random();

        public abstract IEnumerator GetEnumerator();
    }
}