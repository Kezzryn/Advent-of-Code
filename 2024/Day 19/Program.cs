try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(Environment.NewLine + Environment.NewLine);

    Dictionary<char, List<string>> towels = new()
    {
        {'w', [] },
        {'u', [] },
        {'b', [] },
        {'r', [] },
        {'g', [] }
    };
    List<string> targetPatterns = [.. puzzleInput[1].Split(Environment.NewLine)];

    foreach (string s in puzzleInput[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
    {
        towels[s[0]].Add(s);
    }

    foreach (List<string> towelset in towels.Values)
    {
        towelset.Sort((x, y) => x.Length == y.Length ? 0 : x.Length > y.Length ? -1 : 1);
    }

    static long countPatterns(string testPattern, int currIndex, Dictionary<int, long> cache, Dictionary<char, List<string>> towels)
    {
        long numMatches = 0;
        foreach (string pattern in towels[testPattern[currIndex]])
        {
            int nextIndex = currIndex + pattern.Length;
            if (nextIndex > testPattern.Length) continue; 
            
            string sub = testPattern.Substring(currIndex, pattern.Length);
            if (sub == pattern)
            {
                if(nextIndex == testPattern.Length) numMatches++;

                if (nextIndex < testPattern.Length)
                {
                    if(cache.TryGetValue(nextIndex, out long cachedValues))
                    {
                        numMatches += cachedValues;
                    }
                    else
                    {
                        numMatches += countPatterns(testPattern, nextIndex, cache, towels);
                    }
                }
                    
            }
        }
        cache.TryAdd(currIndex, numMatches);
        return numMatches;
    }

    PriorityQueue<int, int> queue = new();
    HashSet<int> inQueue = [];
    
    int part1Answer = 0;
    long part2Answer = 0;

    foreach (string testPattern in targetPatterns)
    {
        queue.Clear();
        queue.Enqueue(0, 0);
        
        inQueue.Clear();
        inQueue.Add(0);

        bool matchFound = false;
        List<HashSet<string>> numMatches = [];
        for (int i = 0; i < testPattern.Length; i++)
        {
            numMatches.Add([]);
        }

        while (queue.TryDequeue(out int stringIndex, out _))
        {
            foreach (string pattern in towels[testPattern[stringIndex]])
            {
                int nextIndex = stringIndex + pattern.Length;

                if (nextIndex > testPattern.Length) continue;

                if (testPattern.Substring(stringIndex, pattern.Length) == pattern)
                {
                    if (nextIndex == testPattern.Length) matchFound = true;

                    numMatches[stringIndex].Add(pattern);
                    if (nextIndex < testPattern.Length && inQueue.Add(nextIndex) == true)
                        queue.Enqueue(nextIndex, nextIndex);
                }
            }
        }

        if (matchFound)
        {
            part1Answer++;
            part2Answer += countPatterns(testPattern, 0, [], towels);
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} valid towel arrangments.");
    Console.WriteLine($"Part 2: Those arrangments can be made {part2Answer} different ways.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}