using AoC_2017_Day_10;
using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int NUM_PART2_ROUNDS = 64;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    List<int> puzzleInputPart1 = puzzleInput.Split(',').Select(int.Parse).ToList();

    StringBuilder sb = new(); 
    sb.Append(puzzleInput);
    sb.Append((char)17);
    sb.Append((char)31);
    sb.Append('I');
    sb.Append('/');
    sb.Append((char)23);
    List<int> puzzleInputPart2 = sb.ToString().Select(x => (int)x).ToList();

    KnotHash knot = new();
    puzzleInputPart1.ForEach(knot.TieKnot);
    int part1Answer = knot.Part1Answer();

    knot.ResetList();

    for (int i = 0; i < NUM_PART2_ROUNDS; i++)
    {
        puzzleInputPart2.ForEach(knot.TieKnot);
    }

    string part2Answer = knot.Part2Answer();

    Console.WriteLine($"Part 1: The product of the first two elements of the list is {part1Answer}.");
    Console.WriteLine($"Part 2: The knot hash of the puzzle input is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}