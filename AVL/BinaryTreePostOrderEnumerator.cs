using System;
using System.Collections;
using System.Collections.Generic;

namespace AVL
{
    public partial class BinaryTree<T> where T : IComparable
    {
        internal class BinaryTreePostOrderEnumerator : IEnumerator<T>
        {
            private BinaryTreeNode<T> current;
            private BinaryTree<T> tree;
            internal Queue<BinaryTreeNode<T>> TraverseQueue;

            public BinaryTreePostOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                TraverseQueue = new Queue<BinaryTreeNode<T>>();
                VisitNode(this.tree.Root);
            }

            private void VisitNode(BinaryTreeNode<T> node)
            {
                if (node == null)
                    return;
                VisitNode(node.LeftChild);
                VisitNode(node.RightChild);
                TraverseQueue.Enqueue(node);
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