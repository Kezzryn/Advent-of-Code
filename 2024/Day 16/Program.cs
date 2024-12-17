using AoC_2024_Dat_16;
using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    ReindeerMazes reindeerMazes = new(puzzleInput);
    reindeerMazes.A_Star();
    
    int part1Answer = reindeerMazes.NumSteps;

    List<Point2D> finalPath = new(reindeerMazes.FinalPath);

    reindeerMazes.DoPart = 2; 
    reindeerMazes.ClearCache();
    
    Console.ReadKey();
    finalPath.AddRange(reindeerMazes.FinalPath);

    int part2Answer = finalPath.Distinct().Count();

    Console.WriteLine($"Part 1: The fastest score is {part1Answer}.");
    Console.WriteLine($"Part 2: There are {part2Answer} tiles along the best routes.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}