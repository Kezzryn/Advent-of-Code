using AoC_2016_Day_20;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Slice blockList = new();
    Slice allowList = new();

    foreach (string line in puzzleInput)
    {
        uint[] a = line.Split('-').Select(uint.Parse).ToArray();

        blockList.AddRange(a[0], a[1]);
    }

    blockList.FlattenSegments(); //merge and sort the data load.

    Segment[] blockListSegment = blockList.GetSegments().OrderBy(x => x.Start).ToArray();

    for (int i = 0; i < blockListSegment.GetUpperBound(0); i++)
    {
        //check for range before the first 
        if (i == 0 && blockListSegment[i].Start != 0) allowList.AddRange(0, blockListSegment[i].Start - 1);

        allowList.AddRange(blockListSegment[i].End + 1, blockListSegment[i + 1].Start - 1);

        //check for range after the last 
        if (i == blockListSegment.GetUpperBound(0) - 1 && blockListSegment[i + 1].End != uint.MaxValue) allowList.AddRange(blockListSegment[i + 1].End, uint.MaxValue);
    }

    uint part1Answer = allowList.GetSegments().Select(x => x.Start).Min();
    long part2Answer = allowList.GetSegments().Select(x => x.Length == 0 ? 1 : (long)x.Length).Sum();

    Console.WriteLine($"Part 1: The lowest-valued IP is {part1Answer}");
    Console.WriteLine($"Part 2: There are {part2Answer} valid IPs.");
}

catch (Exception e)
{
    Console.WriteLine(e);
}
