using System;
using System.Linq;
using System.Collections.Generic;

class Graph
{
    private Dictionary<string, int> vertexMap; // Словарь для отображения вершин в индексы
    public List<Edge> Edges { get; set; }

    public Graph()
    {
        Edges = new List<Edge>();
        vertexMap = new Dictionary<string, int>();
    }

    public void AddEdge(string vertex1, string vertex2, int weight)
    {
        // Преобразуем имена вершин в индексы
        if (!vertexMap.ContainsKey(vertex1))
        {
            vertexMap[vertex1] = vertexMap.Count;
        }
        if (!vertexMap.ContainsKey(vertex2))
        {
            vertexMap[vertex2] = vertexMap.Count;
        }

        int v1Index = vertexMap[vertex1];
        int v2Index = vertexMap[vertex2];

        Edges.Add(new Edge(v1Index, v2Index, weight));
    }

    public List<Edge> Kruskal()
    {
        List<Edge> mst = new List<Edge>();
        UnionFind uf = new UnionFind(vertexMap.Count);  // Количество уникальных вершин

        // Сортировка рёбер по весу
        var sortedEdges = Edges.OrderBy(e => e.Weight).ToList();

        foreach (var edge in sortedEdges)
        {
            int u = uf.Find(edge.Vertex1);
            int v = uf.Find(edge.Vertex2);

            if (u != v)
            {
                mst.Add(edge);
                uf.Union(u, v);
            }
        }

        return mst;
    }
}

class Edge
{
    public int Vertex1 { get; set; }
    public int Vertex2 { get; set; }
    public int Weight { get; set; }

    public Edge(int vertex1, int vertex2, int weight)
    {
        Vertex1 = vertex1;
        Vertex2 = vertex2;
        Weight = weight;
    }
}

class UnionFind
{
    private int[] parent;
    private int[] rank;

    public UnionFind(int n)
    {
        parent = new int[n];
        rank = new int[n];

        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
        }
    }

    public int Find(int x)
    {
        if (parent[x] != x)
        {
            parent[x] = Find(parent[x]);
        }
        return parent[x];
    }

    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX != rootY)
        {
            if (rank[rootX] > rank[rootY])
            {
                parent[rootY] = rootX;
            }
            else if (rank[rootX] < rank[rootY])
            {
                parent[rootX] = rootY;
            }
            else
            {
                parent[rootY] = rootX;
                rank[rootX]++;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Graph graph = new Graph();

        graph.AddEdge("A", "B", 1);
        graph.AddEdge("A", "C", 3);
        graph.AddEdge("B", "C", 2);
        graph.AddEdge("C", "D", 4);
        graph.AddEdge("B", "D", 5);

        var mst = graph.Kruskal();

        Console.WriteLine("Минимальное остовное дерево:");
        foreach (var edge in mst)
        {
            Console.WriteLine($"{edge.Vertex1} - {edge.Vertex2}: {edge.Weight}");
        }
    }
}
