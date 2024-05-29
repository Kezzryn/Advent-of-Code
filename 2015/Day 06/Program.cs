using AoC_2015_Day_06;
using System.Diagnostics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Stopwatch sw = Stopwatch.StartNew();
    XmasLightGrid part1Answer = new(XmasLightGrid.Ruleset.Toggle);
    XmasLightGrid part2Answer = new(XmasLightGrid.Ruleset.Brighten);

    //[turn [on]|[off]|[toggle] 643,149 through 791,320
    //starts empty. Not having an entry Empty is off/0. 

    foreach (string instruction in puzzleInput)
    {
        part1Answer.Instruction(instruction);
        part2Answer.Instruction(instruction);
    }

    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds);
    Console.WriteLine($"Part 1: There are {part1Answer.NumLit()} lights on.");
    Console.WriteLine($"Part 2: There is {part2Answer.Luminosity()} brightness.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}


