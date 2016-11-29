using System.Collections.Generic;

namespace Anaximander.Linq
{
    public interface IBoxedEnumerable<out T> : IEnumerable<IEnumerable<T>>
    {
    }
}