try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    
    /*
    tlmnwl (474) -> nidvi, alpas, urexzjf, ftolbk, efulo
    thwgxk (50)
    soxzrwm (83)
    */

    Dictionary<string, (string parentNode, int num)> theTree = new();

    foreach (string node in puzzleInput)
    {
        string[] supportNode = node.Split("->");

        string[] parent = supportNode[0].Split(' ', StringSplitOptions.TrimEntries).ToArray();

        if (supportNode.GetUpperBound(0) > 0)
        {
            string[] children = supportNode[1].Split(',', StringSplitOptions.TrimEntries);

            foreach (string child in children)
            {
                if (!theTree.TryAdd(child, (parent[0], 0)))
                {
                    theTree[child] = (parent[0], theTree[child].num);
                }
            }
        }
    
        if (!theTree.TryAdd(parent[0], (string.Empty, int.Parse(parent[1].Trim("()".ToCharArray())))))
        {
            // not added, do we already have this? 
            if (theTree.TryGetValue(parent[0], out (string parentNode, int num) value))
            {
                value.num = int.Parse(parent[1].Trim("()".ToCharArray()));
            }
        }
    }

    (string rootNode, _) = theTree.ElementAt(0).Value;


    while (rootNode != String.Empty)
    {
        if (theTree[rootNode].parentNode == String.Empty) break;
        rootNode = theTree[rootNode].parentNode;
    }

    Console.WriteLine($"Part 1: {rootNode}");

    //starting at 
    

    Console.WriteLine($"Part 2: ");
}
catch (Exception e)
{
    Console.WriteLine(e);
}