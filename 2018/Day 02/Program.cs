try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer =
        puzzleInput.Count(x => x.GroupBy(g => g).Any(x => x.Count() == 2)) *
        puzzleInput.Count(x => x.GroupBy(g => g).Any(x => x.Count() == 3));

    string part2Answer = "";
    foreach((int boxAIndex , int boxBIndex) in from a in Enumerable.Range(0, puzzleInput.Length)
                                               from b in Enumerable.Range(0, puzzleInput.Length)
                                               where a != b
                                               select (a, b))
    {
        IEnumerable<(bool B, char C)> qry = puzzleInput[boxAIndex].Select((x, i) => (x == puzzleInput[boxBIndex][i], x));
        if (qry.Where(x => !x.B).Count() == 1)
        {
            part2Answer = String.Join("", qry.Where(x => x.B).Select(x => x.C));
            break;
        }
    }

    Console.WriteLine($"Part 1: The checksum for the box IDs is {part1Answer}.");
    Console.WriteLine($"Part 2: The common letters of the box IDs are {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}