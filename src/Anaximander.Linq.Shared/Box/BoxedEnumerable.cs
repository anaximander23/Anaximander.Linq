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

                    if (windowEnumerator.Current == null)
                    {
                        yield return _source;
                        yield break;
                    }

                    T current = windowEnumerator.Current.First();
                    if (buffer == null)
                    {
                        buffer = new List<T> { current };
                    }

                    if (windowEnumerator.Current.Count() < 2)
                    {
                        yield return buffer;
                        yield break;
                    }

                    T next = windowEnumerator.Current.Last();

                    if (!endReached)
                    {
                        if (_sameBoxWhile(current, next))
                        {
                            buffer.Add(next);
                        }
                        else
                        {
                            yield return buffer;
                            buffer = new List<T> { next };
                        }
                    }
                } while (!endReached);

                yield return buffer;
            }
        }
    }
}