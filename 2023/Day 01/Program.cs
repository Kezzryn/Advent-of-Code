try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    char[] numbers = "1234567890".ToArray();

    int part1Answer = puzzleInput.Select(x => ((x[x.IndexOfAny(numbers)] - '0') * 10) + (x[x.LastIndexOfAny(numbers)] - '0')).Sum();

    // maintain first and last letter for the "eightwo" case.
    Dictionary<string, string> replaceValues = new()
    {
        {"one", "o1e"}, 
        {"two", "t2o"},
        {"three", "t3e"},
        {"four", "f4r"},
        {"five", "f5e" },
        {"six", "s6x" },
        {"seven", "s7n" },
        {"eight", "e8t" },
        {"nine", "n9e" }
    };

    string cleanUp(string s)
    {
        foreach (string to_replace in replaceValues.Keys)
        {
            s = s.Replace(to_replace, replaceValues[to_replace]);
        }
        return s;
    }

    int part2Answer = puzzleInput.Select(cleanUp).Select(x => ((x[x.IndexOfAny(numbers)] - '0') * 10) + (x[x.LastIndexOfAny(numbers)] - '0')).Sum();

    Console.WriteLine($"Part 1: The sum of the digit only calibration values is {part1Answer}.");
    Console.WriteLine($"Part 2: When accounting for words, the calibration value sum is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}