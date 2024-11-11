using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KruskalAlgorithm
{
    // Класс для представления рёбер графа
    public class Edge : IComparable<Edge>
    {
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }

        // Сравнение рёбер по весу
        public int CompareTo(Edge other) => Weight.CompareTo(other.Weight);
    }

    // Класс для представления графа
    public class Graph
    {
        private int _vertices;
        private List<Edge> _edges;

        public Graph(int vertices)
        {
            _vertices = vertices;
            _edges = new List<Edge>();
        }

        public void AddEdge(int source, int destination, int weight)
        {
            _edges.Add(new Edge(source, destination, weight));
        }

        // Находим минимальное остовное дерево с использованием алгоритма Краскала
        public List<Edge> KruskalMST()
        {
            List<Edge> result = new List<Edge>(); // Хранение рёбер МСТ
            _edges.Sort(); // Сортировка рёбер по весу

            int[] parent = new int[_vertices];
            for (int i = 0; i < _vertices; i++)
                parent[i] = i;

            foreach (Edge edge in _edges)
            {
                int sourceRoot = Find(parent, edge.Source);
                int destinationRoot = Find(parent, edge.Destination);

                if (sourceRoot != destinationRoot)
                {
                    result.Add(edge);
                    Union(parent, sourceRoot, destinationRoot);
                }
            }

            return result;
        }

        // Найти корень вершины
        private int Find(int[] parent, int vertex)
        {
            if (parent[vertex] != vertex)
                parent[vertex] = Find(parent, parent[vertex]);
            return parent[vertex];
        }

        // Объединение двух поддеревьев
        private void Union(int[] parent, int root1, int root2)
        {
            int root1Parent = Find(parent, root1);
            int root2Parent = Find(parent, root2);
            parent[root1Parent] = root2Parent;
        }

        // Метод для тестирования и сбора данных
        public static void TestGraph(int vertices, List<Edge> edges)
        {
            Graph graph = new Graph(vertices);
            foreach (var edge in edges)
            {
                graph.AddEdge(edge.Source, edge.Destination, edge.Weight);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            List<Edge> mst = graph.KruskalMST();
            stopwatch.Stop();

            int totalWeight = 0;
            foreach (Edge edge in mst)
            {
                totalWeight += edge.Weight;
            }

            Console.WriteLine($"Число вершин: {vertices}, Число рёбер: {edges.Count}, Суммарный вес МСТ: {totalWeight}, Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Пример тестирования на нескольких графах

            // Полный граф
            var edges1 = new List<Edge>
            {
                new Edge(0, 1, 4), new Edge(0, 2, 3), new Edge(1, 2, 1),
                new Edge(1, 3, 2), new Edge(2, 3, 4), new Edge(3, 4, 2)
            };
            Graph.TestGraph(5, edges1);

            // Разреженный граф
            var edges2 = new List<Edge>
            {
                new Edge(0, 1, 6), new Edge(0, 3, 5), new Edge(1, 2, 1),
                new Edge(3, 4, 3)
            };
            Graph.TestGraph(5, edges2);

            // Циклический граф
            var edges3 = new List<Edge>
            {
                new Edge(0, 1, 4), new Edge(1, 2, 3), new Edge(2, 3, 2),
                new Edge(3, 4, 4), new Edge(4, 0, 1)
            };
            Graph.TestGraph(5, edges3);

            // Большой граф
            var edges4 = new List<Edge>();
            int vertices4 = 50;
            Random rand = new Random();
            for (int i = 0; i < vertices4; i++)
            {
                for (int j = i + 1; j < vertices4; j++)
                {
                    edges4.Add(new Edge(i, j, rand.Next(1, 10)));
                }
            }
            Graph.TestGraph(vertices4, edges4);
        }
    }
}

