using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    //find guard start point. 
    (int x, int y) startPos = (0,0);

    for(int guardY = 0; guardY < puzzleInput.Length; guardY++)
    {
        int guardX = puzzleInput[guardY].IndexOf('^');
        if (guardX == -1) continue;
        startPos.x = guardX;
        startPos.y = guardY;
        break;  
    }

    int maxX = puzzleInput[0].Length;
    int maxY = puzzleInput.Length;

    Cursor guard = new(startPos.x, startPos.y, 0, -1)
    {
        DoMirrorTurns = true
    };
    HashSet<(int X, int Y)> part1Steps = [];
    
    bool isDone = false;
    do
    {
        part1Steps.Add(guard.XY);
        (int nextStepX, int nextStepY) = guard.NextStep();
        if (nextStepX < 0 || nextStepX >= maxX || nextStepY < 0 || nextStepY >= maxY) //next step is off the map 
        {
            isDone = true;
        }
        else
        {
            if (puzzleInput[nextStepY][nextStepX] == '#')
            {
                guard.TurnRight();
            }
            else
            {
                guard.Step();
            }
        }
    }
    while (!isDone);

    int part1Answer = part1Steps.Count;

    // part 2! 
    int part2Answer = 0;

    foreach ((int blockX, int blockY) in part1Steps.Where(x => x != startPos))
    {
        guard = new(startPos.x, startPos.y, 0, -1)
        {
            DoMirrorTurns = true
        };
        HashSet<Cursor> turns = [];
        isDone = false;
        do
        {
            (int nextStepX, int nextStepY) = guard.NextStep();
            if (nextStepX < 0 || nextStepX >= maxX || nextStepY < 0 || nextStepY >= maxY) //next step is off the map 
            {
                isDone = true;
            }
            else
            {
                if (puzzleInput[nextStepY][nextStepX] == '#' || (nextStepX == blockX && nextStepY == blockY))
                {
                    if (turns.Add(guard))
                    {
                        guard.TurnRight();
                    }
                    else
                    {
                        part2Answer++;
                        isDone = true;
                    }
                }
                else
                {
                    guard.Step();
                }
            }
        }
        while (!isDone);
    }

    Console.WriteLine($"Part 1: The guard moves through {part1Answer} spaces before leaving the map.");
    Console.WriteLine($"Part 2: There are {part2Answer} places to place an blocker to create a loop.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}