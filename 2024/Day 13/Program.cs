using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const long PART2_OFFSET = 10000000000000;

    List<List<long>> puzzleInput = File.ReadAllText(PUZZLE_INPUT)
        .Split(Environment.NewLine + Environment.NewLine)
            .Select(s => Regex.Matches(s, "\\d+\\d+").Select(x => long.Parse(x.Value)).ToList()).ToList();

    static long SolveClawMachine(List<long> clawMachine, long offset = 0)
    {
        //indexes for machine.
        const int aX = 0;   const int aY = 1;   //X and Y for button A
        const int bX = 2;   const int bY = 3;   //X and Y for button B
        const int pX = 4;   const int pY = 5;   //X and Y for target prize.

        (long targetX, long targetY) = (clawMachine[pX] + offset, clawMachine[pY] + offset);

        long det = clawMachine[aX] * clawMachine[bY] - clawMachine[aY] * clawMachine[bX];

        long a = (targetX * clawMachine[bY] - targetY * clawMachine[bX]) / det;
        long b = (clawMachine[aX] * targetY - clawMachine[aY] * targetX) / det;

        if ((clawMachine[aX] * a) + (clawMachine[bX] * b) == targetX &&
            (clawMachine[aY] * a) + (clawMachine[bY] * b) == targetY) 
        {
            return (a * 3) + b;
        }

        return 0;
    }

    long part1Answer = puzzleInput.Sum(x => SolveClawMachine(x));
    long part2Answer = puzzleInput.Sum(x => SolveClawMachine(x, PART2_OFFSET));

    Console.WriteLine($"Part 1: The fewest tokens to spend to win all prizes is {part1Answer}.");
    Console.WriteLine($"Part 2: When adjusting for the offset, it'll cost {part2Answer} tokens.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}