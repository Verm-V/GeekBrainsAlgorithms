using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_01_01
{
    class Program
    {
        /// <summary> Минимальное число для ввода </summary>
        private static readonly int MIN_NUMBER = 2;
        /// <summary> Максимальное число для ввода </summary>
        private static readonly int MAX_NUMBER = int.MaxValue;


        static void Main(string[] args)
        {
            //Ввод числа
            int number = NumberInput("Введите целое положительное число: ", MIN_NUMBER, MAX_NUMBER, false);

            //Проверка числа на принадлежность к простым и вывод результа
            Console.WriteLine("Число {0}{1} простое", number, PrimeNumberCheck(number) ? string.Empty : " НЕ");

            MessageWaitKey(string.Empty);
        }


        /// <summary>
        /// Проверка, является ли число простым
        /// </summary>
        /// <param name="number">Проверяемое число</param>
        /// <returns>True, если число простое</returns>
        private static bool PrimeNumberCheck(int number)
        {
            int d = 0;
            int i = 2;
            while (i < number)
            {
                if (number % i == 0) d++;
                i++;
            }

            return (d == 0);
        }


        #region ---- Вспомогательные методы ----

        /// <summary>
        /// Метод запрашивает у пользователя целое int число.
        /// </summary>
        /// <param name="message">Сообщение для пользователя</param>
        /// <param name="min">Минимальное значение ввода</param>
        /// <param name="max">Максимальное значение ввода</param>
        /// <param name="isOneDigit">Запрашивать одну цифру или несколько</param>
        /// <returns>Введенное пользователем целое число больше нуля.</returns>
        private static int NumberInput(string message, int min, int max, bool isOneDigit = true)
        {
            bool isInputCorrect = false; //флаг проверки
            int input = 0;
            Console.WriteLine($"{message} (от {min} до {max})");
            while (!isInputCorrect) //Цикл будет повторятся, пока вводимое число не пройдет все проверки
            {
                if (isOneDigit)
                    isInputCorrect = int.TryParse(Console.ReadKey().KeyChar.ToString(), out input);
                else
                    isInputCorrect = int.TryParse(Console.ReadLine(), out input);

                if (isInputCorrect && (input < min || input > max))
                    isInputCorrect = false;

                if (!isInputCorrect)
                    if (isOneDigit)
                        try
                        {
                            Console.CursorLeft--;//Если ввели что-то не то, то просто возвращаем курсор на прежнее место
                        }
                        catch
                        {
                            //В случае ошибки, ввода каких-либо управляющих символов или попытках выхода курсора
                            //за пределы консоли, просто ничего не делаем и остаемся на месте
                        }
                    else
                        Console.WriteLine("Ошибка. Повторите ввод.");
            }
            return input;
        }

        /// <summary> выводит на экран сообщение и ждет нажатия любой клавиши </summary>
        /// <param name="message">сообщение для пользователя</param>
        private static void MessageWaitKey(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Нажмите любую клавишу.");
            Console.ReadKey();
        }

        #endregion

    }


}
