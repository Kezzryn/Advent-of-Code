using System.Text.RegularExpressions;

try
{
    const long START_CODE = 20151125;
    const long MULTIPLIER = 252533;
    const long DIVISOR = 33554393;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    var matches = Regex.Matches(puzzleInput, "\\d+").Select(x => int.Parse(x.Value));
    (int targetRow, int targetCol) = (matches.First(), matches.Last());

    /*
     *   1234
     * 1 0259
     * 2 148
     * 3 37
     * 4 6
     * 
     * Problem: What is the index of row 2, col 3? 
     * 
     * This can be calculated as triangular number.
     * https://en.wikipedia.org/wiki/Triangular_number
     */

    int triangle = ((targetRow + targetCol - 1) * (targetRow + targetCol) / 2) - targetRow;

    long part1Answer = START_CODE;
    for (int i = 0; i < triangle; i++)
    {
        part1Answer *= MULTIPLIER;
        part1Answer %= DIVISOR;
    }
    
    Console.WriteLine($"Part 1: The code for the weather machine is: {part1Answer}");   
}
catch (Exception e)
{
    Console.WriteLine(e);
}