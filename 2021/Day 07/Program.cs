try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART2 = true;
    List<int> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(int.Parse).ToList();

    static int CalcFuel(List<int> crabSubs, bool doPart2 = false)
    {
        int min = crabSubs.Min();
        int max = crabSubs.Max();

        int bestFuel = int.MaxValue;

        List<int> fuelCost = new();
        for (int i = min; i <= max; i++)
        {
            fuelCost.Add(i);
            if (doPart2 && i != min) fuelCost[i] += fuelCost[i - 1];
            //fuelCost.Add(i == min || !doPart2 ? i : i + fuelCost[i - 1]); 
        }

        for (int pos = min; pos <= max; pos++)
        {
            int testFuel = 0;
            foreach (int crab in crabSubs)
            {
                testFuel += fuelCost[Math.Abs(pos - crab)];
                if (testFuel >= bestFuel) break;
            }

            if (testFuel < bestFuel) bestFuel = testFuel;
        }

        return bestFuel;
    }

    int part1Answer = CalcFuel(puzzleInput);
    int part2Answer = CalcFuel(puzzleInput, DO_PART2);

    Console.WriteLine($"Part 1: With basic fuel costs, it'll take {part1Answer} fuel to move the crab subs.");
    Console.WriteLine($"Part 2: With crab fuel costs, it'll take {part2Answer} fuel to position the crab subs.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}