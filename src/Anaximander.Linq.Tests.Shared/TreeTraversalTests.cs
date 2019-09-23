using Anaximander.Linq.TreeTraversal;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anaximander.Linq.Tests
{
    public class TreeTraversalTests
    {
        [Fact]
        public void WhenTraversingSimpleTree_FindsCorrectPaths()
        {
            IEnumerable<NTreeNode> tree = GenerateTree();

            IEnumerable<IEnumerable<string>> paths = TreeTraverser
                .GetTraversalPaths(tree.First(), node => node.Children)
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

        private IEnumerable<NTreeNode> GenerateTree()
        {
            return new[]
            {
                new NTreeNode
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
                }
            };
        }
    }

    internal class NTreeNode
    {
        public string Label { get; set; }

        public IEnumerable<NTreeNode> Children { get; set; }
    }
}