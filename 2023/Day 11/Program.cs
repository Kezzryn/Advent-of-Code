static int TaxiDistance((int X, int Y) A, (int X, int Y) B) => Math.Abs(A.X - B.X) + Math.Abs(A.Y  - B.Y);

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    List<(int X, int Y)> galaxyMap = (from y in Enumerable.Range(0, puzzleInput.Count)
                                       from x in Enumerable.Range(0, puzzleInput[0].Length)
                                       where puzzleInput[y][x] == '#'
                                       select (x, y)).ToList();

    HashSet<int> emptyCol = Enumerable.Range(0, puzzleInput[0].Length).Except(galaxyMap.Select(g => g.X).Distinct()).ToHashSet();
    HashSet<int> emptyRow = Enumerable.Range(0, puzzleInput.Count).Except(galaxyMap.Select(g => g.Y).Distinct()).ToHashSet();

    long CalcAnswer(int multiplier)
    {
        long returnValue = 0;

        for (int i = 0; i < galaxyMap.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxyMap.Count; j++)
            {
                int offsetX = emptyCol.Count(x => int.Min(galaxyMap[i].X, galaxyMap[j].X) <= x && x <= int.Max(galaxyMap[i].X, galaxyMap[j].X));
                int offsetY = emptyRow.Count(y => int.Min(galaxyMap[i].Y, galaxyMap[j].Y) <= y && y <= int.Max(galaxyMap[i].Y, galaxyMap[j].Y));
                returnValue += TaxiDistance(galaxyMap[i], galaxyMap[j]) + ((offsetX + offsetY) * multiplier) - (offsetX + offsetY);
            }
        }

        return returnValue;
    }

    long part1Answer = CalcAnswer(1);
    long part2Answer = CalcAnswer(1000000); 

    Console.WriteLine($"Part 1: The sum of the doubled distances is {part1Answer}.");
    Console.WriteLine($"Part 2: When acconting for million time farther, the sum of the distances distances are {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}