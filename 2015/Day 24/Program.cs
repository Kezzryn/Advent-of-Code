using AoC_2015_Day_24;

try
{
    bool verbose = false;
    const int NUM_BAGS_PART1 = 3;
    const int NUM_BAGS_PART2 = 4;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<long> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(long.Parse).Reverse().ToList();

    long FindCombo(List<long> solutionSpace, int numBags)
    {
        long targetWeight = solutionSpace.Sum() / numBags;

        List<List<long>> comboList = new();

        while (comboList.Count < solutionSpace.Count)
        {
            comboList.Add(solutionSpace);
            if (verbose) Console.WriteLine($"SS: {solutionSpace.Count} Testing number of packages. {comboList.Count} numBags: {numBags}");

            var result = comboList.CartesianProduct().Where(x => x.Sum() == targetWeight);

            if (verbose) Console.WriteLine($"CP result generated.");

            if (result.Any())
            {
                if (verbose) Console.WriteLine($"SS: {solutionSpace.Count}: Found {result.Count()} potentially matching bags.");
                return result.Select(x => x.Aggregate((x, y) => x * y)).Min();
            }
                // Removed depth checking after manually confirming that we don't have to worry about that sort of thing with this specific puzzle input. 
                // TODO figure out how to speed this up, cause right now it runs /forever/ at higher depths.

                /*
                // depth check. 
                if (numBags == 2)
                {
                    if (verbose) Console.WriteLine("Found 2 bag solution returning.");
                    return 1;
                }

                foreach (var r in result)
                {
                    if (verbose) Console.WriteLine($"Calling FindCombo for {numBags -1}");
                    if (FindCombo(solutionSpace.Except(r).ToList(), numBags - 1) != -1)
                    {
                        if (verbose) Console.WriteLine($"Found a sub match, returning up!");
                        return result.Select(x => x.Aggregate((x, y) => x * y)).Min();
                    }
                }
            }
            else
            {
               if (verbose) Console.WriteLine("No matching bags.");
            }
            */
        }
        return -1; 
    }

    long part1 = FindCombo(puzzleInput, NUM_BAGS_PART1);
    Console.WriteLine($"Part 1: The best Quantum Entanglement of the smallest bag is {part1}.");

    long part2 = FindCombo(puzzleInput, NUM_BAGS_PART2);
    Console.WriteLine($"Part 2: The with {NUM_BAGS_PART2} places for bags, the best Quantum Entanglement is now {part2}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}