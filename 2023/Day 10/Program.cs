static (int X, int Y) MakeMap(List<string> inputData, Dictionary<(int X, int Y), ((int X, int Y) A, (int X, int Y) B)> mapDic, out char startChar)
{
    startChar = '\0'; // part 1 cheat to help make part 2 work :P 

    (int X, int Y) NORTH = (0, -1);
    (int X, int Y) SOUTH = (0, 1);
    (int X, int Y) EAST = (1, 0);
    (int X, int Y) WEST = (-1, 0);

    int maxX = inputData[0].Length;
    int maxY = inputData.Count;

    (int, int) startVal = (-1, -1);

    foreach ((int X, int Y) in from y in Enumerable.Range(0, maxY)
                               from x in Enumerable.Range(0, maxX)
                               select (x, y))
    {
        char currentChar = inputData[Y][X];
        if (currentChar == 'S')
        {
            startVal = (X, Y);
            bool GO_NORTH = "|7F".Contains(inputData[Y - 1][X]);
            bool GO_SOUTH = "|LJ".Contains(inputData[Y + 1][X]);
            bool GO_WEST = "-FL".Contains(inputData[Y][X - 1]);
            bool GO_EAST = "-7J".Contains(inputData[Y][X + 1]);

            currentChar = GO_NORTH && GO_SOUTH ? '|'
                        : GO_NORTH && GO_WEST ? 'J'
                        : GO_NORTH && GO_EAST ? 'L'
                        : GO_SOUTH && GO_WEST ? '7'
                        : GO_SOUTH && GO_EAST ? 'F'
                        : GO_EAST && GO_WEST ? '-'
                        : throw new Exception();
            startChar = currentChar;

        }

        mapDic[(X, Y)] = currentChar switch
        {
            '.' => ((X, Y), (X, Y)), // open ground. We shouldn't be here.
            '|' => (AddTuple((X, Y), NORTH), AddTuple((X, Y), SOUTH)), // is a vertical pipe connecting north and south.
            '-' => (AddTuple((X, Y), EAST),  AddTuple((X, Y), WEST)), // is a horizontal pipe connecting east and west.
            'L' => (AddTuple((X, Y), NORTH), AddTuple((X, Y), EAST)), // is a 90 - degree bend connecting north and east.
            'J' => (AddTuple((X, Y), NORTH), AddTuple((X, Y), WEST)), // is a 90 - degree bend connecting north and west.
            '7' => (AddTuple((X, Y), SOUTH), AddTuple((X, Y), WEST)), // is a 90 - degree bend connecting south and west.
            'F' => (AddTuple((X, Y), SOUTH), AddTuple((X, Y), EAST)), // is a 90 - degree bend connecting south and east.
            _ => throw new NotImplementedException()
        };
    }

    return startVal;
}

static HashSet<(int X, int Y)> RunMap((int X, int Y) start, Dictionary<(int X, int Y), ((int X, int Y) A, (int X, int Y) B)> mapDic)
{
    (int X, int Y) cursor = start;
    HashSet<(int X, int Y)> history = new();

    do
    {
        if (!history.Add(cursor))
        {
  //          Console.WriteLine($"Loop break at {cursor} after {history.Count} steps.");
            return history;
        }
//        Console.WriteLine($"cursor at: {cursor} => A: {mapDic[cursor].A} (History: {history.Contains(mapDic[cursor].A)}) B: {mapDic[cursor].B} (History: {history.Contains(mapDic[cursor].B)}) ");

        cursor = history.Contains(mapDic[cursor].A) ? mapDic[cursor].B : mapDic[cursor].A;
    } while (cursor != start);

    return history;
}

static (int X, int Y) AddTuple((int X, int Y) A, (int X, int Y) B) => ((A.X + B.X), (A.Y + B.Y));

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    Dictionary<(int X, int Y), ((int X, int Y) A, (int X, int Y) B)> part1Map = new();

    (int X, int Y) startP1 = MakeMap(puzzleInput, part1Map, out char startChar);
    HashSet<(int X, int Y)> historyP1 = RunMap(startP1, part1Map);
    int part1Answer = historyP1.Count / 2;

    //Expand the map out to 3x3 grid.
    List<string> part2Input = new();
    foreach (string line in puzzleInput)
    {
        string newLine = line.Replace('S', startChar); // as captured from part 1. It was hell to account for otherwise.
        newLine = String.Join("", newLine.Select((x,i) => $"{("J7-".Contains(newLine[i]) ? '-' : '.')}{x}{("LF-".Contains(newLine[i]) ? '-' : '.')}"));
        part2Input.Add(newLine);
    }

    for (int y = 0; y < part2Input.Count - 1; y++)
    {
        string newLine = String.Join("", part2Input[y].Select((x, i) => $"{("LJ|".Contains(part2Input[y][i]) ? '|' : '.')}"));
        part2Input.Insert(y++, newLine);

        newLine = String.Join("", part2Input[y].Select((x, i) => $"{("F7|".Contains(part2Input[y][i]) ? '|' : '.')}"));
        part2Input.Insert(y++ + 1, newLine);
    }

    Dictionary<(int X, int Y), ((int X, int Y) A, (int X, int Y) B)> part2Map = new();
    (int X, int Y) startP2 = MakeMap(part2Input, part2Map, out _);
    startP2 = (((startP1.X * 3) - 1), (startP1.Y * 3) + 1); // adjust for expansion

    HashSet<(int X, int Y)> historyP2 = RunMap(startP2, part2Map);

    // Basic flood fill.
    List<(int X, int Y)> neighbors = new() { (0, -1), (0, 1), (1, 0), (-1, 0) };
    HashSet<(int X, int Y)> outside = new();
    Queue<(int X, int Y)> queue = new();

    queue.Enqueue((0, 0));

    int maxX = part2Input[0].Length;
    int maxY = part2Input.Count;

    while (queue.TryDequeue(out (int X, int Y) outQueue))
    {
        outside.Add(outQueue);
        foreach (var neighbor in neighbors.Select(x => AddTuple(outQueue, x))
            .Where(x => 0 <= x.X && x.X <= maxX && 0 <= x.Y && x.Y <= maxY))
        {
            if (outside.Contains(neighbor) || historyP2.Contains(neighbor)) continue;
            if (!queue.Contains(neighbor)) queue.Enqueue(neighbor);
        }
    }

    var query = from y in Enumerable.Range(1, part2Input.Count).Where(w => (w % 3) == 1)
                from x in Enumerable.Range(1, part2Input[0].Length).Where(w => (w % 3) == 1)
                select (x, y);
    
    int part2Answer = query.Count(s => !(historyP2.Contains(s) || outside.Contains(s)));

    for (int y = 1; y < part2Input.Count; y += 3)
    {
        for (int x = 1; x < part2Input[0].Length; x += 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (historyP2.Contains((x, y))) Console.ForegroundColor = ConsoleColor.Green;
            if (outside.Contains((x, y))) Console.ForegroundColor = ConsoleColor.DarkGray;
            
            Console.Write(part2Input[y][x]);
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    Console.WriteLine($"Part 1: There are at most {part1Answer} steps from one end of the loop to the farthest point.");
    Console.WriteLine($"Part 2: There are {part2Answer} inside spaces.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}


