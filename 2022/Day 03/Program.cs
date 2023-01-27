try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 3.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int scorePart1 = 0;
    int scorePart2 = 0;

    //part 1 
    //for each line 
    //cut the line in half, then search each line for the item that appears in each line. 
    //convert the char to a number, and sum. 

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        string left = puzzleInput[i][..(puzzleInput[i].Length / 2)];
        string right = puzzleInput[i][(puzzleInput[i].Length / 2)..];

        foreach (char c in left)
        {
            if (right.Contains(c))
            {
                scorePart1 += (c >= 'A' && c <= 'Z') ? 27 + (c - 'A') : 1 + (c - 'a');
                break;
            }
        }
    }


    //part 2
    //for each set of three lines 
    //find the char that appears in all lines
    //convert the char to a number, and sum. 

    for (int i = 0; i < puzzleInput.Length; i += 3)
    {
        foreach (char c in puzzleInput[i])
        {
            if (puzzleInput[i + 1].Contains(c) && puzzleInput[i + 2].Contains(c))
            {
                scorePart2 += (c >= 'A' && c <= 'Z') ? 27 + (c - 'A') : 1 + (c - 'a');
                break;
            }
        }
    }

    Console.WriteLine($"Part one: {scorePart1}");
    Console.WriteLine($"Part two: {scorePart2}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}