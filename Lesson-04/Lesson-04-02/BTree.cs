using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_04_02
{
    class BTree : IBinaryTree
    {
        /// <summary>Ширина поля для печати дерева в консоль</summary>
        const int OUTPUT_WIDTH = 80;

        /// <summary>Корень дерева</summary>
        private Node root;
        /// <summary>Количество элементов в дереве</summary>
        private int count;
        /// <summary>Высота дерева</summary>
        private int height;

        #region ---- CONSTRUCTORS ----

        public BTree()
        {
            root = null;
            count = 0;
            height = 0;
        }

        public BTree(int value)
        {
            root = new Node(value);
            count = 1;
            height = 1;
        }

        #endregion

        #region ---- INTERFACE ----

        public void AddNode(int value)
        {
            if (root == null)
            {
                root = new Node(value);
                count = 1;
                height = 1;
            }
            else
            {
                int currentHeight = 1;
                currentHeight = AddNodeAfter(root, value, currentHeight);
                if(height<currentHeight) height = currentHeight;
            }
        }


        public void ClearTree()
        {
            root = null;
            count = 0;
            height = 0;
        }

        public bool FindNOde(int Value)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            return count;
        }
        public int GetHeight()
        {
            return height;
        }

        public void RemoveNode(int value)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Print()
        {
            root.PrintPretty("", Node.NodePosition.center, true, false);
        }


        private int AddNodeAfter(Node node, int value, int currentHeight)
        {
            if(value <= node.Data)
            {
                if (node.Left == null)
                {
                    node.Left = new Node(value);
                    count++;
                }
                else
                {
                    currentHeight = AddNodeAfter(node.Left, value, currentHeight);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new Node(value);
                    count++;
                }
                else
                    currentHeight = AddNodeAfter(node.Right, value, currentHeight);
            }
            return currentHeight+1;
        }

    }


}
