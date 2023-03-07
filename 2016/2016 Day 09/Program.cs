try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    int cursor = 0;
    int nextIndex = 0;

    int part1Answer = 0;

    while (cursor < puzzleInput.Length)
    {
        nextIndex = puzzleInput.IndexOf('(', cursor);
        if (nextIndex == -1)
        {
            part1Answer += puzzleInput.Length - cursor;
            cursor = puzzleInput.Length;
            continue;
        } 

        part1Answer += nextIndex - cursor;

        cursor = nextIndex; // set cursor to the start of '('

        nextIndex = puzzleInput.IndexOf(')', cursor) + 1; // move after the match. 

        var s = puzzleInput[cursor..nextIndex].Trim("()".ToCharArray()).Split('x').Select(int.Parse);
        part1Answer += s.Aggregate((x, y) => x * y);

        cursor = s.First() + nextIndex;
    }
    
    Console.WriteLine($"Part 1: The uncompressed size is: {part1Answer}.");
    //    Console.WriteLine($"Part 2: {part2Answer} of IPs that support SSL.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}