using AoC_2017_Day_18; 

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Duet DuoA = new(puzzleInput, 0);
    Duet DuoB = new(puzzleInput, 1);

    bool isDone = false;
    do
    {
        DuoA.Run();
        DuoB.Run();

        if (DuoA.GetProgramOutput(out long outA)) DuoB.SetProgramInput(outA);
        if (DuoB.GetProgramOutput(out long outB)) DuoA.SetProgramInput(outB);

        if (DuoA.CurrentState != State.Running && DuoB.CurrentState != State.Running)
        {
            Console.WriteLine($"{DuoA.CurrentState} {DuoB.CurrentState}");
            isDone = true;
        }

    } while (!isDone);
        
    long part1Answer = DuoA.FirstRCVValue();
    long part2Answer = DuoB.NumSentMessages();
    
    Console.WriteLine($"Part 1: The first non-zero rcv value is {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}