using System.Collections.Generic;
using System.Linq;
using Anaximander.Linq.TreeTraversal;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class TreeTraversalTests
    {
        [Fact]
        public void WhenTraversingSimpleTree_FindsCorrectPaths()
        {
            NTreeNode tree = GenerateTree();

            IEnumerable<IEnumerable<string>> paths = Generate
                .Traversals(tree, node => node.Children)
                .Select(path => path.Select(node => node.Label).ToArray())
                .ToArray();

            string[][] expected = new[]
            {
                new[] { "A", "B", "D", "H" },
                new[] { "A", "B", "D", "I" },
                new[] { "A", "B", "E", "J" },
                new[] { "A", "B", "E", "K" },
                new[] { "A", "C", "F", "L" },
                new[] { "A", "C", "F", "M" },
                new[] { "A", "C", "G", "N" },
                new[] { "A", "C", "G", "O" },
            };

            Assert.Equal(expected, paths);
        }

        [Fact]
        public void WhenTraversingLoopingTree_WithCycleHandlingTruncate_FindsCorrectPaths()
        {
            NTreeNode tree = GenerateTreeWithCycle();

            IEnumerable<IEnumerable<string>> paths = Generate
                .Traversals(tree, node => node.Children, CyclicGraphBehaviour.Truncate)
                .Select(path => path.Select(node => node.Label).ToArray())
                .ToArray();

            string[][] expected = new[]
            {
                new[] { "A", "B", "D", "H" },
                new[] { "A", "B", "D", "I" },
                new[] { "A", "B", "E", "J" },
                new[] { "A", "C", "F", "L" },
                new[] { "A", "C", "F", "M" },
                new[] { "A", "C", "G", "N" },
                new[] { "A", "C", "G", "O" },
                new[] { "A", "B", "E", "K", "B" }
            };

            Assert.Equal(expected, paths);
        }

        [Fact]
        public void WhenTraversingLoopingTree_WithCycleHandlingThrow_ThrowsCyclicGraphException()
        {
            NTreeNode tree = GenerateTreeWithCycle();

            Assert.Throws<CyclicGraphException>(() => Generate.Traversals(tree, node => node.Children, CyclicGraphBehaviour.Throw).ToList());
        }

        private static NTreeNode GenerateTree()
        {
            return new NTreeNode
            {
                Label = "A",
                Children = new[]
                {
                    new NTreeNode
                    {
                        Label = "B",
                        Children = new[]
                        {
                            new NTreeNode
                            {
                                Label = "D",
                                Children = new[]
                                {
                                    new NTreeNode
                                    {
                                        Label = "H"
                                    },
                                    new NTreeNode
                                    {
                                        Label = "I"
                                    }
                                }
                            },
                            new NTreeNode
                            {
                                Label = "E",
                                Children = new[]
                                {
                                    new NTreeNode
                                    {
                                        Label = "J"
                                    },
                                    new NTreeNode
                                    {
                                        Label = "K"
                                    }
                                }
                            }
                        }
                    },
                    new NTreeNode
                    {
                        Label = "C",
                        Children = new[]
                        {
                            new NTreeNode
                            {
                                Label = "F",
                                Children = new[]
                                {
                                    new NTreeNode
                                    {
                                        Label = "L"
                                    },
                                    new NTreeNode
                                    {
                                        Label = "M"
                                    }
                                }
                            },
                            new NTreeNode
                            {
                                Label = "G",
                                Children = new[]
                                {
                                    new NTreeNode
                                    {
                                        Label = "N"
                                    },
                                    new NTreeNode
                                    {
                                        Label = "O"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static NTreeNode GenerateTreeWithCycle()
        {
            NTreeNode tree = GenerateTree();

            IEnumerable<NTreeNode> flat = new[] { tree };
            for (int i = 0; i < 3; i++)
            {
                flat = flat.Concat(flat.SelectMany(n => n.Children));
            }

            flat = flat.Distinct();

            NTreeNode loopFrom = flat.SingleOrDefault(n => n.Label == "K");
            NTreeNode loopTo = flat.SingleOrDefault(n => n.Label == "B");

            loopFrom.Children = new[] { loopTo };

            return tree;
        }
    }

    internal class NTreeNode
    {
        public string Label { get; set; }

        public IEnumerable<NTreeNode> Children { get; set; }
    }
}