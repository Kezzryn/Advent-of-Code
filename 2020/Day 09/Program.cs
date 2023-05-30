try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PREAMBLE_SIZE = 25;
    long[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(long.Parse).ToArray();

    long[] last25 = new long[PREAMBLE_SIZE];

    Dictionary<int, long> sums = new();
    List<long> cumulativeSum = new();

    Array.Copy(puzzleInput, last25, PREAMBLE_SIZE);

    int offset = 0;
    long part1Answer = 0;
    long part2Answer = 0;

    foreach((int i, int j, int key) in from a in Enumerable.Range(0, PREAMBLE_SIZE)
                              from b in Enumerable.Range(0, PREAMBLE_SIZE)
                              where b > a
                              select(a, b, (a * PREAMBLE_SIZE) + b)) 
    {
        sums.Add(key, last25[i] + last25[j]);
    }

    for (int i = PREAMBLE_SIZE; i < puzzleInput.Length; i++)
    {
        //check cache
        if (!sums.ContainsValue(puzzleInput[i]))
        {
            part1Answer = puzzleInput[i];
            break;
        }

        cumulativeSum.Add(cumulativeSum.LastOrDefault(0) + puzzleInput[i]);

        offset = i % PREAMBLE_SIZE; 
        last25[offset] = puzzleInput[i];

        //update cache
        for (int j = 0; j < PREAMBLE_SIZE; j++)
        {
            if (j == offset) continue;
            sums[(offset * PREAMBLE_SIZE) + j] = last25[offset] + last25[j];
        }
    }

    // My original solve.Runs in 20 - 50 milliseconds.
    //for (int window = 1; window < partialSum.Count; window++)
    //{
    //    for (int i = partialSum.Count - 1; i >= window; i--)
    //    {
    //        if (partialSum[i] < part1Answer) break; // can't get an answer from anything else in the set.
    //        if (partialSum[i] - partialSum[i - window] == part1Answer)
    //        {
    //            var range = puzzleInput.Skip(i - window + PREAMBLE_SIZE).Take(window);
    //            part2Answer = range.Min() + range.Max();
    //            break;
    //        }
    //    }
    //    if (part2Answer != 0) break;
    //}

    // This solve was inspired by /u/andrewsredditstuff
    // https://www.reddit.com/r/adventofcode/comments/k9lfwj/comment/gf6tdhg/

    long windowSum = 0;
    int windowStart = cumulativeSum.Count / 2; //start in the middle, makes the worst case better than starting at an end.
    int windowEnd = windowStart + 1;
    do
    {
        windowSum = cumulativeSum[windowEnd] - cumulativeSum[windowStart];
        if (windowSum > part1Answer) windowStart++;
        if (windowSum < part1Answer) windowEnd++;
    }
    while (windowSum != part1Answer);

    windowStart += PREAMBLE_SIZE; // offset to align back to puzzleInput
    windowEnd += PREAMBLE_SIZE;
    part2Answer = puzzleInput[windowStart..windowEnd].Min() + puzzleInput[windowStart..windowEnd].Max();

    Console.WriteLine($"Part 1: The first number that is not the sum of the previous {PREAMBLE_SIZE} is {part1Answer}.");
    Console.WriteLine($"Part 2: The encryption weakness in my XMAS-encrypted list of numbers is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}