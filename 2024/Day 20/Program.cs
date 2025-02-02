using AoC_2024_Day_20;
using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    GlitchMap glitchMap = new(puzzleInput)
    {
        MaxSteps = 10000
    };
    glitchMap.A_Star();

    foreach(Point2D source in glitchMap.CleanPath.Keys)
    {
        glitchMap.CheckGlitch(source, 2);
    }
    int part1Answer = glitchMap.GlitchPaths.Count;


    glitchMap.GlitchPaths.Clear();
    foreach (Point2D source in glitchMap.CleanPath.Keys)
    {
        glitchMap.CheckGlitch(source, 20);
    }
    int part2Answer = glitchMap.GlitchPaths.Count;

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}