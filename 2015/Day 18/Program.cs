using AoC_2015_Day_18;

try
{
    const bool STUCK_LIGHTS = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    GameOfLife gamePart1 = new(puzzleInput);
    GameOfLife gamePart2 = new(puzzleInput);

    for (int i = 1; i <= 100; i++)
    {
        gamePart1.Step();
        gamePart2.Step(STUCK_LIGHTS);
    }

    Console.WriteLine($"Part 1: Our light grid has {gamePart1.CountLights()} lights on after 100 steps.");
    Console.WriteLine($"Part 2: After getting some lights stuck on, we now have {gamePart2.CountLights()} lights after 100 steps.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}