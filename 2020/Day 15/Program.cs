try
{
    const int PART_1_LOOPS = 2020;
    const int PART_2_LOOPS = 30000000;
    
    static int DoMemoryGame(int numTurns)
    {
        const string PUZZLE_INPUT = "PuzzleInput.txt";

        Dictionary<int, int> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select((a, b) => new { Value = int.Parse(a), Index = b }).ToDictionary(x => x.Value, x => x.Index + 1 );

        int lastNumber = puzzleInput.Last().Key;
        int nextNumber = -1;

        // strip this out so the loop catches it. 
        puzzleInput.Remove(lastNumber);

        for (int turn = puzzleInput.Count + 2; turn <= numTurns; turn++)
        {
            // Remember that we're looking back a turn, so numbers spoken are on turn - 1. 
            if (puzzleInput.TryGetValue(lastNumber, out int lastTimeSpoken))
            {
                // has been spoken before, calculate age difference, then update.
                nextNumber = turn - 1 - lastTimeSpoken;
                puzzleInput[lastNumber] = turn - 1;
            }
            else
            {
                //Has NOT been spoken before, say 0 and add the spoken number to the lists.
                if (!puzzleInput.TryAdd(lastNumber, turn - 1 )) puzzleInput[lastNumber] = turn - 1;

                nextNumber = 0;
            }

            lastNumber = nextNumber;
        }
        return lastNumber;
    }

    int part1Answer = DoMemoryGame(PART_1_LOOPS);
    int part2Answer = DoMemoryGame(PART_2_LOOPS);

    Console.WriteLine($"Part 1: The answer after {PART_1_LOOPS} is {part1Answer}.");
    Console.WriteLine($"Part 2: The answer after {PART_2_LOOPS} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}