using BKH.AoC_AssemBunny;

try
{
    const int PART1_VALUE = 7;
    const int PART2_VALUE = 12;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    AssemBunny asmBny = new(puzzleInput);
    asmBny.SetRegister('a', PART1_VALUE);
    asmBny.Run();
    int part1Answer = asmBny.GetRegister('a');

    Console.WriteLine($"Part 1: The safe code is: {part1Answer}");

    /* Program is made into two parts.  First part calculcates the factorial of the number in register A.
     * the tgl command resets commands in the last part of the loop and provides an escape hatch when C reaches 1.
     * line 1 is initializaton, run once. 
     * 2 - 19, factorial loop
     * b holds a slowly decreasing value of a
     * 2-5 initalizes values. 
     * 6-10  multiplies c by d 
     * 11-13 sets up the next loop 
     * 14-16 adds register d to reg c
     * 17 is the tlg test 
     * line 18 is a jump target for line 19. 
     * line 19 is reset by tlg to break the loop when c == 2,and is immediatly overwritten by line 20 
     * 20-21 set c and d to 72 and 77 respectivly 
     * 22-26 multiplies 72 and 77, adding the result to a 
     * lines 25, 23 and 21 are all reset by tgl, but not run until after line 19 is reset. 
     * 
     * The entire program can be expressed as: f(x) = x! + 5544;
    */

    int part2Answer = Enumerable.Range(1, PART2_VALUE).Aggregate((x, y) => x * y) + 5544;

    Console.WriteLine($"Part 2: After resetting register a, the safe code is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}