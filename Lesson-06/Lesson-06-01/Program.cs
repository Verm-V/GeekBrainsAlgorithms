using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    class Program
    {
        #region -- STRING CONSTS --

        /// <summary> Ключи для словаря аргументов командной строки </summary>
        enum Arguments
        {
            Help,
            Seed,
            Elements,
            Delay,
            HelpText
        }

        /// <summary> Словарь аргументов командной строки </summary>
        private static readonly Dictionary<Arguments, string> arguments = new Dictionary<Arguments, string>
        {
        { Arguments.Help, "-h"},
        { Arguments.Seed, "-s"},
        { Arguments.Elements, "-e"},
        { Arguments.Delay, "-d"},
        { Arguments.HelpText, 
                "-s <int> - seed for random number generator\n" +
                "-e <int> - max number of graph nodes\n" + 
                "-d <int> - visualization delay"},
        };

        /// <summary> Ключи для словаря с сообщениями об ошибках </summary>
        enum Errors
        {
            ItemNotFound,
            RepeatInputError,
        }

        /// <summary> Словарь с сообщениями об ошибках </summary>
        private static readonly Dictionary<Errors, string> errors = new Dictionary<Errors, string>
        {
        { Errors.ItemNotFound, "Элемент не найден."},
        { Errors.RepeatInputError, "Ошибка. Повторите ввод."}
        };

        /// <summary> Ключи для словаря с ссобщениями для пользователя </summary>
        enum Messages
        {
            ChooseOption,
            EnterNumber,
            PressAnyKey,
            From,
            To,
            NumbersList,
            Amount,
            Contain,
            NotContain,
            WhiteSpaceLine
        }

        /// <summary> Словарь с сообщениями для пользователя </summary>
        private static readonly Dictionary<Messages, string> messages = new Dictionary<Messages, string>
        {
        { Messages.ChooseOption, "Выберите опцию:"},
        { Messages.EnterNumber, "Введите число: "},
        { Messages.PressAnyKey, "Нажмите любую клавишу."},
        { Messages.From, "от"},
        { Messages.To, "до"},
        { Messages.NumbersList, "Содержимое дерева"},
        { Messages.Amount, "всего"},
        { Messages.Contain, "Данное число присутствует в дереве."},
        { Messages.NotContain, "Данного числа нет в дереве."},
        { Messages.WhiteSpaceLine, "        "}
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] mainMenu = new string[]
        {
            "Поиск в ширину",
            "Поиск в глубину\n",
            "Выход"
        };

        #endregion

        #region -- NUMBER CONSTS --

        /// <summary>Минимальное значение числа хранимого в узле дерева</summary>
        private const int VALUE_MIN = 0;
        /// <summary>Максимальное значение числа хранимого в узле дерева</summary>
        private const int VALUE_MAX = 99;
        /// <summary>Количество узлов в дереве (для первоначального случайного заполнения)</summary>
        private const int ELEMENTS = 5;
        /// <summary>Максимальный вес ребра в графе</summary>
        private const int MAX_WEIGHT = 5;

        #endregion

        /// <summary>Генератор случайных чисел</summary>
        private static Random rnd;

        static int Main(string[] args)
        {
            #region ---- INIT ----

            int seed = 0;
            int elements = ELEMENTS;
            int delay = GraphPrinter.DELAY;

            //Обработка аругментов командной строки
            if (args.Length != 0)
            {
                if (args[0] == arguments[Arguments.Help])//Вывод справки по аргументам
                {
                    Console.WriteLine(arguments[Arguments.HelpText]);
                    return 0;
                }
                else
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == arguments[Arguments.Seed])//Изменение seed'a
                        {
                            try
                            {
                                int.TryParse(args[i+1], out seed);
                            }
                            catch
                            {
                            }
                        }
                        else if (args[i] == arguments[Arguments.Elements])//Изменение количества элементов
                        {
                            try
                            {
                                int.TryParse(args[i + 1], out elements);
                            }
                            catch
                            {
                            }
                        }
                        else if (args[i] == arguments[Arguments.Delay])//Изменение задержки визуализации
                        {
                            try
                            {
                                int.TryParse(args[i + 1], out delay);
                            }
                            catch
                            {
                            }
                        }

                    }


            }

            if (seed != 0)
                rnd = new Random(seed);
            else
                rnd = new Random();

            //Формируем сообщение главного меню
            string mainMenuMessage = "\n" + messages[Messages.ChooseOption] + "\n\n";
            for (int i = 0; i < mainMenu.Length; i++)
                mainMenuMessage += $"{i + 1} - {mainMenu[i]}\n";

            #endregion

            #region ---- GRAPH CREATION ----
            //Создаем граф
            Graph graph = new Graph();

            //Добавляем в граф вершины
            for (int i = 0; i < elements; i++)
            {
                graph.AddNode(i);
            }

            //Добавляем связи между вершинами
            for (int i = 0; i < elements; i++)
            {
                for (int j = 0; j < rnd.Next(1)+2; j++)
                {
                    bool isGenerated = false;
                    while(!isGenerated)
                    {
                        int toNode = rnd.Next(elements);
                        if(toNode!=i)
                            isGenerated = graph.AddEdge(i, toNode, rnd.Next(MAX_WEIGHT));
                        if (graph.Nodes[i].Edges.Count >= 2) isGenerated = true;
                    }
                }
            }

            #endregion

            #region ---- MAIN CYCLE ----

            //Основной цикл
            bool isExit = false;
            while (!isExit)
            {
                Print(graph, delay);
                int input = NumberInput(mainMenuMessage, 1, mainMenu.Length);
                switch (input)
                {
                    case 1://BFS
                        Print(graph, delay);
                        bool isContain = graph.BFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(graph, delay);
                        break;
                    case 2://DFS
                        Print(graph, delay);
                        isContain = graph.DFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(graph, delay);
                        break;
                    case 3://exit
                        isExit = true;
                        break;
                }

            }

            #endregion

            return 0;

        }


        /// <summary>Вызов печати графаы</summary>
        /// <param name="graph"></param>
        /// <param name="method"></param>
        private static void Print(Graph graph, int delay)
        {
            Console.Clear();
            GraphPrinter.Print(graph, true, delay);

        }

        #region ---- ADDITIONAL METHODS ----

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
            Console.WriteLine($"{message}({messages[Messages.From]} {min} {messages[Messages.To]} {max})");
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
                    {
                        Console.WriteLine(errors[Errors.RepeatInputError]);
                        try
                        {
                            Console.CursorLeft = 0;//Если ввели что-то не то, то просто возвращаем курсор на прежнее место
                            Console.CursorTop -= 2;
                            Console.Write(messages[Messages.WhiteSpaceLine]);
                            Console.CursorLeft = 0;
                        }
                        catch
                        {
                            //В случае ошибки, ввода каких-либо управляющих символов или попытках выхода курсора
                            //за пределы консоли, просто ничего не делаем и остаемся на месте
                        }
                    }
            }
            Console.WriteLine();
            return input;
        }


        /// <summary> Выводит на экран сообщение и ждет нажатия любой клавиши </summary>
        /// <param name="message">Сообщение для пользователя</param>
        private static void MessageWaitKey(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(messages[Messages.PressAnyKey]);
            Console.ReadKey();
        }
        #endregion

    }
}
