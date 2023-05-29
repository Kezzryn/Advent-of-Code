try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PREAMBLE = 25;
    long[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(long.Parse).ToArray();

    long[] last25 = new long[PREAMBLE];
    long[] sums = new long[PREAMBLE * PREAMBLE];

    Array.Copy(puzzleInput, last25, PREAMBLE);

    int offset = 0;
    long part1Answer = 0;
    long part2Answer = 0;

    for (int i = 0; i < PREAMBLE; i++)
    {
        for (int j = i + 1; j < PREAMBLE; j++)
        {
            sums[(i * PREAMBLE) + j] = last25[i] + last25[j];
            sums[(j * PREAMBLE) + i] = last25[i] + last25[j];
        }
    }

    
    for(int i = PREAMBLE; i < puzzleInput.Length; i++)
    {
        if (Array.IndexOf(sums, puzzleInput[i]) == -1)
        {
            part1Answer = puzzleInput[i];
            break;
        }

        offset = i % PREAMBLE;
        last25[offset] = puzzleInput[i];

        for (int j = 0; j < PREAMBLE; j++)
        {
            if (j == offset) continue;
            sums[(offset * PREAMBLE) + j] = last25[offset] + last25[j];
            sums[(j * PREAMBLE) + offset] = last25[offset] + last25[j];
        }
    }

    


    

    Console.WriteLine($"Part 1: The first number that is not the sum of the previous {PREAMBLE} is {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}