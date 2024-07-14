try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToList();

    int part1Answer = puzzleInput.Sum();

    HashSet<int> freqChangeList = [0];
    int part2Answer = 0;
    int freqDeltaIndex = 0;
    do
    {
        part2Answer += puzzleInput[freqDeltaIndex++];
        if (freqDeltaIndex >= puzzleInput.Count) freqDeltaIndex = 0;
    } while (freqChangeList.Add(part2Answer));

    Console.WriteLine($"Part 1: The frequency changes by {part1Answer}.");
    Console.WriteLine($"Part 2: The first duplicated frequency is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}