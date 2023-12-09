try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).Select(x => x[(x.IndexOf(':') + 1)..]).ToList();

    List<long> values = puzzleInput[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

    List<List<(long start, long range, long offset)>> maps =
        puzzleInput.Skip(1)
        .Select(x => x.Split(CRLF, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(' ').Select(long.Parse).ToList()
            ).ToList()
            .Select(x => (x[1], x[2], x[0] - x[1])).ToList()
        ).ToList();

    long part1Answer = long.MaxValue;
    long part2Answer = long.MaxValue;

    long calcVal(long value)
    {
        for (int i = 0; i < maps.Count; i++)
        {
            foreach ((long start, long range, long offset) in maps[i])
            {
                if (start <= value && value <= start + range)
                {
                    value += offset;
                    break;
                }
            }
        }

        return value;
    }
    part1Answer = values.Select(x => calcVal(x)).Min();

    static IEnumerable<long> LongRange(long start, long count)
    {
        // Loosely copied from: https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,fda9d378095a6464
        for (long i = 0; i < count; i++) yield return start + i;
    }

    for (int i = 0; i < values.Count - 1; i += 2)
    {
        //Copied from chapter 5 example at https://www.albahari.com/threading/

        long minOfRange = LongRange(values[i], values[i + 1])
                .AsParallel()
                .Select(calcVal).Min();

        if (minOfRange < part2Answer) part2Answer = minOfRange;
    }

    Console.WriteLine($"Part 1: The lowest location number that corresponds to any initial seed number is: {part1Answer}");
    Console.WriteLine($"Part 2: The lowest location number that falls within any initial seed number range is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}