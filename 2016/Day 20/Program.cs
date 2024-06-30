using BKH.Segments;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT)];

    Segments blockList = new();
    Segments allowList = new();

    puzzleInput.Select(x => x.Split('-').Select(uint.Parse)).ToList()
        .ForEach(x => blockList.AddRange(x.First(), x.Last()));

    List<Slice> blockListSegment = [..blockList.GetSlices()];
    
    if (blockListSegment.First().Start != 0)
        allowList.AddRange(0, blockListSegment.First().Start - 1);

    if (blockListSegment.Last().End != uint.MaxValue)
        allowList.AddRange(blockListSegment.Last().End + 1, uint.MaxValue);

    for (int i = 0; i < blockListSegment.Count - 1; i++)
    {
        allowList.AddRange(blockListSegment[i].End + 1, blockListSegment[i + 1].Start - 1);
    }
    
    uint part1Answer = allowList.GetSlices().Select(x => x.Start).Min();
    long part2Answer = allowList.GetSlices().Sum(x => x.Length == 0 ? 1 : x.Length);

    Console.WriteLine($"Part 1: The lowest-valued IP is {part1Answer}");
    Console.WriteLine($"Part 2: There are {part2Answer} valid IPs.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
