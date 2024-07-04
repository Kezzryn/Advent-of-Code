using AoC_2016_Day_13;
using BKH.Geometry;

try
{
    const int PART2_STEPS = 50;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).FirstOrDefault();

    Map theMap = new(puzzleInput, new(1, 1), new(31, 39));

    theMap.A_Star();
    
    Console.WriteLine($"Part 1: The shortest distance from {theMap.StartPosition} to {theMap.EndPosition} is {theMap.NumSteps}.");
    theMap.DisplayMap();

    int part2Answer = 0;
    theMap.MaxSteps = PART2_STEPS;

    for (int x = 0; x <= PART2_STEPS + theMap.StartPosition.X; x++)
    {
        for (int y = 0; y <= PART2_STEPS + theMap.StartPosition.Y; y++)
        {
            if (theMap.IsValidMapPoint(x, y)) 
            {
                theMap.EndPosition = new Point2D(x, y);
                if (theMap.A_Star()) part2Answer++;
            }
        }
    }

    Console.WriteLine($"Part 2: The number of reachable positions within {PART2_STEPS} from the {theMap.StartPosition} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}