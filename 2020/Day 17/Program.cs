using AoC_Point4D;

try
{
    const bool DO_PART_2 = true;
    const int NUM_CYCLES = 6;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    static int HyperConway(string[] initalState, int numCycles, bool doPart2 = false)
    {
        Dictionary<Point4D, bool> sourceGrid = new();
        Dictionary<Point4D, int> countGrid = new();

        foreach (Point4D v in from y in Enumerable.Range(0, initalState.Length)
                              from x in Enumerable.Range(0, initalState[0].Length)
                              select new Point4D(x, y, 0, 0))
        {
            sourceGrid.Add(v, initalState[v.Y][v.X] == '#');
        }

        IEnumerable<Point4D> neighbors = from x in Enumerable.Range(-1, 3)
                                         from y in Enumerable.Range(-1, 3)
                                         from z in Enumerable.Range(-1, 3)
                                         from w in doPart2 ? Enumerable.Range(-1, 3) : Enumerable.Range(0, 1)
                                         where !(x == 0 && y == 0 && z == 0 && w == 0)
                                         select new Point4D(x, y, z, w);

        for (int generation = 0; generation < numCycles; generation++)
        {
            countGrid.Clear();

            Point4D min = sourceGrid.Keys.Min();
            Point4D max = sourceGrid.Keys.Max();

            min += new Point4D(-1, -1, -1, doPart2 ? -1 : 0);
            max += new Point4D(1, 1, 1, doPart2 ? 1 : 0);

            foreach(Point4D cursor in sourceGrid.Where(x => x.Value).Select(x => x.Key))
            {
                foreach(Point4D neighbor in neighbors)
                {
                    Point4D testPoint = cursor + neighbor;
                    if (!countGrid.TryAdd(testPoint, 1)) countGrid[testPoint]++;
                }
            }

            foreach (Point4D cursor in from x in Enumerable.Range(min.X, (max.X - min.X) + 1)
                                       from y in Enumerable.Range(min.Y, (max.Y - min.Y) + 1)
                                       from z in Enumerable.Range(min.Z, (max.Z - min.Z) + 1)
                                       from w in doPart2 ? Enumerable.Range(min.W, (max.W - min.W) + 1) : Enumerable.Range(0, 1)
                                       select new Point4D(x, y, z, w))
            {
                countGrid.TryGetValue(cursor, out int numNeighbors);
                sourceGrid.TryGetValue(cursor, out bool currentValue);

                bool newValue = (numNeighbors == 3 || (currentValue && numNeighbors == 2));

                if (!sourceGrid.TryAdd(cursor, newValue)) sourceGrid[cursor] = newValue;
            }

        }
        return sourceGrid.Count(x => x.Value == true);
    }

    int part1Answer = HyperConway(puzzleInput, NUM_CYCLES);
    Console.WriteLine($"Part 1: Three dimensional hypercubes generate {part1Answer} spots.");

    int part2Answer = HyperConway(puzzleInput, NUM_CYCLES, DO_PART_2);
    Console.WriteLine($"Part 2: Four dimensional hypercubes generate {part2Answer} spots.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}