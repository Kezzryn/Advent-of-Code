using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    HashSet<Point2D> paperStacks = [];

    foreach (Point2D gridPoint in from x in Enumerable.Range(0, puzzleInput[0].Length)
                                from y in Enumerable.Range(0, puzzleInput.Length)
                                where puzzleInput[y][x] == '@'
                                select new Point2D(x, y))
    {
        paperStacks.Add(gridPoint);
    }

    int part1Answer = paperStacks.Count(x => x.GetAllNeighbors().Count(x => paperStacks.Contains(x)) < 4);

    int part2Answer = paperStacks.Count;
    while (true)
    {
        IEnumerable<Point2D> temp = paperStacks.Where(x => x.GetAllNeighbors().Count(x => paperStacks.Contains(x)) < 4);
        if (!temp.Any()) break;
        paperStacks.ExceptWith(temp);
    }
    part2Answer -= paperStacks.Count;

    Console.WriteLine($"Part 1: Initially there are {part1Answer} rolls of accessible paper.");
    Console.WriteLine($"Part 2: {part2Answer} rolls can be moved to make a path.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}