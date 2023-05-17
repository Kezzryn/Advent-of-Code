using AoC_2019_Day_14;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const long ORE_STOCK = 1000000000000;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, Reaction> recipeBook = new();
    Dictionary<string, long> stock = new()
    {
        { Reaction.ORE, ORE_STOCK }
    };

    //load/init data
    foreach (string line in puzzleInput)
    {
        Reaction r = new(line);
        recipeBook.Add(r.ID, r);
        stock.Add(r.ID, 0);
    }

    bool FuelToOre(long targetAmount)
    {
        //reset between runs.
        foreach(var kvp in stock) stock[kvp.Key] = 0;
        stock[Reaction.ORE] = ORE_STOCK;

        Queue<(string, long)> queue = new();
        queue.Enqueue((Reaction.FUEL, targetAmount));

        while (queue.TryDequeue(out (string ID, long Amt) currentReaction))
        {
            foreach (var (recipeID, recipeAmt) in recipeBook[currentReaction.ID].Inputs)
            {
                long totalNeed = recipeAmt * currentReaction.Amt;
                if (stock[recipeID] >= totalNeed)
                {
                    //pull from stock if able
                    stock[recipeID] -= totalNeed;
                }
                else
                {
                    if (recipeID == Reaction.ORE) return false; //out of ore.

                    long reactionNeeds = totalNeed - stock[recipeID];
                    long needToMake = (reactionNeeds + recipeBook[recipeID].Output - 1) / recipeBook[recipeID].Output;
                    long leftOver = stock[recipeID] + (needToMake * recipeBook[recipeID].Output) - totalNeed;

                    queue.Enqueue((recipeID, needToMake));
                    stock[recipeID] = leftOver;
                }
            }
        }
        return true;
    }

    FuelToOre(1);
    long part1Answer = ORE_STOCK - stock[Reaction.ORE];
    
    int STEP = 1000000;
    long fuel = ORE_STOCK / part1Answer;
    long part2Answer = 0;

    while(fuel < long.MaxValue)
    {
        if (FuelToOre(fuel))
        {
            part2Answer = fuel;
        }
        else
        {
            if (STEP == 1) break;
            //potential overshoot, backup and lower STEP grain
            fuel -= STEP;
            STEP /= 10;
        }
        fuel += STEP;
    }

    Console.WriteLine($"Part 1: {part1Answer} ore is needed to make 1 fuel.");
    Console.WriteLine($"Part 2: {part2Answer} fuel can be made from a trillion ore.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}