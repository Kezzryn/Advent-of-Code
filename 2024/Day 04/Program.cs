try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT).Replace(CRLF, "");

    int rowLength = (int)Math.Sqrt(puzzleInput.Length);

    static bool IsXMAS(string input) => input == "XMAS" || input == "SAMX";

    static bool IsX_MAS(char[] input)
    {
        if (input.Any(x => x == 'X' || x == 'A')) return false;
        if (input[0] == input[1] || input[2] == input[3]) return false;

        return true;
    }

    int part1Answer = 0;
    int part2Answer = 0;

    Dictionary<int, int[]> rules = new()
    {
        { 0, [0, 1, 2, 3] }, //horiz
        { 1, [0, rowLength, 2 * rowLength, 3 * rowLength] }, //vertical
        { 2, [0, rowLength + 1, (2 * rowLength) + 2, (3 * rowLength) + 3] }, //diag forward
        { 3, [0, rowLength - 1, (2 * rowLength) - 2, (3 * rowLength) - 3] },  //diag backward
    };

    char[] chunk = new char[4];
    for (int cursor = 0; cursor < puzzleInput.Length; cursor++)
    {
        int x = cursor % rowLength;

        if (puzzleInput[cursor] == 'M') continue; // sanity check. Can't start with this character.
        if (puzzleInput[cursor] == 'A')
        {
            int[] part2Rule = [-rowLength - 1, rowLength + 1, -rowLength + 1, rowLength - 1];
            // Order by kitty-corner for the IsX_MAS function.

            if (x == 0 || x >= rowLength) continue;
            if (cursor < rowLength) continue;
            if (cursor >= puzzleInput.Length - rowLength) continue;

            for (int i = 0; i < part2Rule.Length; i++)
            {
                chunk[i] = puzzleInput[cursor + part2Rule[i]];
            }
            if (IsX_MAS(chunk)) part2Answer++;
        }
        else
        {
            foreach ((int ruleID, int[] offsets) in rules)
            {
                if (offsets[3] + cursor >= puzzleInput.Length) continue; //valid for vertical and pre-check for slashes. 
                if ((ruleID == 0 || ruleID == 2) && (x + 3) >= rowLength) continue; //valid for horiz and forward slash
                if (ruleID == 3 && x < 3) continue; //valid for back diag slash.  

                for (int i = 0; i < offsets.Length; i++)
                {
                    chunk[i] = puzzleInput[cursor + offsets[i]];
                }
                if (IsXMAS(new(chunk))) part1Answer++;
            }
        }
    }

    Console.WriteLine($"Part 1: XMAS appears {part1Answer} times.");
    Console.WriteLine($"Part 2: X-MAS appears {part2Answer} times.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}