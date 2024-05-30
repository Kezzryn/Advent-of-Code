try
{
    const int TARGET_EGGNOG = 150;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToList();

    Dictionary<int, int> containers = [];
    Queue<(int, int)> queue = new();
    HashSet<int> part1Answers = [];

    for (int i = 0; i < puzzleInput.Count; i++)
    {
        containers.Add(1 << i, puzzleInput[i]);
        queue.Enqueue((1 << i, puzzleInput[i]));
    }

    while (queue.TryDequeue(out (int key, int value) current))
    {
        foreach (int key in containers.Keys.Where(x => (x & current.key) == 0))
        {
            int newValue = current.value + containers[key];

            if (newValue == TARGET_EGGNOG)
            {
                part1Answers.Add(key | current.key);
            }
            else if (newValue < TARGET_EGGNOG)
            {
                queue.Enqueue((key | current.key, newValue));
            }
        }
    }

    IEnumerable<int> part2Answers = part1Answers.Select(int.PopCount);

    Console.WriteLine($"Part 1: The maximum number of containers is {part1Answers.Count}");
    Console.WriteLine($"Part 2: The fewest containers we can use is {part2Answers.Min()} and there are {part2Answers.OrderBy(o => o).GroupBy(g => g).First().Count()} ways to use them.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}