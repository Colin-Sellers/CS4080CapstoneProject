using System;
using System.Collections.Generic;

class RandomGraph
{
    private Random rand = new Random();

    public int[,] generateRandomGraph(int size = 10, char type = 'r', int customEdgeCount = 0, int minWeight = 1, int maxWeight = 20)
    {
        int edges;
        // Set up values
        if (size == 0)
        {
            Console.WriteLine("Error! Invalid Size of 0, Setting to 10");
            size = 10;
        }

        switch(type)
        {
            case 'd':
                // Dense Graph
                edges = size * (size - (size / 2));
                break;
            case 's':
                // Sparse Graph
                edges = Convert.ToInt32(size * Math.Log(size));
                break;
            case 'c':
                // User specified edge count
                if (customEdgeCount > maxEdges(size)) edges = maxEdges(size) + 1;
                else if (customEdgeCount <= 0)
                {
                    Console.WriteLine("Error, Invalid Edge Count, Setting to Sparse");
                    edges =  Convert.ToInt32(size * Math.Log(size));
                }
                else edges = maxEdges(customEdgeCount);
                break;
            case 'r':
                // Random case, same as default case
            default:
                // Random case or incorrect case letter, assume random
                edges = rand.Next(1, maxEdges(size));
                break;
        }
        
        // Ensure Weights are valid
        if (minWeight < 0)
        {
            Console.WriteLine("Invalid Minimum Weight, setting to 1");
            minWeight = 1;
        }
        if (maxWeight < 0)
        {
            Console.WriteLine("Invalid Maximum Weight, setting to 10");
            maxWeight = 10;
        }
        else if (maxWeight > (int.MaxValue / 10))
        {
            Console.WriteLine("Warning, you have set a very high Max Weight value, unintended behavior may happen");
        }

        // Initialize the graph with INF to indicate no connections
        int[,] graph = new int[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                graph[i, j] = (i == j) ? 0 : int.MaxValue; // 0 for self-loops, INF for no edges
            }
        }

        // Create adjacency graph using List
        List<List<int>> adjacencyList = new List<List<int>>(size);

        for (int i = 0; i < size; i++)
        {
            adjacencyList.Add(new List<int>());
        }

        // Add additional random edges
        for (int edge = 0; edge < edges; edge++)
        {
            int v1 = rand.Next(size);
            int v2 = rand.Next(size);

            // Avoid self-loops and duplicate edges
            if (v1 == v2 || adjacencyList[v1].Contains(v2))
            {
                // Retry, substract from edge
                edge--;
                continue;
            }

            // This is a valid place to add an edge
            adjacencyList[v1].Add(v2);
        }

        // Now generate random weights based on adjacency list
        for (int i = 0; i < adjacencyList.Count; i++)
        {
            List<int> thisVertex = adjacencyList[i];
            if (thisVertex.Count == 0)
            {
                // This vertex does not connect to anything, all inf
            }
            else
            {
                // This vertex does have edges, add random weight
                for (int j = 0; j < thisVertex.Count; j++)
                {
                    int connectsToThisVertex = thisVertex[j];
                    graph[i, connectsToThisVertex] = rand.Next(minWeight, maxWeight);
                }
            }
        }

        return graph;
    }

    private int maxEdges(int vertices)
    {
        return vertices * (vertices - 1);
    }
}

