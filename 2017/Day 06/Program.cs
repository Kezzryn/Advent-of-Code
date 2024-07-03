try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split('\t').Select(int.Parse).ToList();

    static long Hash(List<int> data)
    {
        //There are 16 values and the data values never exceed 15, so we can bitpack a long as a hash value.
        long returnValue = 0;

        for(int i = 0; i < data.Count; i++)
        {
            returnValue |= (long)data[i] << (i * 4);
        }

        return returnValue;
    }

    long hashValue = Hash(puzzleInput);
    HashSet<long> uniqueHash = [ hashValue ];
    List<long> hashHistory = [ hashValue ];

    do
    {
        int maxValue = puzzleInput.Max();
        int maxIndex = puzzleInput.IndexOf(maxValue);

        puzzleInput[maxIndex] = 0;
        for(int i = 1; i <= maxValue; i++)
        {
            int index = (i + maxIndex) % puzzleInput.Count;
            puzzleInput[index]++;
        }

        hashValue = Hash(puzzleInput);
        hashHistory.Add(hashValue);
    } while (uniqueHash.Add(hashValue));

    // add one 'cause indexOf is zero bound
    int part2Answer = hashHistory.Count - (hashHistory.IndexOf(hashValue) + 1);

    Console.WriteLine($"Part 1: {uniqueHash.Count} redistribution cycles must be completed before a configuration is repeated.");
    Console.WriteLine($"Part 2: The loop size is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}