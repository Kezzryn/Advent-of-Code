try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, Dictionary<string, int>> seatingPlan = new();

    int FindHappyness()
    {
        //This is a bog standard DFS. 
        int PlanScore(List<string> proposedPlan)
        {
            if (proposedPlan.Count <= 1) return 0;

            int returnValue = 0;

            for (int i = 0; i < proposedPlan.Count; i++)
            {
                returnValue += seatingPlan[proposedPlan[i]][proposedPlan[(i == proposedPlan.Count - 1) ? 0 : i + 1]];
            }

            return returnValue;
        }

        PriorityQueue<List<string>, int> queue = new();
        int maxHappy = 0;
        List<string> bestPlan = new();

        foreach (string person in seatingPlan.Keys) queue.Enqueue(new() { person }, 0);

        while (queue.Count > 0)
        {
            List<string> proposedPlan = queue.Dequeue();

            int currentHappy = PlanScore(proposedPlan);

            if ((proposedPlan.Count == seatingPlan.Count) && currentHappy > maxHappy)
            {
                maxHappy = currentHappy;
                bestPlan = proposedPlan;
            }

            foreach (string person in seatingPlan.Keys.Except(proposedPlan))
            {
                List<string> newPlan = proposedPlan.Append(person).ToList();
                queue.Enqueue(newPlan, -PlanScore(newPlan));
            }
        }

        return maxHappy;
    }

    void AddAndLink(string neighborName, string personName, int happyness )
    {
        if (seatingPlan.ContainsKey(neighborName) && seatingPlan[neighborName].ContainsKey(personName))
        {
            // we need to cross link these two entries. 
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

    foreach (string line in puzzleInput)
    {
        //Carol would gain 90 happiness units by sitting next to Frank.
        //Carol would lose 16 happiness units by sitting next to George.

        string[] splitLine = line[..^1].Split(' '); // strip final period and break on spaces.

        AddAndLink(splitLine[10], splitLine[0], int.Parse(splitLine[3]) * ((splitLine[2] == "gain") ? 1 : -1));
    }

    // setup the queue. 
    int happynessPart1 = FindHappyness();

    Console.WriteLine($"Part 1: When seating everyone there is a {happynessPart1} gain in happyness.");

    foreach (string person in seatingPlan.Keys.ToList())
    {
        AddAndLink(person, "Kezzryn", 0);
        AddAndLink("Kezzryn", person, 0);
    }

    int happynessPart2 = FindHappyness();

    Console.WriteLine($"Part 2: When seating ourselves with everyone else, there is a {happynessPart2} gain in happyness.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}

