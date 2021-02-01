using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_05_01
{
    class Program
    {

        #region -- STRING CONSTS --

        /// <summary> Ключи для словаря аргументов командной строки </summary>
        enum Arguments
        {
            Help,
            Seed,
            SeedHelp
        }

        /// <summary> Словарь аргументов командной строки </summary>
        private static readonly Dictionary<Arguments, string> arguments = new Dictionary<Arguments, string>
        {
        { Arguments.Help, "-h"},
        { Arguments.Seed, "-s"},
        { Arguments.SeedHelp, "-s <int> - seed for random number generator"},
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
            "Бинарный поиск",
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
        private const int ELEMENTS = 20;

        #endregion

        /// <summary>Генератор случайных чисел</summary>
        private static Random rnd;


        static int Main(string[] args)
        {
            #region ---- INIT ----

            int seed = 0;

            //Обработка аругментов командной строки
            if (args.Length != 0)
            {
                if (args[0] == arguments[Arguments.Help])//Вывод справки по аргументам
                {
                    Console.WriteLine(arguments[Arguments.SeedHelp]);
                    return 0;
                }
                else if (args[0] == arguments[Arguments.Seed])//Изменение seed'a
                {
                    try
                    {
                        int.TryParse(args[1], out seed);
                    }
                    catch //Если аргумент не введен или введен неправильно, то будет полный рандом
                    {
                        seed = 0;
                    }
                }
            }

            if(seed!=0)
                rnd = new Random(seed);
            else
                rnd = new Random();

            //Формируем сообщение главного меню
            string mainMenuMessage = "\n" + messages[Messages.ChooseOption] + "\n\n";
            for (int i = 0; i < mainMenu.Length; i++)
                mainMenuMessage += $"{i + 1} - {mainMenu[i]}\n";

            #endregion

            //Создаем дерево
            BTree tree = new BTree();

            for (int i = 0; i < ELEMENTS; i++)
            {
                AddRandomNumberToTree(tree, rnd);
                Console.Clear();
            }

            bool printMethod = false;

            Print(tree, printMethod);


            //Основной цикл
            bool isExit = false;
            while (!isExit)
            {
                int input = NumberInput(mainMenuMessage, 1, mainMenu.Length);
                switch (input)
                {
                    case 1://binary search
                        Print(tree, printMethod);
                        bool isContain = tree.BinarySearch(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(tree, printMethod);
                        break;
                    case 2://BFS
                        Print(tree, printMethod);
                        isContain = tree.BFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(tree, printMethod);
                        break;
                    case 3://DFS
                        Print(tree, printMethod);
                        isContain = tree.DFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(tree, printMethod);
                        break;
                    case 4://exit
                        isExit = true;
                        break;
                }
                
            }



            return 0;

        }

        private static void AddRandomNumberToTree(BTree tree, Random rnd)
        {
            bool isDone = false;
            while (!isDone)
            {
                int newValue = rnd.Next(VALUE_MIN, VALUE_MAX+1);
                if (!tree.Contains(newValue))
                {
                    tree.AddNode(newValue);
                    isDone = true;
                }

            }
        }

        private static void Print(BTree tree, bool method = false)
        {
            Console.Clear();
            if (method)
            {
                tree.Print();
            }
            else
            {
                BTreePrinter.Print(tree.Root, true);
            }
        }


        #region -------- Вспомогательные методы --------

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