using System.Drawing;
using AoC_2018_Day_11;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int MIN_XY = 1;
    const int MAX_XY = 300;

    int gridSerialNumber = int.Parse(File.ReadAllText(PUZZLE_INPUT));

    int[,] fuelCells = new int[MAX_XY + 1, MAX_XY + 1]; // add one to shift to 1 based indexing.
    Dictionary<Point3D, int> powerCells = new();

    foreach (Point p in from a in Enumerable.Range(MIN_XY, MAX_XY)
                        from b in Enumerable.Range(MIN_XY, MAX_XY)
                        select new Point(b, a))
    {
        int rackID = p.X + 10;
        int powerLevel = (rackID * p.Y) + gridSerialNumber;
        powerLevel = (powerLevel * rackID / 100 % 10) - 5;

        fuelCells[p.X, p.Y] = powerLevel;
        powerCells.Add(new(p, 1), powerLevel);
    }

    for (int z = MIN_XY + 1; z <= MAX_XY; z++)
    {
        int maxZPower = 0;
        foreach (Point p in from a in Enumerable.Range(MIN_XY, MAX_XY - z)
                            from b in Enumerable.Range(MIN_XY, MAX_XY - z)
                            select new Point(a, b))
        {
            int powerValue = fuelCells[p.X + (z - 1), p.Y + (z - 1)];

            for(int x = p.X; x < p.X + z; x++)
            {
                powerValue += fuelCells[x, p.Y + (z - 1)];
            }

            for (int y = p.Y; y < p.Y + z; y++)
            {
                powerValue += fuelCells[p.X + (z - 1), y];
            }

            powerValue += powerCells[new(p.X, p.Y, z - 1)];

            if (powerValue > maxZPower) maxZPower = powerValue;

            powerCells.Add(new(p, z), powerValue);
        }
        // There isn't quite a smooth up then down, but once it hits zero, it never recovers.
        // curious that it never goes negative. 
        if (maxZPower == 0) break;
    }

    Point3D part1Answer = powerCells.Where(x => x.Key.Z == 3).OrderByDescending(x => x.Value).First().Key;
    Point3D part2Answer = powerCells.OrderByDescending(x => x.Value).First().Key;

    Console.WriteLine($"Part 1: The largest 3x3 power cell is at {part1Answer.X},{part1Answer.Y}");
    Console.WriteLine($"Part 2: The best power cell we can make is at {part2Answer.X},{part2Answer.Y} with a size of {part2Answer.Z}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}