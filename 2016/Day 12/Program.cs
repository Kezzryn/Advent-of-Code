using BKH.AoC_AssemBunny;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    AssemBunny asmBnyVM = new(puzzleInput);
    asmBnyVM.Run();
    int part1Answer = asmBnyVM.GetRegister('a');

    asmBnyVM.Reset();
    asmBnyVM.SetRegister('c', 1);

    asmBnyVM.Run();
    int part2Answer = asmBnyVM.GetRegister('a');

    Console.WriteLine($"Part 1: The monorail code is: {part1Answer}");
    Console.WriteLine($"Part 2: After setting the ignition key the new monorail code is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
