using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int puzzleInput = int.Parse(File.ReadAllText(PUZZLE_INPUT));

    // this is the lower right corner of the spiral. 
    int upperBase = (int)Math.Sqrt(puzzleInput) + 1;
    int upperLimit = (int)Math.Pow(upperBase, 2);

    // each "side" works out to be the previous base.
    // divide by 2 because we want to get close to the midpoint.
    int segment = (int)Math.Sqrt(puzzleInput) / 2;

    // the distance to the midpoint. 
    // ((upperBase - 1) / 2)  

    // distance to the midpoint from the puzzle data. 
    // (upperLimit - puzzleInput + segment) % segment;

    int part1Answer = ((upperBase - 1) / 2) + (upperLimit - puzzleInput + segment) % segment;

    Console.WriteLine($"Part 1: The distance from the start to {puzzleInput} is {part1Answer}.");

    int part2Answer = 0;

    Dictionary<Point2D, int> memoryMap = [];
    Cursor cursor = new(0,0,1,0);

    int spiralLevel = 0;    // up by 2s as per the p
    int spiralSteps = 0;    // number of steps in the current spiral. on 0, we bump out to the next level. 
    do
    {
        int value = int.Max(1, cursor.XYAsPoint2D.GetAllNeighbors().Sum(x => memoryMap.GetValueOrDefault(x, 0)));
        if (value >= puzzleInput) part2Answer = value;
        if(!memoryMap.TryAdd(cursor.XYAsPoint2D, value)) break;

        if (spiralSteps == 0)
        {
            cursor.Step();
            cursor.TurnLeft();
            spiralLevel += 2;
            spiralSteps = spiralLevel * 4;
        }
        else
        {
            if ((spiralSteps % spiralLevel) == 0) cursor.TurnLeft();
            cursor.Step();
        }

        spiralSteps--;
    } while (part2Answer == 0);

    Console.WriteLine($"Part 2: The first value written that is larger than the input is {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

