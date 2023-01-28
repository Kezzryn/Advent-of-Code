using AoC_2022_Day_11;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    MonkeyTroupe troupePart1 = new(PUZZLE_INPUT);
    MonkeyTroupe troupePart2 = new(PUZZLE_INPUT, "PART2");

    troupePart1.DoRound(20);
    Console.WriteLine($"Part 1: {troupePart1.GetMonkeyBusiness()}");

    troupePart2.DoRound(10000);
    Console.WriteLine($"Part 2: {troupePart2.GetMonkeyBusiness()}");
}

catch (Exception e)
{
    Console.WriteLine(e);
}
