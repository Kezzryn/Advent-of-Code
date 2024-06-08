using System.Text.RegularExpressions;

try
{
    const int DO_ROW = 0;
    const int DO_COL = 1;
    const int MAX_COL = 50;
    const int MAX_ROW = 6;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    bool[,] theGrid = new bool[MAX_COL, MAX_ROW]; // extends left and down. 

    int DrawScreen()
    {
        int countLit = 0;
        Console.SetCursorPosition(0,0);
        for (int row = 0; row < MAX_ROW; row++)
        {
            for (int col = 0; col < MAX_COL; col++)
            {
                if (theGrid[col, row])
                {
                    countLit++;
                    Console.Write('#');
                }
                else
                {
                    Console.Write(' '); 
                }
            }
            Console.WriteLine();
        }
        return countLit;
    }

    void AddRectangle(int w, int t) //w wide, t tall.  start top left corner 
    {
        for (int col = 0; col < w; col++)
        {
            for (int row = 0; row < t; row++)
            {
                theGrid[col, row] = true;
            }
        }
    }

    void RotateRowOrCol(int rc, int value, int rowOrCol) 
    {
        // Inital version had two nearly identical functions. 
        // This variant places the needed row/col data in coords variable depending on which way we're rotating.

        int gridLength = rowOrCol == DO_ROW ? MAX_COL : MAX_ROW;
        bool[] oldData = new bool[gridLength];
        int[] coords = new int[2];

        coords[(rowOrCol + 1) % 2] = rc;

        // copy data out
        for (int i = 0; i < gridLength; i++)
        {
            coords[rowOrCol] = i;
            oldData[i] = theGrid[coords[0], coords[1]];
        }

        // paste data back in, overwriting
        for (int i = 0; i < gridLength; i++)
        {
            coords[rowOrCol] = ((i % gridLength) + value) % gridLength;
            theGrid[coords[0], coords[1]] = oldData[i];
        }
    }

    int part1Answer = 0;
    foreach (string instruction in puzzleInput)
    {
        List<int> nums = GetNumbers().Matches(instruction).Select(x => int.Parse(x.Value)).ToList();

        if (instruction.StartsWith("rect"))
            AddRectangle(nums[0], nums[1]);
        else if (instruction.StartsWith("rotate column"))
            RotateRowOrCol(nums[0], nums[1], DO_COL);
        else if (instruction.StartsWith("rotate row"))
            RotateRowOrCol(nums[0], nums[1], DO_ROW);
        else
            throw new NotImplementedException($"Unknown instruction.{instruction}");
        
        DrawScreen();
        Thread.Sleep(75);
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