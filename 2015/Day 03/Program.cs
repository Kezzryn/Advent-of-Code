using BKH.Geometry;

try
{
    Dictionary<char, Point2D> directions = new()
    {
        { '<', Point2D.Origin.OrthogonalNeighbor(Point2D.Direction.Left)  },
        { '>', Point2D.Origin.OrthogonalNeighbor(Point2D.Direction.Right) },
        { 'v', Point2D.Origin.OrthogonalNeighbor(Point2D.Direction.Down)  },
        { '^', Point2D.Origin.OrthogonalNeighbor(Point2D.Direction.Up)    }
    };

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<Point2D> puzzleInput = File.ReadAllText(PUZZLE_INPUT)
           .Select(x => directions[x]).ToList();

    HashSet<Point2D> part1Answer = [ Point2D.Origin ];
    HashSet<Point2D> part2Answer = [ Point2D.Origin ];

    Point2D soloSanta = Point2D.Origin;
    Point2D pairSanta = Point2D.Origin;
    Point2D roboSanta = Point2D.Origin;

    bool isRoboSanta = false; 

    foreach (Point2D step in puzzleInput)
    {
        soloSanta += step;
        part1Answer.Add(soloSanta);

        if (isRoboSanta)
        {
            roboSanta += step;
            part2Answer.Add(roboSanta);
        }
        else
        {
            pairSanta += step;
            part2Answer.Add(pairSanta);
        }
        isRoboSanta = !isRoboSanta;
    }

    Console.WriteLine($"Part 1: Santa visited {part1Answer.Count} houses.");
    Console.WriteLine($"Part 2: Santa and Robo Santa visited a total of {part2Answer.Count} houses.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}