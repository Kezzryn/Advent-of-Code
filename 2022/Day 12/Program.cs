using AoC_2022_Day_12;
using System.Drawing;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    Map theMap = new(PUZZLE_INPUT);

    int numSteps = 0;
    theMap.A_Star(theMap.DefaultStart(), theMap.DefaultEnd(), out numSteps);

    Console.WriteLine($"Part 1: The shortest distance from {theMap.DefaultStart()} to {theMap.DefaultEnd()} is {numSteps}");
    theMap.DisplayMap();

    Console.WriteLine();
    Console.WriteLine();

    int shortestPath = int.MaxValue;
    List<Point> mapPath = new();

    for (int x = theMap.MapMin.X; x <= theMap.MapMax.X; x++)
    {
        for (int y = theMap.MapMin.Y; y <= theMap.MapMax.Y; y++)
        {
            if (theMap.HeightAt(new Point(x, y)) == 1)
            {
                if (theMap.A_Star(new Point(x, y), theMap.DefaultEnd(), out numSteps, shortestPath))
                {
                    if (numSteps < shortestPath)
                    {
                        mapPath = theMap.finalPath.ToList();
                        shortestPath = numSteps;
                    }
                }
            }
        }
    }
    Console.WriteLine($"Part 2: The shortest distance of any elevation \"a\" map point to the end is {shortestPath}");
    theMap.DisplayMap(mapPath);
}

catch (Exception e)
{
    Console.WriteLine(e);
}
