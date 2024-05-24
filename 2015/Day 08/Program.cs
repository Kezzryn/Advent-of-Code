using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    int part1Answer = puzzleInput.Sum(x => x.Length - Regex.Unescape(x[1..^1]).Length);
    int part2Answer = puzzleInput.Sum(x => $"\"{Regex.Escape(x).Replace("\"", "\\\"")}\"".Length - x.Length);

    Console.WriteLine($"Part 1: The difference of the raw size and the un-escaped size is {part1Answer}.");
    Console.WriteLine($"Part 2: The difference between the escaped size and the raw size is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}




