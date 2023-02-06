using AoC_2015_Day_7;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, Node> nodes = new();

    foreach (string line in puzzleInput)
    {
        string[] temp = line.Split(' ');
        switch (temp.Length)
        {
            case 3:
                // this is a wire. 
                // All wires are [NUM] -> [wire] other than the lx -> a entry. 
                if (UInt16.TryParse(temp[0], out UInt16 value))
                {
                    nodes.Add(temp[2], new Node(value));
                }
                else
                {
                    nodes.Add(temp[2], new Node(temp[0], "", Ops.Wire));
                }
                break;
            case 4:
                // this is a NOT
                nodes.Add(temp[3], new Node(temp[1], "", Ops.Not));
                break;
            case 5:
                // this is everything else. 
                nodes.Add(temp[4], new Node(temp[0], temp[2], temp[1] switch
                {
                    "OR" => Ops.Or,
                    "AND" => Ops.And,
                    "LSHIFT" => Ops.LShift,
                    "RSHIFT" => Ops.RShift,
                    _ => throw new Exception($"Unknown op {temp[1]}")
                }));
                break;
            default:
                throw new Exception($"Unknown split length. {temp.Length}");
        }

    }

    UInt16 GetValue(string nodeName)
    {
        if (!nodes.TryGetValue(nodeName, out Node? node))
        { 
            // this captures where we have a value in one of the "source" fields. 
            if (UInt16.TryParse(nodeName, out UInt16 value)) return value;

            Console.WriteLine($"Unable to load node {nodeName}");
            return 0;
        }

        if (node.Value is not null) return (UInt16)node.Value;

        UInt16 returnValue = node.Op switch
        {
            // Nearly all other wires are caught in the null test above.
            // This catches any wires that hook to other wires, specifically the 'a' wire. 
            Ops.Wire => GetValue(node.SourceA), 
            Ops.Not => (UInt16)~GetValue(node.SourceA),
            Ops.Or => (UInt16)(GetValue(node.SourceA) | GetValue(node.SourceB)),
            Ops.And => (UInt16)(GetValue(node.SourceA) & GetValue(node.SourceB)),
            Ops.RShift => (UInt16)(GetValue(node.SourceA) >> int.Parse(node.SourceB)),
            Ops.LShift => (UInt16)(GetValue(node.SourceA) << int.Parse(node.SourceB)),
            _ =>  throw new Exception($"Unknown op code for {nodeName} - {node.Op}")
        };

        //cache the operation results, or forever circle the tree. 
        node.Value = returnValue;

        return returnValue;
    }

    UInt16 part1 = GetValue("a");
    Console.WriteLine($"Part 1: The signal on wire 'a' is {part1}");

    // Reset for part 2 
    nodes["b"].Value = part1;
    nodes["a"].Value = null;
    foreach (Node node in nodes.Values)
    {
        if (node.Op != Ops.Wire) node.Value = null;
    }

    UInt16 part2 = GetValue("a");
    Console.WriteLine($"Part 2: The signal on wire 'a' is {part2}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}