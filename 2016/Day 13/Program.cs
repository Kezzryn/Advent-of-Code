using AoC_2016_Day_13;
using System.Drawing;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART2_STEPS = 50;
    int puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).FirstOrDefault();

    Map theMap = new(puzzleInput);
    theMap.A_Star(theMap.DefaultStart(), theMap.DefaultEnd(), out int numSteps);
    
    Console.WriteLine($"Part 1: The shortest distance from {theMap.DefaultStart()} to {theMap.DefaultEnd()} is {numSteps}.");
    theMap.DisplayMap();

    int part2Answer = 0;
    
    for (int x = 0; x <= PART2_STEPS + theMap.DefaultStart().X; x++)
    {
        for (int y = 0; y <= PART2_STEPS + theMap.DefaultStart().Y; y++)
        {
            if (theMap.GenMapPoint(x, y) && theMap.A_Star(theMap.DefaultStart(), new Point(x, y), out numSteps, PART2_STEPS)) part2Answer++;
        }
    }

    Console.WriteLine($"Part 2: The number of reachable positions within {PART2_STEPS} from the {theMap.DefaultStart()} is {part2Answer}.");
}

catch (Exception e)
{
    Console.WriteLine(e);
}