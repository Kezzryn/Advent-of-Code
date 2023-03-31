try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int LIST_LENGTH = 256;

    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(int.Parse).ToArray();


    int[] list = new int[256];
    for(int i = 0; i < LIST_LENGTH; i++)
    {
        list[i] = i;
    }

    int cursor = 0;
    int skipSize = 0;

    foreach (int length in puzzleInput)
    {
        if (length > LIST_LENGTH) continue;

        if (cursor + length <= LIST_LENGTH)
        {
            Array.Reverse(list, cursor, length);
        }
        else
        {
            int i = cursor;
            int j = cursor + length - 1;
            while (i < j)
            {
                (list[j % LIST_LENGTH], list[i % LIST_LENGTH]) = (list[i % LIST_LENGTH], list[j % LIST_LENGTH]);
                i++;
                j--;
            }
        }

        cursor += (length + skipSize) % LIST_LENGTH;
        skipSize++;
    }

    int part1Answer = list[0] * list[1];
    Console.WriteLine($"Part 1: The product of the first two elements of the list is {part1Answer}.");
    Console.WriteLine($"Part 2: ");
}
catch (Exception e)
{
    Console.WriteLine(e);
}