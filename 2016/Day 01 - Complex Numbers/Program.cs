using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<(char, int)> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',', StringSplitOptions.TrimEntries).Select(x => (x[0], int.Parse(x[1..]))).ToList();

    Cursor cursor = new(0,0,0,1);

    Point2D intersection = Point2D.Origin;
    HashSet<Point2D> path = [];

    foreach ((char direction, int distance) in puzzleInput)
    {
        if (direction == 'L')
            cursor.TurnLeft();
        else
            cursor.TurnRight();

        for (int i = 1; i <= distance; i++)
        {
            cursor.Step();

            if (!path.Add(cursor.XYAsPoint2D) && intersection == Point2D.Origin) intersection = cursor.XYAsPoint2D;
        }
    }

    int part1Answer = Point2D.TaxiDistance2D(Point2D.Origin, cursor.XYAsPoint2D);
    int part2Answer = Point2D.TaxiDistance2D(Point2D.Origin, intersection);

    Console.WriteLine($"Part 1: The distance to the Easter Bunny headquarters is: {part1Answer}");
    Console.WriteLine($"Part 2: The distance to the first place to visited twice is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}