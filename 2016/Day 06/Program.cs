try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT)];

    //Turn our column data into row data.
    IEnumerable<IEnumerable<char>> puzzleRowToCol = Enumerable.Range(0, puzzleInput[0].Length)
        .Select(col => puzzleInput.Select(row => row[col]));

    string part1Answer = String.Join("",
            puzzleRowToCol.Select(x => x .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .ThenBy(x => x.Key)
                .SelectMany(x => x)
                .First()));

    string part2Answer = String.Join("",
            puzzleRowToCol.Select(x => x.GroupBy(x => x)
                .OrderBy(g => g.Count())
                .ThenBy(x => x.Key)
                .SelectMany(x => x)
                .First()));

    Console.WriteLine($"Part 1: The message from the most characters is: {part1Answer}");
    Console.WriteLine($"Part 2: The message from the least characters is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}