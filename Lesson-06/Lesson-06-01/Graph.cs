using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    /// <summary>Граф</summary>
    public class Graph
    {
        /// <summary>Список вершин графа</summary>
        public List<Node> Nodes { get; }

        /// <summary>Количество вершин в графе</summary>
        public int Count
        {
            get
            {
                return Nodes.Count;
            }
        }

        /// <summary>Конструктор</summary>
        public Graph()
        {
            Nodes = new List<Node>();
        }

        /// <summary>Добавление вершины</summary>
        /// <param name="id">ID вершины</param>
        public void AddNode(int id)
        {
            if(FindNode(id) == null)
                Nodes.Add(new Node(id, this));
        }

        /// <summary>Поиск вершины</summary>
        /// <param name="id">ID вершины</param>
        /// <returns>Найденная вершина</returns>
        public Node FindNode(int id)
        {
            foreach (var node in Nodes)
            {
                if (node.ID.Equals(id))
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>Добавление ребра</summary>
        /// <param name="firstID">Имя первой вершины</param>
        /// <param name="secondID">Имя второй вершины</param>
        /// <param name="weight">Вес ребра соединяющего вершины</param>
        /// <returns>true, если добавление прошло успешно</returns>
        public bool AddEdge(int firstID, int secondID, int weight)
        {
            var node1 = FindNode(firstID);
            var node2 = FindNode(secondID);
            if (node2 != null && node1 != null)
                if (!node1.CheckLinkToNode(secondID))
                {
                    node1.AddEdge(node2, weight);
                    node2.AddEdge(node1, weight);
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Поиск (обход) графа в ширину
        /// </summary>
        /// <param name="id">Искомое значение</param>
        /// <returns>true, если вершина с таким значением есть в графе</returns>
        public bool BFS(int id)
        {
            Queue<Node> bufer = new Queue<Node>();

            return false;
        }

        /// <summary>
        /// Поиск (обход) графа в глубину
        /// </summary>
        /// <param name="id">Искомое значение</param>
        /// <returns>true, если вершина с таким значением есть в графе</returns>
        public bool DFS(int id)
        {
            Stack<Node> bufer = new Stack<Node>();

            return false;
        }
    }
}
