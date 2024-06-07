using AoC_2015_Day_18;

try
{
    const bool DO_STUCK_LIGHTS = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    ElfLights part1Answer = new(puzzleInput);
    ElfLights part2Answer = new(puzzleInput);

    for (int i = 1; i <= 100; i++)
    {
        part1Answer.Step();
        part2Answer.Step(DO_STUCK_LIGHTS);
    }

    Console.WriteLine($"Part 1: Our light grid has {part1Answer.CountCells} lights on after 100 steps.");
    Console.WriteLine($"Part 2: After getting some lights stuck on, we now have {part2Answer.CountCells} lights after 100 steps.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}