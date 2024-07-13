using AoC_2017_TabletVM;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    TabletVM DuoZero = new(puzzleInput);
    DuoZero.SetRegister('p', 0);

    TabletVM DuoOne = new(puzzleInput);
    DuoOne.SetRegister('p', 1);

    DuoZero.InputBuffer = DuoOne.OutputBuffer;
    DuoOne.InputBuffer = DuoZero.OutputBuffer;

    do
    {
        DuoZero.Step();
        DuoOne.Step();
    } while (DuoZero.CurrentState == State.Running || DuoOne.CurrentState == State.Running);
        
    long part1Answer = DuoZero.FirstRCVValue;
    long part2Answer = DuoOne.NumSentMessages;
    
    Console.WriteLine($"Part 1: The first non-zero rcv value is {part1Answer}.");
    Console.WriteLine($"Part 2: Program One sent a value {part2Answer} times.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}