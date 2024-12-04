using System;
using System.Diagnostics;

// Shortest Path Algorithm Complexity Analysis
// Compare between Floyd-Warshall & Dijkstra Algorithms
// By: Colin Sellers / Emily Chiu / Henry Tat / Jared Escarcega

class Program
{
    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Welcome to the menu!");
            Console.WriteLine("Would you like to:");
            Console.WriteLine("(1) Run randomized tests");
            Console.WriteLine("(2) Create a custom graph and test paths");
            Console.WriteLine("(3) Exit program");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunRandomizedTests();
                    break;
                case "2":
                    CreateCustomGraph();
                    break;
                case "3":
                    exit = true;
                    Console.WriteLine("Exiting program!");
                    break;
                default:
                    Console.WriteLine("Error, please pick a valid option (1 - 3)!");
                    break;
            }
        }
    }

    static void RunRandomizedTests()
    {
        Console.WriteLine("Enter the initial graph size:");
        string? inputSize = Console.ReadLine();
        if (!int.TryParse(inputSize, out int initialSize) || initialSize <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            return;
        }

        Console.WriteLine("Enter the number of iterations (how many times to increase the size):");
        string? inputIterations = Console.ReadLine();
        if (!int.TryParse(inputIterations, out int iterations) || iterations <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            return;
        }

        Console.WriteLine("Enter the increment size for each iteration:");
        string? inputIncrement = Console.ReadLine();
        if (!int.TryParse(inputIncrement, out int increment) || increment <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            return;
        }

        for (int i = 0; i < iterations; i++)
        {
            int size = initialSize + i * increment;
            Console.WriteLine($"\nGenerating random graph of size {size}...");

            Stopwatch stopwatch = new Stopwatch();

            // Measure graph generation
            stopwatch.Start();
            Graph graph = new Graph(size, 'r', 0, 1, 20);
            stopwatch.Stop();
            Console.WriteLine($"Graph generated in {stopwatch.ElapsedMilliseconds} ms.");

            // Measure Floyd-Warshall execution
            stopwatch.Restart();
            Console.WriteLine("Running Floyd-Warshall algorithm...");
            graph.floydWarshall();
            stopwatch.Stop();
            Console.WriteLine($"Floyd-Warshall completed in {stopwatch.ElapsedMilliseconds} ms.");

            // Measure Dijkstra's algorithm for all vertices
            stopwatch.Restart();
            Console.WriteLine("Running Dijkstra’s algorithm for all vertices...");
            graph.dijkstraSub();
            stopwatch.Stop();
            Console.WriteLine($"Dijkstra’s algorithm completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }



    static void CreateCustomGraph()
    {
        Graph graph = HandleGraphCreation();
        
        Console.WriteLine("Generated Graph:");
        graph.PrintMatrix(graph.BaseGraph);

        AnalyzeGraph(graph);
    }

    static void AnalyzeGraph(Graph graph)
    {
        bool analyzeGraph = true;

        while (analyzeGraph)
        {
            Console.WriteLine("\nGraph Analysis Menu:");
            Console.WriteLine("1. Run Floyd-Warshall Shortest Path Graph Algorithm");
            Console.WriteLine("2. Run Dijkstra's Shortest Path Algorithm");
            Console.WriteLine("3. Exit Graph Analysis");

            Console.Write("Select an option (1-3): ");
            string? analysisChoice = Console.ReadLine();

            if (string.IsNullOrEmpty(analysisChoice))
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
                continue;
            }

            switch (analysisChoice)
            {
                case "1":
                    Console.WriteLine("Running Floyd-Warshall Algorithm...");
                    graph.floydWarshall();
                    Console.WriteLine("\nShortest Paths (Floyd-Warshall):");
                    graph.PrintMatrix(graph.ShortestPathFloyd);
                    break;

                case "2":
                    dijkstraRunner(graph);
                    break;

                case "3":
                    analyzeGraph = false;
                    Console.WriteLine("Returning to the main menu...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                    break;
            }
        }
    }

    static void dijkstraRunner(Graph graph)
    {
        bool keepRunning = true;
        while (keepRunning)
        {
            Console.WriteLine("Enter two vertices to find the shortest path (start and end):");
            if (!int.TryParse(Console.ReadLine(), out int start) || start < 0 || start >= graph.BaseGraph.GetLength(0))
            {
                Console.WriteLine("Invalid start vertex. Please try again.");
                continue;
            }

            if (!int.TryParse(Console.ReadLine(), out int end) || end < 0 || end >= graph.BaseGraph.GetLength(0))
            {
                Console.WriteLine("Invalid end vertex. Please try again.");
                continue;
            }

            var (dist, prev) = graph.dijkstra(start);

            if (dist[end] == int.MaxValue)
            {
                Console.WriteLine($"No path exists between {start} and {end}. Distance: INF");
            }
            else
            {
                Console.WriteLine($"Shortest path from {start} to {end}:");
                PrintPath(prev, start, end);
                Console.WriteLine($"Distance: {dist[end]}");
            }

            Console.WriteLine("Would you like to test another path? (1 for yes, 0 to exit)");
            keepRunning = Console.ReadLine() == "1";
        }
    }

    static Graph HandleGraphCreation()
    {
        Console.WriteLine("\nGraph Creation:");
        
        int size = GetValidIntegerInput("Enter the number of vertices: ");
        Console.Write("Select graph type (r = Random, d = Dense, s = Sparse, c = Custom Edge Count): ");
        string? typeInput = Console.ReadLine();
        char type = string.IsNullOrEmpty(typeInput) ? 'r' : typeInput[0];

        int edgeCount = 0;
        if (type == 'c')
        {
            edgeCount = GetValidIntegerInput("Enter the custom edge count: ");
        }

        int minWeight = GetValidIntegerInput("Enter the minimum edge weight: ");
        int maxWeight = GetValidIntegerInput("Enter the maximum edge weight: ");

        Graph graph = new Graph(size, type, edgeCount, minWeight, maxWeight);
        Console.WriteLine("\nGraph generated successfully!");
        return graph;
    }

    static int GetValidIntegerInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int value))
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }

    static void PrintPath(int[] prev, int start, int end)
    {
        if (start == end)
        {
            Console.Write(start + " ");
        }
        else if (prev[end] == -1)
        {
            Console.WriteLine("No path exists.");
        }
        else
        {
            PrintPath(prev, start, prev[end]);
            Console.Write(end + " ");
        }
    }
}
