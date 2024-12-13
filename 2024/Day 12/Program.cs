using BKH.Geometry;
using BKH.EnumExtentions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    Dictionary<Point2D, int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .SelectMany((a, y) => a.Select((b, x) => (X: x, Y: y, V: (int)b)))
            .ToDictionary(d => new Point2D(d.X, d.Y), d => d.V);

    static HashSet<Point2D> FindGarden(Dictionary<Point2D, int> theMap)
    {
        Queue<Point2D> queue = [];
        HashSet<Point2D> theGarden = [];
        
        queue.Enqueue(theMap.First().Key);
        while(queue.TryDequeue(out Point2D step))
        {
            theGarden.Add(step);

            foreach(Point2D p in step.GetOrthogonalNeighbors().Where(x => !theGarden.Contains(x) && theMap.TryGetValue(x, out int plantType) && plantType == theMap[step]))
            {
                if (!queue.Contains(p)) queue.Enqueue(p);
            }
        }
        return theGarden;
    }

    static int priceFencing(HashSet<Point2D> gardenPlot) =>
        gardenPlot.Sum(x => x.GetOrthogonalNeighbors().Where(x => !gardenPlot.Contains(x)).Count()) * gardenPlot.Count;
        
    static int priceBulkDiscount(HashSet<Point2D> gardenPlot)
    {
        int numSides = 0;
        for (int y = gardenPlot.Min(x => x.Y); y <= gardenPlot.Max(x => x.Y); y++)
        {
            numSides += gardenPlot.Where(a => a.Y == y && !gardenPlot.Contains(a.OrthogonalNeighbor(Point2D.Direction.Up)))
                .Select(a => a.X)
                .OrderBy(n => n)
                    .GroupConsecutive().Count();

            numSides += gardenPlot.Where(a => a.Y == y && !gardenPlot.Contains(a.OrthogonalNeighbor(Point2D.Direction.Down)))
                .Select(a => a.X)
                .OrderBy(n => n)
                    .GroupConsecutive().Count();
        }

        for (int x = gardenPlot.Min(x => x.X); x <= gardenPlot.Max(x => x.X); x++)
        {
            numSides += gardenPlot.Where(a => a.X == x && !gardenPlot.Contains(a.OrthogonalNeighbor(Point2D.Direction.Left)))
                .Select(a => a.Y)
                .OrderBy(n => n)
                    .GroupConsecutive().Count();

            numSides += gardenPlot.Where(a => a.X == x && !gardenPlot.Contains(a.OrthogonalNeighbor(Point2D.Direction.Right)))
                .Select(a => a.Y)
                .OrderBy(n => n)
                    .GroupConsecutive().Count();
        }

        return numSides * gardenPlot.Count;
    }

    int part1Answer = 0;
    int part2Answer = 0;
    
    while (puzzleInput.Count > 0)
    {
        HashSet<Point2D> theGarden = FindGarden(puzzleInput);

        part1Answer += priceFencing(theGarden);
        part2Answer += priceBulkDiscount(theGarden);

        foreach (Point2D p in theGarden) puzzleInput.Remove(p); 
    }

    Console.WriteLine($"Part 1: The fence price is {part1Answer}.");
    Console.WriteLine($"Part 2: When getting a bulk discount, the fence price now is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}