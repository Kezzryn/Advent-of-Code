using AoC_2024_Day_18;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    FallingBytes fallingBytes = new(puzzleInput, 1024, 71);
    
    fallingBytes.A_Star();
    int part1Answer = fallingBytes.NumSteps;

    string part2Answer = "";
    for (int i = 1024; i < puzzleInput.Length; i++)
    {
        fallingBytes.AddMapBlock(puzzleInput[i]);
        if(!fallingBytes.A_Star())
        {
            part2Answer = puzzleInput[i];
            break;
        }
    }

    Console.WriteLine($"Part 1: It will take {part1Answer} steps to get out.");
    Console.WriteLine($"Part 2: The first blocking byte is at {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}