namespace Anaximander.Linq.TreeTraversal
{
    /// <summary>
    /// How to handle data structures that contain cyclic references
    /// </summary>
    public enum CyclicGraphBehaviour
    {
        /// <summary>
        /// Throw an error if the structure contains cycles
        /// </summary>
        Error,

        /// <summary>
        /// Truncate a path when the cyclic reference is detected, and move on
        /// </summary>
        Truncate
    }
}