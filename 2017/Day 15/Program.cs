try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    const bool DO_PART_2 = true;

    const long PART_1_NUM_PAIRS = 40000000;
    const long PART_2_NUM_PAIRS = 5000000;

    long startValueGenA = int.Parse(puzzleInput[0][^3..]);
    long startValueGenB = int.Parse(puzzleInput[1][^3..]); 
    
    static long GeneratorSolve(long valueGenA, long valueGenB, long numPairs, bool doPart2 = false)
    {
        const long GEN_A_FACTOR = 16807;
        const long GEN_B_FACTOR = 48271;
        const long MODULO = 2147483647;

        Queue<ushort> queueGenA = new();
        Queue<ushort> queueGenB = new();

        long count = 0;
        long rv = 0;

        long divisorGenA = doPart2 ? 4 : 1;
        long divisorGenB = doPart2 ? 8 : 1;

        while (count < numPairs)
        {
            valueGenA = (valueGenA * GEN_A_FACTOR) % MODULO;
            if (valueGenA % divisorGenA == 0 )
            {
                queueGenA.Enqueue((ushort)valueGenA);
            }
            
            valueGenB = (valueGenB * GEN_B_FACTOR) % MODULO;
            if (valueGenB % divisorGenB == 0)
            {
                queueGenB.Enqueue((ushort)valueGenB);
            }

            if (queueGenA.Count > 0 &&  queueGenB.Count > 0)
            {
                count++;
                ushort GenA = queueGenA.Dequeue();
                ushort GenB = queueGenB.Dequeue();

                if (GenA == GenB) rv++;
            }
        }

        return rv;
    }

    long part1Answer = GeneratorSolve(startValueGenA, startValueGenB, PART_1_NUM_PAIRS);

    long part2Answer = GeneratorSolve(startValueGenA, startValueGenB, PART_2_NUM_PAIRS, DO_PART_2);

    Console.WriteLine($"Part 1: There are {part1Answer} matching pairs.");
    Console.WriteLine($"Part 2: There are {part2Answer} matching pairs when they try to syncronize.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}