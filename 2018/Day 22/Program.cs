using System.Drawing;
using AoC_2018_Day_22;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int depth = int.Parse(puzzleInput[0].Split(": ").Last());
    int[] targetCoord = puzzleInput[1].Split(": ").Last().Split(',').Select(int.Parse).ToArray();

    TheMap theMap = new(0, 0, targetCoord[0], targetCoord[1], depth);
    int part1Answer = theMap.CaveRisk();

    theMap.A_Star(new(0, 0, 1), new(targetCoord[0], targetCoord[1], 0), out int part2Answer, out List<Point> finalPath);

    theMap.DrawCave(finalPath);

    Console.WriteLine($"Part 1: The risk level is {part1Answer}.");
    Console.WriteLine($"Part 2: The fastest path takes {part2Answer} minutes.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}