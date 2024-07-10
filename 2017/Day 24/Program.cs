using System.Numerics;

static void CalculateBridgeStrength(Dictionary<ulong, (int plugA, int plugB)> plugs, out int strongestBridge, out int longestBridgeStrength)
{
    // bridge contents, and open plug
    PriorityQueue<(ulong, int), int> queue = new();
    HashSet<ulong> queueContents = [];

    queue.Enqueue((0, 0), 0);
    queueContents.Add(0);

    int longestBridgeLength = 0;
    longestBridgeStrength = 0;
    strongestBridge = 0;

    while (queue.TryDequeue(out (ulong bridge, int openPlug) q, out int bridgeStrength))
    {
        queueContents.Remove(q.bridge);

        if (bridgeStrength > strongestBridge)
        {
            strongestBridge = bridgeStrength;
        }

        int currentBridgeLength = BitOperations.PopCount(q.bridge);
        if (currentBridgeLength >= longestBridgeLength && bridgeStrength > longestBridgeStrength)
        {
            longestBridgeLength = currentBridgeLength;
            longestBridgeStrength = bridgeStrength;
        }

        // search the dictionary for values that contain the current open plug
        foreach (KeyValuePair<ulong, (int, int)> currentPlug in plugs.Where(x => x.Value.plugA == q.openPlug || x.Value.plugB == q.openPlug))
        {
            if ((q.bridge & currentPlug.Key) != 0) continue; //skip used plugs

            ulong newBridgeState = q.bridge | currentPlug.Key;

            int newOpenPlug = currentPlug.Value.Item1 == q.openPlug ? currentPlug.Value.Item2 : currentPlug.Value.Item1;

            int newBridgeStrength = bridgeStrength + currentPlug.Value.Item1 + currentPlug.Value.Item2;
            if (!queueContents.Contains(newBridgeState))
            {
                queue.Enqueue((newBridgeState, newOpenPlug), newBridgeStrength);
            }
        }
    }
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<ulong, (int plugA, int plugB)> plugs = [];

    ulong plugID = 1;
    foreach (string line in puzzleInput)
    {
        var split = line.Split('/').Select(int.Parse).ToArray();
        plugs.Add(plugID, (split[0], split[1]));
        plugID <<= 1;
    }

    CalculateBridgeStrength(plugs, out int part1Answer, out int part2Answer);

    Console.WriteLine($"Part 1: The strongest bridge we can make is {part1Answer}.");
    Console.WriteLine($"Part 2: The strength of the longest bridge we can make is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}