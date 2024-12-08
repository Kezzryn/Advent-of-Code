using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<char, List<Point2D>> theMap = [];

    int maxX = puzzleInput[0].Length;
    int maxY = puzzleInput.Length;
    Point2D maxPoint2D = new(maxX - 1, maxY - 1);

    foreach ((int x, int y) in from y in Enumerable.Range(0, maxY)
                              from x in Enumerable.Range(0, maxX)
                              select (x, y))
    {
        if (puzzleInput[y][x] == '.') continue;
        
        char symbol = puzzleInput[y][x];
        if(!theMap.TryAdd(symbol, [new(x, y)]))
        {
            theMap[symbol].Add(new Point2D(x, y));
        }
    }

    bool InRange(Point2D point) => 
        point.X >= 0 && point.Y >= 0 && point.X <= maxPoint2D.X && point.Y <= maxPoint2D.Y;

    (List<Point2D> antiNodes_Part1, List<Point2D> antiNodes_Part2) CalcAntiNodes(Point2D antenna_A, Point2D antenna_B)
    {
        Point2D delta = antenna_B - antenna_A;

        List<Point2D> antiNodes_Part1 = [];
        Point2D tempAntiNode_A = antenna_A - delta;
        Point2D tempAntiNode_B = antenna_B + delta;
        if (InRange(tempAntiNode_A)) antiNodes_Part1.Add(tempAntiNode_A);
        if (InRange(tempAntiNode_B)) antiNodes_Part1.Add(tempAntiNode_B);

        List<Point2D> antiNodes_Part2 = [antenna_A, antenna_B]; // include the existing antenna

        while (InRange(tempAntiNode_A))
        {
            antiNodes_Part2.Add(tempAntiNode_A);
            tempAntiNode_A -= delta;
        }
        
        while (InRange(tempAntiNode_B))
        {
            antiNodes_Part2.Add(tempAntiNode_B);
            tempAntiNode_B += delta;
        }

        return (antiNodes_Part1, antiNodes_Part2);
    }
    
    HashSet<Point2D> part1Answer = [];
    HashSet<Point2D> part2Answer = [];

    foreach ((char key, List<Point2D> antenna) in theMap)
    {
        for(int a = 0; a < antenna.Count - 1; a++)
        {
            for (int b = a + 1; b < antenna.Count; b++)
            {
                (List<Point2D> antiNodes_Part1, List<Point2D> antiNodes_Part2) = 
                    CalcAntiNodes(antenna[a], antenna[b]);

                part1Answer.UnionWith(antiNodes_Part1);
                part2Answer.UnionWith(antiNodes_Part2);
            }
        }
    }

    Console.WriteLine($"Part 1: The number of antinodes is {part1Answer.Count}.");
    Console.WriteLine($"Part 2: When accounting for resonant harmonics the number of antinodes increases to {part2Answer.Count}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}