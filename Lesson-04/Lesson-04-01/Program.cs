using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Diagnosers;
using System.Collections.Generic;
using System.Text;

namespace Lesson_04_01
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
        }
    }

    //[MemoryDiagnoser] //Оставил для будущих экспериментов.
    [Config(typeof(MyConfig))]

    public class BenchmarkClass
    {
        #region ---- FIELDS ----

        /// <summary>Количество элементов в массиве со случайными координатами</summary>
        private const int ELEMENTS = 10000;
        /// <summary>Длина строк которые будут загоняться в коллекцию</summary>
        private const int LENGTH = 20;
        /// <summary>Строка которая будет использоваться для проверки</summary>
        private const string CHECKSTRING = "HL4JH53KJ45H324H52LK";


        /// <summary>Массив для случайных строк</summary>
        public string[] array = new string[ELEMENTS];
        /// <summary>Хэш таблица для случайных строк</summary>
        public HashSet<string> hashset = new HashSet<string>();


        /// <summary>Генератор случайных чисел</summary>
        private Random rnd = new Random();

        #endregion

        #region ---- CONFIG ----

        /// <summary>
        /// Конфиг бенчмарка
        /// </summary>
        private class MyConfig : ManualConfig
        {
            public MyConfig()
            {

                //AddJob(Job.Dry);
                AddJob(Job.ShortRun);
                AddLogger(ConsoleLogger.Default);
                AddColumn(
                    TargetMethodColumn.Method,
                    StatisticColumn.Mean,
                    StatisticColumn.StdErr,
                    StatisticColumn.StdDev,
                    StatisticColumn.Max,
                    StatisticColumn.Median,
                    BaselineRatioColumn.RatioMean);
                AddExporter(
                    AsciiDocExporter.Default,
                    MarkdownExporter.GitHub,
                    HtmlExporter.Default);
                AddAnalyser(EnvironmentAnalyser.Default);
                AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(maxDepth: 4, exportDiff: false)));
                UnionRule = ConfigUnionRule.AlwaysUseLocal;

            }
        }

        #endregion

        #region ---- CONSTRUCTORS ----

        /// <summary>
        /// Конструктор заполняющий массив координат случайной информацией
        /// </summary>
        public BenchmarkClass()
        {
            //Заполняем массив случайными числами
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = GenerateString();
                hashset.Add(GenerateString());
            }
        }

        #endregion

        #region ---- ADDITIONAL METHODS ----

        /// <summary>Генерирует случаюную строку</summary>
        /// <returns>Строка сгенерированная с помощью генератора случайных чисел</returns>
        public string GenerateString()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder newString = new StringBuilder(LENGTH);

            for (int i = 0; i < LENGTH; ++i)
                newString.Append(chars[rnd.Next(chars.Length)]);

            return newString.ToString();
        }

        #endregion

        #region ---- TESTED METHODS ----

        /// <summary>Проверяет есть ли строка в массиве</summary>
        /// <param name="arr">Проверяемый массив</param>
        /// <param name="checkString">Строка которую ищем</param>
        /// <returns>true если строка есть в массиве</returns>
        public bool CheckStringInCollection(string[] arr, string checkString)
        {
            bool isStringInArray = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == checkString)
                    isStringInArray = true;
            }
            return isStringInArray;
        }

        /// <summary>Проверяет есть ли строка в хэштаблице</summary>
        /// <param name="set">Проверяемая хэштаблица</param>
        /// <param name="checkString">Строка которую ищем</param>
        /// <returns>true если строка есть в хэштаблице</returns>
        public bool CheckStringInCollection(HashSet<string> set, string checkString)
        {
            return set.Contains(checkString);
        }

        #endregion

        #region ---- BENCHMARKS ----

        [Benchmark(Description = "Тест массива", Baseline = true)]
        public void TestArray()
        {
            CheckStringInCollection(array, CHECKSTRING);
        }

        [Benchmark(Description = "Тест хэш таблицы")]
        public void TestHashSet()
        {
            CheckStringInCollection(hashset, CHECKSTRING);
        }

        #endregion

    }
}