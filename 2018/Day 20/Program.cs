using BKH.Geometry;

static int FindMatchingBrace(string input, int startPos)
{
    int matchCount = 0;

    for (int i = startPos; i < input.Length; i++)
    {
        if (input[i] == '(') { matchCount++; }
        if (input[i] == ')') { matchCount--; }
        if (matchCount == 0) return i;
    }
    
    return -1;
}

static void PrintMap(Dictionary<Point2D, HashSet<Point2D>> theMap)
{
    Point2D max = new(theMap.Keys.Max(x => x.X), theMap.Keys.Max(x => x.Y));
    Point2D min = new(theMap.Keys.Min(x => x.X), theMap.Keys.Min(x => x.Y));

    char[,] printMap = new char[((max.X - min.X + 1) * 2) + 1, ((max.Y - min.Y + 1) * 2) + 1];
    Point2D offset = new(printMap.GetUpperBound(0) - ((max.X + 1) * 2) -1, printMap.GetUpperBound(1) - ((max.Y + 1) * 2) - 1);

    //Console.WriteLine($"{printMap.GetUpperBound(0)} {printMap.GetUpperBound(1)}");
    foreach((int x, int y) in from a in Enumerable.Range(0, printMap.GetLength(0))
                              from b in Enumerable.Range(0, printMap.GetLength(1))
                              select (a, b))   
    {
        printMap[x, y] = '#';
    }
     
    foreach(Point2D p in theMap.Keys)
    {
        Point2D target = new((p.X + 1) * 2, (p.Y + 1) * 2);
        target += offset;

        //Console.WriteLine(target);
        printMap[target.X, target.Y] = (p.X == 0 && p.Y == 0) ? 'X' : '.';

        foreach (Point2D p2 in theMap[p])
        {
            Point2D door = target - (p - p2);
            //Console.WriteLine($"{p} -> {target} + {offset} -> {door}");
            printMap[door.X, door.Y] = door.X == target.X ? '-' : '|';
        }
    }
    
    for(int y = printMap.GetUpperBound(1); y >= 0; y--)
    {
        for(int x = 0; x <= printMap.GetUpperBound(0); x++)
        {
            Console.Write(printMap[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART_TWO_THRESHOLD = 1000;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    
    Dictionary<Point2D, HashSet<Point2D>> theMap = new();
    Dictionary<Point2D, int> theMapDist = new();
    
    int LoadMap(string map, Point2D cursor, int dist = 0)
    {
        Dictionary<char, Point2D> direction = new()
        {
            { 'N', new Point2D( 0, 1) },
            { 'S', new Point2D( 0,-1) },
            { 'E', new Point2D( 1, 0) },
            { 'W', new Point2D(-1, 0) }
        };

        Point2D startPoint = cursor;
        int startDist = dist;
         
        for(int i = 0; i < map.Length; i++)
        {
            switch (map[i])
            {
                case 'N' or 'S' or 'E' or 'W':
                    Point2D nextStep = cursor + direction[map[i]];
                    dist++;
                    theMap.TryAdd(nextStep, []);
                    theMapDist.TryAdd(nextStep, dist);

                    theMap[cursor].Add(nextStep);
                    theMap[nextStep].Add(cursor);

                    cursor = nextStep;
                    break;
                case '(':
                    int nextBrace = FindMatchingBrace(map, i);
                    i += LoadMap(map[(i+1)..nextBrace], cursor, dist);
                    break;
                case '|':
                    i += LoadMap(map[(i + 1)..], startPoint, startDist);
                    break;
                case '^':
                    theMap.TryAdd(cursor, []);
                    theMapDist.TryAdd(cursor, dist);
                    break;
                case '$' or ')':
                    // do nothing.
                    break;
                default:
                    throw new NotImplementedException($"unknown map char at {i} code {(int)map[i]} value {map[i]}");
            }
        }
        return map.Length;
    }
     
    //int part1Answer = 0;
    //int part2Answer = 0;

    Point2D farthest = new();

    LoadMap(puzzleInput, new(0, 0));
    //PrintMap(theMap);

    //Loading the map and counting steps as it's loaded is mangatudes of times faster than doing A* searches across the map.
    Console.WriteLine($"Part 1: {theMapDist.Max(x => x.Value)}");
    Console.WriteLine($"Part 2: {theMapDist.Where(x => x.Value >= PART_TWO_THRESHOLD).Count()}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}