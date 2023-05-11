try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PARENT = 0;
    const int CHILD = 1;

    string[][] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Split(')').ToArray()).ToArray();

    // key = node location, value = nullable parent.
    Dictionary<string, string?> graph = new();

    // Load graph
    foreach (string[] parentChild in puzzleInput)
    {
        graph.TryAdd(parentChild[PARENT], null);

        if (!graph.TryAdd(parentChild[CHILD], parentChild[PARENT]) && graph[parentChild[CHILD]] == null)
        {
            graph[parentChild[CHILD]] = parentChild[PARENT];
        }
    }

    Dictionary<string, int> visited = new();
    int part1Answer = 0;
    int part2Answer = 0;

    foreach (string orbitID in graph.Keys)
    {
        string? parentID = graph[orbitID];
        int steps = 0;

        bool doPart2 = false;
        if (orbitID == "YOU" || orbitID == "SAN") doPart2 = true;

        while (parentID != null)
        {
            if (doPart2 && !visited.TryAdd(parentID, steps))
            {
                part2Answer = steps + visited[parentID];
                doPart2 = false;
            }

            steps++;
            parentID = graph[parentID];

        }
        part1Answer += steps;
    }

    Console.WriteLine($"Part 1: The transfer orbit checksum is: {part1Answer}.");
    Console.WriteLine($"Part 2: It will take {part2Answer} transfer to rendezvous with Santa.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}