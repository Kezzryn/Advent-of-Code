using System.Drawing;
using AoC_2018_Day_15;

try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    Dictionary<Point, MapSymbols> theMap = new();
    Dictionary<Point, Entity> combatants = new();

    foreach ((int x, int y) in from a in Enumerable.Range(0, puzzleInput[0].Length)
                               from b in Enumerable.Range(0, puzzleInput.Length)
                               select (a, b))
    {
        switch (puzzleInput[y][x])
        {
            case 'E':
                combatants.Add(new(x, y), new(Entity.Elf));
                theMap.Add(new(x, y), MapSymbols.Open);
                break;
            case 'G':
                combatants.Add(new(x, y), new(Entity.Goblin));
                theMap.Add(new(x, y), MapSymbols.Open);
                break;
            default:
                theMap.Add(new(x, y), puzzleInput[y][x] switch
                {
                    '#' => MapSymbols.Wall,
                    '.' => MapSymbols.Open,
                    _ => throw new NotImplementedException()
                });
                break;
        }
    }

    int part1Answer = 0;
    int part2Answer = 0;

    int roundCounter = 0;

    bool isDone = false;

    bool Fight(Point pos, Entity fighter)
    {
        // NEED PRIORITY TARGETTING BY HP, THEN-BY READING ORDER
        foreach (Size neighbor in AStar.Neighbors)
        {
            if (combatants.TryGetValue(pos + neighbor, out Entity? target) && target.Type != fighter.Type)
            {
                target.HP -= fighter.Attack;
                return true;
            }
        }
        return false;
    }

    HashSet<Point> collisonMap = combatants.Select(x => x.Key).ToHashSet();

    while (!isDone)
    {
        roundCounter++;

        

        foreach ((Point currentFigherPos, Entity currentFighter) in combatants.OrderBy(o => o.Key.Y).ThenBy(t => t.Key.X))
        {
            if (currentFighter.HP <= 0) continue;

            int enemyType = currentFighter.Type == Entity.Elf ? Entity.Goblin : Entity.Elf;

            foreach ((Point targetPos, Entity targetFighter) in combatants.Where(x => x.Value.Type == enemyType).OrderBy(x => AStar.TaxiDistance(currentFigherPos, x.Key)).ThenBy(t => t.Key.Y).ThenBy(t => t.Key.Y))
            {
                if (targetFighter.HP <= 0) continue;



                // Console.WriteLine($"Target checking: {e.Type} at {e.Position} checking on {potentialTarget.Type} at {potentialTarget.Position} (dist: {TaxiDistance(e.Position, potentialTarget.Position)})");

                //Is there a path to the entity? 

                //if we move, update the collision map. 
            }

            // Now that we've moved, try to fight ... 
            Fight(currentFigherPos, currentFighter);
            // if someone dies, remove them from the collisionMap. 

        }

        //combatants.RemoveAll(r => r.HP <= 0);
        //if (!(combatants.Any(a => a.Type == Entity.Elf) && combatants.Any(a => a.Type == Entity.Goblin)))
        //// an awkward way of saying if there are not both elves and goblins left. 
        //{
            isDone = true;
        //    part1Answer = roundCounter * combatants.Select(x => x.HP).Sum();
        //}
    }
    /* one round
     * each entity takes a turn in "reading order" 
     * each turn 
     * -move
     * if adjacent, do not move
     * otherwise, move to closest _adjacent_ position
     * tiebreak in "reading order"
     * 
     * 
     * -attack
     * attack adjacent with lowest HP, tiebreak in reading order.
     * 
     * 
     * 
     */



    


    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}