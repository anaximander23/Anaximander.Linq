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

        IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IEnumerable<T>> GetBoxes()
        {
            if (!_source.Any())
            {
                yield break;
            }
            
            using (IEnumerator<IEnumerable<T>> windowEnumerator = _source.Window(2).GetEnumerator())
            {
                List<T> buffer = null;
                bool endReached = !windowEnumerator.MoveNext();

                if (endReached)
                {
                    yield return _source;
                    yield break;
                }

                while (!endReached)
                {
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

                    endReached = !windowEnumerator.MoveNext();
                }

                yield return buffer;
            }
        }
    }
}