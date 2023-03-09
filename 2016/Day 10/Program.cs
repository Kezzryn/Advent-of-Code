using AoC_2016_Day_10;
using System.Text.RegularExpressions;

try
{
    const int CHIP_ID_1 = 17;
    const int CHIP_ID_2 = 61;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<int, List<int>> output = new();
    Dictionary<int, ChipBot> bots = new();

    // Skip error checking as the bot lists are unique. 
    // sort them to add bots before values, so we have bots to add values to
    foreach (string line in puzzleInput.Where(x => x.StartsWith("bot")).ToList())
    {
        var numbers = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToList();

        bots.Add(numbers[0], new ChipBot(numbers[2], line.Contains("high to output"), numbers[1], line.Contains("low to output")));

    }
    foreach (string line in puzzleInput.Where(x => x.StartsWith("value")).ToList())
    {
        var numbers = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToList();
        bots[numbers[1]].Chips.Add(numbers[0]);
    }

    bool isDone = false;
    int part1Answer = 0;

    void shuffleChips(int target, int value, bool isOutputTarget)
    {
        if (isOutputTarget)
        {
            if (!output.TryAdd(target, new() { value }))
            {
                output[target].Add(value);
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
                bot.Chips.Remove(bot.Chips.Max());

                shuffleChips(bot.GiveLowTo, bot.Chips.Min(), bot.LowIsOutput);
                bot.Chips.Remove(bot.Chips.Min());
            }
        }
    }

    int part2Answer = 
          output[0].FirstOrDefault(1)
        * output[1].FirstOrDefault(1)
        * output[2].FirstOrDefault(1);

    Console.WriteLine($"Part 1: The bot that was reponsable for {CHIP_ID_1} and {CHIP_ID_2} chips was: {part1Answer}");

    Console.WriteLine($"Part 2: The product of the first three bins is: {part2Answer}");


}
catch (Exception e)
{
    Console.WriteLine(e);
}