using AoC_2017_Day_14;
using System.Drawing;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string secretHash = File.ReadAllText(PUZZLE_INPUT);

    bool[,] diskGrid = new bool[128,128];
    int part1Answer = 0;
    int part2Answer = 0;

    for (int row = 0; row < 128; row++)
    {
        string hash = KnotHash.HashStringToBinary($"{secretHash}-{row}");
        part1Answer += hash.Count(c => c == '1');
        
        for (int col = 0; col < 128; col++)
        {
            if (hash[col] == '1') diskGrid[col, row] = true;
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} used squares.");

    
    void ClearRegion(int col, int row)
    {
        Queue<Point> points = new();
        HashSet<Point> set = new();

        List<Size> neighbors = new()
        {
            new Size( 0,  1),
            new Size( 0, -1),
            new Size( 1,  0),
            new Size(-1,  0),
        };

        points.Enqueue(new(col, row));
        while (points.Count > 0)
        {
            Point current = points.Dequeue();
            foreach (Size neighbor in neighbors)
            {
                Point newPoint = current + neighbor;
                if (newPoint.X >= 0 && newPoint.X < 128 &&
                    newPoint.Y >= 0 && newPoint.Y < 128)
                {
                    if (diskGrid[newPoint.X, newPoint.Y]) points.Enqueue(newPoint);
                }
            }
            diskGrid[current.X, current.Y] = false;
        }
    }

    for (int row = 0; row < 128; row++)
    {
        for (int col = 0; col < 128; col++)
        {
            if (diskGrid[col, row])
            {
                ClearRegion(col, row);
                part2Answer++;
            }
        }
    }

    Console.WriteLine($"Part 2: There are {part2Answer} regions.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}