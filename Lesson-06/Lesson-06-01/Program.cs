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
        { Messages.NumbersList, "Содержимое графа"},
        { Messages.Amount, "всего"},
        { Messages.Contain, "Вершина присутствует в графе."},
        { Messages.NotContain, "Такой вершины нет в графе."},
        { Messages.WhiteSpaceLine, "        "}
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] mainMenu = new string[]
        {
            "Поиск в ширину",
            "Поиск в глубину\n",
            "Работа с графом\n",
            "Выход"
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] graphMenu = new string[]
        {
            "Добавить ребро",
            "Удалить ребро",
            "Поменять вершины местами",
            "Передвинуть вершину",
            "Выход"
        };

        /// <summary> Пункты меню передвижения, последний пункт возврат </summary>
        private static readonly string[] moveMenu = new string[]
        {
            "Влево",
            "Вправо",
            "Вниз",
            "Вверх",
            "Выбрать вершину",
            "Выход"
        };

        #endregion

        #region -- NUMBER CONSTS --

        /// <summary>Задержка отрисовки по умолчанию</summary>
        public const int DELAY = 500;

        /// <summary>Минимальное значение числа хранимого в узле дерева</summary>
        private const int VALUE_MIN = 0;
        /// <summary>Максимальное значение числа хранимого в узле дерева</summary>
        private const int VALUE_MAX = 99;
        /// <summary>Количество узлов в дереве (для первоначального случайного заполнения)</summary>
        private const int ELEMENTS = 14;
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
            int delay = DELAY;

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
                rnd = new Random();//2

            //Формируем сообщение главного меню
            string mainMenuMessage = MenuMessage(mainMenu);

            #endregion

            #region ---- GRAPH CREATION ----
            //Создаем граф
            Graph graph = new Graph();
            graph.Delay = delay;

            //Добавляем в граф вершины
            for (int i = 0; i < elements; i++)
            {
                graph.AddNode(i);
            }

            //Добавляем связи между вершинами
            for (int i = 0; i < elements; i++)
            {
                for (int j = 0; j < rnd.Next(1)+1; j++)
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

            //Основной цикл
            MainMenu(graph, mainMenu);

            return 0;

        }


        /// <summary>Вызов печати графаы</summary>
        /// <param name="graph"></param>
        /// <param name="method"></param>
        private static void Print(Graph graph)
        {
            Console.Clear();
            GraphPrinter.ClearText();
            GraphPrinter.Print(graph, true);

        }

        /// <summary>Формирует строку содержащую пункты меню для вывода на экран</summary>
        /// <param name="menu">Массив строк с пунктами меню</param>
        /// <returns>Строку содержащую пункты меню</returns>
        private static string MenuMessage(string[] menu)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"\n{messages[Messages.ChooseOption]}\n");
            for (int i = 0; i < menu.Length; i++)
                stringBuilder.Append($"{(i < menu.Length - 1 ? i + 1 : 0)} - {menu[i]}\n");

            return stringBuilder.ToString();
        }

        /// <summary>Главное меню</summary>
        /// <param name="graph">Граф</param>
        /// <param name="menu">Содержание меню</param>
        private static void MainMenu(Graph graph, string[] menu)
        {
            string menuMessage = MenuMessage(menu);
            Print(graph);

            bool isExit = false;
            while (!isExit)
            {
                Print(graph);
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://BFS
                        Print(graph);
                        bool isContain = graph.BFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(graph);
                        break;
                    case 2://DFS
                        Print(graph);
                        isContain = graph.DFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(graph);
                        break;
                    case 3://graph menu
                        Print(graph);
                        GraphMenu(graph, graphMenu);
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }
            }
        }


        /// <summary>Меню работы с графом</summary>
        /// <param name="graph">Граф</param>
        /// <param name="menu">Содержание меню</param>
        private static void GraphMenu(Graph graph, string[] menu)
        {
            string menuMessage = MenuMessage(menu);
            Print(graph);
            bool isExit = false;
            while (!isExit)
            {
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://add edge between two nodes
                        Print(graph);
                        int firstID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        int secondID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        int weight = NumberInput(messages[Messages.EnterNumber], 0, MAX_WEIGHT - 1, false);
                        graph.AddEdge(firstID, secondID, weight);
                        Print(graph);
                        break;
                    case 2://add edge between two nodes
                        Print(graph);
                        firstID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        secondID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        graph.RemoveEdge(firstID, secondID);
                        Print(graph);
                        break;
                    case 3://change two nodes
                        Print(graph);
                        firstID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        secondID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        GraphPrinter.ChangeTwoNodes(firstID, secondID);
                        Print(graph);
                        break;
                    case 4://move one node
                        Print(graph);
                        MoveMenu(graph, moveMenu);
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }
                Console.Clear();
                Print(graph);
            }

        }



        /// <summary>Меню передвижения вершины</summary>
        /// <param name="graph">Граф</param>
        /// <param name="menu">Содержание меню</param>
        private static void MoveMenu(Graph graph, string[] menu)
        {
            string menuMessage = MenuMessage(menu);
            int id = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
            Print(graph);
            bool isExit = false;
            while (!isExit)
            {
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://Left
                        GraphPrinter.MoveNode(id, -1, 0);
                        break;
                    case 2://Right
                        GraphPrinter.MoveNode(id, 1, 0);
                        break;
                    case 3://Down
                        GraphPrinter.MoveNode(id, 0, 1);
                        break;
                    case 4://Up
                        GraphPrinter.MoveNode(id, 0, -1);
                        break;
                    case 5://change node
                        Print(graph);
                        id = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }
                Console.Clear();
                Print(graph);
            }
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
