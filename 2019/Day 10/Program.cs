using System.Drawing;

static List<Point> GetLine(Point p0, Point p1)
{
    // Linear interpolation
    // from https://www.redblobgames.com/grids/line-drawing.html

    List<Point> points = new();
    int dx = p1.X - p0.X;
    int dy = p1.Y - p0.Y;
    int N = Math.Max(Math.Abs(dx), Math.Abs(dy));
    double divN = (N == 0) ? 0.0 : 1.0 / N;
    double xstep = dx * divN;
    double ystep = dy * divN;
    double x = p0.X, y = p0.Y;

    for (int step = 0; step <= N; step++, x += xstep, y += ystep)
    {
        Point newPoint = new((int)Math.Round(x), (int)Math.Round(y));
        if (newPoint != p0 && newPoint != p1) points.Add(newPoint);
    }
    return points;
}

static bool IsOnLine(Point start, Point end, Point test)
{
    //https://stackoverflow.com/questions/17692922/check-is-a-point-x-y-is-between-two-points-drawn-on-a-straight-line
    int crossproduct = (test.Y - start.Y) * (end.X - start.X) - (test.X - start.X) * (end.Y - start.Y);

    //compare versus epsilon for floating point values, or != 0 if using integers
    if (Math.Abs(crossproduct) != 0) return false;

    int dotproduct = (test.X - start.X) * (end.X - start.X) + (test.Y - start.Y) * (end.Y - start.Y);
    if (dotproduct < 0) return false;

    int squaredlengthba = (end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y);
    if (dotproduct > squaredlengthba) return false;

    return true;
}

static double GetAngle(Point start, Point end)
{
    double degrees = (Math.Atan2(end.Y - start.Y, end.X - start.X) * 180.0 / Math.PI) + 90;
    if (degrees < 0) degrees += 360;
    return Math.Round(degrees, 3);
}

static int TaxiDistance(Point a,  Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    HashSet<Point> theMap = new();
    foreach (Point asteroid in from y in Enumerable.Range(0, puzzleInput.Length)
                               from x in Enumerable.Range(0, puzzleInput[0].Length)
                               where puzzleInput[y][x] == '#'
                               select new Point(x, y))
    {
        theMap.Add(asteroid);
    }

    Dictionary<Point, List<Point>> visableFrom = new();

    foreach (Point pointFrom in theMap)
    {
        visableFrom.Add(pointFrom, new());
        //Console.WriteLine($"FROM: {pointFrom}");
        foreach (Point pointTo in theMap.Where(x => x != pointFrom))
        {
            //Console.WriteLine($"-- TO {pointTo}"); 
            bool isLOS = true;
            var theLine = GetLine(pointFrom, pointTo);
            foreach (Point potentialCollision in theLine)
            // foreach (Point potentialCollision in GetLine(pointFrom, pointTo))
            {
                if (potentialCollision == pointFrom || potentialCollision == pointTo) continue;
                if (theMap.Contains(potentialCollision))
                {
                    //Console.Write($"---- TEST: {potentialCollision} ");
                    if (IsOnLine(pointFrom, pointTo, potentialCollision))
                    {
                        //  Console.WriteLine("COLLIDE!");
                        isLOS = false;
                        break;
                    }
                    //Console.WriteLine();
               }
            }

            if (isLOS) visableFrom[pointFrom].Add(pointTo);
        }
    }
     
    var part1Answer = visableFrom.OrderByDescending(x => x.Value.Count).FirstOrDefault();

    Point spaceStation = part1Answer.Key;

    Dictionary<double, SortedList<int, Point>> distMap = new();
    foreach (Point p in visableFrom[spaceStation].Where(x => x != spaceStation))
    {
        int dist = TaxiDistance(spaceStation, p);
        double angle = GetAngle(spaceStation, p);

        if (!distMap.TryAdd(angle, new() { { dist, p } }))
        {
            Console.Write($"{p} WARNING hidden by: ");
            foreach (Point pq in distMap[angle].Values) Console.Write($"{pq} ");
            Console.WriteLine();
            distMap[angle].Add(dist, p);
        }
    }

    string part2Answer = "";
    Dictionary<Point, int> destMap = new();
    int asteroidCounter = 0;
    foreach(var kvp in distMap.OrderBy(x => x.Key))
    {   
        if (kvp.Value.Count > 0)
        {
            var first = kvp.Value.First();
            if (visableFrom[spaceStation].Contains(first.Value))
            {
                asteroidCounter++;
                if (asteroidCounter == 200) part2Answer = $"The {asteroidCounter}th rock is at {first.Value} for an ID of {first.Value.X * 100 + first.Value.Y}  ";
                kvp.Value.Remove(first.Key);
                destMap.TryAdd(first.Value, asteroidCounter);
            }
        }
    }

    Console.WriteLine($"Part 1: {part1Answer.Key} is the best asteroid with {part1Answer.Value.Count} sightlines.");
    Console.WriteLine($"Part 2: {part2Answer}");

    Console.WriteLine(); 
    Console.WriteLine();
    
    for(int y = 0; y < puzzleInput.Length; y++)
    {
        for(int x = 0; x < puzzleInput[y].Length;x++)
        {
            Point key = new(x, y);
            if (key == spaceStation)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" @  ");
            }
            else if (destMap.TryGetValue(key, out int value))
            {
                if (value == 200) Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{value,-4:D3}");
            }
            else if (visableFrom[spaceStation].Contains(key))
            {
                //These are a mistake if visable.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" X  ");
            }
            else
            {
                Console.Write(theMap.Contains(key) ? " #  " : "    ");
            }
            Console.ResetColor();
        }
        Console.WriteLine();
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}