using AoC_2024_Day_14;
using BKH.Geometry;
using static BKH.Geometry.Point2D;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int MAX_WIDTH = 101;
    const int MAX_HEIGHT = 103;
    Point2D midPoint = new(MAX_WIDTH / 2, MAX_HEIGHT / 2);

    List<Roomba> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => new Roomba(x, MAX_WIDTH, MAX_HEIGHT)).ToList();

    static int SafetyFactor(List<Roomba> puzzleInput, Point2D midPoint)
    {
        int returnValue = 1;
        foreach (Quadrant quadrent in Enum.GetValues<Quadrant>().Where(x => x != Quadrant.OnGridLine))
        {
            returnValue *= puzzleInput.Count(x => midPoint.IsInQuadrant(x.Position, quadrent));
        }
        return returnValue;
    }

    int step = 100;
    puzzleInput.ForEach(x => x.Step(100));

    int part1Answer = SafetyFactor(puzzleInput, midPoint);

    int part2Answer = 0;
    int maxNeighbors = int.MinValue;

    while (step < MAX_HEIGHT * MAX_WIDTH)
    {
        step++;
        puzzleInput.ForEach(x => x.Step());

        HashSet<Point2D> pos = puzzleInput.Select(x => x.Position).ToHashSet();
        int neighborScore = puzzleInput.Sum(x => x.Position.GetOrthogonalNeighbors().Count(n => pos.Contains(n)));

        if (neighborScore > maxNeighbors)
        {
            maxNeighbors = neighborScore;
            part2Answer = step;
        }
    }

    void DrawMap()
    {
        Console.Clear();
        for (int y = 0; y < MAX_HEIGHT; y++)
        {
            for (int x = 0; x < MAX_WIDTH; x++)
            {
                Point2D current = new(x, y);
                int num = puzzleInput.Count(x => x.Position == current);
                if (num > 0)
                {
                    Console.Write(num);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    Console.WriteLine($"Part 1: The saftey score is {part1Answer} after 100 seconds.");
    Console.WriteLine($"Part 2: The easter egg appears after {part2Answer} seconds.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}