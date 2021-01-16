Курс Алгоритмы и структуры данных (GeekBrains)
Урок 02



---- Задание ----

1. Двусвязный список
Требуется реализовать класс двусвязного списка и операции вставки, удаления и поиска элемента в нём в соответствии с интерфейсом.
namespace GeekBrainsTests
{
	public class Node
	{
    	  public int Value { get; set; }
    	  public Node NextNode { get; set; }
    	  public Node PrevNode { get; set; }
	}

	//Начальную и конечную ноду нужно хранить в самой реализации интерфейса
	public interface ILinkedList
	{
    	  int GetCount(); // возвращает количество элементов в списке
    	  void AddNode(int value);  // добавляет новый элемент списка
    	  void AddNodeAfter(Node node, int value); // добавляет новый элемент списка после определённого элемента
    	  void RemoveNode(int index); // удаляет элемент по порядковому номеру
    	  void RemoveNode(Node node); // удаляет указанный элемент
    	  Node FindNode(int searchValue); // ищет элемент по его значению
	}

}

2. Двоичный поиск
Требуется написать функцию бинарного поиска, посчитать его асимптотическую сложность и проверить работоспособность функции.
