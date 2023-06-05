static long getBit(long x, int pos) => (x & (1L << pos)) != 0 ? 1 : 0;
static long setBit(long x, int pos) => x |= (1L << pos);
static long clearBit(long x, int pos) => x &= ~(1L << pos);

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<long, long> memoryPart1 = new();
    Dictionary<long, long> memoryPart2 = new();

    List<int> posX = new(); // list of X indexes in the current mask.


    long maskOnes = 0;
    long maskZero = 0;

    foreach (string line in puzzleInput)
    {
        if (line.StartsWith("mask"))
        {
            string mask = line.Split('=', StringSplitOptions.TrimEntries).Last();

            maskOnes = Convert.ToInt64(mask.Replace('X', '0'), 2);
            maskZero = Convert.ToInt64(mask.Replace('X', '1'), 2);

            posX = mask.Reverse().Select((a, b) => new { Value = a, Index = b })
                             .Where(x => x.Value == 'X')
                             .Select(c => c.Index).ToList();
        }
        else
        {
            long address = long.Parse(line[(line.IndexOf('[') + 1)..line.IndexOf(']')]);
            long value = long.Parse(line[(line.IndexOf('=') + 2)..].Trim());

            long newValue = (value | maskOnes) & maskZero;
            long newAddress = maskOnes | address;

            if (!memoryPart1.TryAdd(address, newValue)) memoryPart1[address] = newValue;

            // Part two, iterate through all possible "floating bits" and map them to newAddress with the posX mapping.
            foreach (int maskNum in Enumerable.Range(0, (int)Math.Pow(2, posX.Count)))
            {
                for (int pos = 0; pos < posX.Count; pos++)
                {
                    newAddress = getBit(maskNum, pos) == 1
                        ? setBit(newAddress, posX[pos])
                        : clearBit(newAddress, posX[pos]);
                }

                if (!memoryPart2.TryAdd(newAddress, value)) memoryPart2[newAddress] = value;
            }
        }
    }

    long part1Answer = memoryPart1.Sum(x => x.Value);
    long part2Answer = memoryPart2.Sum(x => x.Value);

    Console.WriteLine($"Part 1: The sum of all memory locations is {part1Answer}.");
    Console.WriteLine($"Part 2: Version two creates a sum of {part2Answer} in memory.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}