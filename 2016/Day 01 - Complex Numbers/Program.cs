using System.Drawing;
using System.Numerics;

static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Replace(" ", "").Split(',').ToList();

    Complex direction = new(0, 1);

    Point cursor = new(0, 0);
    Point intersection = new(0, 0);
    HashSet<Point> path = new();

    foreach (string instruction in puzzleInput)
    {
        direction *= (instruction[0] == 'L') ? -Complex.ImaginaryOne : Complex.ImaginaryOne;

        for (int i = 1; i <= int.Parse(instruction[1..]); i++)
        {
            cursor += new Size((int)direction.Real, (int)direction.Imaginary);

            if (path.Contains(cursor) && intersection.X == 0 && intersection.Y == 0) intersection = cursor;
            path.Add(cursor);
        }
    }

    Console.WriteLine($"Part 1: The distance to the Easter Bunny headquarters is: {TaxiDistance(new(0, 0), cursor)}");
    Console.WriteLine($"Part 2: The distance to the first place to visited twice is: {TaxiDistance(new(0, 0), intersection)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}