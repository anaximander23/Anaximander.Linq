using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    internal class SlicedEnumerable<T> : ISlicedEnumerable<T>
    {
        public SlicedEnumerable(IEnumerable<T> source, int sliceSize, int maxNumberOfSlices = 0)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (sliceSize < 1)
            {
                throw new ArgumentException("Slice size must be equal to or greater than 1", nameof(sliceSize));
            }

            _sliceSize = sliceSize;
            _maxNumberOfSlices = maxNumberOfSlices;

            _source = source;
            _processedSlices = new List<IEnumerable<T>>();
        }

        private readonly int _sliceSize;
        private readonly int _maxNumberOfSlices;
        private readonly IEnumerable<T> _source;
        private readonly List<IEnumerable<T>> _processedSlices;
        private IEnumerable<T> _remainder;
        public IEnumerable<IEnumerable<T>> Slices => GetSlices();
        public IEnumerable<T> Remainder => GetRemainder();

        public IEnumerator<IEnumerable<T>> GetEnumerator() => All().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IEnumerable<T>> GetSlices()
        {
            var buffer = new List<T>(_sliceSize);
            var remainder = new List<T>();

            int slicesProcessed = 0;

            using (var enumerator = _source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if ((_maxNumberOfSlices == 0) || (slicesProcessed < _maxNumberOfSlices))
                    {
                        buffer.Add(enumerator.Current);

                        if (buffer.Count == _sliceSize)
                        {
                            yield return buffer;
                            slicesProcessed++;

                            buffer = new List<T>(_sliceSize);
                        }
                    }
                    else
                    {
                        remainder.Add(enumerator.Current);
                    }
                }

                remainder.InsertRange(0, buffer);
                _remainder = remainder;
            }
        }

        private IEnumerable<T> GetRemainder()
        {
            if (_remainder == null)
            {
                GetSlices().ToList();
            }

            foreach (T item in _remainder)
            {
                yield return item;
            }
        }

        private IEnumerable<IEnumerable<T>> All()
        {
            foreach (IEnumerable<T> slice in Slices)
            {
                yield return slice;
            }
            if ((Remainder != null) && Remainder.Any())
            {
                yield return Remainder;
            }
        }
    }
}