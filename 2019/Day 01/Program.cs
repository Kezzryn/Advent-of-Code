try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART_TWO = true;

    int[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToArray();

    static int GetFuel(int weight, bool doPart2 = false)
    {
        int returnValue = (weight / 3) - 2;
        if (doPart2 && returnValue > 0)
        {
            returnValue += GetFuel(returnValue, doPart2);
        }

        return int.Max(returnValue, 0);
    }

    int part1Answer = puzzleInput.Sum(x => GetFuel(x));
    int part2Answer = puzzleInput.Sum(x => GetFuel(x, DO_PART_TWO));
    
    Console.WriteLine($"Part 1: The sum of the fuel requirement for all modules is {part1Answer}.");
    Console.WriteLine($"Part 2: The sum of the fuel requirements for all modules and their fuel is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}