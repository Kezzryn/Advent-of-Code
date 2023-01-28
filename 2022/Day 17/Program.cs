using AoC_2022_Day_17;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    //if we can't find a repeating section after this... raise the MAX. 
    const int MAX_LOOPS = 3000;

    Shaft shaft = new(File.ReadAllText(PUZZLE_INPUT));

    // Dictionary of Shaft hash values. 
    // We use this to find when we start repeating. 
    Dictionary<string, int> keys = new();

    //list of heights attached to each key. Used for repeat calculation. 
    int[] heights = new int[MAX_LOOPS];

    string key = "";
    int numRocksDropped = 0;

    for (int i = 1; i <= MAX_LOOPS; i++)
    {
        shaft.DropRock();
        heights[i] = shaft.ShaftHeight;
        key = shaft.GetHash();

        if (!keys.TryAdd(key, i))
        {
            numRocksDropped = i;
            break;
        }
    }

    long numRocksPart1 = 2022;
    long numRocksPart2 = 1000000000000;


    int currentHeight = heights[numRocksDropped];
    int repeatHeight = currentHeight - heights[keys[key]];
    int loopRocks = numRocksDropped - keys[key];


    long part1NumRepeat = numRocksPart1 / loopRocks;
    long part1ModRepeat = numRocksPart1 % loopRocks;
    long part1TotalHeight = (part1NumRepeat * repeatHeight) + heights[part1ModRepeat];


    long part2NumRepeat = numRocksPart2 / loopRocks;
    long part2ModRepeat = numRocksPart2 % loopRocks;
    long part2TotalHeight = (part2NumRepeat * repeatHeight) + heights[part2ModRepeat];


    Console.WriteLine($"Number of rocks dropped before we found a repeat. {numRocksDropped} Total Height = {currentHeight}");
    Console.WriteLine($"Prev matching key is at {keys[key]} wiht a height of {heights[keys[key]]}");
    Console.WriteLine($"Looping section height equals {repeatHeight} with an intervel of {loopRocks}");
    Console.WriteLine();

    Console.WriteLine($"Part 1: Repeating Blocks {part1NumRepeat} mod {part1ModRepeat} give us a total height of {part1TotalHeight}");
    Console.WriteLine();
    Console.WriteLine($"Part 2: Repeating Blocks {part2NumRepeat} mod {part2ModRepeat} give us a total height of {part2TotalHeight}");
}

catch (Exception e)
{
    Console.WriteLine(e);
}

