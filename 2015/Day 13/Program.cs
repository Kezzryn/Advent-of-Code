static void AddAndLink(string personName, string neighborName, int happyness, Dictionary<string, Dictionary<string, int>> seatingPlan)
{
    if (seatingPlan.ContainsKey(neighborName) && seatingPlan[neighborName].ContainsKey(personName))
    {
        seatingPlan[neighborName][personName] += happyness;
        happyness = seatingPlan[neighborName][personName];
    }

    if (seatingPlan.TryGetValue(personName, out Dictionary<string, int>? neighbor))
    {
        neighbor.Add(neighborName, happyness);
    }
    else
    {
        seatingPlan.Add(personName, new() { { neighborName, happyness }, { personName, 0 } });
    }
}

static int FindHappyness(Dictionary<string, Dictionary<string, int>> seatingPlan)
{
    int PlanScore(List<string> proposedPlan)
    {
        if (proposedPlan.Count <= 1) return 0;
        return proposedPlan.Select((x, i) => seatingPlan[x][proposedPlan[(i == proposedPlan.Count - 1) ? 0 : i + 1]]).Sum();
    }

    PriorityQueue<List<string>, int> queue = new();
    int maxHappy = 0;

    foreach (string person in seatingPlan.Keys) queue.Enqueue([person], 0);

    while (queue.TryDequeue(out List<string>? proposedPlan, out int currentHappy))
    {
        currentHappy = -currentHappy;

        if ((proposedPlan.Count == seatingPlan.Count) && currentHappy > maxHappy) maxHappy = currentHappy;

        foreach (string person in seatingPlan.Keys.Except(proposedPlan))
        {
            List<string> newPlan = [.. proposedPlan, person];
            queue.Enqueue(newPlan, -PlanScore(newPlan));
        }
    }

    return maxHappy;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, Dictionary<string, int>> seatingPlan = [];

    foreach (string line in puzzleInput)
    {
        //Carol would gain 90 happiness units by sitting next to Frank.
        //Carol would lose 16 happiness units by sitting next to George.
        //0     1     2    3  4         5     6  7       8    9  0

        string[] splitLine = line[..^1].Split(' '); // strip final period and break on spaces.
        AddAndLink(splitLine[0], splitLine[10], int.Parse(splitLine[3]) * ((splitLine[2] == "gain") ? 1 : -1), seatingPlan);
    }

    int part1Answer = FindHappyness(seatingPlan);

    //Add in for part 2.
    foreach (string person in seatingPlan.Keys.ToList()) //Use ToList() or the enumeration will change
    {
        AddAndLink(person, "Kezzryn", 0, seatingPlan);
        AddAndLink("Kezzryn", person, 0, seatingPlan);
    }

    int part2Answer = FindHappyness(seatingPlan);

    Console.WriteLine($"Part 1: When seating everyone there is a {part1Answer} gain in happyness.");
    Console.WriteLine($"Part 2: When seating ourselves with everyone else, there is a {part2Answer} gain in happyness.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

