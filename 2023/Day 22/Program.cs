global using Line3D = (int X1, int Y1, int Z1, int X2, int Y2, int Z2);

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    // Note that the first XYZ is <= second XYZ 
    List<Line3D> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
            .Select(x => x.Replace('~', ',')
                .Split(',')
                .Select(int.Parse)
                .ToArray())
            .Select(s => (s[0], s[1], s[2], s[3], s[4], s[5]))
            .ToList();

    static List<Line3D> SettleBlocks(IEnumerable<Line3D> lineList, out int numMoved)
    {
        numMoved = 0;
        List<Line3D> returnList = [];

        foreach (Line3D line in lineList.OrderBy(x => x.Z1))
        {
            int newFloor = returnList.Where(w => w.DoesCollideXY(line)).DefaultIfEmpty().Max(m => m.Z2) + 1;
            
            if(line.Z1 != newFloor) numMoved++;  
            returnList.Add(line.DropLine(line.Z1 - newFloor));
        }
        return returnList;
    }

    static bool CanDisintegrate(Line3D currentBrick, List<Line3D> theLines)
    {
        IEnumerable<Line3D> otherBricks = theLines.Where(w => w != currentBrick);
        IEnumerable<Line3D> supportingBricks = otherBricks.Where(w => currentBrick.IsSupporting(w));

        bool returnValue = true;
        foreach (Line3D line in supportingBricks)
        {
            int numSupportedBy = otherBricks.Where(w => line.IsSupportedBy(w)).Count();
            if (numSupportedBy == 0) returnValue = false;
        }

        return returnValue;
    }

    List<Line3D> settledInput = SettleBlocks(puzzleInput, out _);
    int part1Answer = settledInput.Count(c => CanDisintegrate(c, settledInput));

    int part2Answer = 0;
    foreach (Line3D line in settledInput)
    {
        SettleBlocks(settledInput.Where(w => w != line), out int numMoved);
        part2Answer += numMoved;
    }
        
    Console.WriteLine($"Part 1: There are {part1Answer} bricks that can be safely disintegrated.");
    Console.WriteLine($"Part 2: The sum of the chain reactions is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

internal static class MyExtensions
{
    private static bool TestRange(int A1, int A2, int B1, int B2) => A1 <= B2 && A2 >= B1;
    
    public static Line3D DropLine(this Line3D A, int num = 1)
    {
        return (A.X1, A.Y1, A.Z1 - num, A.X2, A.Y2, A.Z2 - num);
    }

    public static bool DoesCollideXY(this Line3D A, Line3D B)
    {
        return TestRange(A.X1, A.X2, B.X1, B.X2)
            && TestRange(A.Y1, A.Y2, B.Y1, B.Y2);
    }

    public static bool IsSupporting(this Line3D A, Line3D B)
    {
        // Is A supporting B?
        return TestRange(A.X1, A.X2, B.X1, B.X2)
            && TestRange(A.Y1, A.Y2, B.Y1, B.Y2)
            && A.Z2 + 1 == B.Z1;
    }
    public static bool IsSupportedBy(this Line3D A, Line3D B)
    {
        return TestRange(A.X1, A.X2, B.X1, B.X2)
            && TestRange(A.Y1, A.Y2, B.Y1, B.Y2)
            && A.Z1 == B.Z2 + 1;
    }
}