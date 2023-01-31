using AoC_2015_Day_6;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);


    XmasLightGrid myGridPart1 = new (RuleSet.Toggle);
    XmasLightGrid myGridPart2 = new (RuleSet.Brighten);

    //[turn [on]|[off]|[toggle] 643,149 through 791,320
    //starts empty. Not having an entry Empty is off/0. 

    foreach (string instruction in puzzleInput)
    {
        myGridPart1.Instruction(instruction);
        myGridPart2.Instruction(instruction);
    }

    Console.WriteLine($"Part 1: There are {myGridPart1.NumLit()} lights on.");
    Console.WriteLine($"Part 2: There is {myGridPart2.Luminosity()} brightness.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}


