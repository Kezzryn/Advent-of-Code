using AoC_2020_HandHeldVM;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    HandHeldVM handheldVM = new(puzzleInput);
    
    handheldVM.Run();
    
    int part1Answer = handheldVM.GetAccumulator;

    do
    {
        handheldVM.TryFixCode();
    }
    while (handheldVM.Run() == State.Stopped);

    int part2Answer = handheldVM.GetAccumulator;


    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}