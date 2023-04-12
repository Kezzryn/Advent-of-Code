using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int MAX_ELVES = 5;    //Set to 1 for part 1, set to 5 for part 2.
    const int CHAR_OFFSET = 64;
    const int BASE_TASK_TIME = 60;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<int, List<int>> stepDependencies = new();
    HashSet<int> unprocessedSteps = new(); 
     
    foreach (string line in puzzleInput)
    {
        //Step A must be finished before step B can begin.
        //0    1 2    3  4        5      6    7 8   9
        var t = line.Split(' ');
        int stepParent = t[1][0];
        int stepDependent = t[7][0];
        
        unprocessedSteps.Add(stepDependent);
        unprocessedSteps.Add(stepParent);

        if (!stepDependencies.TryAdd(stepDependent, new List<int> { stepParent }))
        {
            stepDependencies[stepDependent].Add(stepParent);
        }
    }

    PriorityQueue<int, int> availQueue = new();
    PriorityQueue<int, int> workQueue = new();
    StringBuilder part1Answer = new();
    int part2Answer = 0;

    while (unprocessedSteps.Count > 0 || workQueue.Count > 0)  
    {
        foreach(int step in unprocessedSteps)
        {
            // if the work step does NOT appear in any of the lists in dependecies 
            if (!stepDependencies.Any(x => x.Key == step))
            {
                availQueue.Enqueue(step, step);
                unprocessedSteps.Remove(step);
            }
        }

        while (availQueue.Count > 0 && workQueue.Count < MAX_ELVES)
        {
            int availJob = availQueue.Dequeue();
            workQueue.Enqueue(availJob, part2Answer + BASE_TASK_TIME + (availJob - CHAR_OFFSET));
        }

        if (!workQueue.TryDequeue(out int nextStep, out int timeFinished)) continue;

        part1Answer.Append((char)nextStep);
        part2Answer = timeFinished;

        foreach (var entry in stepDependencies.Where(x => x.Value.Contains(nextStep))) 
        {
            entry.Value.Remove(nextStep);
            if (entry.Value.Count == 0) stepDependencies.Remove(entry.Key);
        }
    }

    // SET MAX_ELVES to 1 for correct part 1 answer.
    Console.WriteLine($"Part 1: The instruction order is: {part1Answer}"); 

    Console.WriteLine($"Part 2: Working together {MAX_ELVES} elves can finish in {part2Answer} seconds.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}