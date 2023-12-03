static int GetNumber(string input, int digitIndex)
{
    // sanity check. 
    if (!Char.IsAsciiDigit(input[digitIndex])) return -1;

    char[] chars = ".-#$%&*/@+=".ToCharArray();

    // Covers -1 for start of string errors.
    // Which I totally intended.  Honest.
    int startIndex = input.LastIndexOfAny(chars, digitIndex) + 1;

    int endIndex = input.IndexOfAny(chars, startIndex);
    if (endIndex == -1) endIndex = input.Length; // cover end of line matching.

    if (int.TryParse(input[startIndex..endIndex], out int returnValue)) return returnValue;

    return -1;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string SYMBOLS = "-#$%&*/@+=";
    const char GEAR = '*';

    List<(int X, int Y)> neighbors = new()
    {
        (-1, 1), ( 0, 1), ( 1, 1),
        (-1, 0),          ( 1, 0),
        (-1,-1), ( 0,-1), ( 1,-1),
    };

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int maxX = puzzleInput[0].Length - 1;
    int maxY = puzzleInput.Length - 1;

    int part1Answer = 0;
    int part2Answer = 0;

    foreach ((int X, int Y) in from charX in Enumerable.Range(0, puzzleInput[0].Length)
                               from charY in Enumerable.Range(0, puzzleInput.Length)
                               where SYMBOLS.Contains(puzzleInput[charY][charX])
                               select (charX, charY))
    {
        //Use hashset to avoid the case of potential duplicate reads of the same part number.
        //Thank goodness Eric was kind enough to not duplicate part numbers around a part.

        HashSet<int> partNumbers = neighbors
            .Select(n => (nX: n.X + X, nY: n.Y + Y))
            .Where(w => w.nX >= 0 && w.nX <= maxX && w.nY >= 0 && w.nY <= maxY)
                .Select(s => GetNumber(puzzleInput[s.nX], s.nY))
                .Where(num => num != -1)
                .ToHashSet();
        
        part1Answer += partNumbers.Sum();

        if (puzzleInput[Y][X] == GEAR && partNumbers.Count == 2)
        {
            part2Answer += partNumbers.Aggregate((x, y) => x * y);
        }
    }

    Console.WriteLine($"Part 1: The sum of all the part numbers is {part1Answer}.");
    Console.WriteLine($"Part 2: The sum of the gear ratios is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
