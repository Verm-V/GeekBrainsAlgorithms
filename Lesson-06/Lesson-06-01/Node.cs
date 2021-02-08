using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    /// <summary>Вершина графа</summary>
    public class Node
    {
        /// <summary>Граф к которому принадлежит вершина</summary>
        public Graph GraphParent { get; }

        /// <summary>ID вершины</summary>
        public int ID { get; }

        /// <summary>Список ребер</summary>
        public List<Edge> Edges { get; }


        /// <summary>Цвет вершины для вывода в консоли</summary>
        public ConsoleColor Color
        {
            get;
            internal set;
        }

        /// <summary>Конструктор</summary>
        /// <param name="id">ID вершины</param>
        public Node(int id, Graph graph)
        {
            ID = id;
            Edges = new List<Edge>();
            GraphParent = graph;
            Color = ConsoleColor.Gray;
        }

        /// <summary>Добавить ребро</summary>
        /// <param name="node">Вершина</param>
        /// <param name="weight">Вес</param>
        public void AddEdge(Node node, int weight)
        {
            AddEdge(new Edge(node, weight));
        }

        /// <summary>Добавляет ребро</summary>
        /// <param name="newEdge">Ребро</param>
        private void AddEdge(Edge newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>Проверяет есть ли у данной вершины связь с вершиной с указанным ID
        /// Нужен для избегания дублирования ребер</summary>
        /// <param name="id">ID вершины связь с которой нужно проверить</param>
        /// <returns>true, если связь есть</returns>
        public bool CheckLinkToNode(int id)
        {
            foreach (Edge edge in Edges)
            {
                if (edge.ConnectedNode.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
