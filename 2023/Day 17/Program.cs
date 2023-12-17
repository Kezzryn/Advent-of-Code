using AoC_Utilities;
using System.Collections.Generic;
using System.Drawing;

static int NotAStar(Cursor start, (int X, int Y) dest, Dictionary<(int X, int Y), int> theMap, out List<(int, int)> history, bool doPart2 = false)
{
    history = new();
    List<(Cursor, int)> NextSteps(Cursor cursor, bool doPart2 = false)
    {
        int start = doPart2 ? 4 : 1;
        int maxDist = doPart2 ? 10 : 3;
        List<(Cursor, int)> returnValue = new();
        for (int dist = start; dist <= maxDist; dist++)
        {
            int stepCost = 0;
            Cursor cloneCursor = new(cursor);
            cloneCursor.Step(dist);
            if (!theMap.ContainsKey(cloneCursor.XY)) continue; //we're off the map
            stepCost = Enumerable.Range(1, dist).
                Sum(x => theMap[(cursor.X + (x * cursor.GetDir.X),
                                    cursor.Y + (x * cursor.GetDir.Y))]);

            returnValue.Add((cloneCursor.ReturnCloneTurnLeft(), stepCost));
            returnValue.Add((cloneCursor.ReturnCloneTurnRight(), stepCost));
        }
        return returnValue;
    }

    List<(Cursor, int)> NextStepsPart2(Cursor cursor)
    {
        List<(Cursor, int)> returnValue = new();
        for (int dist = 4; dist <= 10; dist++)
        {
            int stepCost = 0;
            Cursor cloneCursor = new(cursor);
            cloneCursor.Step(dist);
            if (!theMap.ContainsKey(cloneCursor.XY)) continue; //we're off the map
            stepCost = Enumerable.Range(1, dist).
                Sum(x => theMap[(cursor.X + (x * cursor.GetDir.X),
                                    cursor.Y + (x * cursor.GetDir.Y))]);

            returnValue.Add((cloneCursor.ReturnCloneTurnRight(), stepCost));
            returnValue.Add((cloneCursor.ReturnCloneTurnLeft(), stepCost));
            
        }
        return returnValue;
    }

    PriorityQueue<Cursor, int> searchQueue = new();
    HashSet<Cursor> inSearchQueue = new();
    Dictionary<Cursor, (int steps, Cursor? parent)> stepCounter = new()
    {
        { start, (0, null) }
    };

    searchQueue.Enqueue(start, 0);
    inSearchQueue.Add(start);
    int bestSteps = int.MaxValue;

    while (searchQueue.TryDequeue(out Cursor? cursor, out _))
    {
        if (cursor == null) break;
        inSearchQueue.Remove(cursor);

        //Have we have arrived?
        if (cursor.XY == dest)
        {
            if (stepCounter[cursor].steps < bestSteps)
            { 
                bestSteps = stepCounter[cursor].steps;

                history.Clear(); //unroll our history. 
                history.Add(cursor.XY);
                Cursor? p = stepCounter[cursor].parent;

                while (p != null)
                {
                    history.Add(p.XY);
                    p = stepCounter[p].parent;
                }
            }
        }
        else
        {
            foreach ((Cursor nextStep, int stepCost) in NextSteps(cursor, doPart2))
            {
                stepCounter.TryAdd(nextStep, (int.MaxValue, null));
                int t_gScore = stepCounter[cursor].steps + stepCost;

                if (t_gScore < stepCounter[nextStep].steps)
                {
                    stepCounter[nextStep] = (t_gScore, cursor);

                    if (!inSearchQueue.Contains(nextStep))
                    {
                        searchQueue.Enqueue(nextStep, stepCounter[nextStep].steps);
                        inSearchQueue.Add(nextStep);
                    }
                }
            }
        }
    }
    return bestSteps;
} 


try
{
    const bool DO_PART2 = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    Dictionary<(int X, int Y), int> theMap = new();

    int maxX = puzzleInput[0].Length - 1;
    int maxY = puzzleInput.Length - 1;

    foreach (((int X, int Y) A, int value) in from y in Enumerable.Range(0, puzzleInput.Length)
                                              from x in Enumerable.Range(0, puzzleInput[0].Length)
                                              select ((x, y), puzzleInput[y][x] - '0'))
    {
        theMap.Add(A, value);
    }

    Cursor start = new(0,0,1,0);
    (int X, int Y) dest = (maxX,  maxY);
    List<(int X, int Y)> history = new();

    int part1Answer = NotAStar(start, dest, theMap, out history);
    int part2Answer = NotAStar(start, dest, theMap, out history, DO_PART2);

    //for(int y = 0; y <= maxY; y++)
    //{
    //    for (int x = 0; x <= maxX; x++)
    //    {
    //        if (history.Contains((x,y))) Console.ForegroundColor = ConsoleColor.Red;
    //        Console.Write(theMap[(x, y)]);
    //        Console.ResetColor();
    //    }
    //    Console.WriteLine();
    //}
    
    Console.WriteLine($"Part 1: A crucible will lose {part1Answer} heat on the journey.");
    Console.WriteLine($"Part 2: An ultra crucible will lose {part2Answer} getting to its destination.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}