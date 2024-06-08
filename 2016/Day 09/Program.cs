static long GetDecompressionLength(string compressed, int version)
{
    int cursor = 0;
    int nextIndex = 0;
    long returnValue = 0;

    while (cursor < compressed.Length)
    {
        nextIndex = compressed.IndexOf('(', cursor);
        if (nextIndex == -1) // end of markers check. 
        {
            returnValue += compressed.Length - cursor;
            cursor = compressed.Length;
            continue;
        }

        returnValue += nextIndex - cursor; // count all the strings before the marker.

        cursor = nextIndex;

        // assumption, we have a ) after a ( 
        nextIndex = compressed.IndexOf(')', cursor) + 1; // move after the match. 

        // Get marker instructions (axb) where a length of the section and b is the number of repeats. Strip the () from the marker instruction.  
         IEnumerable<int> marker = compressed[(cursor + 1)..(nextIndex - 1)].Split('x').Select(int.Parse);
        cursor =  marker.First() + nextIndex;

        returnValue += version switch
        {
            1 => marker.Aggregate((x, y) => x * y),
            2 => marker.Last() * GetDecompressionLength(compressed[nextIndex..cursor], version),
            _ => throw new NotImplementedException()
        };
    }

    return returnValue;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    long part1Answer = GetDecompressionLength(puzzleInput, 1);
    long part2Answer = GetDecompressionLength(puzzleInput, 2);

    Console.WriteLine($"Part 1: The uncompressed size using V1 decoding is: {part1Answer}.");
    Console.WriteLine($"Part 2: The uncompressed size using V2 decoding is: {part2Answer}.");
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}