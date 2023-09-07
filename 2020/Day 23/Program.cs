using AoC_2020_Day_23;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    const int NUM_ROUNDS_PART_1 = 100;
    const int NUM_ROUNDS_PART_2 = 10_000_000;
    const int NUM_CUPS_PART_2 = 1_000_000;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    Mixer crabCupsPart1 = new(puzzleInput);
    crabCupsPart1.Mix(NUM_ROUNDS_PART_1);
    
    Console.WriteLine($"Part 1: After {NUM_ROUNDS_PART_1} shuffles the order of the labels on the cups after cup 1 is: {crabCupsPart1.Part1Answer()}");
 
    Mixer crabCupsPart2 = new(puzzleInput, NUM_CUPS_PART_2);
    crabCupsPart2.Mix(NUM_ROUNDS_PART_2);
    Console.WriteLine($"Part 2: The product of the two cups clockwise from cup 1 is: {crabCupsPart2.Part2Answer()}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}