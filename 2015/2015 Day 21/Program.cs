using AoC_2015_Day_21;
using System.Diagnostics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Entity theBoss = new(puzzleInput);

    PriorityQueue<Entity, int> queue = new();

    foreach (int weapon in Equipment.Weapons.Keys)
    {
        Entity newClone = new (100, 0, 0, 0);
        newClone.AddEquipment(weapon);

        queue.Enqueue(newClone, Equipment.Weapons[weapon].GoldCost);
    }

    Entity testClone = new(0,0, 0, 0);
    Console.WriteLine(Equipment.HasArmor(testClone.MyEquipment));
    testClone.AddEquipment(1<<10);
    Console.WriteLine(Equipment.HasArmor(testClone.MyEquipment));

    int lowestGold = 0; 




    //HashSet<int> answers = new();

    while (queue.Count > 0)
    {
        Entity playerClone = queue.Dequeue();

        if (playerClone.FightMe(theBoss))
        {
            Console.WriteLine($"Winning thing detected! {playerClone.GoldCost}");
            //record this! 
            lowestGold = playerClone.GoldCost;
        } 
/*
        foreach (int key in containers.Keys)
        {
            if ((key & s) != 0) continue;

            int newValue = v + containers[key];

            if (newValue > TARGET_EGGNOG) continue;
            if (newValue == TARGET_EGGNOG)
            {
                answers.Add(key | s);
            }
            else
            {
                queue.Enqueue((key | s, newValue));
            }
        }
*/
    }

     Debugger.Break();

}
catch (Exception e)
{
    Console.WriteLine(e);
}