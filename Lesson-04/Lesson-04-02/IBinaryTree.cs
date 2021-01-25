using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_04_02
{
    public interface IBinaryTree
    {
        void ClearTree(); // очищает список
        int GetCount(); // возвращает количество элементов в дереве
        void AddNode(int value);  // добавляет новый элемент в дерево
        void RemoveNode(int value); // удаляет элемент в дереве, если он там есть
        bool FindNode(int value); //Находит значение в дереве



    }
}
