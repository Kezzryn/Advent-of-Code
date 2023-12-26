global using Point2D = (int X, int Y);

static Point2D AddPoint2D(Point2D A, Point2D B) => (A.X + B.X, A.Y + B.Y);

static int CountSteps(string[] theMap, Point2D start, int startSteps, int maxSteps, bool showMap = false)
{
    // Simple brute force step counter for the map, since there's no other (easy) way to check for occulusions or unreachable spaces
    if (startSteps > maxSteps) return 0;  // sanity check.

    int maxX = theMap[0].Length - 1;
    int maxY = theMap.Length - 1;

    Dictionary<Point2D, int> stepCounter = [];  //keeps a count of steps from start to this point.

    List<Point2D> neighbors = [(1, 0), (-1, 0), (0, 1), (0, -1)];
    PriorityQueue<Point2D, int> queue = new();
    queue.Enqueue(start, startSteps);

    while (queue.TryDequeue(out Point2D current, out int steps))
    {
        if (!stepCounter.TryAdd(current, steps)) continue;
        if (steps + 1 > maxSteps) continue;

        foreach (Point2D p in neighbors)
        {
            Point2D nextStep = AddPoint2D(p, current);
            if (nextStep.X > maxX || nextStep.Y > maxY || nextStep.X < 0 || nextStep.Y < 0) continue;
            if (theMap[nextStep.Y][nextStep.X] == '#') continue;
            if (stepCounter.ContainsKey(nextStep)) continue;
            queue.Enqueue(nextStep, steps + 1);
        }
    }

    //optional map print
    if (showMap && stepCounter.Count != 0)
    {
        for (int y = 0; y < theMap.Length; y++)
        {
            for (int x = 0; x < theMap[y].Length; x++)
            {
                char v = theMap[y][x];
                if (stepCounter.TryGetValue((x, y), out int value))
                {
                    bool colorPoint = int.IsEvenInteger(maxSteps) ? int.IsEvenInteger(value) : int.IsOddInteger(value);
                    if (colorPoint)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        v = v != 'S' ? 'X' : 'S';
                    }
                }
                Console.Write(v);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }

    return (int.IsEvenInteger(maxSteps))
         ? stepCounter.Where(x => int.IsEvenInteger(x.Value)).Count()
         : stepCounter.Where(x => int.IsOddInteger(x.Value)).Count();
}

static long CalcSteps(string[] theMap, int maxSteps)
{
    // the maps are square and the offset is centered.
    int offsetZB = theMap.Select((x, i) => (i, x.IndexOf('S'))).Where(w => w.Item2 != -1).SingleOrDefault((-1, -1)).Item1;
    int offset = offsetZB + 1;

    int boardSide = theMap.Length;  
    int boardSideZB = boardSide - 1; 
    
    Dictionary<Compass, Point2D> entryPointsNEWS = new()
    {
        { Compass.North, (offsetZB, 0) },          { Compass.West, (0, offsetZB) },
        { Compass.East, (boardSideZB, offsetZB) }, { Compass.South, (offsetZB, boardSideZB) }
    };

    Dictionary<Compass, Point2D> entryPointsDiag = new()
    {
        {Compass.NorthWest, (0, 0) },           { Compass.NorthEast, (boardSideZB, 0) },
        {Compass.SouthWest, (0, boardSideZB) }, { Compass.SouthEast, (boardSideZB, boardSideZB) }
    };

    //we don't leave the current map, we cannot do anything fancy.
    if (maxSteps / boardSide <= 0) return CountSteps(theMap, (boardSide / 2, boardSide / 2), 0, maxSteps);

    //Setup a little cache functionality here.
    Dictionary<(Compass, int stepsRemaining), long> stepDict = [];

    int FULL_ODD_CENTER = boardSide * 3;    // 262 is the minimum to fill the board. Do this larger to ensure full coverage.
    int FULL_EVEN_CENTER = boardSide * 4;
    
    //Artifical data population. Flood the map for even and odd steps.
    stepDict.Add((Compass.Center, FULL_EVEN_CENTER), CountSteps(theMap, (0, 0), 0, FULL_EVEN_CENTER));
    stepDict.Add((Compass.Center, FULL_ODD_CENTER), CountSteps(theMap, (0, 0), 0, FULL_ODD_CENTER));

    long GetCacheSteps(Compass compassKey, Point2D start, int stepsTaken)
    {
        int stepRemaining = maxSteps - stepsTaken;
        if (!stepDict.TryGetValue((compassKey, stepRemaining), out long numSteps))
        {
            numSteps = CountSteps(theMap, start, stepsTaken, maxSteps);
            stepDict[(compassKey, stepRemaining)] = numSteps;
        }
        return numSteps;
    }

    //Start with the center tile, which has the same polarity as maxSteps.
    long returnValue = int.IsEvenInteger(maxSteps)
        ? stepDict[(Compass.Center, FULL_EVEN_CENTER)]
        : stepDict[(Compass.Center, FULL_ODD_CENTER)];

    int totalNumBoards = (maxSteps + offset + boardSide - 1) / boardSide;   // round up integer division. 

    //col = 0 is the default returnValue
    for (int col = 1; col < totalNumBoards; col++)
    {
        //how many steps did it take to get to this point?
        int stepsTakenCol = offset + (boardSide * (col - 1));
        if (stepsTakenCol > maxSteps) continue; // The math is generous.

        if ((maxSteps - stepsTakenCol) > boardSide * 2)
        {
            //Flip the polarity step by step for each block we have.
            returnValue += 4 * (int.IsOddInteger(maxSteps) == int.IsOddInteger(col)
                ? stepDict[(Compass.Center, FULL_EVEN_CENTER)]
                : stepDict[(Compass.Center, FULL_ODD_CENTER)]);
        }
        else
        {
            returnValue += entryPointsNEWS.Sum(x => GetCacheSteps(x.Key, x.Value, stepsTakenCol));
        }

        int rowBoards = (maxSteps - stepsTakenCol + boardSide - 1) / boardSide; // integer round up division.
        if (rowBoards <= 0) continue;   
        for (int row = rowBoards; row > 0; row--)
        {
            int stepsTakenRow = stepsTakenCol + offset + (boardSide * (row - 1));
            if (stepsTakenRow > maxSteps) continue;

            if ((maxSteps - stepsTakenRow) > boardSide * 2)
            {
                //Calc the polarity of the line. Half are even, half are odd.
                returnValue += 4 * (row / 2) * (stepDict[(Compass.Center, FULL_ODD_CENTER)] + stepDict[(Compass.Center, FULL_EVEN_CENTER)]);

                //If the row doesn't divide in two, return an extra that matches the polarity of the center column.
                if (int.IsOddInteger(row))
                {
                    returnValue += 4 * (int.IsOddInteger(col)
                            ? stepDict[(Compass.Center, FULL_ODD_CENTER)]
                            : stepDict[(Compass.Center, FULL_EVEN_CENTER)]);
                }
                break;
            }
            else
            {
                returnValue += entryPointsDiag.Sum(s => GetCacheSteps(s.Key, s.Value, stepsTakenRow));
            }
        }
    }

    return returnValue;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    const int PART_ONE_MAX_STEPS = 64;
    const int PART_TWO_MAX_STEPS = 26501365;

    long part1Answer = CalcSteps(puzzleInput, PART_ONE_MAX_STEPS);
    long part2Answer = CalcSteps(puzzleInput, PART_TWO_MAX_STEPS);

    Console.WriteLine($"Part 1: There are {part1Answer} places that can be reached with {PART_ONE_MAX_STEPS} steps.");
    Console.WriteLine($"Part 2: {PART_TWO_MAX_STEPS} can reach {part2Answer} places.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum Compass
{
    NorthWest,
    NorthEast,
    North,
    East,
    South,
    West,
    SouthEast,
    SouthWest,
    Center
};