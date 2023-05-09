using AoC_2018_Day_17;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    Map theMap = new(puzzleInput);
    
    theMap.DripWater();
    theMap.DrawMap();

    int part1Answer = theMap.CountAllWater();
    int part2Answer = theMap.CountStandingWater();

    Console.WriteLine($"Part 1: The number of tiles the water can reach is: {part1Answer}.");
    Console.WriteLine($"Part 2: After the well dries up, the standing water that remains will be: {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}