using System;

namespace AVL
{
    public class AvlTreeNode<T> : BinaryTreeNode<T>
        where T : IComparable
    {
        public AvlTreeNode(T value)
            : base(value)
        {
        }

        public new AvlTreeNode<T> LeftChild
        {
            get => (AvlTreeNode<T>) base.LeftChild;
            set => base.LeftChild = value;
        }

        public new AvlTreeNode<T> RightChild
        {
            get => (AvlTreeNode<T>) base.RightChild;
            set => base.RightChild = value;
        }

        public new AvlTreeNode<T> Parent
        {
            get => (AvlTreeNode<T>) base.Parent;
            set => base.Parent = value;
        }
    }
}