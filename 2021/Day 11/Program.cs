static void WriteOcto(Dictionary<(int, int), int> currOcto)
{
    Console.SetCursorPosition(0, 0);
    ConsoleColor color;
    for (int y = 0; y < 10; y++)
    {
        for (int x = 0; x < 10; x++)
        {
            int curValue = currOcto[(x, y)];
            color = curValue switch
            {
                1 => ConsoleColor.DarkGray,
                2 => ConsoleColor.DarkYellow,
                3 => ConsoleColor.DarkMagenta,
                4 => ConsoleColor.DarkRed,
                5 => ConsoleColor.DarkCyan,
                6 => ConsoleColor.Cyan,
                7 => ConsoleColor.Yellow,
                8 => ConsoleColor.Magenta,
                9 => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
            Console.ForegroundColor = color;
            Console.Write(curValue >= 9 ? 0 : curValue);
        }
        Console.WriteLine();
    }
    Console.ResetColor();
    Console.WriteLine();
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART1_STEPS = 100;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    List<(int x, int y)> neighbors = new()
    {
        (-1, -1), ( 0, -1), ( 1, -1),
        (-1,  0),           ( 1,  0),
        (-1,  1), ( 0,  1), ( 1,  1)
    };
        
    Dictionary<(int X, int Y), int> octopuses = new();

    foreach((int x, int y) in from y in Enumerable.Range(0, puzzleInput.Length)
                              from x in Enumerable.Range(0, puzzleInput[0].Length)
                              select(x,y))
    {
        octopuses[(x, y)] = puzzleInput[y][x] - 48;
    }

    int part1Answer = 0;
    int part2Answer = 0;
    int step = 0;

    Console.Clear();

    while (part2Answer == 0 || part1Answer == 0)
    {
        step++;
        HashSet<(int, int)> hasFlashed = new();

        foreach((int, int) octo in octopuses.Keys)
        {
            octopuses[octo]++;
        }

        bool isDone = false;
        while (!isDone)
        {
            isDone = true;
            foreach (var octo in octopuses.Where(x => x.Value > 9))
            {
                if (!hasFlashed.Add(octo.Key)) continue; // already in collection, skip.
                
                if(step <= PART1_STEPS) part1Answer++;
                isDone = false;
                foreach (var (nX, nY) in neighbors)
                {
                    (int X, int Y) currentNeighbor = (octo.Key.X + nX, octo.Key.Y + nY);
                    if(currentNeighbor.X < 0 || currentNeighbor.Y < 0 || 
                       currentNeighbor.X > 9 || currentNeighbor.Y > 9) continue;
                    
                    octopuses[currentNeighbor]++;
                }
            }
        }

        WriteOcto(octopuses);
        Console.WriteLine($"Step: {step}");
        Thread.Sleep(50); // keep above 50-ish.

        foreach (var octo in hasFlashed)
        {
            octopuses[octo] = 0;    
        }

        if (hasFlashed.Count == octopuses.Count) part2Answer = step;
    }

    Console.WriteLine($"Part 1: After 100 setps there have been {part1Answer} flashes.");
    Console.WriteLine($"Part 2: The first time all the octopusses flash together is turn {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
