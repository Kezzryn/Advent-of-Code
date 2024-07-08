try
{
    const bool BREAK_EARLY = true;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    Dictionary<int, int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split(": ", StringSplitOptions.TrimEntries).Select(int.Parse))
            .ToDictionary(x => x.First(), x => x.Last());

    int Severity(int offset, bool breakEarly = false)
    {
        int rv = 0;
        foreach ((int location, int depth) in puzzleInput)
        {
            int length = (depth - 1) * 2;
            if (((location + offset) % length) == 0)
            {
                if (breakEarly) return -1;
                rv += depth * location;
            }
        }
        return rv;
    }

    int part1Answer = Severity(0);
    int part2Answer = 0;

    while (Severity(++part2Answer, BREAK_EARLY) != 0)
    {
        //spin the loop!
    }

    Console.WriteLine($"Part 1: The severity of the trip is: {part1Answer}.");
    Console.WriteLine($"Part 2: A delay of {part2Answer} is required to not get caught.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}