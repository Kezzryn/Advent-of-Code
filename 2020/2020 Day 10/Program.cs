try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).Append(0).Append(0).ToArray();

    puzzleInput[puzzleInput.Length - 1] = puzzleInput.Max() + 3;
    Array.Sort(puzzleInput);

    int diffOne = 0;
    int diffThree = 0;

    for (int i = 1; i < puzzleInput.Length; i++)
    {
        int diff = puzzleInput[i] - puzzleInput[i - 1];
        if (diff == 1) diffOne++;
        if (diff == 3) diffThree++;
    }
    int part1Answer = diffOne * diffThree;
    Console.WriteLine($"Part 1: The product of 1-jolt ({diffOne}) and 3-jolt ({diffThree}) differences is {part1Answer}.");


    static int ComboCounter(int[] items)
    {
        if (items.Length == 1) return 1;
        if (items.Length == 2)
        {
            // check to see if we can reach the next item in this configuration.
            return (items[1] - items[0] <= 3) ? 1 : 0;
        }

        int returnValue = 0;
        
        for (int j = 1; j < items.Length && j <= 3; j++)
        {
            if (items[j] - items[0] <= 3 ) returnValue += ComboCounter(items[j..]);
        }
        
        return returnValue;
    }

    long part2Answer = 1;   // set to 1 'cause multiplier 
    int rangeStart = 0;
    int rangeEnd = 0;
    do
    {
        if (puzzleInput[rangeEnd + 1] - puzzleInput[rangeEnd] == 3)
        {
            // we only want ranges of 2 or more.
            if (rangeEnd > rangeStart + 1) 
                part2Answer *= ComboCounter(puzzleInput[rangeStart..(rangeEnd + 1)]);   // add one to the rangeEnd to be inclusive.

            rangeStart = rangeEnd + 1;
        }
        rangeEnd++;

    } while (rangeEnd < puzzleInput.Length - 1);

    Console.WriteLine($"Part 2: There are a total of {part2Answer} possible valid combinations.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}