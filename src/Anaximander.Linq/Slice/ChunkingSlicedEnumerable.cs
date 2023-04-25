using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    internal class ChunkingSlicedEnumerable<T> : ISlicedEnumerable<T>
    {
        public ChunkingSlicedEnumerable(IEnumerable<T> source, int sliceSize)
        {
            if (sliceSize < 1)
            {
                throw new ArgumentException("Slice size must be equal to or greater than 1", nameof(sliceSize));
            }

            _sliceSize = sliceSize;
            _chunked = source.Chunk(sliceSize);
        }

        private readonly int _sliceSize;

        private readonly IEnumerable<T[]> _chunked;
        private IEnumerable<T> _remainder;

        public IEnumerable<IEnumerable<T>> Slices => GetSlices();
        public IEnumerable<T> Remainder => GetRemainder();

        public IEnumerator<IEnumerable<T>> GetEnumerator() => All().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IEnumerable<T>> GetSlices()
        {
            IEnumerable<T> last = Enumerable.Empty<T>();

            foreach (var chunkPair in _chunked.Window(2))
            {
                switch (chunkPair.Count())
                {
                    case 0:
                        yield break;

                    case 1:
                        if (chunkPair.First().Length == _sliceSize)
                        {
                            yield return chunkPair.First();
                        }
                        else
                        {
                            last = chunkPair.First();
                            yield break;
                        }
                        break;

                    default:
                        last = chunkPair.Last();
                        yield return chunkPair.First();

                        break;
                }
            }

            if (last is not null && last.Count() == _sliceSize)
            {
                yield return last;
            }

            _remainder = last;
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