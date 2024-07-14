using AoC_2018_Day_03;
using BKH.Geometry;
using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    RectGroup overlappingRects = new();
    Dictionary<int, Rectangle> elfPlans = [];

    foreach (string line in puzzleInput)
    {
        int[] elfdata = GetNumbers().Matches(line).Select(x => int.Parse(x.Value)).ToArray();
        elfPlans.Add(elfdata[0], new Rectangle(
            elfdata[1], elfdata[2],
            elfdata[1] + elfdata[3], elfdata[2] + elfdata[4]));
    }

    int part2Answer = 0;

    foreach((int ID1, Rectangle planA) in  elfPlans)  
    {
        bool isIntersection = false;
        foreach ((int ID2, Rectangle planB) in elfPlans.Where(x => x.Key != ID1))
        {
            if (planA.Overlap(planB))
            {
                isIntersection = true;
                overlappingRects.Add(planA.Intersect(planB));
            }
        }
        if (!isIntersection) part2Answer = ID1;
    }

    long part1Answer = overlappingRects.Rectangles.Sum(x => x.Area);

    Console.WriteLine($"Part 1: There are {part1Answer} square inches of fabric that have multiple claims.");
    Console.WriteLine($"Part 2: {part2Answer} is the only ID that doesn't overlap.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex GetNumbers();
}
