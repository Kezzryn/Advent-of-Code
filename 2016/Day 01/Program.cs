using System.Drawing;

static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',', StringSplitOptions.TrimEntries).ToList();

    Size[] step = new[] // See the Complex Numbers solution for a different way of tracking rotation.
    {
        new Size( 0, 1), // N
        new Size( 1, 0), // E
        new Size( 0,-1), // S
        new Size(-1, 0)  // W
    };

    int direction = 0;
    Point cursor = new (0,0);
    Point intersection = new (0,0);
    HashSet<Point> path = new();

    foreach (string instruction in puzzleInput)
    {
        direction = (direction + instruction[0] switch
        {
            'L' => 3,
            'R' => 1,
            _ => throw new NotImplementedException()
        }) % 4;
    
        for (int i = 1; i <= int.Parse(instruction[1..]); i++)
        {
            cursor += step[direction];
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