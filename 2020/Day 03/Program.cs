try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int TreeHitter(int xSlope, int ySlope)
    {
        const char TREE = '#';
        int returnValue = 0;

        int x = 0;
        for (int y = ySlope; y < puzzleInput.Length; y += ySlope)
        {
            x = (x + xSlope) % puzzleInput[y].Length;
            if (puzzleInput[y][x] == TREE) returnValue++;
        }

        return returnValue;
    }

    int part1Answer = TreeHitter(3, 1);

    List<(int x, int y)> slopeList = new()
    {
        (1,1),  //Right 1, down 1.
        (3,1),  //Right 3, down 1. (This is the slope you already checked.)
        (5,1),  //Right 5, down 1.
        (7,1),  //Right 7, down 1.
        (1,2)   //Right 1, down 2.
    };

    int part2Answer = slopeList.Select(s => TreeHitter(s.x, s.y)).Aggregate((a, b) => a * b);

    Console.WriteLine($"Part 1: With the given slope, {part1Answer} trees will be hit.");
    Console.WriteLine($"Part 2: The product of all the trees hit is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}