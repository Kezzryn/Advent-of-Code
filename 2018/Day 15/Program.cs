using AoC_2018_Day_15;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

try
{
    Dictionary<Point, MapSymbols> theMap = new();
    Dictionary<Point, Entity> combatants = new();

    void PrintMap(string title)
    {
        Point min = new(theMap.Keys.Min(x => x.X), theMap.Keys.Min(x => x.Y));
        Point max = new(theMap.Keys.Max(x => x.X), theMap.Keys.Max(x => x.Y));

        Console.WriteLine(title);
        StringBuilder sb = new();

        for (int y = 0; y <= max.Y; y++)
        {
            sb.Clear();
            sb.Append("  ");
            for (int x = 0; x <= max.X; x++)
            {
                Point key = new(x, y);
                if (combatants.TryGetValue(key, out Entity? entity))
                {
                    sb.Append($"{entity.TypeName}:{entity.HP} ");
                    Console.Write(entity.Type == Entity.Goblin ? 'G' : 'E');
                }
                else
                {
                    Console.Write(theMap[new(x, y)] == MapSymbols.Open ? '.' : '#');
                }
            }
            Console.WriteLine(sb);
        }
        Console.WriteLine();
    }

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int numElves = puzzleInput.Select(x => x.Count(y => y == 'E')).Sum();


    void LoadMaps(int attackBoost)
    {
        theMap.Clear();
        combatants.Clear();

        foreach ((int x, int y) in from a in Enumerable.Range(0, puzzleInput[0].Length)
                                   from b in Enumerable.Range(0, puzzleInput.Length)
                                   select (a, b))
        {
            switch (puzzleInput[y][x])
            {
                case 'E':
                    combatants.Add(new(x, y), new(Entity.Elf, attackBoost));
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
    }

    int part1Answer = 0;
    int part2Answer = 0;

    int STEP = 1000;
    int boost = 0;
    for (boost = 0; boost < int.MaxValue; boost += STEP)
    {
        Console.WriteLine(boost);
        LoadMaps(boost);

        //PrintMap("Start Map");

        int roundCounter = 0;
        bool isDone = false;

        //bool isVerbose = new List<int>() { 1, 2, 22, 23, 24, 25, 26, 27, 28, 46, 47 }.Contains(roundCounter);
        bool isVerbose = false;

        while (!isDone)
        {
            HashSet<Point> initOrder = combatants.Keys.OrderBy(o => o.Y).ThenBy(t => t.X).ToHashSet();
            if (isVerbose)
            {
                Console.WriteLine($"Round start {roundCounter}");
                Console.Write("Init: ");
                foreach (Point p in initOrder)
                {
                    Console.Write($"{p} ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }

            foreach (Point currentFigherPos in initOrder)
            {
                if (isVerbose) Console.WriteLine($"With {currentFigherPos}:");

                if (!combatants.TryGetValue(currentFigherPos, out Entity? currentFighter)) continue;
                Point nextMove = currentFigherPos;

                int enemyType = currentFighter.Type == Entity.Elf ? Entity.Goblin : Entity.Elf;

                if (!AStar.Neighbors.Select(x => currentFigherPos + x).Intersect(combatants.Keys).Where(x => combatants.ContainsKey(x) && combatants[x].Type == enemyType).Any())
                {
                    Dictionary<int, List<Point>> nextMoves = new();
                    foreach ((Point targetPos, Entity targetFighter) in combatants.Where(x => x.Value.Type == enemyType && x.Value.HP > 0))
                    {
                        if (AStar.NextStep(currentFigherPos, targetPos, theMap, combatants.Keys.ToHashSet(), out Point nextStep, out int dist))
                        {
                            if (!nextMoves.TryAdd(dist, new() { nextStep })) nextMoves[dist].Add(nextStep);
                        }
                    }

                    if (nextMoves.Count > 0)
                    {
                        nextMove = nextMoves[nextMoves.Min(x => x.Key)].OrderBy(x => x.Y).ThenBy(x => x.X).First();
                        if (isVerbose) Console.WriteLine($"Moving from {currentFigherPos} to {nextMove}");
                        if (combatants.Remove(currentFigherPos)) combatants.Add(nextMove, currentFighter);
                    }
                }
                else
                {
                    if (isVerbose) Console.WriteLine("Found adjacent Not moving.");
                }

                // Now that we've (potentially) moved, try to fight ... 
                var allTargets = AStar.Neighbors.Select(x => nextMove + x).Intersect(combatants.Keys).Where(x => combatants.ContainsKey(x) && combatants[x].Type == enemyType).OrderBy(x => combatants[x].HP).ThenBy(x => x.Y).ThenBy(x => x.X).ToList();

                if (allTargets.Count == 0)
                {
                    if (isVerbose) Console.WriteLine($"No targets: {nextMove}  was: {currentFigherPos}");
                    continue;
                }

                Point target = allTargets.First();

                combatants[target].HP -= currentFighter.Attack;
                if (isVerbose) Console.WriteLine($"{nextMove} fights {target} target HP = {combatants[target].HP}");

                if (combatants[target].HP <= 0) combatants.Remove(target);

            }

            if (isVerbose) PrintMap($"Round: {roundCounter}");

            if (!(combatants.Any(a => a.Value.Type == Entity.Elf) && combatants.Any(a => a.Value.Type == Entity.Goblin)))
            {
                isDone = true;
            }
            else
            {
                roundCounter++;
            }
        }

        if (boost == 0) part1Answer = roundCounter * combatants.Sum(x => x.Value.HP);

        Console.WriteLine($"{numElves} left: {combatants.Count(c => c.Value.Type == Entity.Elf)}");
        if (combatants.Any(a => a.Value.Type == Entity.Elf))
        {
            Console.Write($"{boost} Elves won");
            if (numElves == combatants.Count(c => c.Value.Type == Entity.Elf))
            {
                Console.Write($" flawlessly");
                if (STEP == 1)
                {
                    part2Answer = roundCounter * combatants.Sum(x => x.Value.HP);
                    break;
                }
            }
            Console.WriteLine("!");
            boost -= STEP;
            STEP /= 10;
            Console.WriteLine($"Boost step down: {boost} {STEP}");
        }
        else
        {
            Console.WriteLine($"{boost}: Elves lost.");
        }
    }

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}