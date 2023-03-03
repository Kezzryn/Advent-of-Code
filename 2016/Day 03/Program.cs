try
{
    // A valid triangle has the sum of any two sides greater than the last once. 
    // Note that this must be true for ALL three combinations. However, this can be shortcut by summing the
    // two smallest sides and comparing the result to the remaining side.
    // From wikipedia: This can be stated as: max(a,b,c) < a + b + c - max(a,b,c)
    
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[][] triangles = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split(' ',StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .OrderBy(x => x)
            .ToArray())
        .ToArray();

    int part1 = triangles.Sum(x => (x.Take(2).Aggregate((a, b) => a + b) > x.Last()) ? 1 : 0);
    Console.WriteLine($"The total number of valid triangles is {part1}.");

    int part2 = 0;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<Range> columns = new()
    {
        new Range(2, 5),
        new Range(7, 10),
        new Range(12, 15)
    };

    for (int line = 0; line < puzzleInput.Length; line += 3)
    {
        foreach (Range col in columns)
        {
            List<int> triangleSet = new() {
                int.Parse(puzzleInput[line][col].Trim()),
                int.Parse(puzzleInput[line + 1][col].Trim()),
                int.Parse(puzzleInput[line + 2][col].Trim())
            };
            triangleSet = triangleSet.OrderBy(x => x).ToList();
            part2 += (triangleSet.Take(2).Aggregate((a, b) => a + b) > triangleSet.Last()) ? 1 : 0;
        }
    }

    Console.WriteLine($"The total number of vertially counted triangles is {part2}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}