using System.Collections.Generic;

namespace Anaximander.Linq
{
    public interface IShufflingEnumerable<out T> : IEnumerable<T>
    {
    }
}