using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    int raw = 0;
    int part1 = 0;
    int part2 = 0;
    foreach (string input in puzzleInput)
    {
        string r = input[1..^1]; // strip the outer quotes
        string p1 = Regex.Unescape(r);
        string p2 = Regex.Escape(r);
        raw += input.Length;
        part1 += p1.Length; // -2 to handle the outer quotes that the Regex doesn't strip.
        part2 += p2.Length; // +4 to handle the missing outer quote that Reged doesn't escape.

        Console.WriteLine($"{input} {r} {p1} {p2}");
         
        Console.WriteLine($"{input} {raw} {part1} {part2}");

    }

    Console.WriteLine($"Part 1: The answer is {raw - part1}");
    Console.WriteLine($"Part 2: The answer is {part2 - part1}");




}
catch (Exception e)
{
    Console.WriteLine(e);
}




