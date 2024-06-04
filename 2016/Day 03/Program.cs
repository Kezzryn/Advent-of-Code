try
{
    // A valid triangle has the sum of any two sides greater than the last once. 
    // Note that this must be true for ALL three combinations. However, this can be shortcut by summing the
    // two smallest sides and comparing the result to the remaining side.
    // From wikipedia: This can be stated as: max(a,b,c) < a + b + c - max(a,b,c)

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    IEnumerable<IEnumerable<int>> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse));

    //This will turn our column data into row data.
    //Yes, it's voodoo.
    IEnumerable<IEnumerable<int>> puzzlePart2 = puzzleInput.Chunk(3)
        .SelectMany(chunks => Enumerable.Range(0, 3)
            .Select(column => chunks
                .Select(row => row.ElementAt(column))));

    static bool IsValidTriangle(IEnumerable<int> nums) => nums.OrderBy(x => x).Take(2).Sum() > nums.Max();

    int part1Answer = puzzleInput.Count(IsValidTriangle);
    int part2Answer = puzzlePart2.Count(IsValidTriangle);
    
    Console.WriteLine($"The total number of valid triangles is {part1Answer}.");
    Console.WriteLine($"The total number of vertically ordered triangles is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}