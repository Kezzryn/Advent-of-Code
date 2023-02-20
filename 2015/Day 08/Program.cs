using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    int raw = 0;
    int part1 = 0;
    int part2 = 0;


    foreach (string input in puzzleInput)
    {
        string p1 = Regex.Unescape(input[1..^1]); // strip the outer quotes
        string p2 = $"\"{Regex.Escape(input).Replace("\"","\\\"")}\""; //we need to escape the outer quotes, and add new ones. The Regex does the rest. 

        raw += input.Length;
        part1 += p1.Length;
        part2 += p2.Length;
    }

    Console.WriteLine($"Part 1: The answer is {raw - part1}");
    Console.WriteLine($"Part 2: The answer is {part2 - raw}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}




