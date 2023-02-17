using AoC_2022_Day_23;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART1_NUM_ROUNDS = 10;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Grove grovePart1 = new(puzzleInput);
    Grove grovePart2 = new(puzzleInput);

    grovePart1.SpreadElves(PART1_NUM_ROUNDS);
    Console.WriteLine($"Total area covered at round 10: {grovePart1.GetArea()}");

    Console.WriteLine($"Number of rounds till we're all spread out: {grovePart2.SpreadElves()}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
