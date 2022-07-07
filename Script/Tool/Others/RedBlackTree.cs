using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tool.Others
{
    public enum RedBlack
    {
        Red,
        Black,
    }
    public class RedBlackTree<T> where T : IComparable 
    {
        public class RedBlackTreeNode
        {
            public T Value;
            public RedBlack Color;
            public RedBlackTreeNode Left;
            public RedBlackTreeNode Right;
            public RedBlackTreeNode Parent;

            public RedBlackTreeNode(T value)
            {
                Value = value;
                Color = RedBlack.Red;
            }
            
            public RedBlackTreeNode(T value , RedBlackTreeNode parent)
            {
                Value = value;
                Parent = parent;
                Color = RedBlack.Red;
            }

            public RedBlackTreeNode GetAnotherSon(RedBlackTreeNode son)
            {
                if (son.Equals(Left))
                    return Right;
                return Left;
            }

            public RedBlackTreeNode Brother()
            {
                return Parent.GetAnotherSon(this);
            }

            public bool IsLeft(RedBlackTreeNode son)
            {
                return son.Equals(Left);
            }
            
            public bool IsParentLeft()
            {
                return Equals(Parent.Left);
            }
        }

        public RedBlackTreeNode Root;
        public int Size;
        
        public RedBlackTree()
        {
            Size = 0;
        }

        public IEnumerator<T> PreOrder(RedBlackTreeNode node)
        {
            if(node == null)
                yield break;
            yield return node.Value;
            PreOrder(node.Left);
            PreOrder(node.Right);
        }
        
        public IEnumerable<T> MidOrder(RedBlackTreeNode node)
        {
            if(node == null)
                yield break;
            MidOrder(node.Left);
            yield return node.Value;
            MidOrder(node.Right);
        }
        
        public IEnumerable<T> PostOrder(RedBlackTreeNode node)
        {
            if(node == null)
                yield break;
            PostOrder(node.Left);
            PostOrder(node.Right);
            yield return node.Value;
        }

        public T this[object obj]
        {
            get
            {
                return Find(obj);
            }
        }

        public void Inset(T value)
        {
            if (Root == null)
            {
                Root = new RedBlackTreeNode(value,null);
                Root.Color = RedBlack.Black;
                Size = 1;
                return;
            }

            Inset(Root, value);
        }

        public bool Contain(object value)
        {
            return Find(value).Equals(default);
        }

        public T Find(object value)
        {
            if (Root == null)
            {
                return default;
            }
            return Find(Root, value);
        }

        T Find(RedBlackTreeNode node, object value)
        {
            int result = node.Value.CompareTo(value);
            if (result == 0)
                return node.Value;
            if (result < 0 && node.Right != null)
                return Find(node.Right, value);
            else if(result > 0 && node.Left != null)
                return Find(node.Left, value);
            return default;
        }

        //右旋转节点
        void RightRotate(RedBlackTreeNode node)
        {
            RedBlackTreeNode left = node.Left;
            
            node.Left = left.Right;
            if (left.Right != null)
                left.Right.Parent = node;

            if (node == Root)
                Root = left;
            else if (node.Parent.IsLeft(node))
                node.Parent.Left = left;
            else
                node.Parent.Right = left;

            left.Right = node;
            left.Parent = node.Parent;
            node.Parent = left;    
        }
        
        //左旋转节点
        void LeftRotate(RedBlackTreeNode node)
        {
            RedBlackTreeNode right = node.Right;
            
            node.Right = right.Left;
            if (right.Left != null)
                right.Left.Parent = node;

            if (node == Root)
                Root = right;
            else if (node.Parent.IsLeft(node))
                node.Parent.Left = right;
            else
                node.Parent.Right = right;

            right.Left = node;
            right.Parent = node.Parent;
            node.Parent = right;    
        }


        
        #region 插入节点
        
        void Inset(RedBlackTreeNode node ,T value)
        {
            int result = node.Value.CompareTo(value);
            if (result == 0)
            {
                node.Value = value;
                return;
            }

            if (result > 0)
            {
                if (node.Left == null)
                {
                    RedBlackTreeNode inset = new RedBlackTreeNode(value ,node);
                    node.Left = inset;
                    Size++;
                    CheckNode(inset);
                }
                else
                    Inset(node.Left,value);
            }
            else
            {
                if (node.Right == null)
                {
                    RedBlackTreeNode inset = new RedBlackTreeNode(value ,node);
                    node.Right = inset;
                    Size++;
                    CheckNode(inset);
                }
                else
                    Inset(node.Right,value);
            }
        }
        
        void CheckNode(RedBlackTreeNode node)
        {
            if (node.Equals(Root))
            {
                Root_Node(node);
                return;
            }

            if (node.Parent.Color == RedBlack.Black)
            {
                Parent_Is_Black(node);
                return;
            }

            if (node.Parent.Brother() != null && node.Parent.Brother().Color == RedBlack.Red)
            {
                Parent_Is_Red_Uncle_Is_Red(node);
                return;
            }

            if (node.Parent.IsParentLeft())
            {
                if (node.IsParentLeft())
                {
                    Parent_Is_RedLeft_The_Is_Left(node);
                }
                else
                {
                    Parent_Is_RedLeft_The_Is_Right(node);
                }
            }
            else
            {
                if (node.IsParentLeft())
                {
                    Parent_Is_RedRight_The_Is_Left(node);
                }
                else
                {
                    Parent_Is_RedRight_The_Is_Right(node);
                }
            }
                
        }

        void Root_Node(RedBlackTreeNode node)//插入结点是根结点
        {
            node.Color = RedBlack.Black;
        }

        void Parent_Is_Black(RedBlackTreeNode node) //父结点是黑色
        {
            ;
        }

        void Parent_Is_Red_Uncle_Is_Red(RedBlackTreeNode node) //父节点是红色，叔叔结点是红色
        {
            RedBlackTreeNode parent = node.Parent;
            parent.Color = RedBlack.Black;
            parent.Brother().Color = RedBlack.Black;
            parent.Parent.Color = RedBlack.Red;
            CheckNode(parent.Parent);
        }

        void Parent_Is_RedLeft_The_Is_Left(RedBlackTreeNode node) //父节点是红色在祖父结点左边，插入结点在父结点左边
        {
            RedBlackTreeNode parent = node.Parent;
            parent.Color = RedBlack.Black;
            parent.Parent.Color = RedBlack.Red;
            RightRotate(parent.Parent);
        }

        void Parent_Is_RedLeft_The_Is_Right(RedBlackTreeNode node) //父节点是红色在祖父结点左边，插入结点在父结点右边
        {
            RedBlackTreeNode parent = node.Parent;
            LeftRotate(parent);
            Parent_Is_RedLeft_The_Is_Left(parent);
        }

        void Parent_Is_RedRight_The_Is_Left(RedBlackTreeNode node) //父节点是红色在祖父结点右边，插入结点在父结点左边
        {
            RedBlackTreeNode parent = node.Parent;
            RightRotate(parent);
            Parent_Is_RedRight_The_Is_Right(parent);
        }

        void Parent_Is_RedRight_The_Is_Right(RedBlackTreeNode node) //父节点是红色在祖父结点右边，插入结点在父结点右边
        {
            RedBlackTreeNode parent = node.Parent;
            parent.Color = RedBlack.Black;
            parent.Parent.Color = RedBlack.Red;
            LeftRotate(parent.Parent);
        }
        #endregion
        
        
        
        #region 删除节点
        
        private RedBlackTreeNode _replaceNode;
        public bool Delete(T value)
        {
            if (Root.Value.CompareTo(value) == 0)
            {
                Root = null;
                return true;
            }

            return Delete(Root, value);
        }
        
        bool Delete(RedBlackTreeNode node ,T value)
        {
            if (node == null)
                return false;
            int result = node.Value.CompareTo(value);
            if (result == 0)
            {
                _replaceNode = node;
                Delete(node);
                return true;
            }
            else if (result <= 0)
            {
                return Delete(node.Right, value);
            }
            else
            {
                return Delete(node.Left, value);
            }
        }
        
        void Delete(RedBlackTreeNode node)
        {
            if (node.Left == null && node.Right == null)
            {
                _replaceNode.Value = node.Value;
                _replaceNode = null;
                DeleteCheckNode(node);
            }
            else if(node.Left != null && node.Right != null)
            {
                RedBlackTreeNode mostLeft = GetMostLeftNode(node.Right);
                _replaceNode.Value = node.Value;
                _replaceNode = node;
                Delete(mostLeft);
            }
            else if(node.Left != null)
            {
                RedBlackTreeNode left = node.Left;
                _replaceNode.Value = node.Value;
                _replaceNode = node;
                Delete(left);
            }
            else
            {
                RedBlackTreeNode right = node.Right;
                _replaceNode.Value = node.Value;
                _replaceNode = node;
                Delete(right);
            }
        }

        RedBlackTreeNode GetMostLeftNode(RedBlackTreeNode node)
        {
            if (node.Left == null)
                return node;
            return GetMostLeftNode(node.Left);
        }

        void DeleteCheckNode(RedBlackTreeNode node)
        {
            DeleteSubCheckNode(node);
            DeleteDirectly(node);
        }

        void DeleteSubCheckNode(RedBlackTreeNode node)
        {
            if (node.Color == RedBlack.Red)
            {
                Delete_Node_Is_Red(node);
                return;
            }

            if (node.IsParentLeft())
            {
                if (node.Brother().Color == RedBlack.Red)
                    Delete_The_Is_Left_Brother_Is_Red(node);
                else if (node.Brother().Right != null && node.Brother().Right.Color == RedBlack.Red)
                    Delete_The_Is_Left_BrotherRight_Is_Red(node);
                else if (node.Brother().Left != null && node.Brother().Left.Color == RedBlack.Red)
                    Delete_The_Is_Left_BrotherRight_Is_Black_BrotherLeft_Is_Red(node);
                else
                    Delete_The_Is_Left_BrotherLeftRight_Is_Black(node);
            }
            else
            {
                if (node.Brother().Color == RedBlack.Red)
                    Delete_The_Is_Right_Brother_Is_Red(node);
                else if (node.Brother().Left != null && node.Brother().Left.Color == RedBlack.Red)
                    Delete_The_Is_Right_BrotherLeft_Is_Red(node);
                else if (node.Brother().Right != null && node.Brother().Right.Color == RedBlack.Red)
                    Delete_The_Is_Right_BrotherLeft_Is_Black_BrotherRight_Is_Red(node);
                else
                    Delete_The_Is_Right_BrotherLeftRight_Is_Black(node);
            }
        }

        void DeleteDirectly(RedBlackTreeNode node)
        {
            if (node.Parent.IsLeft(node))
                node.Parent.Left = null;
            else
                node.Parent.Right = null;
            Size--;
            node = null;
        }

        void Delete_Node_Is_Red(RedBlackTreeNode node) //删除结点是红色结点
        {
            node.Color = RedBlack.Black;
        }

        
        //-----------------------左边
        void Delete_The_Is_Left_Brother_Is_Red(RedBlackTreeNode node) //删除结点是黑色结点，其兄弟节点是红色节点
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = RedBlack.Black;
            node.Parent.Color = RedBlack.Red;
            LeftRotate(node.Parent);
            DeleteSubCheckNode(node);
        }
        
        void Delete_The_Is_Left_BrotherRight_Is_Red(RedBlackTreeNode node) //删除结点是黑色结点，其兄弟节点是黑色节点，兄弟节点右节点是红色
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = node.Parent.Color;
            brother.Right.Color = RedBlack.Black;
            node.Parent.Color = RedBlack.Black;
            LeftRotate(node.Parent);
        }
        
        void Delete_The_Is_Left_BrotherRight_Is_Black_BrotherLeft_Is_Red(RedBlackTreeNode node)
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = RedBlack.Red;
            brother.Left.Color = RedBlack.Black;
            RightRotate(brother);
            Delete_The_Is_Left_BrotherRight_Is_Red(node);
        }
        
        void Delete_The_Is_Left_BrotherLeftRight_Is_Black(RedBlackTreeNode node)
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = RedBlack.Red;
            DeleteSubCheckNode(node.Parent);
        }
        
        //-----------------------右边
        
        void Delete_The_Is_Right_Brother_Is_Red(RedBlackTreeNode node) //删除结点是黑色结点，其兄弟节点是红色节点
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = RedBlack.Black;
            node.Parent.Color = RedBlack.Red;
            RightRotate(node.Parent);
            DeleteSubCheckNode(node);
        }
        
        void Delete_The_Is_Right_BrotherLeft_Is_Red(RedBlackTreeNode node) //删除结点是黑色结点，其兄弟节点是黑色节点，兄弟节点右节点是红色
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = node.Parent.Color;
            brother.Left.Color = RedBlack.Black;
            node.Parent.Color = RedBlack.Black;
            RightRotate(node.Parent);
        }
        
        void Delete_The_Is_Right_BrotherLeft_Is_Black_BrotherRight_Is_Red(RedBlackTreeNode node)
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = RedBlack.Red;
            brother.Right.Color = RedBlack.Black;
            LeftRotate(brother);
            Delete_The_Is_Right_BrotherLeft_Is_Red(node);
        }
        
        void Delete_The_Is_Right_BrotherLeftRight_Is_Black(RedBlackTreeNode node)
        {
            RedBlackTreeNode brother = node.Brother();
            brother.Color = RedBlack.Red;
            DeleteSubCheckNode(node.Parent);
        }
        #endregion
    }
}