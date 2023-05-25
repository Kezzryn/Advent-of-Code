using AoC_Map;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART1 = false;
    const bool DO_PART2 = true;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    Map theMap = new(puzzleInput);

    theMap.A_Star(theMap.StartPoint, theMap.EndPoint, out int part1Answer, DO_PART1);
 
    // This works, but could be better.
    // One improvement would be to turn the portal-portal steps into a graph and operate off of those, similar to Day 18
    theMap.A_Star(theMap.StartPoint, theMap.EndPoint, out int part2Answer, DO_PART2);

    Console.WriteLine($"Part 1: The shortest distance from {theMap.StartPoint} to {theMap.EndPoint} is {part1Answer}.");
    Console.WriteLine($"Part 2: When recursing, the shortest distance is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}