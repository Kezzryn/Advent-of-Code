using AoC_2017_KnotHash;
using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string secretHash = File.ReadAllText(PUZZLE_INPUT);

    HashSet<Point2D> diskGrid = [];

    int part1Answer = 0;
    int part2Answer = 0;

    for (int row = 0; row < 128; row++)
    {
        string hash = KnotHash.HashStringToBinary($"{secretHash}-{row}");
        part1Answer += hash.Count(c => c == '1');

        for (int col = 0; col < 128; col++)
        {
            if (hash[col] == '1') diskGrid.Add(new(col, row));
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} used squares.");

    Queue<Point2D> pointsToClear = new();
    HashSet<Point2D> queueContents = [];

    foreach (Point2D p in from x in Enumerable.Range(0, 128)
                          from y in Enumerable.Range(0, 128)
                          select new Point2D(x, y))
    {
        if(diskGrid.Contains(p))
        {
            pointsToClear.Clear();
            queueContents.Clear();

            pointsToClear.Enqueue(p);
            while (pointsToClear.TryDequeue(out Point2D current))
            {
                foreach (Point2D neighbor in current.GetOrthogonalNeighbors())
                {
                    if (diskGrid.Remove(neighbor)) pointsToClear.Enqueue(neighbor);
                }
            }

            part2Answer++;
        }
    }

    Console.WriteLine($"Part 2: There are {part2Answer} regions.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}