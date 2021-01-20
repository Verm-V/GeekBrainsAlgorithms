using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Lesson_03
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }


    [Config(typeof(MyConfig))]

    public class BenchmarkClass
    {
        #region ---- FIELDS ----

        /// <summary>Минимальное возможное значение координат</summary>
        private const int MIN = -1000;
        /// <summary>Максимальное возможное значение координат</summary>
        private const int MAX = -1000;
        /// <summary>Количество элементов в массиве со случайными координатами</summary>
        private const int ELEMENTS = 1000;
        /// <summary>Указатель для перебора кординат в массиве</summary>
        private int index = 0;

        /// <summary>Массив содержащий случайные значения координат</summary>
        private int[] randomCoordinates = new int[ELEMENTS];

        #endregion

        #region ---- CONFIG ----

        /// <summary>
        /// Конфиг бенчмарка
        /// </summary>
        private class MyConfig : ManualConfig
        {
            public MyConfig()
            {
                AddColumn(StatisticColumn.Max);
                AddColumn(StatisticColumn.Median);

            }
        }

        #endregion

        #region ---- CONSTRUCTORS ----

        /// <summary>
        /// Конструктор заполняющий массив координат случайной информацией
        /// </summary>
        public BenchmarkClass()
        {
            ///Заполняем массив случайными числами
            Random rnd = new Random();
            for (int i = 0; i < randomCoordinates.Length; i++)
            {
                randomCoordinates[i] = rnd.Next(MIN, MAX);
                index = rnd.Next(ELEMENTS);
            }
        }

        #endregion

        #region ---- ADDITIONAL METHODS ----

        /// <summary>
        /// Выдает два числа из массива координат и сдвигает его указатель на следующую позицию
        /// </summary>
        /// <returns>Кортеж с двумя координатами</returns>
        public (float, float) giveMeCoordinates()
        {
            if (index > ELEMENTS - 4)
                index = 0;
            else
                index += 2;
            return (randomCoordinates[index], randomCoordinates[index + 1]);
        }

        #endregion

        #region ---- DATA TYPES ----

        /// <summary>Точка с координатами виде класса со сзначениями X, Y во float</summary>
        public class PointClassFloat
        {
            public float X { get; set; }
            public float Y { get; set; }

            public PointClassFloat((float x, float y) coord)
            {
                X = coord.x;
                Y = coord.y;
            }
        }

        /// <summary>Точка с координатами виде структуры со сзначениями X, Y во float</summary>
        public struct PointStructFloat
        {
            public float X;
            public float Y;

            public PointStructFloat((float x, float y) coord)
            {
                X = coord.x;
                Y = coord.y;
            }
        }
        /// <summary>Точка с координатами виде структуры со сзначениями X, Y в double</summary>
        public struct PointStructDouble
        {
            public double X;
            public double Y;
            public PointStructDouble((double x, double y) coord)
            {
                X = coord.x;
                Y = coord.y;
            }

        }

        #endregion

        #region ---- TESTED METHODS ----
        /// <summary>Рассчет расстояния между двумя точками реализованными классов во float</summary>
        /// <param name="pointOne">Точка 1</param>
        /// <param name="pointTwo">Точка 2</param>
        /// <returns>Расстояние между точками</returns>
        public float PointDistanceClassFloat(PointClassFloat pointOne, PointClassFloat pointTwo)
        {
            float x = pointOne.X - pointTwo.X;
            float y = pointOne.Y - pointTwo.Y;
            return MathF.Sqrt((x * x) + (y * y));
        }

        /// <summary>Рассчет расстояния между двумя точками реализованными структурами во float</summary>
        /// <param name="pointOne">Точка 1</param>
        /// <param name="pointTwo">Точка 2</param>
        /// <returns>Расстояние между точками</returns>
        public float PointDistanceStructFloat(PointStructFloat pointOne, PointStructFloat pointTwo)
        {
            float x = pointOne.X - pointTwo.X;
            float y = pointOne.Y - pointTwo.Y;
            return MathF.Sqrt((x * x) + (y * y));
        }

        /// <summary>Рассчет расстояния между двумя точками реализованными структурами в double</summary>
        /// <param name="pointOne">Точка 1</param>
        /// <param name="pointTwo">Точка 2</param>
        /// <returns>Расстояние между точками</returns>
        public double PointDistanceStructDouble(PointStructDouble pointOne, PointStructDouble pointTwo)
        {
            double x = pointOne.X - pointTwo.X;
            double y = pointOne.Y - pointTwo.Y;
            return Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>Рассчет квадрата расстояния между двумя точками реализованными структурами во float</summary>
        /// <param name="pointOne">Точка 1</param>
        /// <param name="pointTwo">Точка 2</param>
        /// <returns>Квадрат расстояния между точками</returns>
        public float PointDistanceShortStructFloat(PointStructFloat pointOne, PointStructFloat pointTwo)
        {
            float x = pointOne.X - pointTwo.X;
            float y = pointOne.Y - pointTwo.Y;
            return ((x * x) + (y * y));
        }

        #endregion

        #region ---- BENCHMARKS ----

        [Benchmark(Description = "Расстояние через классы float", Baseline = true)]
        public void TestPointDistanceClassFloat()
        {
            PointClassFloat pointOne = new PointClassFloat(giveMeCoordinates());
            PointClassFloat pointTwo = new PointClassFloat(giveMeCoordinates());
            PointDistanceClassFloat(pointOne, pointTwo);
        }

        [Benchmark(Description = "Расстояние через структуры float")]
        public void TestPointDistanceStructFloat()
        {
            PointStructFloat pointOne = new PointStructFloat(giveMeCoordinates());
            PointStructFloat pointTwo = new PointStructFloat(giveMeCoordinates());
            PointDistanceStructFloat(pointOne, pointTwo);
        }

        [Benchmark(Description = "Расстояние через структуры double")]
        public void TestPointDistanceStructDouble()
        {
            PointStructDouble pointOne = new PointStructDouble(giveMeCoordinates());
            PointStructDouble pointTwo = new PointStructDouble(giveMeCoordinates());
            PointDistanceStructDouble(pointOne, pointTwo);
        }

        [Benchmark(Description = "Квадрат расстояния через структуры float")]
        public void TestPointDistanceShortStructFloat()
        {
            PointStructFloat pointOne = new PointStructFloat(giveMeCoordinates());
            PointStructFloat pointTwo = new PointStructFloat(giveMeCoordinates());
            PointDistanceShortStructFloat(pointOne, pointTwo);
        }

        #endregion

    }
}