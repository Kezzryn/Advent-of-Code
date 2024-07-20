using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int MIN_XY = 1;
    const int MAX_XY = 300;

    int gridSerialNumber = int.Parse(File.ReadAllText(PUZZLE_INPUT));

    int[,] fuelCells = new int[MAX_XY + 1, MAX_XY + 1]; // add one to shift to 1 based indexing.
    Dictionary<Point3D, int> powerCells = [];   // a powercell is the upper left corner of a grid of fuel cells.
                                                // the z axis represents the size of the grid.

    foreach (Point2D p in from a in Enumerable.Range(MIN_XY, MAX_XY)
                          from b in Enumerable.Range(MIN_XY, MAX_XY)
                          select new Point2D(a, b))
    {
        int rackID = p.X + 10;
        int powerLevel = (rackID * p.Y) + gridSerialNumber;
        powerLevel = (powerLevel * rackID / 100 % 10) - 5;

        fuelCells[p.X, p.Y] = powerLevel;
        powerCells.Add(new(p, 1), powerLevel);
    }

    for (int cellSize = MIN_XY + 1; cellSize <= MAX_XY; cellSize++)
    {
        int maxPower = 0;
        foreach (Point2D p in from a in Enumerable.Range(MIN_XY, MAX_XY - cellSize) // stop short of the edge.
                            from b in Enumerable.Range(MIN_XY, MAX_XY - cellSize)
                            select new Point2D(a, b))
        {
            int nextX = p.X + (cellSize - 1);
            int nextY = p.Y + (cellSize - 1);

            /*
             EG: cell size 3
             p points to cell A 
             fuelCells grid: 
             ABC
             DEF
             GHI
            */

            //pV = I
            int powerValue = fuelCells[nextX, nextY];

            //pV += G + H
            for (int x = p.X; x < p.X + cellSize; x++)
            {
                powerValue += fuelCells[x, nextY];
            }

            //pV += C + F
            for (int y = p.Y; y < p.Y + cellSize; y++)
            {
                powerValue += fuelCells[nextX, y];
            }

            //pv += ABDE as calculatd from the previous layer
            powerValue += powerCells[new(p.X, p.Y, cellSize - 1)];

            if (powerValue > maxPower) maxPower = powerValue;

            powerCells.Add(new(p, cellSize), powerValue);
        }
        // Testing shows that there isn't quite a smooth up then down, but once it hits zero, it never recovers.
        // It never goes negative, but the test remains. 
        if (maxPower <= 0) break;
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