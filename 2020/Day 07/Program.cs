try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string SHINY_GOLD = "shiny gold";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, Dictionary<string, int>> bagTree = new();

    foreach(string line in puzzleInput)
    {
        var splitA = line.Split(" bags contain ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string bagID = splitA[0];

        bagTree.Add(bagID, new());
        if (line.EndsWith("no other bags.")) continue;
        
        foreach(string subBag in splitA[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            var splitB = subBag.Split(' ');

            int numBags = int.Parse(splitB[0]);
            string subBagID = splitB[1] + " " + splitB[2];
            
            bagTree[bagID].Add(subBagID, numBags);
        }
    }

    bool CanContain(string bagID, string bagTypeID = SHINY_GOLD)
    {
        if (bagTree[bagID].ContainsKey(bagTypeID)) return true;

        foreach(string childID in bagTree[bagID].Keys)
        {
            if (CanContain(childID, bagTypeID)) return true;
        }

        return false;
    }

    int NumContainingBags(string bagTypeID = SHINY_GOLD)
    {
        int returnValue = bagTree[bagTypeID].Values.Sum();

        foreach(var childBag in bagTree[bagTypeID])
        {
            returnValue += childBag.Value * NumContainingBags(childBag.Key);
        }

        return returnValue;
    }

    int part1Answer = bagTree.Keys.Where(x => x != SHINY_GOLD).Select(x => CanContain(x)).Count(x => x == true);
    int part2Answer = NumContainingBags();

    Console.WriteLine($"Part 1: {part1Answer} bag colors can contain at least one shiny gold bag.");
    Console.WriteLine($"Part 2: {part2Answer} individual bags are required inside my single shiny gold bag.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}