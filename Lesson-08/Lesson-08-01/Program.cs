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
        private const int MIN_VALUE = -10000;
        /// <summary>Максимально возможное значение элемента в сортируемом списке</summary>
        private const int MAX_VALUE = 10000;

        /// <summary>Генератор случайных чисел</summary>
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            //Создаем и заполняем список чисел
            List<int> unsorted = new List<int>();
            Console.WriteLine("Создаем спсок случайных чисел");
            for (int i = 0; i < ELEMENTS; i++)
                unsorted.Add(rnd.Next(MAX_VALUE - MIN_VALUE) + MIN_VALUE);
            Console.WriteLine("Список случайных чисел отсортирован");

            //Сортируем
            Console.WriteLine("Сортируем список");
            List<int> sorted = BucketSort(unsorted);
            Console.WriteLine("Список отсортирован\n");
            Console.WriteLine("Если установить в конце метода Main точку останова,\n" +
                "то в окне отладки можно будет сравнить два списка чисел,\n" +
                "неотсортированный и отсортированный.");


            Console.ReadLine();

        }

        /// <summary>Выполняет bucket sort на заданном списке чисел</summary>
        /// <param name="numbers">Список чисел который нужно отсортировать</param>
        /// <param name="minValue">Минимальное значение во входящем списке чисел</param>
        /// <param name="maxValue">Максимальное значение во входящем списке чисел</param>
        /// <param name="minBucketSize">Параметр рекурсии, 
        /// если размер bucket'а больше, то он будет отсортирован рекурсивно через bucket sort,
        /// иначе через сортировку вставками (insertion sort)</param>
        /// <returns>Отсортированный список, входящий список остается неизменным (для удобства отслеживания результата)
        /// ВНИМАНИЕ!, при слишком маленьком значении возможно переполнение стека при рекурсии</returns>
        public static List<int> BucketSort(List<int> numbers, int minBucketSize = 0)
        {
            //Определяем минимальные и максимальные значения чисел в списке
            int minValue = numbers.Min();
            int maxValue = numbers.Max();
            //Если все числа в списке одинаковые, то возвращаем список как есть - сортировка не нужна
            if (minValue == maxValue) return numbers; 

            //Определяем минимальный размер бакета, если он не задан.
            //Если не задан, то берем равным размеру списка и не используем рекурсию
            if (minBucketSize == 0) minBucketSize = numbers.Count;
            //На тот случай если входящий список оказался слишком маленьким
            if (minBucketSize < 2) minBucketSize = 2;

            List<int> sorted = new List<int>();//Список для отсортированных значений

            //Определяем количество бакетов
            //Берем корень квадратный от количества элементов в списке
            //Тогда при равномерном распределении чисел в списке, количество элмементов в каждом бакете
            //приблизительно будет равно количеству самих бакетов
            //Если же распределение чисел будет отличаться от равномерного, тогда нужно будет искать другую
            //зависимость для количества бакетов, либо задавать его явно.
            int numOfBuckets = (int)Math.Sqrt(numbers.Count);
            numOfBuckets = (numOfBuckets < 2) ? 2 : numOfBuckets;//На случай слишком маленького списка
            int bucketInterval = (int)((maxValue - minValue) / numOfBuckets) + 1;//Количество чисел в диапазоне каждого бакета

            //Создаем массив бакетов
            List<int>[] buckets = new List<int>[numOfBuckets];
            for (int i = 0; i < numOfBuckets; i++)
                buckets[i] = new List<int>();

            //Просматриваем входящий список и помещаем каждое число в соответствующий бакет
            for (int i = 0; i < numbers.Count; i++)
            {
                //Определяем номер бакета в который пойдет число
                int bucket = (int)((numbers[i] - minValue) / bucketInterval);
                buckets[bucket].Add(numbers[i]);
            }

            //Сортируем каждый бакет и затем помещаем отсортированные значения в общий отсортированный список
            for (int i = 0; i < numOfBuckets; i++)
            {
                //Если очередной бакет не пустой, от сортируем его и добавляем в общий отсортированный список
                if(buckets[i].Count != 0)
                {
                    List<int> temp = new List<int>();//Временный список в который заносим текущий бакет
                                                     //Если в бакете чисел больше заданного порога, то уходим в рекурсию
                    if (buckets[i].Count > minBucketSize)
                        temp = BucketSort(buckets[i], minBucketSize);
                    //Иначе используем сортировку вставками
                    else
                        temp = InsertionSort(buckets[i]);

                    sorted.AddRange(temp);
                }
            }

            return sorted;
        }

        /// <summary>
        /// Сортировка списка чисел вставками (insertion sort)
        /// </summary>
        /// <param name="numbers">Список чисел для сортировки</param>
        /// <returns>Отсортированный список чисел</returns>
        public static List<int> InsertionSort(List<int> numbers)
        {
            for (var i = 1; i < numbers.Count; i++)
            {
                //Проверяемое значение
                var key = numbers[i];
                //Указатель в отсортированной части списка
                var j = i;
                //Если не дошли до начала и текущее значение меньше предыдущего,
                //то меняем их местами и двигаем указатель назад
                while ((j > 0) && (numbers[j - 1] > key))
                {
                    (numbers[j - 1], numbers[j]) = (numbers[j], numbers[j - 1]);
                    j--;
                }
            }

            return numbers;
        }



    }
}
