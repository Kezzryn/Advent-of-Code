//Thanks 2019, Day 24!
static int getBit(int x, int pos) => (x & (1 << pos)) != 0 ? 1 : 0;
static int setBit(int x, int pos) => x |= 1 << pos;
//static int clearBit(int x, int pos) => x &= ~(1 << pos);

try
{
    const bool KEEP_MOST_COMMON = true;
    const bool KEEP_LEAST_COMMON = false;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => Convert.ToInt32(x, 2)).ToList();

    int numBits = File.ReadLines(PUZZLE_INPUT).First().Length - 1;
    int majorityReadings = puzzleInput.Count / 2;
    
    int gammaRate = 0;
    int epsilonRate = 0;

    for (int i = 0; i <= numBits;  i++)
    {
        int numSetBits = puzzleInput.Sum(x => getBit(x, i));

        if (numSetBits > majorityReadings) 
            gammaRate = setBit(gammaRate, i);
        else
            epsilonRate = setBit(epsilonRate, i);
    }

    int part1Answer = gammaRate * epsilonRate;
    Console.WriteLine($"Part 1: The power consumption of the submarine is {part1Answer}.");

    int ReduceList(List<int> list, bool keepMostCommonBit)
    {
        for (int i = numBits; i >= 0; i--)
        {
            int numSetBits = list.Sum(x => getBit(x, i));
            int mostCommonBit = numSetBits >= list.Count - numSetBits ? 1 : 0; //use Count - numSetBits because / 2 rounds down and screws things up.
            int leastCommonBit = mostCommonBit == 1 ? 0 : 1;

            list.RemoveAll(x => getBit(x, i) != (keepMostCommonBit ? mostCommonBit : leastCommonBit));

            if (list.Count == 1) break;
        }

        return list.FirstOrDefault(-1);
    }

    int oxyRate = ReduceList(new(puzzleInput), KEEP_MOST_COMMON);         //use new so we don't clobber the input list.
    int CO2Rate = ReduceList(new(puzzleInput), KEEP_LEAST_COMMON);

    int part2Answer = oxyRate * CO2Rate;
    Console.WriteLine($"Part 2: The life support rating of the submarine is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}