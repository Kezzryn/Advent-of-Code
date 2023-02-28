using System.Text.RegularExpressions;

try
{
    const int TARGET_ROW = 0;
    const int TARGET_COL = 1;
    const long START_CODE = 20151125;
    const long MULTIPLIER = 252533;
    const long DIVISOR = 33554393;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    
    int[] codeSearchTarget = Regex.Matches(puzzleInput, "\\d+").Select(x => int.Parse(x.Value)).ToArray();

    static long calcCode(long value) => value * MULTIPLIER % DIVISOR;

    int row = 1;
    int col = 1;

    long code = START_CODE;

    while (!(row == codeSearchTarget[TARGET_ROW] && col == codeSearchTarget[TARGET_COL]))
    {
        row--;
        col++;

        if (row <= 0)
        {
            row = col;
            col = 1;
        }

        code = calcCode(code);
    } 

    Console.WriteLine($"Part 1: The code for the weather machine is: {code}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}