using BKH.AoC_Point2D;
using System.Numerics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<(char, int)> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(x => (x[0], int.Parse(x[1..]))).ToList();

    Complex direction = new(0, 1);

    Point2D cursor = Point2D.Origin;
    Point2D intersection = Point2D.Origin;
    HashSet<Point2D> path = [];

    foreach ((char direction, int distance) in puzzleInput)
    {
        direction *= (instruction[0] == 'L') ? -Complex.ImaginaryOne : Complex.ImaginaryOne;

        for (int i = 1; i <= distance) ; i++)
        {
            cursor += new Point2D((int)direction.Real, (int)direction.Imaginary);

            if (path.Contains(cursor) && intersection.X == 0 && intersection.Y == 0) intersection = cursor;
            path.Add(cursor);
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