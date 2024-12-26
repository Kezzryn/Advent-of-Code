try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, HashSet<string>> lanParty = [];

    foreach (string line in puzzleInput)
    {
        string[] split = line.Split('-');
        if (!lanParty.TryAdd(split[0], [split[1]])) lanParty[split[0]].Add(split[1]);
        
        if (!lanParty.TryAdd(split[1], [split[0]])) lanParty[split[1]].Add(split[0]);
    }

    HashSet<string> part1Answer = [];

    foreach (string key in lanParty.Keys.Where(k => k.StartsWith('t')))
    {
        List<string> connections = [.. lanParty[key]];
        for (int i = 0; i < connections.Count; i++)
        {
            string keyA = connections[i];
            for(int j = i + 1; j < connections.Count; j++)
            {
                string keyB = connections[j];
                if (lanParty[keyA].Contains(keyB))
                {
                    List<string> t = [key, keyA, keyB];
                    t.Sort();
                    string hash = t.Aggregate((a, b) => a + b);
                    part1Answer.Add(hash);
                }
            }
        }
    }

    List<string> computers = [.. lanParty.Keys.Order()];
    List<string> part2Answer = [];

    for (int i = 0; i < computers.Count; i++)
    {
        string keyA = computers[i];
        List<string> tempList = [keyA];
        for (int j = i + 1; j < computers.Count; j++)
        {
            string keyB = computers[j];
            if(tempList.All(x => lanParty[x].Contains(keyB))) 
                tempList.Add(keyB);
        }
        if(tempList.Count > part2Answer.Count) part2Answer = new(tempList);
    }

    Console.WriteLine($"Part 1: There are {part1Answer.Count} sets of three interconnected computers containing one or more with a name starting with 't'.");
    Console.WriteLine($"Part 2: The lan party password is: {String.Join(",", part2Answer)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}