using AoC_2015_Day_22;

int ArenaBattle(Wizard startWizard, Boss startBoss, bool isHardMode = false)
{
    int BestMana = int.MaxValue;

    bool doEffects(Wizard wiz, Boss boz)
    {
        wiz.DoEffects();
        boz.DoEffects();
        if (boz.IsDead && wiz.ManaSpent < BestMana)
            BestMana = wiz.ManaSpent;

        return boz.IsDead;
    }

    PriorityQueue<(Wizard player, Boss boss), int> queue = new();
    foreach (SpellNames spell in Enum.GetValues(typeof(SpellNames)))
    {
        var wiz = new Wizard(startWizard);
        var boz = new Boss(startBoss);

        if (isHardMode) wiz.LoseHP(1);
        wiz.CastSpell(spell, boz);
        queue.Enqueue((wiz, boz), wiz.ManaSpent);
    }

    while (queue.Count > 0)
    {
        if (!queue.TryPeek(out _, out int priority)) break; // if we fail to peek something's wrong. 
        if (priority >= BestMana) break; // it doesn't get any better than this. 

        (Wizard thePlayer, Boss theBoss) = queue.Dequeue();
        
        if (doEffects(thePlayer, theBoss)) continue; // if true, game over, boss died!

        theBoss.Attack(thePlayer);
        if (isHardMode) thePlayer.LoseHP(1);
        if (thePlayer.IsDead) continue; //we lost!

        if (doEffects(thePlayer, theBoss)) continue; // if true, game over, boss died!

        foreach (SpellNames spell in Enum.GetValues(typeof(SpellNames)))
        {
            var wiz = new Wizard(thePlayer);
            var boz = new Boss(theBoss);

            if (wiz.CastSpell(spell, boz) && wiz.ManaSpent <= BestMana)
            {
                queue.Enqueue((wiz, boz), wiz.ManaSpent);
            }
        }
    }
    return BestMana;
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