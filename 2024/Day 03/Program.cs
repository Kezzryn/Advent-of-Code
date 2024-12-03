using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    static int CalcMul(string input) => FindMul().Matches(input)
            .Sum(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value));

    int part1Answer = CalcMul(puzzleInput);
    int part2Answer = 0;
    
    string[] part2Parsed = puzzleInput.Split("do()");
    foreach(string chunk in part2Parsed)
    {
        int indexEnd = chunk.IndexOf("don't()");
        if (indexEnd == -1) indexEnd = chunk.Length - 1;
        part2Answer += CalcMul(chunk[..indexEnd]);
    }

    Console.WriteLine($"Part 1: The sum of all the mul() operations is: {part1Answer}.");
    Console.WriteLine($"Part 2: When accounting for the do/don't operations, the sum is: {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

partial class Program
{
    [GeneratedRegex("mul\\((\\d+),(\\d+)\\)")]
    private static partial Regex FindMul();
}