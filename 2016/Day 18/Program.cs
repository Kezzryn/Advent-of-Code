try
{
    const int ROWS_PART_1 = 40;
    const int ROWS_PART_2 = 400000;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    bool[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Select(x => x == '.').ToArray();

    int part1Answer = 0;
    int part2Answer = puzzleInput.Count(x => x);

    for (int i = 2; i <= ROWS_PART_2; i++)
    // start at 2 to account for priming read. 
    {
        puzzleInput = puzzleInput.Select((x, j) => ((j == 0) || puzzleInput[j - 1]) == ((j == puzzleInput.Length - 1) || puzzleInput[j + 1])).ToArray();

        part2Answer += puzzleInput.Count(x => x);

        if (i == ROWS_PART_1) part1Answer = part2Answer;
    }

    Console.WriteLine($"Part 1: The number of safe spaces in {ROWS_PART_1} is {part1Answer}.");

    Console.WriteLine($"Part 2: The number of safe spaces in {ROWS_PART_2} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}