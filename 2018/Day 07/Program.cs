using System.Text;

static void ElfScheduler(string[] puzzleInput, int numElves, out string part1Answer, out int part2Answer)
{
    const int CHAR_OFFSET = 64;
    const int BASE_TASK_TIME = 60;

    Dictionary<int, List<int>> stepDependencies = [];
    HashSet<int> unprocessedSteps = [];

    foreach (string line in puzzleInput)
    {
        //Step A must be finished before step B can begin.
        //0    1 2    3  4        5      6    7 8   9
        var t = line.Split(' ');
        int stepParent = t[1][0];
        int stepDependent = t[7][0];

        unprocessedSteps.Add(stepDependent);
        unprocessedSteps.Add(stepParent);

        if (!stepDependencies.TryAdd(stepDependent, [stepParent]))
        {
            stepDependencies[stepDependent].Add(stepParent);
        }
    }

    PriorityQueue<int, int> availQueue = new();
    PriorityQueue<int, int> workQueue = new();
    StringBuilder part1AnswerSB = new();
    part2Answer = 0;

    while (unprocessedSteps.Count > 0 || workQueue.Count > 0)
    {
        foreach (int step in unprocessedSteps)
        {
            // if the work step does NOT appear in any of the lists in dependecies 
            if (!stepDependencies.Any(x => x.Key == step))
            {
                availQueue.Enqueue(step, step);
                unprocessedSteps.Remove(step);
            }
        }

        while (availQueue.Count > 0 && workQueue.Count < numElves)
        {
            int availJob = availQueue.Dequeue();
            workQueue.Enqueue(availJob, part2Answer + BASE_TASK_TIME + (availJob - CHAR_OFFSET));
        }

        if (!workQueue.TryDequeue(out int nextStep, out int timeFinished)) continue;

        part1AnswerSB.Append((char)nextStep);
        part2Answer = timeFinished;

        foreach (var entry in stepDependencies.Where(x => x.Value.Contains(nextStep)))
        {
            entry.Value.Remove(nextStep);
            if (entry.Value.Count == 0) stepDependencies.Remove(entry.Key);
        }
    }

    part1Answer = part1AnswerSB.ToString();
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    ElfScheduler(puzzleInput, 1, out string part1Answer, out _);
    ElfScheduler(puzzleInput, 5, out _, out int part2Answer);

    Console.WriteLine($"Part 1: The instruction order is: {part1Answer}");
    Console.WriteLine($"Part 2: Working together, 5 elves can finish in {part2Answer} seconds.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}