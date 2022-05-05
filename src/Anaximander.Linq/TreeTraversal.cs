using System;
using System.Collections.Generic;

namespace Anaximander.Linq.TreeTraversal
{
    /// <summary>
    /// Provides methods for working with tree-like data structures
    /// </summary>
    public static class TreeTraverser
    {
        // Adapted from code by MSDN user OlofPetterson
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/63282240-0fbb-40c1-a716-1371e77a991d/getting-all-paths-in-tree-traversal#5819bb97-34cb-49ef-b202-031495157590-isAnswer
        // retrieved 2019-09-23 12:32

        /// <summary>
        /// Finds all of the ways that a treelike data structure can be linearly traversed
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the structure</typeparam>
        /// <param name="root">The element from which to begin all paths</param>
        /// <param name="childrenSelector">A selector that traverses from one element to its immediate children</param>
        /// <param name="cyclicGraphBehaviour">How to handle data structures that contain cyclic references</param>
        /// <returns>A collection of all the linear paths that can be taken through the structure in the parent-to-child direction</returns>
        ///
        [Obsolete("Moved to Generate.Traversals(); will be removed in future release.")]
        public static IEnumerable<IEnumerable<TElement>> GetTraversalPaths<TElement>(
            TElement root,
            Func<TElement, IEnumerable<TElement>> childrenSelector,
            CyclicGraphBehaviour cyclicGraphBehaviour = CyclicGraphBehaviour.Throw)
        {
            return Generate.Traversals(root, childrenSelector, cyclicGraphBehaviour);
        }
    }
}