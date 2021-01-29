using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_04_02
{
    /// <summary>Класс двоичного дерева поиска</summary>
    public class BTree
    {
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
                AddToNode(Root, value);

            Count++;
        }

        /// <summary>Рекурсивное добавление нового узла после указанного</summary>
        /// <param name="node">Узел после которого добавляется новый</param>
        /// <param name="value">Значение для хранения в новом узле</param>
        private void AddToNode(Node node, int value)
        {
            int check = value.CompareTo(node.Value);
            // 1 вариант: Значение добавлемого узла меньше чем значение текущего узла.      
            if (check < 0)
            {
                //Созданем левый узел, если он отсутствует
                if (node.Left == null)
                    node.Left = new Node(value, node, this);
                // Иначе спускаемся вниз по левому узлу
                else
                    AddToNode(node.Left, value);
            }
            // 2 Вариант: Значение добавлемого узла больше чем значение текущего узла.      
            else if (check > 0)
            {
                //Создаем правый узел, если он отсутствует
                if (node.Right == null)
                    node.Right = new Node(value, node, this);
                // Иначе спускаемся вниз по правому
                else
                    AddToNode(node.Right, value);
            }
            node.Balance();
        }

        #endregion

        #region ---- FIND METHODS ----

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

        #endregion
    }

}