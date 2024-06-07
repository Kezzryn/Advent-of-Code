using AoC_2015_Day_07;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    Dictionary<string, Node> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split(" -> "))
            .ToDictionary(x => x.Last(), x => new Node(x.First()));

    ushort GetValue(string nodeName)
    {
        if (!puzzleInput.TryGetValue(nodeName, out Node? node))
        {
            // this captures where we have a value in one of the "source" fields. 
            if (ushort.TryParse(nodeName, out ushort value)) return value;

            throw new Exception($"Unable to find {nodeName}");
        }

        //cache the operation results, or forever circle the tree. 
        node.Value ??= node.Op switch
        {
            // Nearly all other wires are caught in the null test above.
            // This catches any wires that hook to other wires, specifically the 'a' wire. 
            Node.Ops.Wire => GetValue(node.SourceA),
            Node.Ops.Not => (ushort)~GetValue(node.SourceA),
            Node.Ops.Or => (ushort)(GetValue(node.SourceA) | GetValue(node.SourceB)),
            Node.Ops.And => (ushort)(GetValue(node.SourceA) & GetValue(node.SourceB)),
            Node.Ops.RShift => (ushort)(GetValue(node.SourceA) >> ushort.Parse(node.SourceB)),
            Node.Ops.LShift => (ushort)(GetValue(node.SourceA) << ushort.Parse(node.SourceB)),
            _ => throw new Exception($"Unknown op code for {nodeName} - {node.Op}")
        };

        return (ushort)node.Value;
    }

    ushort part1Answer = GetValue("a");

    // Reset for part 2 
    puzzleInput["b"].Value = part1Answer;
    puzzleInput["a"].Value = null;
    
    foreach (Node node in puzzleInput.Values.Where(x => x.Op != Node.Ops.Wire))
    {
        node.Value = null;
    }

    ushort part2Answer = GetValue("a");
    
    Console.WriteLine($"Part 1: The signal on wire 'a' is {part1Answer}");
    Console.WriteLine($"Part 2: When overriding the signal on wire 'b' with {part1Answer}, 'a' returns {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}