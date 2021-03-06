using System;
using System.Collections;
using System.Collections.Generic;

namespace AVL
{
    public partial class BinaryTree<T> where T : IComparable
    {
        internal class BinaryTreePreOrderEnumerator : IEnumerator<T>
        {
            private BinaryTreeNode<T> current;
            private BinaryTree<T> tree;
            internal Queue<BinaryTreeNode<T>> TraverseQueue;

            public BinaryTreePreOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                TraverseQueue = new Queue<BinaryTreeNode<T>>();
                VisitNode(this.tree.Root);
            }

            private void VisitNode(BinaryTreeNode<T> node)
            {
                while (true)
                {
                    if (node == null) return;
                    TraverseQueue.Enqueue(node);
                    VisitNode(node.LeftChild);
                    node = node.RightChild;
                }
            }

            public T Current => current.Value;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                current = TraverseQueue.Count > 0 ? TraverseQueue.Dequeue() : null;

                return (current != null);
            }
        }
    }
}