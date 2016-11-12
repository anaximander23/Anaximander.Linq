using System.Collections.Generic;

namespace Anaximander.Linq
{
    public interface IBoxedEnumerable<T> : IEnumerable<IEnumerable<T>>
    {
    }
}