using AoC_2023_Day_14;

try
{
    const int OPEN = 0;
    const int ROCK = 1;
    const int STONE = 2;
 
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int[,] theMap = new int[puzzleInput[0].Length, puzzleInput.Length];

    foreach ((int X, int Y) in from y in Enumerable.Range(0, puzzleInput.Length)
                              from x in Enumerable.Range(0, puzzleInput[0].Length)
                              select(x, y))
    {
        theMap[X, Y] = puzzleInput[puzzleInput.Length - Y - 1][X] switch
        {
            '#' => ROCK,
            'O' => STONE,
            '.' => OPEN,
            _ => throw new NotImplementedException()
        };
    }

    static void RockAndRoll(int[] array, bool rollTowardsZero = true)
    {
        int i = rollTowardsZero ? 0 : array.GetUpperBound(0);
        int cursorDest = -1;

        while((0 <= i && i <= array.GetUpperBound(0)))
        {
            if (array[i] == OPEN  && cursorDest == -1) cursorDest = i;
            if (array[i] == ROCK) cursorDest = -1;
            if (array[i] == STONE && cursorDest != -1)
            {
                (array[cursorDest], array[i]) = (array[i], array[cursorDest]);
                cursorDest += rollTowardsZero ? 1 : -1;
            }

            i += rollTowardsZero ? 1 : -1;
        }
    }

    void RollNorthSouth(bool NorthSouth)
    {
        for(int col = 0; col <= theMap.GetUpperBound(0); col++)
        {
            int[] slice = theMap.SliceCol(col).ToArray();
            RockAndRoll(slice, NorthSouth);
            theMap.PasteCol(col, slice);
        }
    }

    void RollEastWest(bool EastWest)
    {
        for (int row = 0; row <= theMap.GetUpperBound(1); row++)
        {
            int[] slice = theMap.SliceRow(row).ToArray();
            RockAndRoll(slice, EastWest);
            theMap.PasteRow(row, slice);
        }
    }

    void PrintMap()
    {
        for (int y = puzzleInput.Length - 1; y >= 0; y--)
        {
            Console.Write($"{y+1,-4}");
            for (int x = 0; x < puzzleInput.Length; x++)
            {
                Console.Write((theMap[x, y] == OPEN ? '.' : theMap[x, y] == ROCK ? '#' : 'O'));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    const bool ROLL_NORTH = false;
    const bool ROLL_SOUTH = true;
    const bool ROLL_EAST = false;
    const bool ROLL_WEST = true;

    static int Score(int[,] array) => Enumerable.Range(0, array.GetUpperBound(1) + 1).Sum(s => array.SliceRow(s).Count(c => c == STONE) * (s + 1));

    List<int[,]> mapHistory = new();

    RollNorthSouth(ROLL_NORTH);
    int part1Answer = Score(theMap);

    int part2Answer = -1;
    do
    {
        RollNorthSouth(ROLL_NORTH);
        RollEastWest(ROLL_WEST);
        RollNorthSouth(ROLL_SOUTH);
        RollEastWest(ROLL_EAST);

        var results = mapHistory.Select((s,i) => s.Compare(theMap) ? i + 1 : -1).Where(w => w != -1);
        if (results.Any())
        {
            part2Answer = Score(mapHistory[(1000000000 - results.First() - 1) % (mapHistory.Count + 1 - results.First()) + results.First()]);
        }

        mapHistory.Add((int[,])theMap.Clone());

    } while (part2Answer == -1);

    Console.WriteLine($"Part 1: The total load is {part1Answer}.");
    Console.WriteLine($"Part 2: After 1,000,000,000 rotations, the total load is {part2Answer}.");
    Console.WriteLine();
    PrintMap();
}
catch (Exception e)
{
    Console.WriteLine(e);
}
