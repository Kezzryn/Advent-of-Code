static int DecryptPass(string pass)
{
    static int BinSearch(string str, int max)
    {
        int min = 0;

        foreach (char s in str)
        {
            int newRange = (max - min) / 2;
            if (s == 'F' || s == 'L')
                max -= newRange + 1;
            else
                min += newRange + 1;
        }
        return max;
    }

    int row = BinSearch(pass[..7], 127);
    int col = BinSearch(pass[^3..], 7);

    return (row * 8) + col;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(DecryptPass).ToList();

    int part1Answer = puzzleInput.Max();

    int part2Answer = Enumerable.Range(puzzleInput.Min(), puzzleInput.Count + 1).Except(puzzleInput).FirstOrDefault(-1);

    Console.WriteLine($"Part 1: The highest seat ID on a boarding pass is {part1Answer}.");
    Console.WriteLine($"Part 2: My seat is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}