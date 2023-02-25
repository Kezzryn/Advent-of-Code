using AoC_2015_Day_21;

static int ArenaBattles(Entity theBoss, bool isPart2 = false)
{
    PriorityQueue<Entity, int> queue = new();
    int bestGold = isPart2 ? int.MinValue : int.MaxValue;

    foreach (int weapon in Equipment.Weapons.Keys)
    {
        Entity newClone = new(100, 0, 0, 0);
        newClone.AddEquipment(weapon);

        queue.Enqueue(newClone, newClone.GoldCost);
    }
        
    while (queue.Count > 0)
    {
        Entity playerClone = queue.Dequeue();

        if (!isPart2 && playerClone.GoldCost > bestGold) break; // can't get any better.

        bool fightOutcome = playerClone.FightMe(theBoss);

        if (isPart2) // reverse the conditions. 
        {
            if (fightOutcome) continue; // can't get any better. 
            if (playerClone.GoldCost > bestGold) bestGold = playerClone.GoldCost;
        }
        else
        {
            if (fightOutcome && playerClone.GoldCost < bestGold) bestGold = playerClone.GoldCost;
        }

        List<Entity> cloneArmy = new(); 

        // add clones to try the boss, adding armor then rings. 
        // if we come in with armor or rings, we'll add the 3rd type of item.
        if (!Equipment.HasArmor(playerClone.MyEquipment))
        {
            foreach (int armor in Equipment.Armor.Keys)
            {
                Entity newClone = new(playerClone);
                newClone.AddEquipment(armor);
                cloneArmy.Add(newClone);
            }
        }

        if (!Equipment.HasRings(playerClone.MyEquipment))
        {
            foreach (int ring in Equipment.Rings.Keys)
            {
                Entity newClone = new(playerClone);
                newClone.AddEquipment(ring);
                cloneArmy.Add(newClone);
            }
        }

        foreach (Entity clone in cloneArmy)
        {
            if (isPart2)
            {
                // invert the gold cost to make the queue work for us.
                if (clone.GoldCost > bestGold) queue.Enqueue(clone, -clone.GoldCost);
            } 
            else
            {
                if (clone.GoldCost < bestGold) queue.Enqueue(clone, clone.GoldCost);
            }
        }
    }
    return bestGold;
}

try
{
    const bool IS_PART_2 = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Entity theBoss = new(puzzleInput);

    int lowestGold = ArenaBattles(theBoss);
    Console.WriteLine($"Part 1: The lowest amount we can spend and win is, {lowestGold}");

    int highestGold = ArenaBattles(theBoss, IS_PART_2);
    Console.WriteLine($"Part 2: The most amount we can spend and lose is, {highestGold}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}