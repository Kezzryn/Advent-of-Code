using AoC_2017_Day_10;
using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int NUM_PART2_ROUNDS = 64;

    string puzzleInputPart2 = File.ReadAllText(PUZZLE_INPUT);
    int[] puzzleInputPart1 = puzzleInputPart2.Split(',').Select(int.Parse).ToArray();

    StringBuilder sb = new(); 
    sb.Append(puzzleInputPart2);
    sb.Append((char)17);
    sb.Append((char)31);
    sb.Append('I');
    sb.Append('/');
    sb.Append((char)23);
    puzzleInputPart2 = sb.ToString();

    KnotHash knot = new();

    foreach (int puzzleData in puzzleInputPart1)
    {
        knot.TieKnot(puzzleData);
    }
    int part1Answer = knot.Part1Answer();

    knot.ResetList();

    for (int i = 0; i < NUM_PART2_ROUNDS; i++) {
        foreach(char c in puzzleInputPart2)
        {
            knot.TieKnot(c);
        }
    }

    string part2Answer = knot.Part2Answer();

    Console.WriteLine($"Part 1: The product of the first two elements of the list is {part1Answer}.");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}