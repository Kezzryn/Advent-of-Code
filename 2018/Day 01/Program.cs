try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleinput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToArray();

    int part1Answer = puzzleinput.Sum();

    HashSet<int> freqChangeList = new() { 0 };
    int part2Answer = 0;
    int freqDeltaIndex = 0;
    while (true)
    {
        part2Answer += puzzleinput[freqDeltaIndex];
        freqDeltaIndex = (freqDeltaIndex + 1) % puzzleinput.Length;
        if (!freqChangeList.Add(part2Answer)) break;
    }

    Console.WriteLine($"Part 1: The frequency changes by {part1Answer}.");
    Console.WriteLine($"Part 2: The first duplicated frequency is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}