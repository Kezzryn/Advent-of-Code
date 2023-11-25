using AoC_AStar;
using System.Drawing;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<Point, int> theMapP1 = new();
    Dictionary<Point, int> theMapP2 = new();

    foreach (Point p in from y in Enumerable.Range(0, puzzleInput.Length)
                        from x in Enumerable.Range(0, puzzleInput[0].Length)
                        select(new Point(x,y)))
    {
        theMapP1[p] = puzzleInput[p.Y][p.X] - 48;
        theMapP2[p] = puzzleInput[p.Y][p.X] - 48;

        int multiplier = puzzleInput.Length;
        foreach (Size s in from y in Enumerable.Range(0, 5)
                           from x in Enumerable.Range(0, 5)
                           where (!(x == 0 && y == 0))
                           select (new Size(x * multiplier, y * multiplier)))
        {
            int newVal = (theMapP2[p] + ((s.Width + s.Height) / multiplier)) % 9;
            theMapP2[p + s] = (newVal == 0) ? 9 : newVal;
        }
    }

    Point start = new(0,0);
    Point destPart1 = new (puzzleInput.Length - 1, puzzleInput.Length - 1);
    Point destPart2 = new ((puzzleInput.Length * 5) - 1, (puzzleInput.Length * 5) - 1); 

    AStar.A_Star(start, destPart1, theMapP1, out int part1Answer, out _);
    AStar.A_Star(start, destPart2, theMapP2, out int part2Answer, out _);

    Console.WriteLine($"Part 1: The risk for the small map is {part1Answer}.");
    Console.WriteLine($"Part 2: The risk for the large map is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}