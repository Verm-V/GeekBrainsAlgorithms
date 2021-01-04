using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_01_03
{
    class Program
    {

        /// <summary> Максимальное количество членов последовательности 
        /// больше 93 ставить не стоит, т.к. используется тип ulong и 
        /// на 94-м члене последовательности произойдет переполнение</summary>
        private static readonly int SEQUENCE_MAX = 93;


        static void Main(string[] args)
        {
            //Запросим какой элемент последовательности нужен пользователю
            int sequenceMax = NumberInput("Введите количество членов последовательности Фибоначчи: ", 1, SEQUENCE_MAX, false);

            //Рассчет с помощью цикла
            Console.WriteLine("\nРассчет с помощью цикла");
            Console.WriteLine($"Элемент №{sequenceMax} последовательности Фибоначчи равен: {FibonachiCycle(sequenceMax)}\n");

            //Рассчет с помощью рекурсии
            //Массив для хранения рассчитанных чисел Фибоначчи
            ulong[] memory = new ulong[sequenceMax + 1];
            memory[1] = 1;
            //Делаем рассчет нужного элемента последовательности
            Console.WriteLine("\nРассчет с помощью рекурсии");
            Console.WriteLine($"Элемент №{sequenceMax} последовательности Фибоначчи равен: {Fibonachi(sequenceMax, memory)}\n");

            //Поскольку в массиве все еще хранятся элементы, то можно их все вывести на экран
            Console.WriteLine($"Вся последовательность чисел Фибоначчи до элемента №{sequenceMax}\n");
            for (int i = 1; i <= sequenceMax; i++)
                Console.WriteLine("{0,2:d} - {1}", i, memory[i]);


            MessageWaitKey(string.Empty);

        }

        #region ---- Методы вычисления чисел Фибоначчи ----

        /// <summary>
        /// Рекурсивно рассчитывает члены последовательноси Фибоначчи до заданного номера
        /// Вычисление происходит по формуле: F(n) = F(n-1) + F(n-2)
        /// Используется обычная рекурсия без заморочек. Метод применим приблизительно до номера 40
        /// Далее время рассчета очень сильно увеличивается, и уже даже на 43-м нужно ждать минуты 3,
        /// а на 50 более получаса.
        /// </summary>
        /// <param name="n">Номер члена последовательности до которого считать</param>
        /// <returns>Значение члена последовательности</returns>
        private static ulong Fibonachi(int n)
        {
            if (n > 1)
                return (ulong)(Fibonachi(n - 1) + Fibonachi(n - 2));
            else
                return (ulong)n;

            //Конструкцию выше можно заменить на одну строку с использованием тернарного оператора:
            //return n > 1 ? (ulong)(FibStandart(n - 1) + FibStandart(n - 2)) : (ulong)n;
        }


        /// <summary>
        /// Рекурсивно рассчитывает члены последовательноси Фибоначчи до заданного номера
        /// используя массив для хранения ранее рассчитанных значений
        /// массив передается параметром и должен содержать количество элементов равное 
        /// значению рассчитываемого члена + 1
        /// Метод работает значительно быстрее, теперь можно рассчитать последовательность
        /// до 93-го члена за секунду.
        /// </summary>
        /// <param name="n">Номер члена последовательности до которого считать</param>
        /// <param name="memory">Массив элементов хранящий ранее рассчитанные члены последовательности</param>
        /// <returns>Значение члена последовательности</returns>
        private static ulong Fibonachi(int n, ulong[] memory)
        {
            if (n <= 1) return (ulong)n;

            //Если в массиве памяти данный элемент уже вычислен, то вернем его, иначе пойдем по рекурсии
            if (memory[n] != 0)
                return memory[n];
            else
            {
                memory[n] = Fibonachi(n - 1, memory) + Fibonachi(n - 2, memory);
                return memory[n];
            }
        }

        /// <summary>
        /// Вычисляе члены последовательности Фибоначчи с использованием цикла
        /// </summary>
        /// <param name="n">Номер члена последовательности до которого считать</param>
        /// <returns>Значение члена последовательности</returns>
        private static ulong FibonachiCycle(int n)
        {
            
            ulong x1 = 1; //Рассчитываемый член последовательности
            ulong x0 = 1; //Предыдущий член последовательности

            for (int i = 2; i < n; i++)
            {
                x1 = x0 + x1;
                x0 = x1 - x0;
            }
            return x1;
        }

        #endregion


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

        /// <summary> Выводит на экран сообщение и ждет нажатия любой клавиши </summary>
        /// <param name="message">Сообщение для пользователя</param>
        private static void MessageWaitKey(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Нажмите любую клавишу.");
            Console.ReadKey();
        }

        #endregion
    }
}
