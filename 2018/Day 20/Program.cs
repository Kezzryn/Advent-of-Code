using System.Diagnostics;
using System.Drawing;
using AoC_2018_Day_20;

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

static void PrintMap(Dictionary<Point, HashSet<Point>> theMap)
{
    Point max = new(theMap.Keys.Max(x => x.X), theMap.Keys.Max(x => x.Y));
    Point min = new(theMap.Keys.Min(x => x.X), theMap.Keys.Min(x => x.Y));

    char[,] printMap = new char[((max.X - min.X + 1) * 2) + 1, ((max.Y - min.Y + 1) * 2) + 1];
    Size offset = new(printMap.GetUpperBound(0) - ((max.X + 1) * 2) -1, printMap.GetUpperBound(1) - ((max.Y + 1) * 2) - 1);

    //Console.WriteLine($"{printMap.GetUpperBound(0)} {printMap.GetUpperBound(1)}");
    foreach((int x, int y) in from a in Enumerable.Range(0, printMap.GetLength(0))
                              from b in Enumerable.Range(0, printMap.GetLength(1))
                              select (a, b))   
    {
        printMap[x, y] = '#';
    }
     
    foreach(Point p in theMap.Keys)
    {
        Point target = new((p.X + 1) * 2, (p.Y + 1) * 2);
        target += offset;

        //Console.WriteLine(target);
        printMap[target.X, target.Y] = (p.X == 0 && p.Y == 0) ? 'X' : '.';

        foreach (Point p2 in theMap[p])
        {
            Point door = target - (Size)(p - (Size)p2);
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
    
    Dictionary<Point, HashSet<Point>> theMap = new();
    Dictionary<Point, int> theMapDist = new();
    
    int LoadMap(string map, Point cursor, int dist = 0)
    {
        Dictionary<char, Size> direction = new()
        {
            { 'N', new Size( 0, 1) },
            { 'S', new Size( 0,-1) },
            { 'E', new Size( 1, 0) },
            { 'W', new Size(-1, 0) }
        };

        Point startPoint = cursor;
        int startDist = dist;
         
        for(int i = 0; i < map.Length; i++)
        {
            switch (map[i])
            {
                case 'N' or 'S' or 'E' or 'W':
                    Point nextStep = cursor + direction[map[i]];
                    dist++;
                    theMap.TryAdd(nextStep, new());
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
                    theMap.TryAdd(cursor, new());
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
     
    int part1Answer = 0;
    int part2Answer = 0;

    Point farthest = new();

    Stopwatch sw = Stopwatch.StartNew();
    LoadMap(puzzleInput, new(0, 0));
    PrintMap(theMap);

    Console.WriteLine($"Fast answer part 1: {theMapDist.Max(x => x.Value)}");
    Console.WriteLine($"Fast answer part 2: {theMapDist.Where(x => x.Value >= PART_TWO_THRESHOLD).Count()}");

    Console.WriteLine($"Map load and search took {sw.ElapsedMilliseconds} ms.");
    sw.Stop();

    sw.Restart();
    foreach (Point dest in theMap.Keys)
    {
        AStar.A_Star(new(0, 0), dest, theMap, out int numSteps, out _);

        if (numSteps >= part1Answer)
        {
            part1Answer = numSteps;
            farthest = dest;
        }
        if (numSteps >= PART_TWO_THRESHOLD) part2Answer++;
    }
    sw.Stop();

    Console.WriteLine();
    Console.WriteLine($"A* took {sw.ElapsedMilliseconds} ms to search {theMap.Count} points.");
    Console.WriteLine($"Part 1: The room at {farthest} is {part1Answer} doors away.");
    Console.WriteLine($"Part 2: There are {part2Answer} rooms that are {PART_TWO_THRESHOLD} doors away or more.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}