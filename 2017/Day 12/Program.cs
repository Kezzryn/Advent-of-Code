try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<int, List<int>> theTree = new();
      
    // Loads an entire group at once. 
    // the linked nodes are bi-directional, so we get the whole group/cluster with a little bit of recursion.
    static void LoadGroup(int lineNum, string[] puzzleInput, Dictionary<int, List<int>> theTree)
    {
        string[] temp = puzzleInput[lineNum].Split("<->", StringSplitOptions.TrimEntries).ToArray();
        int key = int.Parse(temp[0]);
        int[] linkedNodes = temp[1].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();

        if (theTree.TryAdd(key, new()))
        {
            foreach (int node in linkedNodes)
            {
                if (!theTree[key].Contains(node))
                {
                    theTree[key].Add(node);
                }
            }

            foreach (int node in linkedNodes)
            {
                if (key != node) LoadGroup(node, puzzleInput, theTree);
            }
        }
    }

    LoadGroup(0, puzzleInput, theTree);
    int part1Answer = theTree.Count;
    int part2Answer = 1; //add in group 0

    for (int i = 1; i < puzzleInput.Length; i++)
    {
        if (!theTree.ContainsKey(i))
        {
            LoadGroup(i, puzzleInput, theTree);
            part2Answer++;
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} items in group zero.");
    Console.WriteLine($"Part 2: There are {part2Answer} groups.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}