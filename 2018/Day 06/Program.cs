using System.Drawing;

static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

try
{
    const int DUPE_DEFAULT = -1;
    const int PART_2_REGION_SIZE = 10000;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    Dictionary<Point, int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
            .Select(x => x.Split(", "))
            .Select(x => new Point(int.Parse(x.First()), int.Parse(x.Last())))
            .ToDictionary(x => x, x => 0);

    // determine the corners.
    // Experimentation shows that this encapsulates the area needed for part two.
    Point min = new(puzzleInput.Min(x => x.Key.X), puzzleInput.Min(x => x.Key.Y));
    Point max = new(puzzleInput.Max(x => x.Key.X), puzzleInput.Max(x => x.Key.Y));

    HashSet<Point> infinitePoints = new();

    int part2Answer = 0;
    foreach (Point gridPoint in from x in Enumerable.Range(min.X, max.X - min.X)
                                from y in Enumerable.Range(min.Y, max.Y - min.Y)
                                select new Point(x, y))
    {
        int closestDistance = int.MaxValue;
        int dupeDistance = DUPE_DEFAULT;
        Point closestPoint = new();

        int sumDistance = 0;
        foreach (Point inputPoint in puzzleInput.Keys)
        {
            int distance = TaxiDistance(inputPoint, gridPoint);
            sumDistance += distance;

            if (distance == closestDistance) dupeDistance = distance;
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = inputPoint;
            }
        }

        if (sumDistance < PART_2_REGION_SIZE) part2Answer++;

        if (dupeDistance == DUPE_DEFAULT || closestDistance < dupeDistance)
        {
            puzzleInput[closestPoint]++;
            if (gridPoint.X == min.X || gridPoint.X == max.X
             || gridPoint.Y == min.Y || gridPoint.Y == max.Y)
            {
                infinitePoints.Add(closestPoint);
            }
        }
    }

    int part1Answer = puzzleInput.Where(x => !infinitePoints.Contains(x.Key)).Select(v => v.Value).Max();

    Console.WriteLine($"Part 1: The largest area is {part1Answer}.");
    Console.WriteLine($"Part 2: The region is {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}