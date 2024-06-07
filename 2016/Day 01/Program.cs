using BKH.AoC_Point2D;

try
{
    List<Point2D.Direction> steps =
    [ // See the Complex Numbers solution for a different way of tracking rotation.
        Point2D.Direction.Up,     // N
        Point2D.Direction.Right,  // E
        Point2D.Direction.Down,   // S
        Point2D.Direction.Left    // W
    ];

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<(int, int)> puzzleInput = File.ReadAllText(PUZZLE_INPUT)
        .Split(',', StringSplitOptions.TrimEntries)
        .Select(x => ((x[0] == 'L' ? 3 : 1), int.Parse(x[1..])))
        .ToList();

    int facingIndex = 0;
    Point2D cursor = Point2D.Origin;
    Point2D intersection = Point2D.Origin;
    HashSet<Point2D> path = [];

    foreach ((int direction, int distance) in puzzleInput)
    {
        facingIndex = (facingIndex + direction) % 4;

        for (int i = 1; i <= distance; i++)
        {
            cursor = cursor.OrthogonalNeighbor(steps[facingIndex]);
            if(!path.Add(cursor) && intersection == Point2D.Origin) intersection = cursor;
        }
    }

    int part1Answer = Point2D.TaxiDistance2D(Point2D.Origin, cursor);
    int part2Answer = Point2D.TaxiDistance2D(Point2D.Origin, intersection);

    Console.WriteLine($"Part 1: The distance to the Easter Bunny headquarters is: {part1Answer}");
    Console.WriteLine($"Part 2: The distance to the first place to visited twice is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}