﻿using System;
using System.Collections.Generic;
using System.Linq;
using Anaximander.Linq.TreeTraversal;

namespace Anaximander.Linq
{
    /// <summary>
    /// Provides methods for generating collections or sequences of data.
    /// </summary>
	public static class Generate
    {
        /// <summary>
        /// Generates a sequence of values by applying a function to the previous value in the sequence.
        /// </summary>
        /// <param name="first">The starting value for the sequence</param>
        /// <param name="getNext">A function that transforms a value in the sequence into the following value</param>
        /// <param name="shouldContinue">A predicate that takes the current value in the sequence and returns true as long as the sequence should continue, and false when it should stop</param>
        /// <returns></returns>
		public static IEnumerable<T> Sequence<T>(T first, Func<T, T> getNext, Func<T, bool> shouldContinue)
        {
            if (shouldContinue(first))
            {
                yield return first;
            }

            var current = first;

            while (shouldContinue(current))
            {
                current = getNext(current);
                yield return current;
            }
        }

        /// <summary>
        /// Generates a sequence of values by applying a function to the previous value in the sequence.
        /// </summary>
        /// <param name="first">The starting value for the sequence</param>
        /// <param name="getNext">A function that transforms a value in the sequence into the following value</param>
        /// <returns></returns>
        public static IEnumerable<T> Sequence<T>(T first, Func<T, T> getNext)
            => Sequence(first, getNext, x => true);

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
        public static IEnumerable<IEnumerable<TElement>> Traversals<TElement>(
            TElement root,
            Func<TElement, IEnumerable<TElement>> childrenSelector,
            CyclicGraphBehaviour cyclicGraphBehaviour = CyclicGraphBehaviour.Throw)
        {
            Queue<PathBuilder<TElement>> pathBuilder = new Queue<PathBuilder<TElement>>();
            pathBuilder.Enqueue(new PathBuilder<TElement>(root));

            while (pathBuilder.Any())
            {
                PathBuilder<TElement> node = pathBuilder.Dequeue();

                if (node.IsCyclic)
                {
                    switch (cyclicGraphBehaviour)
                    {
                        case CyclicGraphBehaviour.Truncate:
                            yield return node.Path;
                            continue;

                        default:
                        case CyclicGraphBehaviour.Throw:
                            throw new CyclicGraphException();
                    }
                }

                IEnumerable<TElement> children = childrenSelector(node.Current);

                var isLeafNode = (children is null) || !children.Any();

                if (isLeafNode)
                {
                    yield return node.Path;
                }
                else
                {
                    foreach (TElement child in children)
                    {
                        pathBuilder.Enqueue(node.Append(child));
                    }
                }
            }
        }

        /// <summary>
        /// Generates the Fibonacci sequence. Note that after 93 iterations, the sequence will overflow a 64-bit unsigned integer.
        /// </summary>
        public static IEnumerable<ulong> Fibonacci()
        {
            return Sequence(
                    (current: 0ul, next: 1ul),
                    x =>
                    {
                        checked
                        {
                            return (x.next, x.current + x.next);
                        }
                    })
                .Select(x => x.current);
        }

        /// <summary>
        /// Returns the nth Fibonacci number (zero-indexed). Note that indexes above 93 will overflow a 64-bit unsigned integer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ulong Fibonacci(int index)
        {
            return Fibonacci()
                .Skip(index)
                .Take(1)
                .Single();
        }

        internal class PathBuilder<TElement>
        {
            public PathBuilder(TElement root)
            {
                _path = new Queue<TElement>();
                _path.Enqueue(root);
            }

            public PathBuilder(IEnumerable<TElement> path, TElement current)
            {
                _path = new Queue<TElement>(path);
                _path.Enqueue(current);
            }

            public PathBuilder(IEnumerable<TElement> path, TElement current, bool cyclic)
            {
                _path = new Queue<TElement>(path);
                _path.Enqueue(current);

                IsCyclic = cyclic;
            }

            public readonly bool IsCyclic;
            private readonly Queue<TElement> _path;
            public IEnumerable<TElement> Path { get => _path; }
            public TElement Current => _path.Last();

            public PathBuilder<TElement> Append(TElement next)
            {
                bool cyclic = false;

                if (Path.Contains(next))
                {
                    cyclic = true;
                }

                return new PathBuilder<TElement>(Path, next, cyclic);
            }
        }
    }
}