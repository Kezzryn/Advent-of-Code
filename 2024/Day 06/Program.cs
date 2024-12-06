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
    HashSet<(int X, int Y)> steps = [];
    

    bool isDone = false;
    do
    {
        steps.Add(guard.XY);
        (int x, int y) = guard.NextStep();
        if (x < 0 || x >= maxX || y < 0 || y >= maxY)
        {
            //next step is off the map 
            isDone = true;
        }
        else
        {
            if (puzzleInput[y][x] == '#') guard.TurnRight();
            guard.Step();
        }
    }
    while (!isDone);

    int part1Answer = steps.Count;

    // part 2! 
    int part2Answer = 0;

    foreach ((int X, int Y) blockPath in steps)
    {
        guard = new(startPos.x, startPos.y, 0, -1)
        {
            DoMirrorTurns = true
        };
        HashSet<Cursor> turns = [];
        bool isLoop = false;
        isDone = false;
        do
        {
            (int x, int y) = guard.NextStep();
            if (x < 0 || x >= maxX || y < 0 || y >= maxY)
            {
                //next step is off the map 
                isLoop = false;
                isDone = true;
            }
            else
            {
                if (puzzleInput[y][x] == '#' || (x == blockPath.X && y == blockPath.Y))
                {
                    if (turns.Add(guard))
                    {
                        guard.TurnRight();
                    }
                    else
                    {
                        isLoop = true;
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
        if (isLoop) part2Answer++;
    }

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}