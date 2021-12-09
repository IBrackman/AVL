using System;

namespace AVL
{
    public class BinaryTreeNode<T>
        where T : IComparable
    {
        private T value;

        public virtual T Value
        {
            get => value;
            set => this.value = value;
        }

        public virtual BinaryTreeNode<T> LeftChild { get; set; }

        public virtual BinaryTreeNode<T> RightChild { get; set; }

        public virtual BinaryTreeNode<T> Parent { get; set; }

        public virtual BinaryTree<T> Tree { get; set; }

        public virtual bool IsLeaf => ChildCount == 0;

        public virtual bool IsInternal => ChildCount > 0;

        public virtual bool IsLeftChild => Parent != null && Parent.LeftChild == this;

        public virtual bool IsRightChild => Parent != null && Parent.RightChild == this;

        public virtual int ChildCount
        {
            get
            {
                var count = 0;

                if (LeftChild != null)
                    count++;

                if (RightChild != null)
                    count++;

                return count;
            }
        }

        public virtual bool HasLeftChild => LeftChild != null;

        public virtual bool HasRightChild => RightChild != null;

        public BinaryTreeNode(T value)
        {
            this.value = value;
        }
    }
}