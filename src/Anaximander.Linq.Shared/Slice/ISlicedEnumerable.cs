using System.Collections.Generic;

namespace Anaximander.Linq
{
    public interface ISlicedEnumerable<T> : IEnumerable<IEnumerable<T>>
    {
        IEnumerable<IEnumerable<T>> Slices { get; }
        IEnumerable<T> Remainder { get; }
    }
}