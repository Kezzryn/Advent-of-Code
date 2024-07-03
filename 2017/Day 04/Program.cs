try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    IEnumerable<string[]> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Split(' '));

    // distinct drops dupes.
    int part1Answer = puzzleInput.Count(w => w.Length == w.Distinct().Count());

    // sort the string letters to find the anagram matches, then... drop dupes.
    int part2Answer = puzzleInput
        .Select(x => x.Select(s => String.Concat(s.OrderBy(c => c))))
        .Count(w => w.Count() == w.Distinct().Count());

    Console.WriteLine($"Part 1: The number without duplicates is: {part1Answer}");
    Console.WriteLine($"Part 2: The number without anagrams is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}