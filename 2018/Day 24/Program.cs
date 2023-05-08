using AoC_2018_Day_24;

static void ChooseTargets(List<ArmyGroup> attackers, List<ArmyGroup> defenders)
{
    foreach (ArmyGroup attacker in attackers.OrderByDescending(x => x.EffectivePower).ThenByDescending(y => y.Initative))
    {
        attacker.TargetIndex = -1;
        var target =
            defenders.OrderByDescending(x => x.DamageTest(attacker))
            .ThenByDescending(x => x.EffectivePower)
            .ThenByDescending(x => x.Initative)
            .Where(x => x.IsTargetted == false).DefaultIfEmpty(null).First();

        if (target != null && target.DamageTest(attacker) > 0)
        {
            attacker.TargetIndex = defenders.IndexOf(target);
            target.IsTargetted = true;
        }
    }
}

static int Fight(ArmyGroup attacker, List<ArmyGroup> defenders)
{
    if (attacker.TargetIndex == -1 || attacker.Units <= 0) return 0;
    return defenders[attacker.TargetIndex].TakeDamage(attacker);
}

static void CleanupRound(List<ArmyGroup> dirty)
{
    dirty.RemoveAll(x => x.Units <= 0);
    dirty.ForEach(x => x.IsTargetted = false);
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n"; 

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF+CRLF).ToArray();
    List<ArmyGroup> immuneSystem = new();
    List<ArmyGroup> infection = new();

    void LoadArmies(int boostDamage)
    {
        immuneSystem.Clear();
        infection.Clear();

        foreach (string army in puzzleInput)
        {
            if (army.Contains("Immune System:"))
            {
                immuneSystem = army.Split(CRLF)[1..].Select(x => new ArmyGroup(x, boostDamage)).ToList();
            }

            if (army.Contains("Infection:"))
            {
                infection = army.Split(CRLF)[1..].Select(x => new ArmyGroup(x)).ToList();
            }
        }
    }


    int answerPart1 = 0;
    int answerPart2 = 0;
    int STEP = 10000;
    int boost = 0;
    // Overengineered this. Was expecting a much larger boost needed than the one found.
    for (boost = 0; boost < int.MaxValue; boost += STEP)
    {
        LoadArmies(boost);
        int maxInit = int.Max(immuneSystem.Max(x => x.Initative), infection.Max(x => x.Initative));

        bool isStaleMate = false;
        while (infection.Count > 0 && immuneSystem.Count > 0)
        {
            ChooseTargets(infection, immuneSystem);
            ChooseTargets(immuneSystem, infection);

            int infectionUnitsKilled = 0;
            int immuneUnitsKilled = 0;

            for (int i = maxInit; i >= 1; i--)
            {
                // I hate this, but it works. ?Fix it later? 
                if (infection.Any(x => x.Initative == i))
                {
                    immuneUnitsKilled += Fight(infection.First(x => x.Initative == i), immuneSystem);
                }
                // if if NOT if else, 'cause we may be missing init steps from losses.
                if (immuneSystem.Any(x => x.Initative == i))
                {
                    infectionUnitsKilled += Fight(immuneSystem.First(x => x.Initative == i), infection);
                }
            }

            if (infectionUnitsKilled == 0 && immuneUnitsKilled == 0)
            {
                isStaleMate = true;
                break;
            }

            CleanupRound(infection);
            CleanupRound(immuneSystem);
        }

        if (isStaleMate) continue; // skip scorekeeping 

        if (boost == 0) answerPart1 = infection.Sum(x => x.Units) + immuneSystem.Sum(x => x.Units);

        if (immuneSystem.Count > 0)
        {
            if (STEP == 1)
            {
                answerPart2 = immuneSystem.Sum(x => x.Units);
                break;
            }
            else
            {
                //potential overshoot, backup and lower STEP grain
                boost -= STEP;
                STEP /= 10;
            }
        }
    }

    Console.WriteLine($"Part 1: The number of units of the winning army is {answerPart1} units.");
    Console.WriteLine($"Part 2: After getting a boost of {boost} attack power, the immune system wins with a minimum of {answerPart2} units.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}