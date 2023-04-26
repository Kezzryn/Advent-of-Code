try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART_1_NUMLOOPS = 10;
    const int PART_2_NUMLOOPS = 1000000000;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    MapSymbols[,,] theMap = new MapSymbols[puzzleInput[0].Length, puzzleInput.Length, 2];

    foreach ((int x, int y) in from y in Enumerable.Range(0, puzzleInput.Length)
                               from x in Enumerable.Range(0, puzzleInput[0].Length)
                               select (x, y))
    {
        theMap[x, y, 0] = puzzleInput[y][x] switch
        {
            '.' => MapSymbols.Open,
            '|' => MapSymbols.Tree,
            '#' => MapSymbols.Lumberyard,
            _ => throw new NotImplementedException()
        };
    }

    void CountAdjacent(int sourceX, int sourceY, int current, out int numTrees, out int numLumberyards)
    {
        numLumberyards = 0;
        numTrees = 0;

        int maxX = int.Min(sourceX + 1, theMap.GetUpperBound(0));
        int maxY = int.Min(sourceY + 1, theMap.GetUpperBound(1));
        int minX = int.Max(sourceX - 1, theMap.GetLowerBound(0));
        int minY = int.Max(sourceY - 1, theMap.GetLowerBound(1));

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (x == sourceX && y == sourceY) continue;
                if (theMap[x, y, current] == MapSymbols.Lumberyard) numLumberyards++;
                if (theMap[x, y, current] == MapSymbols.Tree) numTrees++;
            }
        }
    }

    void DrawMap(int currentState)
    {
        Console.Clear();
        ConsoleColor prevConsoleFGColor = Console.ForegroundColor;
        for (int y = 1; y <= theMap.GetUpperBound(1) - 1; y++)
        {
            for (int x = 1; x <= theMap.GetUpperBound(0) - 1; x++)
            {
                switch (theMap[x, y, currentState])
                {
                    case MapSymbols.Open:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('.');
                        break;
                    case MapSymbols.Tree:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('|');
                        break;
                    case MapSymbols.Lumberyard:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('#');
                        break;
                }
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = prevConsoleFGColor;
        ;
    }

    int part1Answer = 0;
    int part2Answer = 0;
    int indexCurrentState = 0;
    bool isDone = false;
    int numLoops = 0;
    Dictionary<int, int> scoreTracker = new();
    int[] frameScore = new int[2]; 
    
    do
    {
        numLoops++;
        int indexNextState = (indexCurrentState + 1) % 2;
        int numTrees = 0;
        int numLumberyards = 0;
        
        foreach ((int x, int y) in from y in Enumerable.Range(0, theMap.GetLength(1))
                                   from x in Enumerable.Range(0, theMap.GetLength(0))
                                   select (x, y))
        {
            CountAdjacent(x, y, indexCurrentState, out int adjTrees, out int adjumberyards);
            MapSymbols nextState = theMap[x, y, indexCurrentState] switch
            {
                MapSymbols.Open => adjTrees >= 3 ? MapSymbols.Tree : MapSymbols.Open,
                    MapSymbols.Tree => adjumberyards >= 3 ? MapSymbols.Lumberyard : MapSymbols.Tree,
                    MapSymbols.Lumberyard => adjTrees >= 1 && adjumberyards >= 1 ? MapSymbols.Lumberyard : MapSymbols.Open,
                    _ => throw new NotImplementedException()
                };

            theMap[x, y, indexNextState] = nextState;
            numTrees += nextState == MapSymbols.Tree ? 1 : 0;
            numLumberyards += nextState == MapSymbols.Lumberyard ? 1 : 0;
        };
        frameScore[indexNextState] = numTrees * numLumberyards;
        if (numLoops == PART_1_NUMLOOPS) part1Answer = frameScore[indexNextState];
        
        if (!scoreTracker.TryAdd(frameScore[indexNextState], numLoops))
        {
            int nextDiff = numLoops - scoreTracker[frameScore[indexNextState]];
            int currentDiff = numLoops - scoreTracker[frameScore[indexCurrentState]] - 1;
            if (currentDiff == nextDiff)
            {
                int targetScore = ((PART_2_NUMLOOPS - numLoops) % nextDiff) + (numLoops - nextDiff);
                part2Answer = scoreTracker.Where(x => x.Value == targetScore).First().Key;
                isDone = true;
            }
        }

        indexCurrentState = indexNextState;

        if (numLoops > 1000) isDone = true;
    } while (!isDone);

    Console.WriteLine($"Part 1: After {PART_1_NUMLOOPS} minutes the resource value is: {part1Answer}");
    Console.WriteLine($"Part 2: After {PART_2_NUMLOOPS} minutes the resource value is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum MapSymbols
{
    Open,
    Tree,
    Lumberyard
}