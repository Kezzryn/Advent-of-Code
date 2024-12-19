using AoC_2024_Day_17;
using System.Data;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    long initialA = 0;
    long initialB = 0;
    long initialC = 0;
    List<long> prog = [];

    foreach (string s in puzzleInput)
    {
        if (s.StartsWith("Register A")) initialA = long.Parse(s.Split(':').Last());
        if (s.StartsWith("Register B")) initialB = long.Parse(s.Split(':').Last());
        if (s.StartsWith("Register C")) initialC = long.Parse(s.Split(':').Last());
        if (s.StartsWith("Program")) prog = s.Split(':').Last().Split(',').Select(long.Parse).ToList();
    }

    ThreeBitVM tbvm = new(prog, initialA, initialB, initialC);
    tbvm.Run();

    string part1Answer = String.Join(",", tbvm.OutputQueue);

    static long SeeSharpVersion(long aStart, out long aEnd, bool doFullLoop = false)
    {
        long a = aStart;

        long returnValue = 0;
        do
        {
            long b = a % 8;
            b ^= 1;
            long c = a >> (int)b;
            b ^= 5;
            b ^= c;
            returnValue *= 10;
            returnValue += (b % 8);
            aEnd = a;
            a >>= 3;
        } while (a != 0 && doFullLoop);

        return returnValue;
    }

    List<long> targets = new(prog);
    Queue<(long, int)> queue = new();
    queue.Enqueue((0, targets.Count - 1));
    long part2Answer = long.MaxValue;
    while (queue.TryDequeue(out (long aValue, int targetIndex) q))
    {
        for (long i = 0; i < 8; i++)
        {
            long newA = (q.aValue << 3) | i; 
            long outputValue = SeeSharpVersion(newA, out long aRegister);

            if (outputValue == targets[q.targetIndex])
            {
                if (q.targetIndex == 0)
                {
                    if (newA < part2Answer) part2Answer = newA;
                }
                else
                {
                    queue.Enqueue((aRegister, q.targetIndex - 1));
                }
            }
        }
    }



    Console.WriteLine($"Part 1: With the default settings, the program outputs: {part1Answer}");
    Console.WriteLine($"Part 2: The lowest initial value for register A that creates a copy of the program is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
};