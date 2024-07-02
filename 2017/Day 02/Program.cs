try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    IEnumerable<IOrderedEnumerable<int>> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split('\t', StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse).OrderBy(x => x));

    int part1Answer = puzzleInput.Sum(x => x.Last() - x.First());

    var part2Answer = puzzleInput.Sum(data => (from a in data
                                               from b in data
                                               where a != b && a > b && a % b == 0
                                               select a / b).FirstOrDefault(0));

    Console.WriteLine($"Part 1: The spreadsheet's checksum is {part1Answer}.");
    Console.WriteLine($"Part 2: The even number's checksum is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}