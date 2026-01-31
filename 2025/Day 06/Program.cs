try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT).ToList()];

    long part1Answer = 0;
    long part2Answer = 0;

    bool isDone = false;
    int cursor = puzzleInput.Last().IndexOfAny(['+', '*'], 0);
    do
    {
        bool doAdd = puzzleInput.Last()[cursor] == '+';
        int nextPos = puzzleInput.Last().IndexOfAny(['+', '*'], cursor + 1);

        if(nextPos == -1)
        {
            isDone = true;
            nextPos = puzzleInput[0].Length;
        }

        long part1Temp = doAdd ? 0 : 1; //prime to 1 for multiplication so we don't multiply by zero.
        long part2Temp = doAdd ? 0 : 1;

        List<List<char>> part2Rotate = [];

        foreach (string input in puzzleInput.SkipLast(1))
        {
            string subString = input[cursor..nextPos];
            //Build the array for part 2.
            for(int i = 0; i < subString.Length; i++)
            {
                if (i >= part2Rotate.Count) part2Rotate.Add([]);
                part2Rotate[i].Add(subString[i]);
            }

            if (long.TryParse(subString, out long result))
            {
                if (doAdd)
                {
                    part1Temp += result;
                }
                else
                {
                    part1Temp *= result;
                }
            }

        }

        foreach(string part in part2Rotate.Select(x => new string([.. x])))
        {
            if (long.TryParse(part, out long result))
            {
                if (doAdd)
                {
                    part2Temp += result;
                }
                else
                {
                    part2Temp *= result;
                }
            }
        }

        part1Answer += part1Temp;
        part2Answer += part2Temp;
        cursor = nextPos;
    } while (!isDone);

    Console.WriteLine($"Part 1: The sum of the individual problems is {part1Answer}.");
    Console.WriteLine($"Part 2: When working in columns, the sum of the problems is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}