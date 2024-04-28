using AoC_2021_Day_23;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Skip(2).Take(2).ToArray();

    //for part one we pass in a four room configuration where the last two rooms are already filled.
    int[] a = new int[4];
    Array.Fill(a, Amphipods.AMBER_POD);
    int[] b = new int[4];
    Array.Fill(b, Amphipods.BRONZE_POD);
    int[] c = new int[4];
    Array.Fill(c, Amphipods.COPPER_POD);
    int[] d = new int[4];
    Array.Fill(d, Amphipods.DESERT_POD);

    static int translateChar(char c)
    {
        return c switch
        { 
            'A' => Amphipods.AMBER_POD,
            'B' => Amphipods.BRONZE_POD,
            'C' => Amphipods.COPPER_POD,
            'D' => Amphipods.DESERT_POD,
            _ => throw new NotImplementedException()
        };
    }

    for (int i = 0; i < puzzleInput.Length;i++)
    {
        a[i] = translateChar(puzzleInput[i][3]);
        b[i] = translateChar(puzzleInput[i][5]);
        c[i] = translateChar(puzzleInput[i][7]);
        d[i] = translateChar(puzzleInput[i][9]);
    }

    static int GetTotalEnergy(Amphipods amphipods)
    {
        PriorityQueue<Amphipods, int> queue = new();
        Dictionary<Int128, int> inQueue = [];

        queue.Enqueue(amphipods, amphipods.Energy);

        int bestEnergy = int.MaxValue;

        List<(int, int)> moveHistory = [];

        while (queue.TryDequeue(out Amphipods ?element, out int totalEnergy))
        {
            if (element.AllAtHome() && element.Energy < bestEnergy)
            {
                moveHistory = new(element.MoveHistory);
                bestEnergy = element.Energy;
                continue;
            }

            foreach((int moveFrom, int moveTo) in element.BuildMoveList())
            {
                Amphipods nextStep = new(element);
                nextStep.MoveAmphipod(moveFrom, moveTo);
                if (nextStep.Energy < bestEnergy)
                {
                    Int128 hash = nextStep.GetHash();
                    if (inQueue.TryGetValue(hash, out int oldEnergy))
                    {
                        if (nextStep.Energy < oldEnergy)
                        {
                            queue.Enqueue(nextStep, nextStep.Energy);
                            inQueue[hash] = nextStep.Energy;
                        }
                    }
                    else
                    {
                        queue.Enqueue(nextStep, nextStep.Energy);
                        inQueue.Add(hash, nextStep.Energy);
                    }
                }
            }
        }

        //foreach(var v in moveHistory)
        //{
        //    Console.WriteLine($"{v.Item1} -> {v.Item2}");
        //}
        //Console.WriteLine();

        return bestEnergy;
    }


    int part1Answer = GetTotalEnergy(new(a, b, c, d));

    //I could do some whacky string manipulation for part 2. 
    //or I could spend five minutes and hardcode the changes.
    a[3] = a[1];
    b[3] = b[1];
    c[3] = c[1];
    d[3] = d[1];

    //#D#C#B#A#
    a[1] = Amphipods.DESERT_POD;
    b[1] = Amphipods.COPPER_POD;
    c[1] = Amphipods.BRONZE_POD;
    d[1] = Amphipods.AMBER_POD;

    //#D#B#A#C#
    a[2] = Amphipods.DESERT_POD;
    b[2] = Amphipods.BRONZE_POD;
    c[2] = Amphipods.AMBER_POD;
    d[2] = Amphipods.COPPER_POD;

    int part2Answer = GetTotalEnergy(new(a, b, c, d));

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}