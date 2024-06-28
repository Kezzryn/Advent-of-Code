using BKH.Geometry;

static string KeypadSolver(List<string> input, Dictionary<Point2D, char> keypad)
{
    Point2D cursor = Point2D.Origin;    //the keypads are "centered" on 0,0.

    string returnValue = "";
    foreach (string instruction in input)
    {
        foreach (char direction in instruction)
        {
            Point2D nextStep = direction switch
            {
                'U' => cursor.OrthogonalNeighbor(Point2D.Direction.Up),
                'D' => cursor.OrthogonalNeighbor(Point2D.Direction.Down),
                'L' => cursor.OrthogonalNeighbor(Point2D.Direction.Left),
                'R' => cursor.OrthogonalNeighbor(Point2D.Direction.Right),
                _ => throw new NotImplementedException()
            };

            if(keypad.ContainsKey(nextStep)) cursor = nextStep;
        }
        returnValue += keypad[cursor];
    }
    return returnValue; 
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT)];

    Dictionary<Point2D, char> keypadPart1 = new()
    {
        { new(-1, 1), '1' }, { new(0, 1), '2' }, { new(1, 1), '3' },
        { new(-1, 0), '4' }, { new(0, 0), '5' }, { new(1, 0), '6' },
        { new(-1,-1), '7' }, { new(0,-1), '8' }, { new(1,-1), '9' },
    };

    Dictionary<Point2D, char> keypadPart2 = new()
    {
                                                  { new( 2, 2), '1' },
                             { new( 1, 1), '2' }, { new( 2, 1), '3' }, { new( 3, 1), '4' },
        { new( 0, 0), '5' }, { new( 1, 0), '6' }, { new( 2, 0), '7' }, { new( 3, 0), '8' }, { new( 4, 0), '9' },
                             { new( 1,-1), 'A' }, { new( 2,-1), 'B' }, { new( 3,-1), 'C' },
                                                  { new( 2,-2), 'D' }
    };

    string part1Answer = KeypadSolver(puzzleInput, keypadPart1);
    string part2Answer = KeypadSolver(puzzleInput, keypadPart2);

    Console.WriteLine($"Part 1: The bathroom door code is: {part1Answer}");
    Console.WriteLine($"Part 2: The insane bathroom door code is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}