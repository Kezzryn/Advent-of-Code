using System.Numerics;

static long FindCombo(IEnumerable<long> solutionSpace, int numBags)
{
    Dictionary<int, long> bitMask = solutionSpace.Select((x, i) => (1 << i, x)).ToDictionary();
    long CalcWeight(int x) => bitMask.Where(w => (w.Key & x) != 0).Sum(x => x.Value);
    long CalcQuantum(int x) => bitMask.Where(w => (w.Key & x) != 0).Select(x => x.Value).Aggregate((x, y) => x * y);

    long targetWeight = solutionSpace.Sum() / numBags;

    long bestQuantum = long.MaxValue;
    int bestNumPackage = int.MaxValue;

    PriorityQueue<int, long> queue = new(); //key, weight.
    HashSet<int> inQueue = [];

    foreach(int key in bitMask.Keys)
    {
        queue.Enqueue(key, -CalcWeight(key));
        inQueue.Add(key);
    }

    while(queue.TryDequeue(out int packageMask, out long weight))
    {
        weight = -weight;
        int packageCount = BitOperations.PopCount((uint)packageMask);
        if (weight == targetWeight && packageCount <= bestNumPackage)
        {
            long currentQuantium = CalcQuantum(packageMask);
            if (currentQuantium < bestQuantum)
            {
                bestNumPackage = packageCount;
                bestQuantum = currentQuantium;
            }
            continue;
        }

        foreach (int present in bitMask.Keys.Where(x => (x & packageMask) == 0))
        {
            int newKey = present | packageMask;
            if (inQueue.Contains(newKey)) continue; //we've seen this combo. 

            if (BitOperations.PopCount((uint)newKey) > bestNumPackage) continue; //we have a smaller solution.

            long newWeight = CalcWeight(newKey);
            if (newWeight > targetWeight) continue; //Too heavy!

            queue.Enqueue(newKey, -newWeight);
            inQueue.Add(newKey);
        }
    }

    return bestQuantum;
}

try
{
    const int NUM_BAGS_PART1 = 3;
    const int NUM_BAGS_PART2 = 4;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    IEnumerable<long> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(long.Parse);

    long part1Answer = FindCombo(puzzleInput, NUM_BAGS_PART1);
    long part2Answer = FindCombo(puzzleInput, NUM_BAGS_PART2);

    Console.WriteLine($"Part 1: The best Quantum Entanglement of the smallest bag is {part1Answer}.");
    Console.WriteLine($"Part 2: The with {NUM_BAGS_PART2} places for bags, the best Quantum Entanglement is now {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
