using System;
using System.Collections;
using System.Collections.Generic;

namespace AVL
{
    public partial class BinaryTree<T> : ICollection<T>
        where T : IComparable
    {
        public enum TraversalMode
        {
            InOrder = 0,
            PostOrder,
            PreOrder
        }

        private BinaryTreeNode<T> head;
        private readonly Comparison<IComparable> comparer = CompareElements;
        private int size;

        public virtual BinaryTreeNode<T> Root
        {
            get => head;
            set => head = value;
        }

        public virtual bool IsReadOnly => false;

        public virtual int Count => size;

        public virtual TraversalMode TraversalOrder { get; set; } = TraversalMode.InOrder;

        public BinaryTree()
        {
            head = null;
            size = 0;
        }

        public virtual void Add(T value)
        {
            var node = new BinaryTreeNode<T>(value);
            Add(node);
        }

        public virtual void Add(BinaryTreeNode<T> node)
        {
            while (true)
            {
                if (head == null)
                {
                    head = node;
                    node.Tree = this;
                    size++;
                }
                else
                {
                    if (node.Parent == null) node.Parent = head;

                    var insertLeftSide = comparer(node.Value, node.Parent.Value) <= 0;

                    if (insertLeftSide)
                    {
                        if (node.Parent.LeftChild == null)
                        {
                            node.Parent.LeftChild = node;
                            size++;
                            node.Tree = this;
                        }
                        else
                        {
                            node.Parent = node.Parent.LeftChild;
                            continue;
                        }
                    }
                    else
                    {
                        if (node.Parent.RightChild == null)
                        {
                            node.Parent.RightChild = node;
                            size++;
                            node.Tree = this;
                        }
                        else
                        {
                            node.Parent = node.Parent.RightChild;
                            continue;
                        }
                    }
                }

                break;
            }
        }

        public virtual BinaryTreeNode<T> Find(T value)
        {
            var node = head;
            while (node != null)
            {
                if (node.Value.Equals(value))
                    return node;
                var searchLeft = comparer(value, node.Value) < 0;

                node = searchLeft ? node.LeftChild : node.RightChild;
            }

            return null;
        }

        public virtual bool Contains(T value)
        {
            return (Find(value) != null);
        }

        public virtual bool Remove(T value)
        {
            var removeNode = Find(value);

            return Remove(removeNode);
        }

        public virtual bool Remove(BinaryTreeNode<T> removeNode)
        {
            if (removeNode == null || removeNode.Tree != this)
                return false;

            var wasHead = (removeNode == head);

            if (Count == 1)
            {
                head = null;
                removeNode.Tree = null;

                size--;
            }
            else if (removeNode.IsLeaf)
            {
                if (removeNode.IsLeftChild)
                    removeNode.Parent.LeftChild = null;
                else
                    removeNode.Parent.RightChild = null;

                removeNode.Tree = null;
                removeNode.Parent = null;

                size--;
            }
            else if (removeNode.ChildCount == 1)
            {
                if (removeNode.HasLeftChild)
                {
                    removeNode.LeftChild.Parent = removeNode.Parent;

                    if (wasHead)
                        Root = removeNode.LeftChild;

                    if (removeNode.IsLeftChild)
                        removeNode.Parent.LeftChild = removeNode.LeftChild;
                    else
                        removeNode.Parent.RightChild = removeNode.LeftChild;
                }
                else
                {
                    removeNode.RightChild.Parent = removeNode.Parent;

                    if (wasHead)
                        Root = removeNode.RightChild;

                    if (removeNode.IsLeftChild)
                        removeNode.Parent.LeftChild = removeNode.RightChild;
                    else
                        removeNode.Parent.RightChild = removeNode.RightChild;
                }

                removeNode.Tree = null;
                removeNode.Parent = null;
                removeNode.LeftChild = null;
                removeNode.RightChild = null;

                size--;
            }
            else
            {
                var successorNode = removeNode.LeftChild;
                while (successorNode.RightChild != null)
                {
                    successorNode = successorNode.RightChild;
                }

                removeNode.Value = successorNode.Value;

                Remove(successorNode);
            }


            return true;
        }

        public virtual void Clear()
        {
            var enumerator = GetPostOrderEnumerator();
            while (enumerator.MoveNext())
            {
                Remove(enumerator.Current);
            }

            enumerator.Dispose();
        }

        public virtual int GetHeight() => GetHeight(Root);

        public virtual int GetHeight(T value)
        {
            var valueNode = Find(value);
            return value != null ? GetHeight(valueNode) : 0;
        }

        public virtual int GetHeight(BinaryTreeNode<T> startNode)
        {
            if (startNode == null)
                return 0;
            return 1 + Math.Max(GetHeight(startNode.LeftChild), GetHeight(startNode.RightChild));
        }

        public virtual int GetDepth(T value)
        {
            var node = Find(value);
            return GetDepth(node);
        }

        public virtual int GetDepth(BinaryTreeNode<T> startNode)
        {
            var depth = 0;

            if (startNode == null)
                return depth;

            var parentNode = startNode.Parent;
            while (parentNode != null)
            {
                depth++;
                parentNode = parentNode.Parent;
            }

            return depth;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            switch (TraversalOrder)
            {
                case TraversalMode.InOrder:
                    return GetInOrderEnumerator();
                case TraversalMode.PostOrder:
                    return GetPostOrderEnumerator();
                case TraversalMode.PreOrder:
                    return GetPreOrderEnumerator();
                default:
                    return GetInOrderEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual IEnumerator<T> GetInOrderEnumerator() => new BinaryTreeInOrderEnumerator(this);

        public virtual IEnumerator<T> GetPostOrderEnumerator() => new BinaryTreePostOrderEnumerator(this);

        public virtual IEnumerator<T> GetPreOrderEnumerator() => new BinaryTreePreOrderEnumerator(this);

        public virtual void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public virtual void CopyTo(T[] array, int startIndex)
        {
            var enumerator = GetEnumerator();

            for (var i = startIndex; i < array.Length; i++)
            {
                if (enumerator.MoveNext())
                    array[i] = enumerator.Current;
                else
                    break;
            }
        }

        public static int CompareElements(IComparable x, IComparable y) => x.CompareTo(y);
    }
}