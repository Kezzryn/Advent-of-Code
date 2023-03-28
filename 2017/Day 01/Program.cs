try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    int part1Answer = 0;
    int part2Answer = 0;


    for (int i = 0; i < puzzleInput.Length; i++)
    {
        // use modulo math to wrap when we hit the end, or when finding the midpoint. 
        int nextChar = (i + 1) % puzzleInput.Length;

        if (puzzleInput[i] == puzzleInput[nextChar]) 
        {
            part1Answer += int.Parse(puzzleInput[i].ToString());
        }

        nextChar = (i + (puzzleInput.Length /2)) % puzzleInput.Length;

        if (puzzleInput[i] == puzzleInput[nextChar])
        {
            part2Answer += int.Parse(puzzleInput[i].ToString());
        }
    }

    Console.WriteLine($"Part 1: The captcha answer is: {part1Answer}.");

    Console.WriteLine($"Part 2: The captcha of halfway digit is {part2Answer}.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}