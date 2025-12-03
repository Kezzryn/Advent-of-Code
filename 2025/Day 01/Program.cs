try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer = 0;
    int part2Answer = 0;

    int dialValue = 50;

    foreach (string line in puzzleInput)
    {
        int rotation = int.Parse(line[1..]);

        part2Answer += rotation / 100; //count loops.
        rotation %= 100; //normalize.

        if (line[0] == 'L')
        {
            if (rotation <= dialValue)  //Do not pass zero.
            {
                dialValue -= rotation; 
            }
            else //passed zero. Do not count if we start on zero.
            {
                if (dialValue != 0) part2Answer++;
                dialValue = dialValue - rotation + 100;
            }
        }
        else
        {
            dialValue += rotation;
            if (dialValue > 100) part2Answer++; //looped.
            dialValue %= 100;   
        }
        if (dialValue == 0)
        {
            part1Answer++;
            part2Answer++;
        }
    }

    Console.WriteLine($"Part 1: The door code is {part1Answer}.");
    Console.WriteLine($"Part 2: When applying the 0x434C49434B password method, the door code is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}