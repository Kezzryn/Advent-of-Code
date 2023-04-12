using AoC_2018_Day_08;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(' ').Select(int.Parse).ToArray();

    Dictionary<int, Node> dataTree = new();

    int LoadTree(int cursor = 0)
    {
        int ID = cursor;
        int headerChild = puzzleInput[cursor++];
        int headerMeta = puzzleInput[cursor++];

        dataTree.Add(ID, new Node());
        
        foreach (int i in Enumerable.Range(0, headerChild))
        {
            dataTree[ID].Children.Add(cursor);
            cursor = LoadTree(cursor);
        }

        foreach (int i in Enumerable.Range(cursor, headerMeta))
        {
            dataTree[ID].MetaData.Add(puzzleInput[i]);
        }

        if (dataTree[ID].Children.Count == 0)
        {
            dataTree[ID].Value = dataTree[ID].MetaData.Sum();
        }
        else
        {
            dataTree[ID].Value = 0;
            foreach (int child in dataTree[ID].MetaData)
            {
                if (child > dataTree[ID].Children.Count) continue;
                int childID = dataTree[ID].Children[child - 1]; //zero bound adjustment 
                dataTree[ID].Value += dataTree[childID].Value;
            }
        }

        return cursor += headerMeta;
    }

    LoadTree();

    int part1Answer = dataTree.Select(x => x.Value.MetaData.Sum()).Sum();
    int part2Answer = dataTree[0].Value;

    Console.WriteLine($"Part 1: The sum of the meta data is {part1Answer}.");
    Console.WriteLine($"Part 2: The value of the root node is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}