int FindStartMarker(string inputData, int WindowSize)
{
    //Report the number of characters from the beginning of the buffer to the end of the first marker.
    //Where the marker is defined as all characters that are all different.

    int returnValue = -1;
    string block = "";

    for (int i = 0; i <= inputData.Length - WindowSize; i++)
    {
        block = inputData.Substring(i, WindowSize);

        if (block.Distinct().Count() == WindowSize)
        {
            returnValue = i + WindowSize;
            break;
        }
    }
    return returnValue;
}

try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 6.txt"; ;
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    Console.WriteLine($"Start of Packet: {FindStartMarker(puzzleInput, 4)}");
    Console.WriteLine($"Start of Message: {FindStartMarker(puzzleInput, 14)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}