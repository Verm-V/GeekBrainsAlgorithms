using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{

    /// <summary>Выводит в консоль содержимое заданного графа</summary>
    public static class GraphPrinter
    {
        /// <summary>Задержка отрисовки по умолчанию</summary>
        public const int DELAY = 0;

        /// <summary>Ширина поля на котором рисуется граф</summary>
        private const int WIDTH = 60;
        /// <summary>Высота поля на котором рисуется граф</summary>
        private const int HEIGHT = 21;

        /// <summary>Координата X центра окружности на которой лежат вершины графа</summary>
        private static readonly int CX = WIDTH / 2;
        /// <summary>Координата Y центра окружности на которой лежат вершины графа</summary>
        private static readonly int CY = HEIGHT / 2;

        /// <summary>Радиус окружности на которой лежат вершины графа</summary>
        private const int RADIUS = 10;

        /// <summary>Дополнительная информация об узле, для вывода его на экран</summary>
        class NodeInfo
        {
            /// <summary>Указатель на узел</summary>
            public Node node;
            /// <summary>Текст выводимый на экран</summary>
            public string text;
            /// <summary>Координата X на экране</summary>
            public int x;
            /// <summary>Координата Y на экране</summary>
            public int y;
        }

        /// <summary>Выводит граф и связи между его узлами на экран</summary>
        /// <param name="graph">Граф выводимый на экран</param>
        /// <param name="clearColors">Очищать ли цвета вершин</param>
        /// <param name="delay">Задержка вывода акран</param>
        /// <param name="textFormat">Формат вывода вершин на экран</param>
        public static void Print(Graph graph, bool clearColors = true, int delay = DELAY, string textFormat = "[00]")
        {
            NodeInfo[] info = new NodeInfo[graph.Count];
            Console.ForegroundColor = ConsoleColor.Gray;
            PrintClearField();

            //Рассчет положения узлов графа на экране
            double alpha = (2 * Math.PI / graph.Count);//Угловой шаг для отрисовки узлов

            for (int i = 0; i < graph.Count; i++)
            {
                int x = (int)(Math.Cos(alpha * i) * RADIUS * 2.5) + CX;//Координата X, на 2 умножаем для уменьшения овальности
                int y = (int)(Math.Sin(alpha * i) * RADIUS) + CY;//Координата Y
                info[i] = new NodeInfo { node = graph.Nodes[i], text = graph.Nodes[i].ID.ToString(textFormat), x = x, y = y };
            }

            //Вывод на экран связей между вершинами и самих вершин
            for (int i = 0; i < info.Length; i++)
            {
                for (int j = 0; j < info[i].node.Edges.Count; j++)
                {
                    int toNode = info[i].node.Edges[j].ConnectedNode.ID;
                    ConsoleColor color= (ConsoleColor)(info[i].node.Edges[j].Weight + 9);
                    PrintLine(info[i], info[toNode], color);
                    System.Threading.Thread.Sleep(delay);//небольшая задержка для наглядности работы алгоритма
                }
            }
            PrintLegend();
        }

        /// <summary>Выводит на экран подсказку о соответствии цветов и весов ребер</summary>
        private static void PrintLegend()
        {
            Console.SetCursorPosition(CX, HEIGHT);
            Console.Write("Обозначение весов:");
            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(CX, HEIGHT + 1 + i);
                ConsoleColor color = (ConsoleColor)(i + 9);
                Console.BackgroundColor = color;
                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("="+i);

            }
            Console.SetCursorPosition(0, HEIGHT);
        }

        /// <summary>Рисует связь между двумя узлами и сами узлы
        /// Рисование линии сделано по модификации алгоритма Брезенхэма</summary>
        /// <param name="info0">Первый узел</param>
        /// <param name="info1">Второй узел</param>
        /// <param name="color">Цвет узлов (не связи)</param>
        private static void PrintLine(NodeInfo info0, NodeInfo info1, ConsoleColor color = ConsoleColor.Gray)
        {
            int x0 = info0.x;
            int y0 = info0.y;
            int x1 = info1.x;
            int y1 = info1.y;
            //Меняем кординаты местами, так чтобы рисовка всегда шла сверху вниз
            if (y1 < y0)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }
            //Дельты координат концов отрезка
            int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
            int dy = y1 - y0;
            //Направления приращения
            int sx = (x1 >= x0) ? (1) : (-1);
            int sy = 1;
            string symbol = "_";
            Console.ForegroundColor = color;
            if (dy < dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                int x = x0 + sx;
                int y = y0;
                for (int i = 1; i <= dx; i++)
                {
                    symbol = "_";
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                        symbol = (x0 < x1) ? "\\" : "/";
                    }
                    else
                        d += d1;
                    PrintSymbol(x, y, symbol);
                    x += sx;
                }


            }
            else
            {
                {
                    int d = (dx << 1) - dy;
                    int d1 = dx << 1;
                    int d2 = (dx - dy) << 1;
                    int x = x0;
                    int y = y0 + sy;
                    for (int i = 1; i <= dy; i++)
                    {
                        symbol = "|";
                        if (d > 0)
                        {
                            d += d2;
                            x += sx;
                            symbol = (x0 < x1) ? "\\" : "/";
                        }
                        else
                            d += d1;
                        PrintSymbol(x, y, symbol);
                        y += sy;
                    }
                }

            }
            Console.ForegroundColor = ConsoleColor.Gray;
            PrintNode(info0);
            PrintNode(info1);

        }

        /// <summary>
        /// Печатает на экране единичный символ в заданных координатах
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="symbol"></param>
        private static void PrintSymbol(int x, int y, string symbol = "*")
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
            Console.SetCursorPosition(0, HEIGHT);

        }
        
        /// <summary>Печатает на экране содержимое вершины графа</summary>
        /// <param name="nodeInfo"></param>
        private static void PrintNode(NodeInfo nodeInfo)
        {
            Console.SetCursorPosition(nodeInfo.x - nodeInfo.text.Length/2, nodeInfo.y);
            Console.Write(nodeInfo.text);
            Console.SetCursorPosition(0, HEIGHT);
        }


        /// <summary>Печатает на экране пустое поле затирая информацию</summary>
        private static void PrintClearField()
        {
            for (int r = 0; r < HEIGHT; r++)
            {
                for (int c = 0; c < WIDTH; c++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
