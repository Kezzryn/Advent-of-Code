try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<int, List<int>> theTree = new();
    
    int part1Answer = 0;
    int part2Answer = 0;
    
    void LoadLine(int lineNum)
    {
        string[] temp = puzzleInput[lineNum].Split("<->", StringSplitOptions.TrimEntries).ToArray();
        int key = int.Parse(temp[0]);
        int[] children = temp[1].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();

        if (theTree.TryAdd(key, new()))
        {
            foreach (int child in children)
            {
                if (!theTree[key].Contains(child))
                {
                    theTree[key].Add(child);
                }
            }

            foreach (int child in children)
            {
                if (key != child) LoadLine(child);
            }
        }
    }

    for(int i = 0; i < puzzleInput.Length; i++)
    {
        if (!theTree.ContainsKey(i))
        {
            part2Answer++;
            LoadLine(i);
            if (i == 0) part1Answer = theTree.Count;
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} items in group zero.");
    Console.WriteLine($"Part 2: There are {part2Answer} groups.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}