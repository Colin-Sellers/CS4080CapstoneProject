using System;

class Graph
{
    public int[,] BaseGraph { get; private set; }
    public int[,]? ShortestPathFloyd { get; private set; }
    public int[,]? ShortestPathDijkstra { get; private set; }

    private const int INF = int.MaxValue;

    public Graph(int size = 10, char type = 'r', int customEdgeCount = 0, int minWeight = 1, int maxWeight = 20)
    {
        RandomGraph generator = new RandomGraph();
        BaseGraph = generator.generateRandomGraph(size, type, customEdgeCount, minWeight, maxWeight);
        ShortestPathFloyd = null;
        ShortestPathDijkstra = null;
    }

    // Floyd-Warshall Algorithm for All-Pairs Shortest Paths
    public void floydWarshall()
    {
        int size = BaseGraph.GetLength(0);
        ShortestPathFloyd = (int[,])BaseGraph.Clone();

        for (int k = 0; k < size; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Skip if intermediate path is invalid
                    if (ShortestPathFloyd[i, k] == INF || ShortestPathFloyd[k, j] == INF)
                    {
                        continue;
                    }

                    int newDist = ShortestPathFloyd[i, k] + ShortestPathFloyd[k, j];
                    if (ShortestPathFloyd[i, j] > newDist)
                    {
                        ShortestPathFloyd[i, j] = newDist;
                    }
                }
            }
        }
    }

    // Dijkstra's Algorithm for Single-Source Shortest Path
    public (int[] Dist, int[] Prev) dijkstra(int source)
    {
        int size = BaseGraph.GetLength(0);
        int[] dist = new int[size];
        int[] prev = new int[size];
        bool[] visited = new bool[size];

        // Initialize distances and predecessors
        for (int i = 0; i < size; i++)
        {
            dist[i] = BaseGraph[source, i] == INF ? INF : BaseGraph[source, i];
            prev[i] = BaseGraph[source, i] == INF ? -1 : source;
            visited[i] = false;
        }

        dist[source] = 0;

        for (int i = 0; i < size - 1; i++)
        {
            int u = MinDistance(dist, visited);
            if (u == -1) break;

            visited[u] = true;

            for (int v = 0; v < size; v++)
            {
                // Skip if no edge or distance is infinite
                if (!visited[v] && BaseGraph[u, v] != INF && dist[u] != INF)
                {
                    int newDist = dist[u] + BaseGraph[u, v];
                    if (newDist < dist[v])
                    {
                        dist[v] = newDist;
                        prev[v] = u;
                    }
                }
            }
        }

        return (dist, prev);
    }

    // Compute All-Pairs Shortest Paths using Dijkstra's Algorithm
    public void dijkstraSub()
    {
        int size = BaseGraph.GetLength(0);
        ShortestPathDijkstra = new int[size, size];

        for (int i = 0; i < size; i++)
        {
            var (dist, _) = dijkstra(i);

            for (int j = 0; j < size; j++)
            {
                ShortestPathDijkstra[i, j] = dist[j];
            }
        }
    }

    private int MinDistance(int[] dist, bool[] visited)
    {
        int min = INF;
        int minIndex = -1;

        for (int i = 0; i < dist.Length; i++)
        {
            if (!visited[i] && dist[i] <= min)
            {
                min = dist[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public void PrintMatrix(int[,]? matrix)
    {
        if (matrix == null)
        {
            Console.WriteLine("Matrix is null.");
            return;
        }

        int size = matrix.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(matrix[i, j] == INF ? "INF\t" : $"{matrix[i, j]}\t");
            }
            Console.WriteLine();
        }
    }
}
