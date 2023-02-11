try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, Dictionary<string, int>> cityList = new();
    HashSet<string> uniqueCityNames = new();
    int maxDistance = 0;

    // Mini function to calculate the distance between two cities.  
    int calcDistance(List<string> x)
    {
        return x.Zip(x.Skip(1))
            .Select(pair => cityList[pair.First][pair.Second])
            .Sum();
    }

    // Load in the puzzle data.
    foreach (string puzzleLine in puzzleInput)
    {
        // EG Norrath to Tristram = 142;
        string[] splitPuzzle = puzzleLine.Split(' ');

        if (cityList.TryGetValue(splitPuzzle[0], out var city))
        {
            cityList[splitPuzzle[0]].TryAdd(splitPuzzle[2], int.Parse(splitPuzzle[4]));
        }
        else
        {
            cityList.Add(splitPuzzle[0], new() { { splitPuzzle[2], int.Parse(splitPuzzle[4]) } });
        }

        if (cityList.TryGetValue(splitPuzzle[2], out var reverseCity))
        {
            cityList[splitPuzzle[2]].TryAdd(splitPuzzle[0], int.Parse(splitPuzzle[4]));
        }
        else
        {
            cityList.Add(splitPuzzle[2], new() { { splitPuzzle[0], int.Parse(splitPuzzle[4]) } });
        }

        uniqueCityNames.Add(splitPuzzle[0]);
        uniqueCityNames.Add(splitPuzzle[2]);
        maxDistance = int.Max(maxDistance, int.Parse(splitPuzzle[4]));
    }

    // Ensure that all combo's have keys. 
    foreach (string outerCity in uniqueCityNames)
    {
        if (!cityList.ContainsKey(outerCity)) cityList.Add(outerCity, new());
        foreach (string innerCity in uniqueCityNames)
        {
            if (!cityList[outerCity].ContainsKey(innerCity)) cityList[outerCity].Add(innerCity, maxDistance * 100);
            if (outerCity == innerCity) cityList[outerCity][innerCity] = 0;
        }
    }

    // Branch and bound function
    // It does not properly bound when trying to Maximize and falls back to a comprehensive search.
    // This seems likely as the base operators push the answer closer to zero.
    // A more comprehensive bounding function would need to be created.
        List<string> BnB(string startCity, bool isMaximize = false)
    {
        int factor = isMaximize ? 1 : 1;

        List<string> solution = new()
        {
            startCity
        };

        int current_optimum_distance = isMaximize ? int.MinValue : int.MaxValue;

        PriorityQueue<List<string>, int> candidate_queue = new();

        candidate_queue.Enqueue(solution, current_optimum_distance);

        while (candidate_queue.Count > 0)
        {
            List<string> curr_candidate_solution = candidate_queue.Dequeue();
            List<string> nextSteps = uniqueCityNames.Except(curr_candidate_solution).ToList();

            if (nextSteps.Count < 1) // no more cities to explore. We have potential solution. 
            {
                // test for potential problem solution. 
                int testDistance = calcDistance(curr_candidate_solution) * factor;
                if (isMaximize
                    ? testDistance > current_optimum_distance
                    : testDistance < current_optimum_distance)
                {
                    solution = curr_candidate_solution;
                    current_optimum_distance = testDistance;
                }
            }
            else
            {
                foreach (string nextCity in nextSteps)
                {
                    List<string> new_candidate_solution = curr_candidate_solution.Append(nextCity).ToList();

                    int newDistance = calcDistance(new_candidate_solution) * factor;

                    if (isMaximize
                        ? newDistance >= current_optimum_distance
                        : newDistance <= current_optimum_distance )
                    {
                        candidate_queue.Enqueue(new_candidate_solution, newDistance);
                    } 
                }
            }
        }
        return solution;

    }

    // Branch and bound for each starting city. 
    Dictionary<string, List<string>> solutionsPart1 = new();
    Dictionary<string, List<string>> solutionsPart2 = new();
    foreach (string city in uniqueCityNames)
    {
        solutionsPart1.Add(city, BnB(city));
        solutionsPart2.Add(city, BnB(city, true));
    }

    int part1Answer = int.MaxValue;
    int part2Answer = int.MinValue;

    foreach (List<string> solution in solutionsPart1.Values)
    {
        part1Answer = int.Min(calcDistance(solution), part1Answer);
    }

    foreach (List<string> solution in solutionsPart2.Values)
    {
        part2Answer = int.Max(calcDistance(solution), part2Answer);
    }
        
    Console.WriteLine($"Part 1: The shortest distance is: {part1Answer}");
    Console.WriteLine($"Part 2: The longest distance is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}




