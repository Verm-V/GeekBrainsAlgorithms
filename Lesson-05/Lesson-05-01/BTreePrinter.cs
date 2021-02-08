using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_05_01
{
    public static class BTreePrinter
    {
        class NodeInfo
        {
            public Node node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public NodeInfo Parent, Left, Right;
            public ConsoleColor color;
        }

        public static void Print(this Node root, bool clearColors = true, string textFormat = "[0]", int spacing = 4, int topMargin = 2, int leftMargin = 2)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            if (root == null) return;
            int rootTop = Console.CursorTop + topMargin;
            List<NodeInfo> last = new List<NodeInfo>();
            Node next = root;
            for (int level = 0; next != null; level++)
            {
                if (clearColors) next.Color = ConsoleColor.Gray;
                NodeInfo item = new NodeInfo { node = next, Text = next.Value.ToString(textFormat), color = next.Color };
                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + spacing;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.node.Left)
                    {
                        item.Parent.Left = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                    }
                    else
                    {
                        item.Parent.Right = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                    }
                }
                next = next.Left ?? next.Right;
                for (; next == null; item = item.Parent)
                {
                    int top = rootTop + 2 * level;
                    Console.ForegroundColor = item.color;
                    Print(item.Text, top, item.StartPos);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (item.Left != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Print("/", top + 1, item.Left.EndPos);
                        Print("_", top, item.Left.EndPos + 1, item.StartPos);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    if (item.Right != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Print("_", top, item.EndPos, item.Right.StartPos - 1);
                        Print("\\", top + 1, item.Right.StartPos - 1);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    if (--level < 0) break;
                    if (item == item.Parent.Left)
                    {
                        item.Parent.StartPos = item.EndPos + 1;
                        next = item.Parent.node.Right;
                    }
                    else
                    {
                        if (item.Parent.Left == null)
                            item.Parent.EndPos = item.StartPos - 1;
                        else
                            item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }

        private static void Print(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
        }
    }
}