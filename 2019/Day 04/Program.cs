static IEnumerable<int> GetDigits(int source)
{
    int individualFactor = 0;
    int tennerFactor = Convert.ToInt32(Math.Pow(10, source.ToString().Length));
    do
    {
        source -= tennerFactor * individualFactor;
        tennerFactor /= 10;
        individualFactor = source / tennerFactor;

        yield return individualFactor;
    } while (tennerFactor > 1);
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split('-').Select(int.Parse).ToArray();

    List<int> part1Answer = new();
    List<int> part2Answer = new();
    List<int> pairs = new();

    for (int i = puzzleInput[0]; i <= puzzleInput[1]; i++)
    {
        var digits = GetDigits(i).ToList();
        bool adjacent = digits.Take(digits.Count - 1).Where((item, index) => digits[index + 1] == item).Any();
        bool ascending = digits.Take(digits.Count - 1).Where((item, index) => item <= digits[index + 1]).Count() == digits.Count - 1;

        if (adjacent && ascending)
        {
            part1Answer.Add(i);

            pairs.Clear();
            for (int j = 0; j < digits.Count - 1; j++)
            {
                if (digits[j] == digits[j + 1]) pairs.Add((digits[j] * 10) + digits[j]);
            }

            if (pairs.Count > 0 && pairs.GroupBy(x => x).Where(x => x.Count() == 1).Any()) part2Answer.Add(i);
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer.Count} potential passwords.");
    Console.WriteLine($"Part 2: There are {part2Answer.Count} potential passwords with increased constraints.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}