using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public class SlicedEnumerable<T> : ISlicedEnumerable<T>
    {
        public SlicedEnumerable(IEnumerable<T> source, int sliceSize)
        {
            _sliceSize = sliceSize;

            _windowEnumerator = source.Window(_sliceSize).GetEnumerator();

            _processedSlices = new List<IEnumerable<T>>();
        }

        public IEnumerable<IEnumerable<T>> Slices => GetSlices();
        public IEnumerable<T> Remainder => GetRemainder();

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

                if (!endReached)
                {
                    moved = moved + 1;

                    if ((moved == _sliceSize) || (sliceIndex == 0))
                    {
                        moved = 0;
                        sliceIndex = sliceIndex + 1;

                        List<T> current = _windowEnumerator.Current.ToList();

                        if (current.Count() == _sliceSize)
                        {
                            _processedSlices.Add(current);
                            yield return current;
                        }
                    }
                }
            } while (!endReached);

            _remainder = _windowEnumerator.Current.Skip(_sliceSize - moved).ToList();
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

        public IEnumerator<IEnumerable<T>> GetEnumerator() => All().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}