using AoC_2016_Day_17;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);


    Map theMap = new(puzzleInput);
    theMap.A_Star(theMap.DefaultStart, theMap.DefaultEnd, out int numSteps);

    Console.WriteLine($"Part 1: The final path is {theMap.History}");

    //Console.WriteLine($"Part 2: The number of reachable positions within {PART2_STEPS} from the {theMap.DefaultStart()} is {part2Answer}.");
}

catch (Exception e)
{
    Console.WriteLine(e);
}