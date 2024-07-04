using AoC_2016_Day_24;

static int FindPath(Map theMap, bool returnToStart = false)
{
    PriorityQueue<string, int> queue = new();

    queue.Enqueue("0", 0);
    int targetPathLength = theMap.WayPoints.Count + (returnToStart ? 1 : 0);

    int numSteps = int.MaxValue;

    while (queue.TryDequeue(out string? path, out int currentSteps))
    {
        if (path.Length == targetPathLength)
        {
            if (currentSteps < numSteps) numSteps = currentSteps;
            continue;
        }

        if (currentSteps >= numSteps) continue;

        if (returnToStart && !theMap.WayPoints.Keys.Except(path).Any())
        {
            if (theMap.A_Star(theMap.WayPoints[path[^1]], theMap.WayPoints[path[0]]))
            {
                string nextStep = path + path[0];
                queue.Enqueue(nextStep, theMap.NumSteps + currentSteps);
            }
        }
        else
        {
            foreach (char wp in theMap.WayPoints.Keys.Except(path))
            {
                if (theMap.A_Star(theMap.WayPoints[path[^1]], theMap.WayPoints[wp]))
                {
                    string nextStep = path + wp;
                    queue.Enqueue(nextStep, theMap.NumSteps + currentSteps);
                }
            }
        }
    }  
    return numSteps;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool RETURN_TO_START = true;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Map theMap = new(puzzleInput);

    Console.WriteLine($"Part 1: The shortest path is: {FindPath(theMap)}");
    Console.WriteLine($"Part 2: The shortest path with a return to start is: {FindPath(theMap, RETURN_TO_START)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}