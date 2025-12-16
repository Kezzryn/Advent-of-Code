try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<List<int>> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT).Select(x => x.ToCharArray().Select(x => x - '0').ToList())];

    static long MaxJoltage(List<int> batteries, int numBatteries)
    {
        int[] indexes = new int[numBatteries];
        int cursor = 0;
        
        for (int i = 0; i < numBatteries; i++)
        {
            List<int> window = batteries[cursor..^(numBatteries - 1 - i)];
            int maxValue = window.Max();
            cursor += window.IndexOf(maxValue) + 1;
            indexes[i] = maxValue;
        }

        long returnValue = 0;
        long multiplier = 1;

        for(int i = indexes.Length - 1; i >= 0 ; i--)
        {
            returnValue += indexes[i] * multiplier;
            multiplier *= 10;
        }
        return returnValue; 
    }

    long part1Answer = puzzleInput.Sum(x => MaxJoltage(x, 2));
    long part2Answer = puzzleInput.Sum(x => MaxJoltage(x, 12));

    Console.WriteLine($"Part 1: The total output joltage from banks of 2 is {part1Answer}.");
    Console.WriteLine($"Part 2: When swithcing to banks of 12, the total joltage is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}