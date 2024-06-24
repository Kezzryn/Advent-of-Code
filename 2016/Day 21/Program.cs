using AoC_2016_Day_21;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string PART_ONE_BASE = "abcdefgh";
    const string PART_TWO_BASE = "fbgdceah";
    const bool DO_REVERSE = true;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Scrambler part1Scrambler = new(puzzleInput);
    part1Scrambler.ScrambleText(PART_ONE_BASE);
    Console.WriteLine($"Part 1: The result of scrambling {PART_ONE_BASE} is {part1Scrambler}");

    Scrambler part2Scrambler = new(puzzleInput, DO_REVERSE);
    part2Scrambler.ScrambleText(PART_TWO_BASE);
    Console.WriteLine($"Part 2: The result of unscrambling {PART_TWO_BASE} is {part2Scrambler}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
