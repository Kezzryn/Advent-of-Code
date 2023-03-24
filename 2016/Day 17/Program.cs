using AoC_2016_Day_17;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);


    Map theMap = new(puzzleInput);
    theMap.A_Star(theMap.DefaultStart, theMap.DefaultEnd, out _);

    Console.WriteLine($"Part 1: The final path is {theMap.History}.");

    int part2Answer = theMap.A_Slow(theMap.DefaultStart, theMap.DefaultEnd);
    Console.WriteLine($"Part 2: The longest path is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}