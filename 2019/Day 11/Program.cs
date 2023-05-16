using AoC_2019_IntcodeVM;
using System.Numerics;

try
{
    const bool DO_PART_TWO = false;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();
    IntcodeVM paintBot = new(puzzleInput);

    Complex direction = new(0, 1);
    Complex cursor = new(0,0);

    Dictionary<Complex, long> theMap = new()
    {
        { cursor, DO_PART_TWO ? 1 : 0 }
    };

    HashSet<Complex> path = new();

    bool isDone = false;
    while(!isDone)
    {
        if(paintBot.IsOutput())
        {
            paintBot.GetOutput(out long color);
            paintBot.GetOutput(out long turn);

            if (!theMap.TryAdd(cursor, color)) theMap[cursor] = color;
            path.Add(cursor);

            direction *= turn == 0 ? Complex.ImaginaryOne : -Complex.ImaginaryOne;
            cursor += direction;
        }

        theMap.TryGetValue(cursor, out long value);

        paintBot.SetInput(value);
        State currentState = paintBot.Run();

        if (currentState == State.Halted) isDone = true;
    }

    int part1Answer = path.Count;

    Console.WriteLine($"Part 1: The number of squares painted is: {(DO_PART_TWO ? "PART TWO ACTIVE" : part1Answer)}");
    Console.WriteLine($"Part 2: The hull registration code is:");
    Console.WriteLine();

    Complex min = new(theMap.Keys.Min(x => x.Real), theMap.Keys.Min(x => x.Imaginary));
    Complex max = new(theMap.Keys.Max(x => x.Real), theMap.Keys.Max(x => x.Imaginary));

    for(int y = (int)max.Imaginary; y >=  min.Imaginary; y--)
    {
        for (int x = (int)min.Real; x <= max.Real; x++)
        {
            theMap.TryGetValue(new(x, y), out long value);
            Console.Write(value == 1 ? '#' : ' ');
        }
        Console.WriteLine();
    }
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}