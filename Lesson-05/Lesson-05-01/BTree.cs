using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_05_01
{
    /// <summary>Класс двоичного дерева поиска</summary>
    public class BTree
    {
        /// <summary>Задержка для визуализации алгоритма</summary>
        private const int DELAY = 250;

        #region ---- PROPERTIES

        /// <summary>Корень дерева</summary>
        public Node Root
        {
            get;
            internal set;
        }

        /// <summary>Количество узлов в дереве</summary>
        public int Count
        {
            get;
            private set;
        }

        #endregion

        #region ---- ADDITION METHODS ----

        /// <summary>Добавляет новый узел</summary>
        /// <param name="value">Значение хранимое в узле</param>
        public void AddNode(int value)
        {
            // 1 вариант:  Дерево пустое - создаем корень дерева
            if (Root == null)
                Root = new Node(value, null, this);
            // 2 вариант: Дерево не пустое - ищем место для добавления нового узла
            else
            {
                //AddToNode(Root, value);
                Node current = Root;//Текущий узел

                bool isDone = false;//Флаг окончания процесса вставки
                while (!isDone)//Проверяем пока не дойдем до пустого узла
                {
                    Node curent = Root;
                    int check = current.CompareTo(value);
                    if (check > 0)//Если вставляемое значение меньше то влево
                    {
                        if(current.Left!=null)
                        {
                            current = current.Left;
                        }
                        else
                        {
                            current.Left = new Node(value, current, this);
                            Count++;
                            current.Balance();
                            isDone = true;
                        }
                    }
                    else if (check < 0)//Если вставляемое значение больше то вправо
                    {
                        if (current.Right != null)
                        {
                            current = current.Right;
                        }
                        else
                        {
                            current.Right = new Node(value, current, this);
                            Count++;
                            current.Balance();
                            isDone = true;
                        }
                    }
                    else//Если совпадает, то заносить в дерево не будем (дубликаты не нужны)
                        isDone = true;
                }

            }


        }

        #endregion

        #region ---- SEARCH METHODS ----

        /// <summary>Проверяет содержит ли дерево заданное значение</summary>
        /// <param name="value">Искомое значение</param>
        /// <returns>true, если узел с таким значением есть в дереве</returns>
        public bool Contains(int value)
        {
            return Find(value) != null;
        }

        /// <summary> 
        /// Находит и возвращает узел который содержит искомое значение.
        /// Если значение не найдено, возвращает null. 
        /// Так же возвращает родительский узел.
        /// </summary> /// 
        /// <param name="value">Значение поиска</param> 
        /// <param name="parent">Родительский элемент для найденного значения/// </param> 
        /// <returns> Найденный узел (null, если узел не найден) /// </returns> 
        private Node Find(int value)
        {
            // Указатель на текущий узел. Начинаем с корня дерева
            Node current = Root;

            // Пока текщий узел на пустой 
            while (current != null)
            {
                int result = current.CompareTo(value);

                // Если значение меньшне текущего - переход влево 
                if (result > 0)
                    current = current.Left;
                // Если значение больше текщего - переход вправо
                else if (result < 0)
                    current = current.Right;
                // Если значение совпадает - узел найден
                else
                    break;
            }
            return current;
        }

        /// <summary>Возвращает количество узлов в дереве</summary>
        /// <returns>Количество узлов в дереве</returns>
        public int GetCount()
        {
            return Count;
        }

        #endregion

        #region ---- NEW SEARCH METHODS ----

        /// <summary>
        /// Метод бинарного поиска с задержкой и раскраской для визуализации алгоритма
        /// </summary>
        /// <param name="value">Искомое значение</param>
        /// <returns>true, если узел с таким значением есть в дереве</returns>
        public bool BinarySearch(int value)
        {
            // Указатель на текущий узел. Начинаем с корня дерева
            Node current = Root;

            // Пока текщий узел на пустой 
            while (current != null)
            {
                int result = current.CompareTo(value);
                ColorPrint();

                // Если значение меньшне текущего - переход влево 
                if (result > 0)
                {
                    current.Color = ConsoleColor.Red;
                    current = current.Left;
                }
                // Если значение больше текщего - переход вправо
                else if (result < 0)
                {
                    current.Color = ConsoleColor.Red;
                    current = current.Right;
                }
                // Если значение совпадает - узел найден
                else
                {
                    current.Color = ConsoleColor.Green;
                    break;
                }
            }

            ColorPrint();

            return current != null; ;
        }

        /// <summary>
        /// Поиск (обход) дерева в ширину
        /// </summary>
        /// <param name="value">Искомое значение</param>
        /// <returns>true, если узел с таким значением есть в дереве</returns>
        public bool BFS(int value)
        {
            Queue<Node> bufer = new Queue<Node>();
                        
            bufer.Enqueue(this.Root);
            
            bool isFound = false;
            while (bufer.Count!=0 && !isFound)
            {
                Node element = bufer.Dequeue();
                if (element.Value == value) isFound = true;
                element.Color = isFound ? ConsoleColor.Green : ConsoleColor.Red;
                ColorPrint();
                if (element.Left != null) bufer.Enqueue(element.Left);
                if (element.Right != null) bufer.Enqueue(element.Right);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

            return isFound;
        }

        /// <summary>
        /// Поиск (обход) дерева в глубину
        /// </summary>
        /// <param name="value">Искомое значение</param>
        /// <returns>true, если узел с таким значением есть в дереве</returns>
        public bool DFS(int value)
        {
            Stack<Node> bufer = new Stack<Node>();

            bufer.Push(this.Root);

            bool isFound = false;
            while (bufer.Count != 0 && !isFound)
            {
                Node element = bufer.Pop();
                if (element.Value == value) isFound = true;
                element.Color = isFound ? ConsoleColor.Green : ConsoleColor.Red;
                ColorPrint();
                if (element.Right != null) bufer.Push(element.Right);
                if (element.Left != null) bufer.Push(element.Left);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

            return isFound;
        }

        #endregion

        #region ---- REMOVE METHODS ----

        /// <summary>Удаление элемента из дерева</summary>
        /// <param name="value">Значение узла который нужно удалить из дерева</param>
        /// <returns>true, если операция прошла успешно. false, если узел не найден.</returns>
        public bool RemoveNode(int value)
        {
            Node current; //Указатель на удаляемый узел

            //Проверяем есть ли узел с заданным значением в дереве
            current = Find(value);

            //Если не нашли, то выходим
            if (current == null)
                return false;

            //Узел начиная с которого будем балансировать дереве после удаления
            Node treeToBalance = current.Parent;
            Count--;

            // 1 вариант: Если у удаляемого узла нет правого потомка.
            //
            // До                 После
            //     8                 8
            //    / \               / \
            //  (5)  9             2   9
            //  /   / \               / \
            // 2   10  16            10  16
            //     


            if (current.Right == null)
            {
                //Если удаляемый узел - корень дерева, то перемещаем левое поддерево, на его место.
                if (current.Parent == null)
                {
                    Root = current.Left;
                    //Убераем ссылку на родителя у корня
                    if (Root != null)
                        Root.Parent = null;
                }
                //Если удаляемый узел не является корнем дерева
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    // Если значение родительского узла больше значения удаляемого,
                    // делаем левого потомка удаляемого узла, левым потомком его родительского узла.  
                    if (result > 0)
                        current.Parent.Left = current.Left;
                    // Если значение родительского узла меньше чем удаляемого,                 
                    // делаем левого потомка удаляемого узла - правым потомком его родительского узла.
                    else if (result < 0)
                        current.Parent.Right = current.Left;
                }
            }

            // 2 Вариант: Если у правого потомка удаляемого узла нет левого потомка, 
            // тогда правый потомок удаляемого узла
            // становится потомком родительского узла.      
            //
            // До                 После
            //      8                  8
            //    /   \               / \
            //  (5)     9           6     9
            //  / \    / \        / \     / \
            // 2   6  10  16     2   7   10  16
            //      \
            //       7
            //

            // Если у правого потомка нет левого потомка
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                //Если удаляемый узел - корень дерева, то перемещаем правое поддерево, на его место.
                if (current.Parent == null)
                {
                    Root = current.Right;
                    //Убераем ссылку на родителя у корня
                    if (Root != null)
                        Root.Parent = null;
                }
                //Если удаляемый узел не является корнем дерева
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    // Если значение родительского узла больше значения удаляемого,
                    // делаем правого потомка удаляемого узла, левым потомком его родительского узла
                    if (result > 0)
                        current.Parent.Left = current.Right;
                    // Если значение родительского узла меньше значения удаляемого,                 
                    // делаем правого потомка удаляемого узла, правым потомком его родительского узла
                    else if (result < 0)
                        current.Parent.Right = current.Right;
                }
            }

            // 3 вариант: Если правый потомок удаляемого узла имеет левого потомка,      
            // заменяем удаляемый узел, на крайнего левого потомка правого потомка.     
            //
            // До                 После
            //      8                  8
            //    /   \               / \
            //  (5)     9           6     9
            //  / \    / \        / \     / \
            // 2   7  10  16     2   7   10  16
            //    / 
            //   6
            //

            else
            {
                // Находим крайний левы узел для правого потомка удаляемого узла.
                Node leftmost = current.Right.Left;

                while (leftmost.Left != null)
                    leftmost = leftmost.Left;

                // Родительское правое поддерево становится родительским левым поддеревом.         
                leftmost.Parent.Left = leftmost.Right;

                // Присвоить крайнему левому узлу, ссылки на правого и левого потомка удаляемого узла.         
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;


                //Если удаляемый узел - корень дерева, то перемещаем крайний левый узел, на его место.
                if (current.Parent == null)
                {
                    Root = leftmost;
                    //Убераем ссылку на родителя у корня
                    if (Root != null)
                        Root.Parent = null;
                }
                //Если удаляемый узел не является корнем дерева
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    // Если значение родительского узла больше значения удаляемого,                 
                    // делаем крайнего левого потомка удаляемого узла, левым потомком его родительского узла
                    if (result > 0)
                        current.Parent.Left = leftmost;
                    // Если значение родительского узла, меньше значения удаляемого,                 
                    // делаем крайнего левого потомка удаляемого узла, правым потомком его родительского узла
                    else if (result < 0)
                        current.Parent.Right = leftmost;
                }
            }

            //Если мы не в корне то балансируем дерево, начиная с родительского потомка удаленного узла
            if (treeToBalance != null)
                treeToBalance.Balance();
            //Если в корне, то просто балансируем от корня.
            else
            {
                //Балансируем только если дерево не пустое
                if (Root != null)
                    Root.Balance();
            }

            return true;
        }

        /// <summary>Очистка дерева</summary>
        public void ClearTree()
        {
            Root = null;
            Count = 0;
        }

        #endregion

        #region ---- PRINT METHODS ----

        /// <summary>
        /// Выводит структуру дерева в консоль.
        /// </summary>
        public void Print()
        {
            if (Root != null)
                Root.PrintNode("", Node.NodePosition.center, true, false);
            else
                Console.WriteLine("Tree is empty.");
        }

        /// <summary>
        /// Вывод дерева на экран с раскраской узлов
        /// </summary>
        private void ColorPrint()
        {
            Console.Clear();
            BTreePrinter.Print(Root, false);
            System.Threading.Thread.Sleep(DELAY);//небольшая задержка для наглядности работы алгоритма
        }

        #endregion
    }

}