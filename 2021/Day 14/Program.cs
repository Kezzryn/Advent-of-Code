try
{
    const int PART1_STEPS = 10;
    const int PART2_STEPS = 40;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    string basePolymer = puzzleInput[0];
    
    Dictionary<string, char> rules = new();
    for(int i = 2; i < puzzleInput.Length; i++)
    {
        rules.Add(puzzleInput[i][0..2], puzzleInput[i][^1]);
    }

    int sourcePtr = 1; //swaps at the top of the main loop. 
    int targetPtr = 0;
    Dictionary<string, long>[] polymerPairCount = new Dictionary<string, long>[] { new(), new() };
    for (int i = 0; i < basePolymer.Length - 1; i++)
    {
        if (!polymerPairCount[targetPtr].TryAdd(basePolymer[i..(i + 2)], 1))
            polymerPairCount[targetPtr][basePolymer[i..(i + 2)]] += 1;
    }

    Dictionary<char, long> charSums = new();
    foreach (char c in basePolymer.Distinct())
    {
        charSums[c] = basePolymer.Count(x => x == c);
    }

    //Done initalization... 
    long part1Answer = 0;
    for (int step = 1; step <= PART2_STEPS; step++)
    {
        sourcePtr = (sourcePtr + 1) % 2;
        targetPtr = (targetPtr + 1) % 2;

        polymerPairCount[targetPtr].Clear();

        foreach ((string pair, long count) in polymerPairCount[sourcePtr])
        {
            if(rules.TryGetValue(pair, out char insertChar))
            {
                if (!polymerPairCount[targetPtr].TryAdd($"{pair[0]}{insertChar}", count))
                    polymerPairCount[targetPtr][$"{pair[0]}{insertChar}"] += count;

                if (!polymerPairCount[targetPtr].TryAdd($"{insertChar}{pair[1]}", count))
                    polymerPairCount[targetPtr][$"{insertChar}{pair[1]}"] += count;

                if (!charSums.TryAdd(insertChar, 1))
                    charSums[insertChar] += count;
            }
        }

        if(step == PART1_STEPS) part1Answer = (charSums.Values.Max()) - (charSums.Values.Min());
    }

    long part2Answer = (charSums.Values.Max()) - (charSums.Values.Min());

    Console.WriteLine($"Part 1: The difference of the most and the least elemetns after {PART1_STEPS} is {part1Answer}.");
    Console.WriteLine($"Part 2: After {PART2_STEPS} the difference is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}