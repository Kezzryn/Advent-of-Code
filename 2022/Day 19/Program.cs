using AoC_2022_Day_19;

try
{
    const int PART1_TIME = 24;
    const int PART2_TIME = 32;
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int answerPart1 = puzzleInput
        .Select(x => new RoboBlueprint(x, PART1_TIME).GetQualityLevel())
        .ToList()
        .Sum();

    int answerPart2 = puzzleInput
        .Take(3)
        .Select(x => new RoboBlueprint(x, PART2_TIME).GetNumGeodes())
        .ToList()
        .Aggregate((x,y) => x * y);

    Console.WriteLine($"Part 1: The sum of the quality Levels is: {answerPart1}");
    Console.WriteLine($"Part 2: The sum of the quality Levels is: {answerPart2}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
