using AoC_2015_Day_15;

try
{
    const int MAX_CALORIES = 500;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    CookieMaker cookiePart1 = new(puzzleInput);
    CookieMaker cookiePart2 = new(puzzleInput, MAX_CALORIES);

    Console.WriteLine($"Part 1: Our best tasting cookie is rated: {cookiePart1.GetBestCookie()}.");
    Console.WriteLine($"Part 2: Our best {MAX_CALORIES} calorie cookie is rated only: {cookiePart2.GetBestCookie()}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}