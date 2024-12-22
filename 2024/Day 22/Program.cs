try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    List<long> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(long.Parse).ToList();

    static List<long> ProcessSecretNumber(long secretNumber, int numLoops)
    {
        static long MixAndPrune(long secretNumber, long value) => (secretNumber ^ value) % 16777216;

        List<long> returnValue = [];
        returnValue.Add(secretNumber);
        for (int i = 0; i < numLoops; i++)
        {
            secretNumber = MixAndPrune(secretNumber, secretNumber * 64);
            secretNumber = MixAndPrune(secretNumber, secretNumber / 32);
            secretNumber = MixAndPrune(secretNumber, secretNumber * 2048);
            returnValue.Add(secretNumber);
        }

        return returnValue;
    }

    List<List<long>> secretNumberLists = puzzleInput.Select(x => ProcessSecretNumber(x, 2000)).ToList();
    long part1Answer = secretNumberLists.Sum(x => x.Last());

    static Dictionary<string, int> GetPriceCodes(List<long> secretNumbers)
    {
        static string BuildKey(List<int> source) => source.Select(x => x.ToString()).Aggregate((x, y) => x + y); //copied from 2015 day 15. 

        static List<int> FindDiffs(List<int> source) => source
            .TakeWhile((x, i) => i < source.Count - 1)
            .Select((x, i) => source[i] - source[i + 1])
            .ToList();

        List<int> lastDigits = secretNumbers.Select(x => (int)(x % 10)).ToList();

        List<(List<int> Diffs, int value)> windows = Enumerable.Range(5, lastDigits.Count - 4)
            .Select(i =>  (Diffs: FindDiffs(lastDigits[(i - 5)..i]), lastDigits[i - 1])) 
            .Where(x => x.Diffs.All(a => a != 0))
            .ToList();

        Dictionary<string, int> priceCodes = [];
        windows.ForEach(x => priceCodes.TryAdd(BuildKey(x.Diffs), x.value)); //Use TryAdd to get the first one only.

        return priceCodes;
    }

    Dictionary<string, int> masterPriceList = [];

    foreach ((string key, int value) in secretNumberLists.SelectMany(GetPriceCodes))
    {
        if (!masterPriceList.TryAdd(key, value)) masterPriceList[key] += value;
    }

    int part2Answer = masterPriceList.Values.Max();

    Console.WriteLine($"Part 1: The sum of the 2000th secret numbers is {part1Answer}.");
    Console.WriteLine($"Part 2: I can get {part2Answer} bananas.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}