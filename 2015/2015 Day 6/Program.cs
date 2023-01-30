using AoC_2015_Day_6;
using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);


    XmasLightGrid myGrid = new ();

    //[turn [on]|[off]|[toggle] 643,149 through 791,320

    // starts empty. Not having an entry Empty is off/false. 

    foreach (string instruction in puzzleInput)
    {
        // toggle 857,493 through 989,970
        // turn off 812,389 through 865,874
        // turn on 599,989 through 806,993
        int[] numbers = FindNumbers().Matches(instruction).Select(x => int.Parse(x.Value)).ToArray();

        if (instruction.StartsWith("toggle")) myGrid.Toggle(numbers[0], numbers[1], numbers[2], numbers[3]);
        
        if (instruction.StartsWith("turn on")) myGrid.TurnOn(numbers[0], numbers[1], numbers[2], numbers[3]);
        
        if (instruction.StartsWith("turn off")) myGrid.TurnOff(numbers[0], numbers[1], numbers[2], numbers[3]);
    }

    Console.WriteLine($"Part 1: There are {myGrid.NumLit()} lights on.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}
partial class Program
{
    [GeneratedRegex("\\d+")]
    private static partial Regex FindNumbers();
}

