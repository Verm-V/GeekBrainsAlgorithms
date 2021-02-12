using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_08_01
{
    class Program
    {
        /// <summary>Количество элементов в сортируемом списке</summary>
        private const int ELEMENTS = 10000;
        /// <summary>Минимально возможное значение элемента в сортируемом списке</summary>
        private const int MIN_VALUE = 0;
        /// <summary>Максимально возможное значение элемента в сортируемом списке</summary>
        private const int MAX_VALUE = 10000;

        /// <summary>Генератор случайных чисел</summary>
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            //Создаем и заполняем список чисел
            List<int> numbers = new List<int>();
            for (int i = 0; i < ELEMENTS; i++)
                numbers.Add(rnd.Next(MAX_VALUE - MIN_VALUE) + MIN_VALUE);

            BucketSort(numbers, MIN_VALUE, MAX_VALUE);

            Console.ReadLine();

        }

        /// <summary>
        /// Выполняет bucket sort на заданном массиве
        /// </summary>
        /// <param name="numbers">Список который нужно отсортировать</param>
        /// <param name="min">минимальное значение в наборе чисел в списке</param>
        /// <param name="max">максимальное значение в наборе чисел в списке</param>
        private static void BucketSort(List<int> numbers, int min, int max)
        {
            //Определяем количество bucket'ов
            int buckets = (int)Math.Sqrt(numbers.Count);


        }
    }
}
