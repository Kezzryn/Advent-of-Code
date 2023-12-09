try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<List<int>> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Split(' ').Select(int.Parse).ToList()).ToList();

    static (int, int) FindValue(List<int> values)
    {
        (int start, int end) = values.All(x => x == 0) 
                ? (0, 0)
                : FindValue(values.SkipLast(1).Select((x, i) => values[i + 1] - x).ToList());

        return ((values.First() - start), (values.Last() + end));
    }

    (int part2Answer, int part1Answer) = puzzleInput.Select(x => FindValue(x)).Aggregate((a,b) => ((a.Item1 + b.Item1),(a.Item2 + b.Item2)));

    Console.WriteLine($"Part 1: The sum of the last numbers is {part1Answer}.");
    Console.WriteLine($"Part 2: The sum of the first numbers are {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}