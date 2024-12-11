using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    Dictionary<Point2D, int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .SelectMany((a, y) => a.Select((b, x) => (X: x, Y: y, V: b - '0')))
            .ToDictionary(d => new Point2D(d.X, d.Y), d => d.V);

    static IEnumerable<Point2D> WalkTrailHead(Point2D currPos, Dictionary<Point2D, int> theMap)
    {
        if (theMap[currPos] == 9) return [currPos];

        return currPos.GetOrthogonalNeighbors()
            .Where(x => theMap.TryGetValue(x, out int nextHeight) && nextHeight == theMap[currPos] + 1)
                .SelectMany(x => WalkTrailHead(x, theMap));
    }

    IEnumerable<IEnumerable<Point2D>> trailheads = puzzleInput
        .Where(x => x.Value == 0)
            .Select(x => WalkTrailHead(x.Key, puzzleInput));

    int part1Answer = trailheads.Sum(x => x.Distinct().Count());
    int part2Answer = trailheads.Sum(x => x.Count());

    Console.WriteLine($"Part 1: The trailhead score is {part1Answer}.");
    Console.WriteLine($"Part 2: The trailhead rating is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}