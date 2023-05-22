using AoC_Map;
using System.Drawing;

static List<Point> HashToPointList(long pointHash)
{
    List<Point> returnValue = new();
    // pull byte pairs out of the long. 
    // assumption there are up to 4 (x,y) bytes
    // assumption#2: 0,0 values can to be discarded.

    byte[] byteArray = BitConverter.GetBytes(pointHash);
    for (int i = 0; i < byteArray.Length; i += 2)
    {
        if (byteArray[i] == 0 && byteArray[i + 1] == 0) continue; // skip 0,0 
        returnValue.Add(new Point(byteArray[i], byteArray[i + 1]));
    }

    return returnValue;
}
static long ListToPointHash(List<Point> pointList)
{
    long returnValue = 0;
    
    for (int i = 0; i < pointList.Count; i++)
    {
        byte x = (byte)pointList[i].X;
        byte y = (byte)pointList[i].Y;

        returnValue |= (long)y << ((i * 16) + 8);
        returnValue |= (long)x << i * 16;
    }
    return returnValue;
}
static long UpdatePointHash(Point point, long hash, int position)
{
    int offset = position * 16;

    long returnValue = hash & ~((long)ushort.MaxValue << offset);

    returnValue |= (long)point.Y << (offset + 8);
    returnValue |= (long)point.X << offset;

    return returnValue;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Map theMap = new();

    Dictionary<int, Point> theKeys = new();

    List<Point> startPoints = new();

    int allKeys = 0;

    foreach (Point p in from y in Enumerable.Range(0, puzzleInput.Length)
                        from x in Enumerable.Range(0, puzzleInput[0].Length)
                        select new Point(x, y))
    {
        char mapValue = puzzleInput[p.Y][p.X];
        switch (mapValue)
        {
            case '#':
                theMap.SetNode(p, MapSymbols.Wall);
                break;
            case '.':
                theMap.SetNode(p, MapSymbols.Open);
                break;
            case '@':
                theMap.SetNode(p, MapSymbols.Open);
                startPoints.Add(p);
                break;
            default:
                if (mapValue >= 'a' && mapValue <= 'z')
                {
                    theMap.SetNode(p, MapSymbols.Open);
                    theKeys.Add(1 << (mapValue - 96), p);
                    allKeys |= 1 << (mapValue - 96);
                }
                else if (mapValue >= 'A' && mapValue <= 'Z')
                {
                    theMap.SetNode(p, MapSymbols.Door);
                    theMap.AddDoor(p, 1 << (mapValue - 64));
                }
                else
                {
                    throw new Exception($"Unknown Mapsymbol: {mapValue}");
                }
                break;
        }
    }

    int DoSearch()
    {
        PriorityQueue<(long, int), int> queue = new();
        Dictionary<(long, int), int> bestState = new();

        long hashPosition = ListToPointHash(startPoints);
        queue.Enqueue((hashPosition, 0), 0);

        int returnValue = int.MaxValue;

        while (queue.TryDequeue(out (long posHash, int heldKeys) current, out int currentNumSteps))
        {
            if (!bestState.TryAdd(current, currentNumSteps))
            {
                if (currentNumSteps >= bestState[current]) continue;
            }

            if (current.heldKeys == allKeys)
            {
                if (currentNumSteps < returnValue) returnValue = currentNumSteps;
                continue;
            }

            foreach ((int keyID, Point keyLoc) in theKeys.Where(x => (x.Key & current.heldKeys) == 0))
            {
                List<Point> currentPoints = HashToPointList(current.posHash);
                for (int i = 0; i < currentPoints.Count; i++) 
                {
                    if (theMap.TestPath(currentPoints[i], keyLoc, current.heldKeys, out int numSteps))
                    {
                        (long, int) newKey = (UpdatePointHash(keyLoc, current.posHash, i), current.heldKeys | keyID);
                        int newNumSteps = currentNumSteps + numSteps;

                        if (bestState.TryGetValue(newKey, out int bestNumSteps))
                        {
                            if (bestNumSteps > newNumSteps) queue.Enqueue(newKey, newNumSteps);
                        }
                        else
                        {
                            queue.Enqueue(newKey, newNumSteps);
                        }
                    }
                }
            }
        }
        return returnValue;
    }

    int part1Answer = DoSearch();

    theMap.ClearCache();
    Point startPoint = startPoints.First();
    startPoints.Clear();
    theMap.SetNode(startPoint, MapSymbols.Wall);

    foreach (Size s in Map.Neighbors) theMap.SetNode(startPoint + s, MapSymbols.Wall);
    foreach (Size s in Map.DiagonalNeighbors) startPoints.Add(startPoint + s);

    //theMap.PrintMap();  

    int part2Answer = DoSearch();

    Console.WriteLine($"Part 1: The minimum number of steps to collect all the keys is {part1Answer}.");
    Console.WriteLine($"Part 2: Deploying bots, the minimum number of steps to collect all the keys is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
