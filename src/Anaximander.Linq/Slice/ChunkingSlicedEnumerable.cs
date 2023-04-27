using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    internal class ChunkingSlicedEnumerable<T> : ISlicedEnumerable<T>
    {
        public ChunkingSlicedEnumerable(IEnumerable<T> source, int sliceSize, int? sliceCount = null)
        {
            if (sliceSize < 1)
            {
                throw new ArgumentException("Slice size must be equal to or greater than 1", nameof(sliceSize));
            }

            _sliceSize = sliceSize;
            _sliceCount = sliceCount;

            _chunked = source.Chunk(sliceSize);
        }

        private readonly int _sliceSize;
        private readonly int? _sliceCount;

        private readonly IEnumerable<T[]> _chunked;
        private IEnumerable<T> _remainder;

        public IEnumerable<IEnumerable<T>> Slices => GetSlices();
        public IEnumerable<T> Remainder => GetRemainder();

        public IEnumerator<IEnumerable<T>> GetEnumerator() => All().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IEnumerable<T>> GetSlices()
        {
            _remainder = Enumerable.Empty<T>();

            foreach ((IEnumerable<T> chunk, int index) in _chunked.Select((x, i) => (chunk: x, index: i)))
            {
                if ((_sliceCount is null || index < _sliceCount) && chunk.Count() == _sliceSize)
                {
                    yield return chunk;
                }
                else
                {
                    _remainder = _remainder.Concat(chunk);
                }
            }
        }

        private IEnumerable<T> GetRemainder()
        {
            if (_remainder == null)
            {
#pragma warning disable CA1806 // Do not ignore method results
                GetSlices().ToList();
#pragma warning restore CA1806 // Do not ignore method results
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