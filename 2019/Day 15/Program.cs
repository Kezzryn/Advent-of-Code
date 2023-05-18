using AoC_AStar;
using AoC_2019_IntcodeVM;
using System.Drawing;
using System.Diagnostics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string MAPFILE_INPUT = "TheMap.txt";

    const long GO_NORTH = 1;
    const long GO_SOUTH = 2;
    const long GO_WEST = 3;
    const long GO_EAST = 4;

    const long ROBO_RES_WALL = 0;
    const long ROBO_RES_MOVE = 1;
    const long ROBO_RES_OXY = 2;

    const long TILE_EMPTY = 0;
    const long TILE_WALL = 1;
    const long TILE_ROBOT = 2;
    const long TILE_OXY = 3;
    const long TILE_CORRIDOR = 4;
    const long TILE_GHOST = 5;

    //Load the VM
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();
    IntcodeVM arcade = new(puzzleInput);

    //Load the map. (hand generated)
    string[] mapInput = File.ReadAllLines(MAPFILE_INPUT);

    Dictionary<Point, MapSymbols> theMap = new();
    Point robo = new();
    Point oxy = new();
    foreach (Point p in from y in Enumerable.Range(0, mapInput.Length)
                        from x in Enumerable.Range(0, mapInput[0].Length)
                        select new Point(x, y))
    {
        theMap.Add(p, mapInput[p.Y][p.X] == '#' ? MapSymbols.Wall : MapSymbols.Open);
        if (mapInput[p.Y][p.X] == 'O') oxy = p;
        if (mapInput[p.Y][p.X] == 'S') robo = p;
    }

    //Part 1: Find the distance from the start to the oxygen unit. 
    AStar.A_Star(robo, oxy, theMap, out int part1AStar, out _);
    Console.WriteLine($"Part 1 A*: There are {part1AStar} steps from the start {robo} to the oxygen unit at {oxy}.");

    // Part 2: Find the distance from the oxygen unit to the farthest point away on the map.
    Stopwatch sw = Stopwatch.StartNew();

    int part2AStar = 0;
    Point farthest = new(0, 0);
    HashSet<Point> seen = new();    // cull paths we've visited.

    foreach (Point p in theMap.Where(x => x.Value == MapSymbols.Open).Select(x => x.Key).OrderByDescending(x => AStar.TaxiDistance(oxy, x)))
    {
        if (seen.Contains(p)) continue;
        AStar.A_Star(oxy, p, theMap, out int numSteps, out List<Point> finalPath);
        finalPath.ForEach(x => seen.Add(x));

        if (numSteps > part2AStar)
        {
            part2AStar = numSteps;
            farthest = p;
        }
    }
    sw.Stop();
    Console.WriteLine($"Part 2 A*: It takes {part2AStar} minutes for the oxygen to get from the {oxy} oxygen unit to the farthest part of the ship at {farthest}. Found in {sw.ElapsedMilliseconds} ms.");

    // And a thought on how to do that faster.
    sw.Restart();
    Dictionary<Point, int> distMap = new();

    void GetDist(Point cursor, int numSteps = 0)
    {
        distMap.TryAdd(cursor, numSteps);
        foreach(Size step in AStar.Neighbors)
        {
            Point nextStep = cursor + step;

            if(!distMap.ContainsKey(nextStep) && theMap[nextStep] == MapSymbols.Open)
            {
                GetDist(nextStep, numSteps + 1);
            }
        }
    }

    GetDist(oxy);
    sw.Stop();

    var part2Recursive = distMap.Where(x => x.Value == distMap.Max(x => x.Value)).First();

    Console.WriteLine($"Part 2 Recursive: It takes {part2Recursive.Value} minutes for the oxygen to get from the {oxy} oxygen unit to the farthest part of the ship at {part2Recursive.Key}. Found in {sw.ElapsedMilliseconds} ms.");
      

    Console.WriteLine();
    Console.WriteLine("Press any key to drive the robot.");
    Console.ReadKey();

    State currentState = State.Paused_For_Input;
    int part1Answer = 0;
    Dictionary<long, Size> directions = new()
    {
        { GO_NORTH, new( 0,-1) },
        { GO_SOUTH, new( 0, 1) },
        { GO_EAST,  new( 1, 0) },
        { GO_WEST,  new(-1, 0) },
    };

    static void PrintBlock(int x, int y, long value, int steps = -1)
    {
        const int OFFSET_Y = 2;
        if (steps > -1)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"Steps: {steps,-4}");
            if (value == TILE_ROBOT) Console.Write($" Robo: {x}, {y}");
        }
        Console.SetCursorPosition(x, y + OFFSET_Y);
        Console.Write(value switch
        {
            TILE_EMPTY => ' ',
            TILE_WALL => '█',
            TILE_CORRIDOR => '▒',
            TILE_OXY => '∞',
            TILE_ROBOT => 'Θ',
            TILE_GHOST => '#',
            _ => throw new NotImplementedException()
        });
    }

    Console.Clear();
    Console.CursorVisible = false;
    foreach ((int x, int y) in from x in Enumerable.Range(0, theMap.Keys.Max(m => m.X)+1)
                               from y in Enumerable.Range(0, theMap.Keys.Max(m => m.Y)+1)
                               where theMap[new Point(x,y)] == MapSymbols.Wall
                               select (x, y))
    {
        PrintBlock(x, y, TILE_GHOST);
    }

    while (currentState != State.Halted)
    {
        PrintBlock(robo.X, robo.Y, TILE_ROBOT, part1Answer);

        ConsoleKeyInfo readKey = Console.ReadKey();
        long input = readKey.Key switch
        {
            ConsoleKey.R => 99,
            ConsoleKey.RightArrow => GO_EAST,
            ConsoleKey.LeftArrow => GO_WEST,
            ConsoleKey.UpArrow => GO_NORTH,
            ConsoleKey.DownArrow => GO_SOUTH,
            _ => -1
        }; 

        if (input == 99)
        {
            part1Answer = 0;
            continue;
        }
        if (input == -1) break;
        arcade.SetInput(input);
        
        currentState = arcade.Run();

        Point nextStep = robo + directions[input];

        while (arcade.IsOutput())
        {
            arcade.GetOutput(out long result);
            switch (result)
            {
                case ROBO_RES_OXY:
                    PrintBlock(robo.X, robo.Y, TILE_EMPTY);
                    robo = nextStep;
                    oxy = nextStep;
                    PrintBlock(robo.X, robo.Y, TILE_OXY, part1Answer++);
                    break;
                case ROBO_RES_MOVE:
                    PrintBlock(robo.X, robo.Y, robo == oxy ? TILE_OXY : TILE_CORRIDOR);
                    robo = nextStep;
                    PrintBlock(robo.X, robo.Y, TILE_ROBOT, part1Answer++);
                    break;
                case ROBO_RES_WALL:
                    PrintBlock(nextStep.X, nextStep.Y, TILE_WALL);
                    break;
                default:
                    break;
            }
        }
    }

}
catch (Exception e)
{
    Console.WriteLine(e);
}

