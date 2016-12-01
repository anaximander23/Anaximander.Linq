using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    internal class BoxedEnumerable<T> : IBoxedEnumerable<T>
    {
        public BoxedEnumerable(IEnumerable<T> source, Func<T, T, bool> sameBoxWhile)
        {
            _source = source;
            _sameBoxWhile = sameBoxWhile;
        }

        private readonly IEnumerable<T> _source;
        private readonly Func<T, T, bool> _sameBoxWhile;

        public IEnumerator<IEnumerable<T>> GetEnumerator() => GetBoxes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IEnumerable<T>> GetBoxes()
        {
            using (IEnumerator<IEnumerable<T>> windowEnumerator = _source.Window(2).GetEnumerator())
            {
                List<T> buffer = null;
                var endReached = false;

                do
                {
                    endReached = !windowEnumerator.MoveNext();

                    if (buffer == null)
                    {
                        buffer = new List<T>();
                    }

                    T current = default(T);

                    if (windowEnumerator.Current == null)
                    {
                        yield break;
                    }

                    if (windowEnumerator.Current.Count() == 1)
                    {
                        buffer.Add(current);
                        yield return buffer;
                        yield break;
                    }

                    current = windowEnumerator.Current.First();
                    buffer.Add(current);

                    T next = windowEnumerator.Current.Last();

                    if (!_sameBoxWhile(current, next))
                    {
                        yield return buffer;
                        buffer = new List<T>();
                    }
                } while (!endReached);

                yield return buffer;
            }
        }
    }
}