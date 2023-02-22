try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int TARGET_EGGNOG = 150;

    int[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToArray();

    Dictionary<int, int> containers = new();
    Queue<(int, int)> queue = new();
    HashSet<int> answers = new();

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        containers.Add(1 << i, puzzleInput[i]);
        queue.Enqueue((1 << i, puzzleInput[i]));
    }

    while (queue.Count > 0)
    {
        (int s, int v) = queue.Dequeue();

        foreach (int key in containers.Keys)
        {
            if ((key & s) != 0) continue; 

            int newValue = v + containers[key];

            if (newValue > TARGET_EGGNOG) continue;
            if (newValue == TARGET_EGGNOG)
            { 
                answers.Add(key | s);
            }
            else
            {
                queue.Enqueue((key | s, newValue));
            }
        }
    }

    Console.WriteLine($"Part 1: The maximum number of containers is {answers.Count}");

    int BitCount(int value)
    {
        int returnValue = 0;
        for (int i = 0; i < puzzleInput.Length; i++)
        {
            returnValue += ((1 << i & value) != 0) ? 1 : 0;
        }
        return returnValue;
    }

    var bitCount = answers.Select(x => BitCount(x));

    Console.WriteLine($"Part 2: The fewest containers we can use is {bitCount.Min()} and there are {bitCount.OrderBy(o => o).GroupBy(g => g).First().Count()} ways to use them.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

