using System.Collections.Generic;

namespace Anaximander.Linq
{
    /// <summary>
    /// An IEnumerable that randomly re-orders its contents each time it is enumerated.
    /// </summary>
    public interface IShufflingEnumerable<out T> : IEnumerable<T>
    {
    }
}