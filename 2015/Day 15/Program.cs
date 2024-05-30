using AoC_2015_Day_15;

try
{
    const int CALORIE_TARGET = 500;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    CookieMaker cookie = new(puzzleInput);

    int part1Answer = cookie.GetBestCookie();

    cookie.TargetCalories = CALORIE_TARGET;
    int part2Answer = cookie.GetBestCookie();

    Console.WriteLine($"Part 1: Our best tasting cookie is rated: {part1Answer}.");
    Console.WriteLine($"Part 2: Our best {CALORIE_TARGET} calorie cookie is rated only: {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}