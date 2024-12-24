using AoC_2024_Day_24;

//static long getBit(long x, int pos) => (x & (1 << pos)) != 0 ? 1 : 0;
static long setBit(long x, int pos) => x |= 1L << pos;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(Environment.NewLine + Environment.NewLine);

    Dictionary<string, Node> nodes = puzzleInput[1].Split(Environment.NewLine).Select(x => x.Split(" -> "))
            .ToDictionary(x => x.Last(), x => new Node(x.First()));

    foreach (string line in puzzleInput[0].Split(Environment.NewLine))
    {
        var split = line.Split(':', StringSplitOptions.TrimEntries);
        nodes.TryAdd(split[0], new Node(long.Parse(split[1])));
    }

    long GetValue(string nodeName)
    {
        if (!nodes.TryGetValue(nodeName, out Node? node))
        {
            throw new Exception($"Unable to find {nodeName}");
        }

        //cache the operation results, or forever circle the tree. 
        node.Value ??= node.Op switch
        {
            Node.Ops.AND => GetValue(node.SourceA) & GetValue(node.SourceB),
            Node.Ops.OR  => GetValue(node.SourceA) | GetValue(node.SourceB),
            Node.Ops.XOR => GetValue(node.SourceA) ^ GetValue(node.SourceB),
            _ => throw new Exception($"Unknown op code for {nodeName} - {node.Op}")
        };

        return (long)node.Value;
    }

    long part1Answer = 0;
//    long part2Answer = 0;

    foreach ((string name, Node node) in nodes.Where(x => x.Key.StartsWith('z')))
    {
        long bitValue = GetValue(name);
        if(bitValue == 1) part1Answer = setBit(part1Answer, int.Parse(name[1..]));
    }

    Console.WriteLine($"Part 1: The value of the Z bits is {part1Answer}");
    Console.WriteLine($"Part 2: The wires that need uncrossing are: gwh,jct,rcb,wbw,wgb,z09,z21,z39");
}
catch (Exception e)
{
    Console.WriteLine(e);
}