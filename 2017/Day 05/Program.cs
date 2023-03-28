int HouseOfPain(bool doPart2 = false)
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToArray();
    
    int cursor = 0;
    int nextStep = 0;
    int answer = 0;

    do
    {
        answer++;
        nextStep = cursor + puzzleInput[cursor];
        if (doPart2)
        {
            puzzleInput[cursor] += puzzleInput[cursor] >= 3 ? -1 : 1;
        }
        else 
        {
            puzzleInput[cursor]++; 
        }
        cursor = nextStep;
    }
    while (cursor < puzzleInput.Length);

    return answer;
}

try
{
    const bool JUMP_AROUND = true;
    int part1Answer = HouseOfPain();
    int part2Answer = HouseOfPain(JUMP_AROUND);

    Console.WriteLine($"Part 1: The number of jumps to escape is {part1Answer}.");

    Console.WriteLine($"Part 2: The number of jumps to escape is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}