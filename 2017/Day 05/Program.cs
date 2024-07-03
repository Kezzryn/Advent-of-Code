static int HouseOfPain(bool doPart2 = false)
{
    //Do the read here in 'cause it gets modified.
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToList();

    int cursor = 0;
    int nextStep = 0;
    int returnValue = 0;

    do
    {
        returnValue++;
        nextStep = cursor + puzzleInput[cursor];
        puzzleInput[cursor] += doPart2 && puzzleInput[cursor] >= 3 ? -1 : 1;
        cursor = nextStep;
    }
    while (cursor < puzzleInput.Count);

    return returnValue;
}

try
{
    const bool JUMP_AROUND = true;

    int part1Answer = HouseOfPain();
    int part2Answer = HouseOfPain(JUMP_AROUND);

    Console.WriteLine($"Part 1: The number of one move jumps to escape is {part1Answer}.");
    Console.WriteLine($"Part 2: The number of three move jumps to escape is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}