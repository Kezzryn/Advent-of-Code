using AoC_2015_Day_22;

int ArenaBattle(Wizard startWizard, Boss startBoss, bool isHardMode = false)
{
    int bestMana = int.MaxValue;

    bool doEffects(Wizard wizard, Boss boss)
    {
        wizard.DoEffects();
        boss.DoEffects();
        if (boss.IsDead && wizard.ManaSpent < bestMana)
            bestMana = wizard.ManaSpent;

        return boss.IsDead;
    }

    PriorityQueue<(Wizard, Boss), int> queue = new();
    foreach (SpellNames spell in Enum.GetValues(typeof(SpellNames)))
    {
        var cloneWizard = new Wizard(startWizard);
        var cloneBoss = new Boss(startBoss);

        if (isHardMode) cloneWizard.Bleed(1);
        cloneWizard.CastSpell(spell, cloneBoss);
        queue.Enqueue((cloneWizard, cloneBoss), cloneWizard.ManaSpent);
    }

    while (queue.TryPeek(out _, out int priority)) // if we fail to peek something's wrong, or the queue's empty
    {
        if (priority >= bestMana) break; // it doesn't get any better than this. 

        (Wizard theWizard, Boss theBoss) = queue.Dequeue();
        
        if (doEffects(theWizard, theBoss)) continue; // if true, game over, boss died!

        theBoss.Attack(theWizard);
        if (isHardMode) theWizard.Bleed(1);
        if (theWizard.IsDead) continue; //we lost!

        if (doEffects(theWizard, theBoss)) continue; // if true, game over, boss died!

        foreach (SpellNames spell in Enum.GetValues(typeof(SpellNames)))
        {
            var cloneWizard = new Wizard(theWizard);
            var cloneBoss = new Boss(theBoss);

            if (cloneWizard.CastSpell(spell, cloneBoss) && cloneWizard.ManaSpent <= bestMana)
            {
                queue.Enqueue((cloneWizard, cloneBoss), cloneWizard.ManaSpent);
            }
        }
    }
    return bestMana;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int START_HP = 50;
    const int START_MANA = 500;
    const bool HARD_MODE = true;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1 = ArenaBattle(new Wizard(START_HP, START_MANA), new Boss(puzzleInput));
    //int part1 = ArenaBattle(new Wizard(10, 250), new Boss(new[] { "Hit Points: 14", "Damage: 8" }));
    Console.WriteLine($"Part 1: The least amount of mana we can spend is: {part1}");

    int part2 = ArenaBattle(new Wizard(START_HP, START_MANA), new Boss(puzzleInput), HARD_MODE);
    Console.WriteLine($"Part 2: For hard mode, the least amount of mana we can spend is: {part2}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}