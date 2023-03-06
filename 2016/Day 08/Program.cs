using System.Text.RegularExpressions;

try
{
    const int DO_ROW = 0;
    const int DO_COL = 1;
    const int MAX_COL = 50;
    const int MAX_ROW = 6;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    bool[,] theGrid = new bool[MAX_COL, MAX_ROW];

    int DrawScreen()
    {
        int countLit = 0;
        Console.SetCursorPosition(0,0);
        for (int row = 0; row <= theGrid.GetUpperBound(1); row++)
        {
            for (int col = 0; col <= theGrid.GetUpperBound(0); col++)
            {
                Console.Write(theGrid[col, row] ? '#' : ' ');
                countLit += theGrid[col, row] ? 1 : 0;
            }
            Console.WriteLine();
        }
        return countLit;
    }

    void rect(int w, int t) //a wide, b tall.  start top left corner. 
    {
        for (int col = 0; col < w; col++)
        {
            for (int row = 0; row < t; row++)
            {
                theGrid[col, row] = true;
            }
        }
    }

    void rotate(int rc, int value, int rowOrCol) 
    {
        // Inital version had two nearly identical functions. 
        // This variant places the needed row/col data in coords variable depending on which way we're rotating.

        int gridLength = theGrid.GetLength(rowOrCol);
        bool[] oldData = new bool[gridLength];
        int[] coords = new int[2];

        // copy data out
        for(int i = 0; i <= theGrid.GetUpperBound(rowOrCol); i++)
        {
            coords[rowOrCol] = i;
            coords[(rowOrCol + 1) % 2] = rc;

            oldData[i] = theGrid[coords[0], coords[1]];
        }

        // paste data back in, overwriting
        for (int i = 0; i <= theGrid.GetUpperBound(rowOrCol); i++)
        {
            coords[rowOrCol] = ((i % gridLength) + value) % gridLength;
            coords[(rowOrCol + 1) % 2] = rc;

            theGrid[coords[0], coords[1]] = oldData[i];
        }
    }

    int part1Answer = 0;
    foreach (string instruction in puzzleInput)
    {
        var nums = GetNumbers().Matches(instruction);

        int a = int.Parse(nums[0].Value);
        int b = int.Parse(nums[1].Value);

        if (instruction.Contains("rect")) rect(a, b);

        if (instruction.Contains("column")) rotate(a, b, DO_COL);

        if (instruction.Contains("row")) rotate(a, b, DO_ROW);

        // uncomment for cute animation. 
        // DrawScreen();
        // Thread.Sleep(75);
    }

    part1Answer = DrawScreen();
    Console.WriteLine();
    Console.WriteLine($"Part 1: The number of lit symbols is: {part1Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

partial class Program
{
    [GeneratedRegex("\\d+")]
    private static partial Regex GetNumbers();
}
