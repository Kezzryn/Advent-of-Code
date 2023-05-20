using AoC_2019_IntcodeVM;
using System.Drawing;

List<Size> Neighbors = new()
        {
            new Size(0,-1),
            new Size(-1,0),
            new Size(1,0),
            new Size(0,1)
        };

Dictionary<ConsoleKey, string> part2Instructions = new()
{
    { ConsoleKey.M, "A,B,A,B,A,C,A,C,B,C" },
    { ConsoleKey.A, "R,6,L,10,R,10,R,10" },
    { ConsoleKey.B, "L,10,L,12,R,10" },
    { ConsoleKey.C, "R,6,L,12,L,10" }
};

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    //Load the VM
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();
    IntcodeVM aftbot = new(puzzleInput);

    //********* PART ONE
    Dictionary<Point, MapSymbols> theMap = new();

    int xLoad = -1;
    int yLoad = 0;

    Console.Clear();
    Console.CursorVisible = false;
     
    while (aftbot.Run() != State.Halted)
    {
        while (aftbot.GetOutput(out char output))
        {
            xLoad++;
            if (output == '\n')
            {
                xLoad = -1;
                yLoad++;
            }
            else
            {
                theMap.Add(new(xLoad, yLoad), output == '.' ? MapSymbols.Space : MapSymbols.Corridor);
            }
            Console.Write(output);
        }
    }

    // There has to be a better way to do this. 
    int part1Answer = 0;
    foreach((Point k, MapSymbols _) in theMap.Where(x => x.Value == MapSymbols.Corridor))
    {
        int count = 0;
        foreach(Point neighbor in Neighbors.Select(x => k + x))
        {
            if (theMap.TryGetValue(neighbor, out MapSymbols mapSymbol) && mapSymbol == MapSymbols.Corridor) count++;
        }
        if (count == 4) part1Answer += k.X * k.Y;
    }

    Console.WriteLine($"Part 1: The sum of the alignment parameters for the scaffold intersections is {part1Answer}.");
    Console.WriteLine();
    Console.WriteLine("Calibration complete. Press any key to activate the robot.");
    _ = Console.ReadKey();

    //********* PART TWO 
    Console.Clear();
    aftbot.Reset(puzzleInput);
    aftbot.SetMemory(0, 2);

    State currentState = State.Paused_For_Input;
    bool isMap = false;
    int numMapLines = 0;

    while (currentState != State.Halted)
    {
        currentState = aftbot.Run();

        if(currentState == State.Paused_For_Output)
        {
            while (aftbot.GetOutput(out char output))
            {
                if (!isMap && output == '.')
                {
                    isMap = true;
                    Console.SetCursorPosition(0, 0);
                }
                if (isMap && output == '\n') numMapLines++;
                Console.Write(output);

                if (numMapLines == 48)
                {
                    Console.WriteLine("Press any key to continue...");
                    _ = Console.ReadKey();
                    numMapLines = 0;
                    isMap = false;
                }
            }
        }

        if (currentState == State.Paused_For_Input)
        {
            ConsoleKeyInfo keyPress = Console.ReadKey();
            switch (keyPress.Key)
            {
                case ConsoleKey.M:
                case ConsoleKey.A:
                case ConsoleKey.B:
                case ConsoleKey.C:
                    Console.Write("\b ");
                    Console.WriteLine(part2Instructions[keyPress.Key]);
                    aftbot.SetInput(part2Instructions[keyPress.Key]);
                    break;
                case ConsoleKey.Y:
                case ConsoleKey.N:
                    Console.Write("\b ");
                    Console.WriteLine(keyPress.Key);
                    aftbot.SetInput(keyPress.Key == ConsoleKey.Y ? "y\n" : "n\n");
                    Console.Clear();
                    break;
                default:
                    break;
            }
        }
    }
    Console.Clear();
    aftbot.GetOutput(out long part2Answer);
    Console.WriteLine($"Part 2: The cleaning bot picked up {part2Answer} units of space dust on its travels.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum MapSymbols
{
    Space = ' ',
    Corridor = '▒',
    Robot = 'Θ'
}