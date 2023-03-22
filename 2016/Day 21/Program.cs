using AoC_2016_Day_21;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string PART_ONE_BASE = "abcdefgh";
    const string PART_TWO_BASE = "bgfacdeh";
    const bool DO_REVERSE = true;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Scrambler part1Scrambler = new(PART_ONE_BASE);
    Scrambler part2Scrambler = new(PART_TWO_BASE, DO_REVERSE);
    
    foreach (string line in puzzleInput)
    {
        part1Scrambler.Step(line);
    }

    foreach (string line in puzzleInput.Reverse())
    {
        part2Scrambler.Step(line);
    }



    Console.WriteLine($"Part 1: The result of scrambling {PART_ONE_BASE} is {part1Scrambler}");
    Console.WriteLine($"Part 1: The result of unscrambling {PART_TWO_BASE} is {part2Scrambler}");



}
catch (Exception e)
{
    Console.WriteLine(e);
}
