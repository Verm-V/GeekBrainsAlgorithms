using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_08_02
{

    class Program
    {
        #region ---- CONSTANTS ----

        /// <summary> имя файла для записи </summary>
        const string filename = "numbers.txt";

        /// <summary>Количество элементов в сортируемом списке</summary>
        private const int ELEMENTS = 1000;
        /// <summary>Минимально возможное значение элемента в сортируемом списке</summary>
        private const int MIN_VALUE = 0;
        /// <summary>Максимально возможное значение элемента в сортируемом списке</summary>
        private const int MAX_VALUE = 10000;

        /// <summary>Количество элементов при котором бакет можно сортировать не внешне, а в оперативной памяти</summary>
        private const int MIN_BUCKET_SIZE = 10;

        #endregion

        static void Main(string[] args)
        {
            //Создаем и заполняем файл со случайными числами
            Random rnd = new Random();//Генератор случайных чисел
            using (FileStream fstream = new FileStream(filename, FileMode.Create))
            {
                Console.WriteLine("Создаем файл с случайными числами");
                for (int i = 0; i < ELEMENTS; i++)
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(rnd.Next(MIN_VALUE, MAX_VALUE).ToString() + Environment.NewLine);
                    fstream.Write(array, 0, array.Length);
                }
                Console.WriteLine("Файл со случайными числами создан");
                Console.WriteLine($"Имя файла: {filename}\n");
            }

            //Сортируем
            Console.WriteLine("Сортируем содержимое файла");
            string sortedFilename = ExternalBucketSort(filename, MIN_BUCKET_SIZE);
            Console.WriteLine($"Отсортированные данные находятся в файле: {sortedFilename}\n");

            Console.WriteLine("Press ENTER");
            Console.ReadLine();

        }


        #region ---- SORTING METHODS ----

        /// <summary>
        /// Рекурсивная External Bucket Sort
        /// </summary>
        /// <param name="unsortedFilename">Имя файла содержимое которого нужно отсортировать</param>
        /// <param name="minBucketSize">Размер бакета, меньше которого можно сортировать в оперативной памяти</param>
        /// <returns>Имя файла с отсортированными данными</returns>
        public static string ExternalBucketSort(string unsortedFilename, int minBucketSize = 0)
        {

            //Определяем минимальные и максимальные значения чисел в в файле, а также их количество
            int count = 0;
            int minValue = 0;
            int maxValue = 0;
            (count, minValue, maxValue) = GetNumbersListProperties(unsortedFilename);
            //Если все числа в файле одинаковые, то возвращаем как есть - сортировка не нужна
            if (minValue == maxValue) return unsortedFilename;

            //Определяем минимальный размер бакета, если он не задан.
            //Если не задан, то берем равным размеру списка и не используем рекурсию
            if (minBucketSize == 0) minBucketSize = count;
            //На тот случай если входящий список оказался слишком маленьким
            if (minBucketSize < 2) minBucketSize = 2;

            //Генерируем имя файла, в который будем заносить отсортированные значения
            string sortedFilename = new StringBuilder(unsortedFilename.Substring(0, unsortedFilename.Length - 4) + "_s.txt").ToString();
            //Если файл уже существует (остался от предыдущих запусков), то удаляем его.
            File.Delete(sortedFilename);

            //Если размер входящего файла меньше чем заданный ограничитель (т.е. он "влезет в память", то сортируем его вставками)
            if(count<minBucketSize)
            {
                InsertionSort(unsortedFilename, sortedFilename);
                return sortedFilename;
            }

            //Определяем количество бакетов
            //Берем корень квадратный от количества элементов в списке
            //Тогда при равномерном распределении чисел в списке, количество элмементов в каждом бакете
            //приблизительно будет равно количеству самих бакетов
            //Если же распределение чисел будет отличаться от равномерного, тогда нужно будет искать другую
            //зависимость для количества бакетов, либо задавать его явно.
            int numOfBuckets = (int)Math.Sqrt(count);
            numOfBuckets = (numOfBuckets < 2) ? 2 : numOfBuckets;//На случай слишком маленького списка
            int bucketInterval = (int)((maxValue - minValue) / numOfBuckets) + 1;//Количество чисел в диапазоне каждого бакета

            //Создаем файлы для бакетов, предварительно удалив существующие (если остались, от предыдущих запусков программы)
            for (int i = 0; i < numOfBuckets; i++)
            {
                string bucketFilename = BucketNameGenerate(unsortedFilename, i);
                File.Delete(bucketFilename);
                File.Create(bucketFilename).Close();
            }

            //Просматриваем входящий файл и помещаем каждое число в соответствующий файл бакета
            string line = string.Empty;
            int number = 0;
            using (StreamReader fstream = new StreamReader(unsortedFilename))
            {
                while ((line = fstream.ReadLine()) != null)
                {
                    count++;
                    try
                    {
                        int.TryParse(line, out number);
                        int bucket = (int)((number - minValue) / bucketInterval);
                        WriteToFileBucket(number, BucketNameGenerate(unsortedFilename, bucket));
                    }
                    catch
                    {
                        //do nothing
                    }

                }
            }

            //Сортируем каждый файл-бакет и затем помещаем отсортированные значения в соответствующий отсортированный файл
            for (int i = 0; i < numOfBuckets; i++)
            {
                //Имя файла текущего сортируемого бакета
                string bucketFilename = BucketNameGenerate(unsortedFilename, i);
                //Сортируем бакет и получаем имя файла с сортированными значениями
                string sortedBucketFileName = ExternalBucketSort(bucketFilename, minBucketSize);
                //Копируем отсортированный бакет в отсортированный файл уровнем выше
                AddSortedBucketToSortedFile(sortedFilename, sortedBucketFileName);
                //Удаляем уже отсортированный файл-бакет за ненадобностью
                File.Delete(bucketFilename);
            }

            return sortedFilename;
        }

        /// <summary>
        /// Сортировка вставками
        /// </summary>
        /// <param name="unsortedFilename">Файл который нужно отсортировать</param>
        /// <param name="sortedFilename">Файл куда будут складываться отсортированные значения</param>
        public static void InsertionSort(string unsortedFilename, string sortedFilename)
        {
            List<int> numbers = new List<int>();

            //Загоняем содержимое файла в список
            using (StreamReader fstream = new StreamReader(unsortedFilename))
            {
                string line = string.Empty;
                int number = 0;
                while ((line = fstream.ReadLine()) != null)
                {
                    try
                    {
                        int.TryParse(line, out number);
                        numbers.Add(number);
                    }
                    catch
                    {
                        //do nothing
                    }

                }

            }
            //Удаляем файл за ненадобностью
            File.Delete(unsortedFilename);

            //Сортируем
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

            //Записываем в результирующий файл
            using (FileStream fstream = new FileStream(sortedFilename, FileMode.OpenOrCreate))
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(numbers[i].ToString() + Environment.NewLine);
                    fstream.Write(array, 0, array.Length);
                }
            }
            


        }

        #endregion

        #region ---- ADDITIONAL METHODS ----

        /// <summary>
        /// Перегоняет содержимое одного файла в конец другого
        /// </summary>
        /// <param name="sortedFilename">Файл в который будут заноситься значения</param>
        /// <param name="sortedBucketFilename">Файл из которого значения будут браться</param>
        public static void AddSortedBucketToSortedFile(string sortedFilename, string sortedBucketFilename)
        {

            using (FileStream toStream = new FileStream(sortedFilename, FileMode.Append))
            {
                using (StreamReader fromStream = new StreamReader(sortedBucketFilename))
                {
                    string line = string.Empty;
                    int number = 0;
                    while ((line = fromStream.ReadLine()) != null)
                    {
                        try
                        {
                            int.TryParse(line, out number);
                            byte[] array = System.Text.Encoding.Default.GetBytes(number.ToString() + Environment.NewLine);
                            toStream.Write(array, 0, array.Length);
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                }
            }
            //Удаляем файл из которого брали значения за ненадобностью
            File.Delete(sortedBucketFilename);

        }



        /// <summary>
        /// Определяет количество чисел в файле, а так же минимальное и максимальное значение среди них
        /// </summary>
        /// <param name="filename">Имя файла содержащего список чисел</param>
        /// <returns>
        /// 1 - количество чисел в файле;
        /// 2 - минимальное значение числа в файле;
        /// 3 - максимальное значение числа в файле
        /// </returns>
        private static (int, int, int) GetNumbersListProperties(string filename)
        {

            string line;
            int count = 0;
            int max = int.MinValue;
            int min = int.MaxValue;
            int number = 0;

            using (StreamReader fstream = new StreamReader(filename))
            {
                while ((line = fstream.ReadLine()) != null)
                {
                    count++;
                    try
                    {
                        int.TryParse(line, out number);
                        if (min > number) min = number;
                        if (max < number) max = number;
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }

            return (count, min, max);

        }

        /// <summary>
        /// Генерирует имя файла для очередного бакета
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <param name="index">Номер бакета, который станет суффиксом имени файла</param>
        /// <returns></returns>
        private static string BucketNameGenerate(string filename, int index)
        {
            return new StringBuilder(filename.Substring(0, filename.Length - 4) + $"_{index}.txt").ToString();
        }

        /// <summary>
        /// Записывает число в указанный файл-бакет
        /// </summary>
        /// <param name="number">Число которое нужно занести в файл-бакет</param>
        /// <param name="filename">Имя файла-бакета</param>
        private static void WriteToFileBucket(int number, string filename)
        {

            using (FileStream fstream = new FileStream(filename, FileMode.Append))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(number + Environment.NewLine);
                fstream.Write(array, 0, array.Length);
            }
        }

        #endregion

    }

}
