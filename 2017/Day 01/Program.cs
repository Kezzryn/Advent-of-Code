try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    char[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).ToCharArray();

    int part1Answer = puzzleInput.Where((x, i) => x == puzzleInput[i + 1 >= puzzleInput.Length ? 0 : i + 1]).Sum(x => x - '0');
    int part2Answer = puzzleInput.Where((x, i) => x == puzzleInput[(i + (puzzleInput.Length / 2)) % puzzleInput.Length]).Sum(x => x - '0');

    Console.WriteLine($"Part 1: The captcha answer is: {part1Answer}.");
    Console.WriteLine($"Part 2: The captcha of halfway digit is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}