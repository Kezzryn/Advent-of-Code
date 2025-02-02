using AoC_2016_Day_10;
using System.Text.RegularExpressions;

try
{
    const int CHIP_ID_1 = 17;
    const int CHIP_ID_2 = 61;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    IEnumerable<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).OrderBy(x => x);

    Dictionary<int, int> output = [];
    Dictionary<int, ChipBot> bots = [];

    //// Skip error checking as the bot lists are unique. 
    //// sort them to add bots before values, so we have bots to add values to
    foreach (string line in puzzleInput)
    {
        if (line.StartsWith("bot"))
        {
            List<int> numbers = GetNumbers().Matches(line).Select(x => int.Parse(x.Value)).ToList();
            bots.Add(numbers[0], new ChipBot(numbers[2], line.Contains("high to output"), numbers[1], line.Contains("low to output")));
        }
        else
        {
            var numbers = GetNumbers().Matches(line).Select(x => int.Parse(x.Value)).ToList();
            bots[numbers[1]].Chips.Add(numbers[0]);
        }
    }

    bool isDone = false;
    int part1Answer = 0;

    void shuffleChips(int target, int value, bool isOutputTarget)
    {
        if (isOutputTarget)
        {
            if (!output.TryAdd(target, value))
            {
                output[target] = value;
            }
        }
        else
        {
            bots[target].Chips.Add(value);
        }
    }

    while (!isDone)
    {
        isDone = true;
        foreach (int botID in bots.Keys)
        {
            ChipBot bot = bots[botID];

            if (bot.Chips.Count == 2)
            {
                isDone = false; // every day we're shuffling.
                if (bot.Chips.Contains(CHIP_ID_1) && bot.Chips.Contains(CHIP_ID_2)) part1Answer = botID;

                shuffleChips(bot.GiveHighTo, bot.Chips.Max(), bot.HighIsOutput);
                shuffleChips(bot.GiveLowTo, bot.Chips.Min(), bot.LowIsOutput);

                bot.Chips.Remove(bot.Chips.Max());
                bot.Chips.Remove(bot.Chips.Min());
            }
        }
    }

    Console.WriteLine($"Part 1: The bot that was responsible for {CHIP_ID_1} and {CHIP_ID_2} chips was: {part1Answer}");

    int part2Answer = output.Where(x => x.Key <= 2).Select(x => x.Value).Aggregate((x, y) => x * y);
    Console.WriteLine($"Part 2: The product of the chips in bins 0, 1, and 2, is: {part2Answer}");
    Console.WriteLine(String.Join(" ", output.Where(x => x.Key <= 2).Select(x => x.Value)));
}
catch (Exception e)
{
    Console.WriteLine(e);
}

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex GetNumbers();
}