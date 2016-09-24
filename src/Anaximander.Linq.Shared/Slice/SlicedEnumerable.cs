using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public class SlicedEnumerable<T> : ISlicedEnumerable<T>
    {
        public SlicedEnumerable(IEnumerable<T> source, int sliceSize)
        {
            _source = source;
            _sliceSize = sliceSize;

            _windowEnumerator = _source.Window(_sliceSize).GetEnumerator();

            _processedSlices = new List<IEnumerable<T>>();
        }

        public IEnumerable<IEnumerable<T>> Slices => GetSlices();
        public IEnumerable<T> Remainder => GetRemainder();

        private readonly IEnumerable<T> _source;
        private readonly int _sliceSize;

        private readonly IEnumerator<IEnumerable<T>> _windowEnumerator;
        private readonly List<IEnumerable<T>> _processedSlices;
        private IEnumerable<T> _remainder;

        private IEnumerable<IEnumerable<T>> GetSlices()
        {
            int sliceIndex = 0;
            int moved = 0;

            while (sliceIndex < _processedSlices.Count)
            {
                yield return _processedSlices[sliceIndex];
                sliceIndex = sliceIndex + 1;
            }

            bool endReached = false;

            do
            {
                endReached = !_windowEnumerator.MoveNext();
                moved = moved + 1;

                if (!endReached)
                {
                    if ((moved == _sliceSize) || (sliceIndex == 0))
                    {
                        moved = 0;
                        sliceIndex = sliceIndex + 1;

                        var current = _windowEnumerator.Current.ToList();
                        _processedSlices.Add(current);
                        yield return current;
                    }
                }
                else
                {
                    _remainder = _windowEnumerator.Current.Skip(_sliceSize - moved);
                }
            } while (!endReached);
        }

        private IEnumerable<T> GetRemainder()
        {
            if (_remainder == null)
            {
                GetSlices().ToList();
            }
            if (_remainder == null)
            {
                _remainder = new List<T>();
            }

            foreach (var item in _remainder)
            {
                yield return item;
            }
        }

        private IEnumerable<IEnumerable<T>> All()
        {
            foreach (var slice in Slices)
            {
                yield return slice;
            }
            if ((Remainder != null) && Remainder.Any())
            {
                yield return Remainder;
            }
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator() => All().GetEnumerator();

        IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}