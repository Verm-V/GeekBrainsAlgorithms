using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07_01
{
    class WaySearcher
    {

        #region ---- FIELDS & PROPERTIES ----

        /// <summary>Минимальная ширина поля</summary>
        public const int MIN_WIDTH = 3;
        /// <summary>Минимальная высота поля</summary>
        public const int MIN_HEIGHT = 3;
        /// <summary>Максимальная ширина поля</summary>
        public const int MAX_WIDTH = 10;
        /// <summary>Максимальная высота поля</summary>
        public const int MAX_HEIGHT = 10;

        /// <summary>Поле</summary>
        private int[,] field;

        #endregion

        #region ---- CONSTRUCTORS ----

        /// <summary>Конструктор</summary>
        /// <param name="width">Ширина поля</param>
        /// <param name="height">Высота поля</param>
        public WaySearcher(int width, int height)
        {
            field = new int[width, height];
            field[4, 2] = -1;//!DEBUG Отладочное препятствие
        }

        #endregion

        #region ---- PRINT METHODS ----

        /// <summary>Печать поля окна в консоли</summary>
        public void PrintField()
        {
            int w = field.GetLength(0);//width
            int h = field.GetLength(1);//height

            //╔ ═ ╗ ╚ ║ ╝ ╠ ╣ ╦ ╩ ╬ █ - символы для рамки поля
            //╟ ╢ ╤ ╧ ┼ ─ │
            Console.Clear();
            for (int r = 0; r < h; r++)
            {

                for (int c = 0; c < w; c++)
                {
                    Console.SetCursorPosition(c * 4, r * 2);
                    if(r==0)
                        Console.Write((c == 0 ? "╔" : "╤") + "═══" + (c == w - 1 ? "╗" : "╤"));
                    else
                        Console.Write((c == 0 ? "╟" : "┼") + "───" + (c == w - 1 ? "╢" : "┼"));
                    Console.SetCursorPosition(c * 4, r * 2 + 1);
                    Console.Write(c == 0 ? "║" : "│");
                    if (field[c, r]<0)
                        Console.Write("███");
                    else           
                        Console.Write($"{ field[c, r],3}");
                }
                Console.Write("║");
            }

            for (int c = 0; c < w; c++)
            {
                Console.SetCursorPosition(c * 4, h * 2);
                Console.Write((c == 0 ? "╚" : "╧") + "═══" + (c == w - 1 ? "╝" : "╧"));
            }

            Console.SetCursorPosition(0, h*2);
        }

        #endregion

    }


}
