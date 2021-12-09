using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using QuickGraph;
using QuickGraph.Graphviz;

namespace AVL
{
    public class AvlTree<T> : BinaryTree<T>
        where T : IComparable
    {
        public new AvlTreeNode<T> Root
        {
            get => (AvlTreeNode<T>) base.Root;
            set => base.Root = value;
        }

        public new AvlTreeNode<T> Find(T value)
        {
            return (AvlTreeNode<T>) base.Find(value);
        }

        public override void Add(T value)
        {
            var node = new AvlTreeNode<T>(value);

            base.Add(node);

            var parentNode = node.Parent;

            while (parentNode != null)
            {
                var balance = GetBalance(parentNode);
                if (Math.Abs(balance) == 2)
                {
                    BalanceAt(parentNode, balance);
                }

                parentNode = parentNode.Parent;
            }
        }

        public override bool Remove(T value)
        {
            var valueNode = Find(value);
            return Remove(valueNode);
        }

        protected new bool Remove(BinaryTreeNode<T> removeNode)
        {
            return Remove((AvlTreeNode<T>) removeNode);
        }

        public bool Remove(AvlTreeNode<T> valueNode)
        {
            var parentNode = valueNode.Parent;

            var removed = base.Remove(valueNode);

            if (!removed)
                return false;

            while (parentNode != null)
            {
                var balance = GetBalance(parentNode);

                if (Math.Abs(balance) == 1)
                    break;

                if (Math.Abs(balance) == 2)
                    BalanceAt(parentNode, balance);

                parentNode = parentNode.Parent;
            }

            return true;
        }

        protected virtual void BalanceAt(AvlTreeNode<T> node, int balance)
        {
            switch (balance)
            {
                case 2:
                {
                    var rightBalance = GetBalance(node.RightChild);

                    switch (rightBalance)
                    {
                        case 1:
                        case 0:
                            RotateLeft(node);
                            break;
                        case -1:
                            RotateRight(node.RightChild);

                            RotateLeft(node);
                            break;
                    }

                    break;
                }
                case -2:
                {
                    var leftBalance = GetBalance(node.LeftChild);
                    switch (leftBalance)
                    {
                        case 1:
                            RotateLeft(node.LeftChild);

                            RotateRight(node);
                            break;
                        case -1:
                        case 0:
                            RotateRight(node);
                            break;
                    }

                    break;
                }
            }
        }

        protected virtual int GetBalance(AvlTreeNode<T> root) => GetHeight(root.RightChild) - GetHeight(root.LeftChild);

        protected virtual void RotateLeft(AvlTreeNode<T> root)
        {
            var pivot = root?.RightChild;

            if (pivot == null) return;
            var rootParent = root.Parent;
            var isLeftChild =
                (rootParent != null) && rootParent.LeftChild == root;
            var makeTreeRoot = root.Tree.Root == root;

            root.RightChild = pivot.LeftChild;
            pivot.LeftChild = root;

            root.Parent = pivot;
            pivot.Parent = rootParent;

            if (root.RightChild != null)
                root.RightChild.Parent = root;

            if (makeTreeRoot)
                pivot.Tree.Root = pivot;

            if (isLeftChild)
                rootParent.LeftChild = pivot;
            else if (rootParent != null)
                rootParent.RightChild = pivot;
        }

        protected virtual void RotateRight(AvlTreeNode<T> root)
        {
            var pivot = root?.LeftChild;

            if (pivot == null) return;
            var rootParent = root.Parent;
            var isLeftChild =
                (rootParent != null) && rootParent.LeftChild == root;
            var makeTreeRoot = root.Tree.Root == root;

            root.LeftChild = pivot.RightChild;
            pivot.RightChild = root;

            root.Parent = pivot;
            pivot.Parent = rootParent;

            if (root.LeftChild != null)
                root.LeftChild.Parent = root;

            if (makeTreeRoot)
                pivot.Tree.Root = pivot;

            if (isLeftChild)
                rootParent.LeftChild = pivot;
            else if (rootParent != null)
                rootParent.RightChild = pivot;
        }

        public void PrintTree(string dotPath, string fileName)
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVertex(Root.Value);
            PrintSubTree(graph, Root);

            var graphViz =
                new GraphvizAlgorithm<T, Edge<T>>(graph, @".\", QuickGraph.Graphviz.Dot.GraphvizImageType.Png);

            graphViz.FormatVertex += FormatVertex;

            graphViz.FormatEdge += FormatEdge;


            graphViz.Generate(new FileDotEngine(), $"{fileName}");

            Process.Start("cmd.exe", "/C " + $@"{dotPath} -T png {fileName}.dot > {fileName}.png");

            Thread.Sleep(1000);

            Process.Start($"{fileName}.png");
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<T> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();

            e.VertexFormatter.Shape = QuickGraph.Graphviz.Dot.GraphvizVertexShape.Box;

            e.VertexFormatter.StrokeColor = Color.Black;

            e.VertexFormatter.Font = new Font(FontFamily.GenericSansSerif, 12);
        }

        private static void FormatEdge(object sender, FormatEdgeEventArgs<T, Edge<T>> e)
        {
            e.EdgeFormatter.Font = new Font(FontFamily.GenericSansSerif, 8);

            e.EdgeFormatter.StrokeColor = Color.Black;
        }

        private static void PrintSubTree(IMutableVertexAndEdgeSet<T, Edge<T>> graph, AvlTreeNode<T> root)
        {
            if (root.HasLeftChild)
            {
                graph.AddVertex(root.LeftChild.Value);
                graph.AddEdge(new Edge<T>(root.Value, root.LeftChild.Value));
                PrintSubTree(graph, root.LeftChild);
            }

            if (root.HasRightChild)
            {
                graph.AddVertex(root.RightChild.Value);
                graph.AddEdge(new Edge<T>(root.Value, root.RightChild.Value));
                PrintSubTree(graph, root.RightChild);
            }
        }
    }
}