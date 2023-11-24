try
{
    const bool DO_PART2 = true;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, HashSet<string>> map = new();

    foreach (string line in puzzleInput)
    {
        string[] split = line.Split('-').ToArray();

        map.TryAdd(split[0], new());
        map.TryAdd(split[1], new());
        map[split[0]].Add(split[1]);
        map[split[1]].Add(split[0]);
    }

    int CountPaths(bool doPart2 = false)
    {
        const string DOUBLED = "@@";
        const string START = "start";
        const string END = "end";

        Queue<string> queue = new();
        HashSet<string> paths = new();

        foreach (string s in map[START])
        {
            queue.Enqueue(START + s);
        }

        while (queue.TryDequeue(out string? currentStep))
        {
            if (currentStep is null) throw new NullReferenceException("currentStep is null");

            if (currentStep.EndsWith(END))
            {
                paths.Add(currentStep);
            }
            else
            {
                string currentCave = currentStep[^2..];
                foreach (string nextStep in map[currentCave].Where(x => x != START))
                {
                    bool isBigCave = Char.IsUpper(nextStep, 0);
                    if (isBigCave || (!currentStep.Contains(nextStep)))
                    {
                        queue.Enqueue(currentStep + nextStep);
                    }
                    else
                    {
                        if(doPart2 && !currentStep.StartsWith(DOUBLED))
                        {
                            queue.Enqueue(DOUBLED + currentStep + nextStep);
                        }
                    }
                }
            }
        }

        return paths.Count;
    }

    int part1Answer = CountPaths();
    int part2Answer = CountPaths(DO_PART2);

    Console.WriteLine($"Part 1: The number of paths through the caves when only visiting small caves once is {part1Answer}");
    Console.WriteLine($"Part 2: The number of paths through the caves when visiting one small cave twice is {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}