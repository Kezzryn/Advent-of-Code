using System.Drawing;
using System.Numerics;

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

    Dictionary<Point, int> memoryMap = new();
    Point cursor = new(0,0);
    Complex direction = new(1, 0);

    int GetNeighborValue(Point pos)
    {
        var neighbours = from x in Enumerable.Range(pos.X - 1, 3)
                         from y in Enumerable.Range(pos.Y - 1, 3)
                         where !(x == pos.X && y == pos.Y)
                         select new Point(x, y);
        int rv = neighbours.Select(x => memoryMap.GetValueOrDefault(x, 0)).Sum();

        return rv == 0 ? 1 : rv; // return 1 as a minimum. Otherwise we have to fiddle with priming the memoryMap
    }

    int spiralLevel = 0;    // up by 2s as per the p
    int spiralSteps = 0;    // number of steps in the current spiral. on 0, we bump out to the next level. 
    do
    {
        int value = GetNeighborValue(cursor);
        if (value >= puzzleInput) part2Answer = value;
        if(!memoryMap.TryAdd(cursor, value)) break;

        if (spiralSteps == 0)
        {
            cursor += new Size((int)direction.Real, (int)direction.Imaginary);
            direction *= Complex.ImaginaryOne;
            spiralLevel += 2;
            spiralSteps = spiralLevel * 4;
        }
        else
        {
            if ((spiralSteps % spiralLevel) == 0) direction *= Complex.ImaginaryOne;
            cursor += new Size((int)direction.Real, (int)direction.Imaginary);
        }

        spiralSteps--;
    } while (part2Answer == 0);

    Console.WriteLine($"Part 2: The first value written that is larger than the input is {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

