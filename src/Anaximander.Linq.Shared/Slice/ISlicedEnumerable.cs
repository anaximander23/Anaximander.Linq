using System.Collections.Generic;

namespace Anaximander.Linq
{
    /// <summary>
    /// An enumerable partitioned into slices, with a (possibly empty) remainder.
    /// Can be enumerated directly, in which case <see cref="Slices"/> are enumerated first, then <see cref="Remainder"/>.
    /// </summary>
    public interface ISlicedEnumerable<out T> : IEnumerable<IEnumerable<T>>
    {
        /// <summary>
        /// The equally-sized partitions of the source collection.
        /// </summary>
        IEnumerable<IEnumerable<T>> Slices { get; }

        /// <summary>
        /// The remaining items that could not be evenly divided into <see cref="Slices"/>.
        /// </summary>
        IEnumerable<T> Remainder { get; }
    }
}