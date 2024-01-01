using AoC_MaxStar;
using System.Drawing;

try
{
    const bool DO_PART2 = false;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Point start = new (1, 0);
    Point end = new(puzzleInput[0].Length-2, puzzleInput.Length-1);

    Dictionary<Point, Dictionary<Point, int>> theMapP1 = MaxStar.MakeMap(puzzleInput, start, end);
    int part1Answer = MaxStar.Max_Star(start, end, theMapP1);
    Console.WriteLine($"Part 1: Slippery slopes limit the walk to {part1Answer} steps.");

    Dictionary<Point, Dictionary<Point, int>> theMapP2 = MaxStar.MakeMap(puzzleInput, start, end, DO_PART2);
    int part2Answer = MaxStar.Max_Star(start, end, theMapP2);
    Console.WriteLine($"Part 2: When going up and down hills, the longest walk is {part2Answer} steps.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}