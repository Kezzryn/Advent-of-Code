using AoC_2017_Day_18; 

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Duet DuoZero = new(puzzleInput, 0);
    Duet DuoOne = new(puzzleInput, 1);

    bool isDone = false;
    do
    {
        while (DuoZero.GetProgramOutput(out long outA)) DuoOne.SetProgramInput(outA);
        while (DuoOne.GetProgramOutput(out long outB)) DuoZero.SetProgramInput(outB);

        DuoZero.Run();
        DuoOne.Run();

        if (DuoZero.CurrentState == State.Halted && DuoOne.CurrentState == State.Halted) isDone = true;

        if (DuoZero.CurrentState == State.Paused_For_Input && 
            (DuoOne.CurrentState == State.Halted || (DuoOne.CurrentState == State.Paused_For_Input && !DuoZero.HasOutput())))
        {
            isDone = true;
        }

        if (DuoOne.CurrentState == State.Paused_For_Input &&
            (DuoZero.CurrentState == State.Halted || (DuoZero.CurrentState == State.Paused_For_Input && !DuoOne.HasOutput())))
        {
            isDone = true;
        }


    } while (!isDone);
        
    long part1Answer = DuoZero.FirstRCVValue();
    long part2Answer = DuoOne.NumSentMessages();
    
    Console.WriteLine($"Part 1: The first non-zero rcv value is {part1Answer}.");
    Console.WriteLine($"Part 2: Program One sent a value {part2Answer} times.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}