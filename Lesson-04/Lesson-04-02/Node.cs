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

		public Node(int data, Node parent = null)
        {
			Data = data;
			Parent = parent;
        }

        #region ---- PRINT METHODS ----

        public enum NodePosition
        {
            left,
            right,
            center
        }

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
                    Console.WriteLine(value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void PrintLeftValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("L:");
            Console.ForegroundColor = (value == "-") ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void PrintRightValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("R:");
            Console.ForegroundColor = (value == "-") ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void PrintPretty(string indent, NodePosition nodePosition, bool last, bool empty)
        {

            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }

            var stringValue = empty ? "-" : Data.ToString();
            PrintValue(stringValue, nodePosition);

            if (!empty && (this.Left != null || this.Right != null))
            {

                if (this.Right != null)
                    this.Right.PrintPretty(indent, NodePosition.right, false, false);
                else
                    PrintPretty(indent, NodePosition.right, false, true);

                if (this.Left != null)
                    this.Left.PrintPretty(indent, NodePosition.left, true, false);
                else
                    PrintPretty(indent, NodePosition.left, true, true);
            }
        }

        #endregion


    }
}
