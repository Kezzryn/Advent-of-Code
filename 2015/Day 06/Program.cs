using AoC_2015_Day_06;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    XmasLightGrid part1Answer = new(XmasLightGrid.Ruleset.Toggle);
    XmasLightGrid part2Answer = new(XmasLightGrid.Ruleset.Brighten);

    foreach (string instruction in puzzleInput)
    {
        part1Answer.Instruction(instruction);
        part2Answer.Instruction(instruction);
    }

    Console.WriteLine($"Part 1: There are {part1Answer.NumLit()} lights on.");
    Console.WriteLine($"Part 2: There is {part2Answer.Luminosity()} brightness.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}