try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int LOC = 0;
    const int DEPTH = 1;
    const bool BREAK_EARLY = true;
    int[][] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Split(": ", StringSplitOptions.TrimEntries).Select(int.Parse).ToArray()).ToArray();

    int part1Answer = 0;
    int part2Answer = 0;

    int Severity(int offset, bool breakEarly = false)
    {
        int rv = 0;
        foreach (int[] puzzle in puzzleInput)
        {
            int length = (puzzle[DEPTH] - 1) * 2;
            if (((puzzle[LOC] + offset) % length) == 0)
            {
                if (breakEarly) return -1;
                rv += puzzle[DEPTH] * puzzle[LOC];
            }
        }
        return rv;
    }

    part1Answer = Severity(0);

    bool isDone = false;
    int count = 0;
    while (!isDone)
    {
        if (Severity(count, BREAK_EARLY) == 0)
        {
            part2Answer = count;
            isDone = true;
        }
        count++;
    }

    Console.WriteLine($"Part 1: The severity of the trip is: {part1Answer}.");
    Console.WriteLine($"Part 2: A delay of {part2Answer} is required to not get caught.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}