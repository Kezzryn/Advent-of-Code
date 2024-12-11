try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<long> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(' ').Select(long.Parse).ToList();

    static bool SplitStone(long stone, out (long, long) result)
    {
        result = (0, 0);
        int numDigits = (int)Math.Floor(Math.Log10(stone) + 1);
        if (int.IsOddInteger(numDigits)) return false;

        long multi = (long)Math.Pow(10, numDigits / 2);
        result = (stone / multi, stone % multi);
        return true;
    }

    static long Blink(List<long> puzzleInput, int numLoops)
    {
        Dictionary<long, long> source = puzzleInput.ToDictionary(x => x, y => 1L);
        Dictionary<long, long> target = [];

        void AddToTarget(long key, long value)
        {
            if (!target.TryAdd(key, value)) target[key] += value;
        }

        for(int i = 0; i < numLoops; i++)
        {
            target.Clear();
            foreach ((long stoneID, long stoneCount) in source)
            {
                if (stoneID == 0)
                {
                    AddToTarget(1, stoneCount);
                }
                else if (SplitStone(stoneID, out (long a, long b) result))
                {
                    AddToTarget(result.a, stoneCount);
                    AddToTarget(result.b, stoneCount);
                }
                else
                {
                    AddToTarget(stoneID * 2024, stoneCount);
                }
            }
            source = new(target);
        }
        return source.Values.Sum(); 
    }

    long part1Answer = Blink(puzzleInput, 25);
    long part2Answer = Blink(puzzleInput, 75);

    Console.WriteLine($"Part 1: Blinking 25 times results in {part1Answer} stones.");
    Console.WriteLine($"Part 2: After 75 blinks there are {part2Answer} stones.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}