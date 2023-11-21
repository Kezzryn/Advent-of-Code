try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<(int, int), int> theMap = new();

    for (int y = 0; y < puzzleInput.Length; y++)
    {
        for (int x = 0; x < puzzleInput[y].Length; x++)
        {
            theMap[(x, y)] = puzzleInput[y][x] - 48;
        }
    }

    List<(int, int)> neighbors = new()
    {
        ( 1, 0),
        ( 0, 1),
        (-1, 0),
        ( 0,-1)
    };

    int part1Answer = 0;

    Dictionary<(int, int), int> basins = new();

    int GetBasinSize(int startX, int startY)
    {
        HashSet<(int, int)> visitedNodes = new();

        Queue<(int, int)> steps = new();
        steps.Enqueue((startX, startY));
        visitedNodes.Add((startX, startY));


        while(steps.TryDequeue(out (int x, int y) currentStep))
        {
            foreach ((int nX, int nY) in neighbors)
            {
                int nextX = currentStep.x + nX;
                int nextY = currentStep.y + nY;

                if (!theMap.TryGetValue((nextX, nextY), out int neighborValue)) continue;
                if (neighborValue == 9) continue;
                if (visitedNodes.Add((nextX, nextY)))
                {
                    steps.Enqueue((nextX, nextY));
                }
            }
        }


        return visitedNodes.Count;
    }


    foreach (((int x, int y), int mapValue) in theMap)
    {
        bool isLowest = true;
        foreach ((int nX, int nY) in neighbors)
        {
            if (!theMap.TryGetValue((x + nX, y + nY), out int neighborValue)) continue;
            if (neighborValue <= mapValue)
            {
                isLowest = false;
                break;
            }
        }
        if (isLowest) 
        { 
            part1Answer += mapValue + 1;
            basins.Add((x, y), GetBasinSize(x, y));
        }
    }

    int part2Answer = basins.Values.OrderByDescending(x => x).Take(3).Aggregate((x,y) => x * y);   

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}