using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    internal class AggregateBoxedEnumerable<T> : IBoxedEnumerable<T>
    {
        public AggregateBoxedEnumerable(IEnumerable<T> source, Func<IEnumerable<T>, bool> shouldContinue)
        {
            _source = source;
            _shouldContinue = shouldContinue;
        }

        private readonly IEnumerable<T> _source;
        private readonly Func<IEnumerable<T>, bool> _shouldContinue;

        public IEnumerator<IEnumerable<T>> GetEnumerator()
            => GetBoxes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private IEnumerable<IEnumerable<T>> GetBoxes()
        {
            if (!_source.Any())
            {
                yield break;
            }

            using (IEnumerator<T> windowEnumerator = _source.GetEnumerator())
            {
                Stack<T> buffer = null;

                while (windowEnumerator.MoveNext())
                {
                    T nextElement = windowEnumerator.Current;

                    if (buffer == null)
                    {
                        buffer = new Stack<T>();
                    }

                    if (_shouldContinue(buffer))
                    {
                        buffer.Push(nextElement);

                        if (!_shouldContinue(buffer))
                        {
                            buffer.Pop();
                            
                            var nextBox = buffer.ToList();
                            nextBox.Reverse();
                            yield return nextBox;

                            buffer.Clear();
                            buffer.Push(nextElement);
                        }
                    }
                }

                if (_shouldContinue(buffer))
                {
                    yield return buffer;
                }
            }
        }
    }
}