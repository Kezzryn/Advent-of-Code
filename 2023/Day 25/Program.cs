using AoC_AStar;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, List<string>> theMap = [];

    foreach(string line in puzzleInput)
    {
        //cmg: qnr nvd lhk bvb
        string key = line[0..3];
        string[] links = line[5..].Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

        theMap.TryAdd(key, []);
        foreach(string link in links)
        {
            theMap[key].Add(link);
            theMap.TryAdd(link, []);
            theMap[link].Add(key);
        }
    }

    //pick a node. Ensure it has at least 4 paths from it.
    string startNode = theMap.Where(x => x.Value.Count > 3).First().Key;

    Dictionary<string, int> distanceMap = [];
    distanceMap[startNode] = 0;

    //super basic distance calc.
    void DistanceMap(string node, int depth)
    {
        int newDepth = depth + 1;
        foreach(string step in theMap[node])
        {
           if(!distanceMap.TryAdd(step, newDepth))
            {
                if (distanceMap[step] > newDepth)
                {
                    distanceMap[step] = newDepth;
                }
                
            }
        }
        
        foreach (string step in theMap[node])
        {
            if (distanceMap[step] >= depth)
            {
                DistanceMap(step, newDepth);
            }
        }
    }

    DistanceMap(startNode, 0);

    //find the node farthest from it.
    string endNode = distanceMap.OrderByDescending(x => x.Value).First().Key;

    //pathfind from s to x three times.
    for(int paths = 0; paths < 3; paths++)
    {
        AStar.A_Star(startNode, endNode, theMap, out int numSteps, out List<string> finalpath);

        // cut the path out
        for(int i = 0; i < finalpath.Count - 1; i++)
        {
            theMap[finalpath[i]].Remove(finalpath[i + 1]);
            theMap[finalpath[i + 1]].Remove(finalpath[i]);
        }
    }

    //reuse this to give us a count of one side of the now split graph.
    distanceMap.Clear();
    DistanceMap(startNode, 0);

    int part1Answer = distanceMap.Count * (theMap.Count - distanceMap.Count);

    Console.WriteLine($"Part 1: The two groups split into {distanceMap.Count} and {theMap.Count - distanceMap.Count}. Their product is {part1Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}