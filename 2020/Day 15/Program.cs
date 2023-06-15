try
{
    const int PART_1_LOOPS = 2020;
    const int PART_2_LOOPS = 30000000;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    static int DoMemoryGame(int numTurns, string startingNumbers)
    {
        Dictionary<int, int> puzzleInput = startingNumbers.Split(',').Select((a, b) => new { Value = int.Parse(a), Index = b }).ToDictionary(x => x.Value, x => x.Index + 1 );

        int lastNumber = puzzleInput.Last().Key;
        int nextNumber = -1;
        int prevTurn = 0;
        int lastTimeSpoken = 0;
        // strip this out so the loop catches it. 
        puzzleInput.Remove(lastNumber);

        for (int turn = puzzleInput.Count + 2; turn <= numTurns; turn++)
        {
            // Remember that we're looking back a turn, so numbers spoken are on turn - 1. 
            prevTurn = turn - 1;
            
            // check for has been spoken before, calculate age difference, then update.
            nextNumber = puzzleInput.TryGetValue(lastNumber, out lastTimeSpoken) ? prevTurn - lastTimeSpoken : 0;
            
            //update last time spoken
            if (!puzzleInput.TryAdd(lastNumber, prevTurn)) puzzleInput[lastNumber] = prevTurn;

            lastNumber = nextNumber;
        }
        return lastNumber;
    }

    int part1Answer = DoMemoryGame(PART_1_LOOPS, puzzleInput);
    int part2Answer = DoMemoryGame(PART_2_LOOPS, puzzleInput);

    Console.WriteLine($"Part 1: The answer after {PART_1_LOOPS} is {part1Answer}.");
    Console.WriteLine($"Part 2: The answer after {PART_2_LOOPS} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}