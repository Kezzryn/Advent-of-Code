using BKH.AoC_AssemBunny;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    AssemBunny asmBny = new(puzzleInput);
    int part1Answer = -1;
    bool isDone = false;
    const int TARGET_MATCHES = 32;
    do
    {
        part1Answer++;
        asmBny.Reset();
        asmBny.SetRegister('a', part1Answer);

        int lastOutput = -1;
        int numMatches = 0;
        State state;

        do
        {
            state = asmBny.Run();
            if (state == State.Paused_For_Output)
            {
                int output = asmBny.OutputQueue.Dequeue();
                if (lastOutput == -1)
                {
                    lastOutput = output;
                } 
                else if ((lastOutput == 1 || lastOutput == 0) && lastOutput != output)
                {
                    lastOutput = output;
                    if (++numMatches >= TARGET_MATCHES)
                    {
                        isDone = true;
                        state = State.Halted;
                    }
                }
                else
                {
                    state = State.Halted;
                }
            }
        } while (state != State.Halted);
    } while (!isDone);

    Console.WriteLine($"Part 1: A signal has been found at index {part1Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}