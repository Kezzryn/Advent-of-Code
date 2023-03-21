using System.Text;

try
{
    const int ROWS_PART_1 = 40;
    const int ROWS_PART_2 = 400000;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    static int solve(int rows, string firstLine)
    {
        const char safeSpace = '.';
        const char trapSpace = '^';
        int row_length = firstLine.Length;

        List<string> puzzle = new()
        {
            firstLine
        };

        for (int i = 0; puzzle.Count < rows; i++)
        {
            StringBuilder newRow = new();

            string lastRow = puzzle.Last(); // performance cache
            
            for (int j = 0; j < row_length; j++)
            {
                char left = (j == 0) ? safeSpace : lastRow[j - 1];
                char right = (j == row_length - 1) ? safeSpace : lastRow[j + 1];

                newRow.Append(left == right ? safeSpace : trapSpace);
            }
            puzzle.Add(newRow.ToString());
        }

        return puzzle.Select(x => x.ToCharArray().Count(c => c == safeSpace)).Sum();
    }

    int part1Answer = solve(ROWS_PART_1, puzzleInput);
    Console.WriteLine($"Part 1: The number of safe spaces in {ROWS_PART_1} is {part1Answer}.");

    int part2Answer = solve(ROWS_PART_2, puzzleInput);
    Console.WriteLine($"Part 2: The number of safe spaces in {ROWS_PART_2} is {part2Answer}.");

}

catch (Exception e)
{
    Console.WriteLine(e);
}
