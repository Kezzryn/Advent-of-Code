using AoC_2015_Day_21;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<Entity> cloneArmy = [];
    foreach ((int weapon, int armor, int rings) in from w in Equipment.Weapons.Keys
                                                   from a in Equipment.Armor.Keys
                                                   from r in Equipment.Rings.Keys
                                                   select (w , a, r))
    {
        Entity newClone = new(100, 0, 0, 0);
        newClone.AddEquipment(weapon);
        newClone.AddEquipment(armor);
        newClone.AddEquipment(rings);

        cloneArmy.Add(new(newClone));
    }

    Entity theBoss = new(puzzleInput);

    int part1Answer = cloneArmy.Where(x => x.BeatBoss(theBoss)).Min(x => x.GoldCost);
    int part2Answer = cloneArmy.Where(x => !x.BeatBoss(theBoss)).Max(x => x.GoldCost);

    Console.WriteLine($"Part 1: The lowest amount we can spend and win is, {part1Answer}");
    Console.WriteLine($"Part 2: The most amount we can spend and lose is, {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}