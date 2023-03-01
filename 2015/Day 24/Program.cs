using AoC_2015_Day_24;

try
{
    //find a solution with the lowest number of items. 

    //check to see if there is a solution with the rest of the set. 

    //if both are true, add to a solution set for that number of items. 

    //once we have the X items searched, 
    // sort by QE and take lowest. 

    const int NUM_BAGS = 4;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<long> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(long.Parse).Reverse().ToList();

    long targetWeight = puzzleInput.Sum() / NUM_BAGS;

    long FindCombo(List<long> solutionSpace, int numBags)
    {
        List<List<long>> comboList = new();

        while (comboList.Count < solutionSpace.Count)
        {
            comboList.Add(solutionSpace);
            Console.WriteLine($"SS: {solutionSpace.Count} Testing number of packages. {comboList.Count} numBags: {numBags}");

            var result = comboList.CartesianProduct().Where(x => x.Sum() == targetWeight); //.Where(x => x.Distinct().Count() == comboList.Count && x.Sum() == targetWeight).ToList();

            Console.WriteLine($"CP result generated."); 
            //var filter = result.Where(x => x.Sum() == targetWeight);
            if (result.Any())
            {
                Console.WriteLine($"SS: {solutionSpace.Count}: Found {result.Count()} potentially matching bags.");
                return result.Select(x => x.Aggregate((x, y) => x * y)).Min();


                // Removed depth checking after manually confirming that we don't have to worry about that sort of thing. 
                // TODO figure out how to speed this up, cause right now it runs /forever/ 

                /*

                // depth check. 
                if (numBags == 2)
                {
                    Console.WriteLine("Found 2 bag solution. Returning 1");
                    return 1;
                }

                foreach (var r in result)
                {
                    Console.WriteLine($"Calling FindCombo for {numBags -1}");
                    if (FindCombo(solutionSpace.Except(r).ToList(), numBags - 1) != -1)
                    {
                        Console.WriteLine($"Found a sub match, returning up!");
                        return result.Select(x => x.Aggregate((x, y) => x * y)).Min();
                    }
                }

                */
            }
            else
            {
                Console.WriteLine("No matching bags.");
            }
        }
        return -1; 
    }

    long part1 = FindCombo(puzzleInput, NUM_BAGS);

    Console.WriteLine($"The smallest package with the best Quantum Entanglement is {part1}.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}