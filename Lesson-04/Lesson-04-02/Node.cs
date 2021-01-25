using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_04_02
{
    /// <summary>Узел двоичного дерева</summary>
    public class Node
    {
        /// <summary>Данные содержащиеся в узле дерева</summary>
        public int Data { get; set; }
        /// <summary>Ссылка на левого потомка</summary>
        public Node Left { get; set; }
        /// <summary>Ссылка на правого потомка</summary>
        public Node Right { get; set; }
        /// <summary>Ссылка на родителя</summary>
        public Node Parent { get; set; }
        /// <summary>Ранг узла в дереве</summary>
        public int Rank { get; set; }

        public Node(int data, int rank = 0, Node parent = null)
        {
            Data = data;
            Parent = parent;
            Rank = rank;
        }

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
            System.Threading.Thread.Sleep(100);//небольшая задержка для наглядности работы алгоритма

            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");//│ ├ └ ─
                indent += "│ ";
            }

            // !!! DEBUG - версия для дебага с распечаткой рангов
            var stringValue = empty ? "--" : (Rank.ToString() + ":" + Data.ToString());
            //var stringValue = empty ? "--" : Data.ToString();
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
