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

        #region ---- PROPERTIES ----

        /// <summary>Количество узлов в дереве</summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>Количество уровней в дереве</summary>
        public int Height
        {
            get
            { 
                return height; 
            } 
        }

        #endregion

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

        /// <summary>
        /// Добавляет новый узел в дерево
        /// </summary>
        /// <param name="value">Значение хранимое в новом узле</param>
        public void AddNode(int value)
        {
            if (root == null)//Если дерево пустое то добавляем корневой узел
            {
                root = new Node(value);
                count = 1;
                height = 1;
            }
            else//Иначе, рекурсивно ищем позицию для добавления узла
            {
                int currentHeight = 1;
                currentHeight = AddChildNode(root, value, currentHeight);
                if(height<currentHeight) height = currentHeight;//Если при добавлении изменилась высота дерева, то корректируем соответствующее поле.
            }
        }

        /// <summary>
        /// Удаляет все элементы из дерева
        /// </summary>
        public void ClearTree()
        {
            root = null;
            count = 0;
            height = 0;
        }

        /// <summary>
        /// Ищет наличие узла в дереве с заданным значением
        /// </summary>
        /// <param name="Value">Искомое значение</param>
        /// <returns>true, если узел с таким значением есть в дереве.</returns>
        public bool FindNOde(int Value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает количество узлов в дереве
        /// </summary>
        /// <returns>Количество узлов в дереве</returns>
        public int GetCount()
        {
            return count;
        }
        
        /// <summary>
        /// Возвращает количество уровней в дереве
        /// </summary>
        /// <returns>Количество уровней в дереве</returns>
        public int GetHeight()
        {
            return height;
        }

        public void RemoveNode(int value)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Выводит структуру дерева в консоль.
        /// </summary>
        public void Print()
        {
            if(root != null)
                root.PrintNode("", Node.NodePosition.center, true, false);
            else
                Console.WriteLine("Tree is empty.");
        }


        /// <summary>
        /// Добавляет потомка для указанного узла дерева
        /// Если место уже занято, то переходит на следующий уровень
        /// </summary>
        /// <param name="node">Узел после ко</param>
        /// <param name="value">Значение для нового узла</param>
        /// <param name="currentHeight">Уровень дерева с которого пришел запрос на добавление</param>
        /// <returns>Текущий уровень дерева</returns>
        private int AddChildNode(Node node, int value, int currentHeight)
        {
            if(value <= node.Data)
            {
                if (node.Left == null)
                {
                    node.Left = new Node(value, currentHeight, node);
                    count++;
                }
                else
                {
                    currentHeight = AddChildNode(node.Left, value, currentHeight+1);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new Node(value, currentHeight, node);
                    count++;
                }
                else
                    currentHeight = AddChildNode(node.Right, value, currentHeight+1);
            }
            return currentHeight+1;
        }

    }


}
