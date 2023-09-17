try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int depthPart1 = 0;
    int depthPart2 = 0;
    int pos = 0;
    int aimPart2 = 0;


    foreach (string[] line in puzzleInput.Select(x => x.Split(' ')).ToArray())
    {
        int lineValue = int.Parse(line[1]);
        switch (line[0])
        {
            case "forward":
                pos += lineValue;
                depthPart2 += aimPart2 * lineValue;
                break;
            case "up":
                depthPart1 -= lineValue;
                aimPart2 -= lineValue;
                break;
            case "down":
                depthPart1 += lineValue;
                aimPart2 += lineValue;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    int part1Answer = pos * depthPart1;
    int part2Answer = pos * depthPart2;

    Console.WriteLine($"Part 1: The product of the final depth by the final position is {part1Answer}.");
    Console.WriteLine($"Part 2: When accounting for aim the product of the final depth and the final position is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}