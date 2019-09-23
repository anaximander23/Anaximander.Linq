using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq.TreeTraversal
{
    public static class TreeTraverser
    {
        // Adapted from code by MSDN user OlofPetterson
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/63282240-0fbb-40c1-a716-1371e77a991d/getting-all-paths-in-tree-traversal#5819bb97-34cb-49ef-b202-031495157590-isAnswer
        // retrieved 2019-09-23 12:32
        public static IEnumerable<IEnumerable<TElement>> GetTraversalPaths<TElement>(TElement root, Func<TElement, IEnumerable<TElement>> childrenSelector)
        {
            Queue<PathBuilder<TElement>> pathBuilder = new Queue<PathBuilder<TElement>>();
            pathBuilder.Enqueue(new PathBuilder<TElement>(root));

            while (pathBuilder.Any())
            {
                PathBuilder<TElement> node = pathBuilder.Dequeue();
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

        public class PathBuilder<TElement>
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

            private readonly Queue<TElement> _path;

            public IEnumerable<TElement> Path { get => _path; }
            public TElement Current => _path.Last();

            public PathBuilder<TElement> Append(TElement next)
            {
                return new PathBuilder<TElement>(Path, next);
            }
        }
    }
}