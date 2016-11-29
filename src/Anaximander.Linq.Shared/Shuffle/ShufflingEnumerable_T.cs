using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    internal class ShufflingEnumerable<T> : ShufflingEnumerable, IShufflingEnumerable<T>
    {
        public ShufflingEnumerable(IEnumerable<T> source)
        {
            _source = source;
        }

        private readonly IEnumerable<T> _source;

        private IEnumerator<T> GetRandomEnumerator()
        {
            return _source.OrderBy(x => ShufflingEnumerable.Random.Next()).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetRandomEnumerator();

        public override IEnumerator GetEnumerator() => GetRandomEnumerator();
    }
}