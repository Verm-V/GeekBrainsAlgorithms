using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_05_01
{

    /// <summary>Единичный узел дерева</summary>
    public class Node : IComparable
    {
        #region ---- FIELDS ----

        /// <summary>Указатель на левый узел</summary>
        private Node left;
        /// <summary>Указатель на правый узел</summary>
        private Node right;

        #endregion

        #region ---- PROPERTIES ----

        /// <summary>Указатель на дерево содержащее узел</summary>
        public BTree Tree
        {
            get;
            private set;
        }

        /// <summary>Указатель на левый узел</summary>
        public Node Left
        {
            get
            {
                return left;
            }

            internal set
            {
                left = value;

                if (left != null)
                {
                    left.Parent = this;  //Устанавливаем указатель на родительский узел у потомка
                }
            }
        }

        /// <summary>Указатель на правый узел</summary>
        public Node Right
        {
            get
            {
                return right;
            }

            internal set
            {
                right = value;

                if (right != null)
                {
                    right.Parent = this;  //Устанавливаем указатель на родительский узел у потомка
                }
            }
        }

        /// <summary>Указатель на родительский узел</summary>
        public Node Parent
        {
            get;
            internal set;
        }

                /// <summary>Значение хранимое в узле</summary>
        public int Value
        {
            get;
            private set;
        }

        /// <summary>Цвет ноды для вывода в консоли</summary>
        public ConsoleColor Color
        {
            get;
            internal set;
        }

        #endregion

        #region ---- CONSTRUCTOR ----
        /// <summary>
        /// Узел двоичного дерева
        /// </summary>
        /// <param name="value">Значение хранимое в узле</param>
        /// <param name="parent">Указатель на родительский узел</param>
        /// <param name="tree">Указатель на дерево содержащее узел</param>
        public Node(int value, Node parent, BTree tree)
        {
            Value = value;
            Parent = parent;
            Tree = tree;
            Color = ConsoleColor.Gray;
        }
        #endregion

        #region ---- INTERFACE ----

        /// <summary>
        /// Реализация интерфейса IComparable
        /// Сравнивает значение хранимое в узле с указанным значением
        /// </summary>
        /// <param name="other">Элемент с которым идет сравнение</param>
        /// <returns>
        /// +1 - если значение больше переданного значения
        /// -1 - если значение меньше переданного значения
        /// 0 если значения равны
        /// </returns>
        public int CompareTo(object other)
        {
            return Value.CompareTo(other);
        }
        #endregion

        #region ---- BALANCE ----

        /// <summary>
        /// Балансировка дерева
        /// Проверяет состояние дерева и вызывает необходимый метод поворота
        /// </summary>
        internal void Balance()
        {
            //Если правая ветка больше левой
            if (State == TreeState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {
                    LeftRightRotation();
                }

                else
                {
                    LeftRotation();
                }
            }
            //Если левая ветка больше правой
            else if (State == TreeState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {
                    RightLeftRotation();
                }
                else
                {
                    RightRotation();
                }
            }
            //Идем вверх по дереву, для проверки необходимости балансировки вышестоящих узлов
            if (this.Parent != null) this.Parent.Balance();


        }

        /// <summary>Рекурсивный рассчет высоты дерева</summary>
        /// <param name="node">Узел дерева от которого считается высота</param>
        /// <returns>Максимальную высоту от корня к ветвям</returns>
        private int MaxChildHeight(Node node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right));
            }

            return 0;
        }

        /// <summary>Высота левого поддерева</summary>
        private int LeftHeight
        {
            get
            {
                return MaxChildHeight(Left);
            }
        }

        /// <summary>Высота правого поддерева</summary>
        private int RightHeight
        {
            get
            {
                return MaxChildHeight(Right);
            }
        }

        /// <summary>Проверка дерева на сбалансированность</summary>
        private TreeState State
        {
            get
            {
                if (LeftHeight - RightHeight > 1)
                {
                    return TreeState.LeftHeavy;
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return TreeState.RightHeavy;
                }

                return TreeState.Balanced;
            }
        }

        /// <summary>
        /// Фактор балансировки, показывает куда нужно будет вращать дерево
        /// Вычисляется через разницу высот деревьев.
        /// </summary>
        private int BalanceFactor
        {
            get
            {
                return RightHeight - LeftHeight;
            }
        }

        /// <summary>Возможные состояния дерева</summary>
        enum TreeState
        {
            Balanced,//Дерево сбалансировано
            LeftHeavy,//Левое дерево больше
            RightHeavy,//Правое дерево больше
        }

        #endregion

        #region ---- ROTATIONS ----

        /// <summary>Левое вращение</summary>
        private void LeftRotation()
        {

            // До
            //     12(this)     
            //      \     
            //       15     
            //        \     
            //         25     
            //     
            // После     
            //       15     
            //      / \     
            //     12  25  

            // Сделать правого потомка новым корнем дерева.
            Node newRoot = Right;
            ReplaceRoot(newRoot);

            // Поставить на место правого потомка - левого потомка нового корня.    
            Right = newRoot.Left;
            // Сделать текущий узел - левым потомком нового корня.    
            newRoot.Left = this;
        }

        /// <summary>Левое вращение</summary>
        private void RightRotation()
        {
            // Было
            //     c (this)     
            //    /     
            //   b     
            //  /     
            // a     
            //     
            // Стало    
            //       b     
            //      / \     
            //     a   c  

            // Левый узел текущего элемента становится новым корнем
            Node newRoot = Left;
            ReplaceRoot(newRoot);

            // Перемещение правого потомка нового корня на место левого потомка старого корня
            Left = newRoot.Right;

            // Правым потомком нового корня, становится старый корень.     
            newRoot.Right = this;
        }

        /// <summary>Лево-правое вращение</summary>
        private void LeftRightRotation()
        {
            Right.RightRotation();
            LeftRotation();
        }

        /// <summary>Право-левое вращение</summary>
        private void RightLeftRotation()
        {
            Left.LeftRotation();
            RightRotation();
        }

        #endregion

        #region ---- ROOT REPLACE ----

        /// <summary>
        /// Замена корня дерева на указанный узел
        /// </summary>
        /// <param name="newRoot">Новый корень дерева</param>
        private void ReplaceRoot(Node newRoot)
        {
            if (this.Parent != null)
            {
                if (this.Parent.Left == this)
                {
                    this.Parent.Left = newRoot;
                }
                else if (this.Parent.Right == this)
                {
                    this.Parent.Right = newRoot;
                }
            }
            else
            {
                Tree.Root = newRoot;
            }

            newRoot.Parent = this.Parent;
            this.Parent = newRoot;
        }

        #endregion

        #region ---- PRINT METHODS ----

        /// <summary>
        /// Позиция узла относительно корня
        /// </summary>
        public enum NodePosition
        {
            left,
            right,
            center
        }

        /// <summary>
        /// Печатает значение узла
        /// </summary>
        /// <param name="value">Печатаемое значение, "-" если узел отсутствует</param>
        /// <param name="nodePostion">Позиция узла относительно корня</param>
        private void PrintValue(string value, NodePosition nodePostion)
        {
            switch (nodePostion)
            {
                case NodePosition.left:
                    PrintLeftValue(value);
                    break;
                case NodePosition.right:
                    PrintRightValue(value);
                    break;
                case NodePosition.center:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(value);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Печать левой ветки узла
        /// </summary>
        /// <param name="value">Печатаемое значение, "--" если узел отсутствует</param>
        private void PrintLeftValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("L:");
            Console.ForegroundColor = (value == "--") ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Печать правой ветки узла
        /// </summary>
        /// <param name="value">Печатаемое значение, "--" если узел отсутствует</param>
        private void PrintRightValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("R:");
            Console.ForegroundColor = (value == "--") ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        /// <summary>
        /// Рекурсивно выводит на в консоль содержимое бинарного дерева
        /// </summary>
        /// <param name="indent">Отступ для печати значения</param>
        /// <param name="nodePosition">Положение ноды относительно корня</param>
        /// <param name="last">true, если это последний узел на этом уровне</param>
        /// <param name="empty">true, если лист пустой</param>
        public void PrintNode(string indent, NodePosition nodePosition, bool last, bool empty)
        {
            //System.Threading.Thread.Sleep(10);//небольшая задержка для наглядности работы алгоритма

            Console.Write(indent);
            if (last)
            {
                Console.Write("└────");
                indent += "     ";
            }
            else
            {
                Console.Write("├────");//│ ├ └ ─
                indent += "│    ";
            }

            var stringValue = empty ? "--" : Value.ToString();
            PrintValue(stringValue, nodePosition);

            if (!empty && (this.Left != null || this.Right != null))
            {

                if (this.Right != null)
                    this.Right.PrintNode(indent, NodePosition.right, false, false);
                else
                    PrintNode(indent, NodePosition.right, false, true);

                if (this.Left != null)
                    this.Left.PrintNode(indent, NodePosition.left, true, false);
                else
                    PrintNode(indent, NodePosition.left, true, true);
            }
        }

        #endregion
    }
}