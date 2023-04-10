using System.Drawing;
using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<int, Rectangle> elfPlans = new();
    int[,] cloth = new int[1000, 1000];

    foreach (string line in puzzleInput)
    {
        int[] elfdata = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();
        elfPlans.Add(elfdata[0], new Rectangle(elfdata[1], elfdata[2], elfdata[3], elfdata[4]));
    }

    int part1Answer = 0;
    int part2Answer = 0;

    foreach((int ID1, Rectangle planA) in  elfPlans)  
    {
        bool isIntersection = false;
        foreach((int ID2, Rectangle planB) in elfPlans) 
        {
            if (ID1 == ID2) continue;
            if (planA.IntersectsWith(planB))
            {
                isIntersection = true;
                Rectangle intersection = Rectangle.Intersect(planA, planB);
                foreach ((int x, int y) in from x in Enumerable.Range(intersection.X, intersection.Width)
                                           from y in Enumerable.Range(intersection.Y, intersection.Height)
                                           select (x, y))
                {
                    cloth[x, y]++;
                }
            }
        }
        if (!isIntersection) part2Answer = ID1;
    }
    
    foreach((int x, int y) in from x in Enumerable.Range(0, cloth.GetUpperBound(0))
                              from y in Enumerable.Range(0, cloth.GetUpperBound(1))
                              select (x, y))
    {
        if (cloth[x,y] >= 2) part1Answer++;
    }

    Console.WriteLine($"Part 1: There are {part1Answer} square inches of fabric that have multiple claims.");
    Console.WriteLine($"Part 2: {part2Answer} is the only ID that doesn't overlap.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}