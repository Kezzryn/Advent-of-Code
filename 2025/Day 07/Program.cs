try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<List<char>> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT).Select(x => x.ToList<char>())];

    long[] beams = new long[puzzleInput[0].Count];

    int part1Answer = 0;

    beams[puzzleInput[0].IndexOf('S')] = 1;
    for (int row = 1; row < puzzleInput.Count; row++)
    {
        IEnumerable<int> splitters = Enumerable.Range(0, puzzleInput[row].Count).Where(i => puzzleInput[row][i] == '^');

        foreach (int item in splitters)
        {
            if (beams[item] != 0)
            {
                part1Answer++;
                beams[item - 1] += beams[item];
                beams[item + 1] += beams[item];
                beams[item] = 0;
            }
        }
    }

    long part2Answer = beams.Sum();

    Console.WriteLine($"Part 1: There are {part1Answer} splits.");
    Console.WriteLine($"Part 2: There are {part2Answer} pathways through the splitters.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}