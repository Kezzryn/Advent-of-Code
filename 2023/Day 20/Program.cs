using AoC_2023_Day_20;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    /*
     * The nodes form four banks of 12 bits. 4096 values each.
     * The basic flip-flop act as an accumulator, in effect, counting up with each low pulse delivered to the "head" of the graph spine.
     * _most_ of the accumulators feeds a conjunction module, in effect having it filter
        complication: the conjunction feeds back into the accumulator rack.
     * hz, qh, pv, xm all act as inverters, flipping the low/high signal from a bank to feed kh.
     * when an "inverter" gets a low signal, which is to say the bank is all HIGH, it sends a HIGH to kh.
     * When kh detects signals from all each bank,
     * NB: This process is detailed in an example in the puzzle.
     */

    const int PRESS_IT_LOTS = 1000;

    Button theButton = new(puzzleInput);

    long part1Answer = theButton.PressPart1(PRESS_IT_LOTS);
    long part2Answer = theButton.PressPart2();
    
    Console.WriteLine($"Part 1: The product of the number of high and low signals after {PRESS_IT_LOTS} button pushes is {part1Answer}.");

    Console.WriteLine($"Part 2: It will take {part2Answer} button pushes to start the machine.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}