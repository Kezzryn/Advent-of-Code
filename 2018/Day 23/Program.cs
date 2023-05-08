using AoC_2018_Day_23;
using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<Point3D, long> nanoSwarm = new();
    Box box = new();

    foreach (string line in puzzleInput)
    {
        long[] numbers = Regex.Matches(line, @"-?\d+").Select(x => long.Parse(x.Value)).ToArray();

        Point3D nanoBot = new(numbers[0], numbers[1], numbers[2]);
        nanoSwarm.Add(nanoBot, numbers[3]);
        box.NanobotsInRange.Add(nanoBot);
    }

    box.Min = new(
        nanoSwarm.Select(x => x.Key.X).Min(),
        nanoSwarm.Select(x => x.Key.Y).Min(),
        nanoSwarm.Select(x => x.Key.Z).Min()
        );

    box.Max = new(
        nanoSwarm.Select(x => x.Key.X).Max(),
        nanoSwarm.Select(x => x.Key.Y).Max(),
        nanoSwarm.Select(x => x.Key.Z).Max()
        );

    (Point3D biggestSignal, long signalValue) = nanoSwarm.OrderByDescending(x => x.Value).First();

    int part1Answer = 0;
    foreach (Point3D point in nanoSwarm.Keys)
    {
        if (Point3D.TaxiDistance(point, biggestSignal) <= signalValue) part1Answer++;
    }

    PriorityQueue<Box, int> queue = new();
    queue.Enqueue(box, nanoSwarm.Count);

    Dictionary<Point3D, long> potentialAnswers = new();

    int maxBotCount = 0;

    while (queue.TryDequeue(out Box? currentBox, out int priority))
    {
        if (Math.Abs(priority) < maxBotCount) break;        // no better answer exists on the queue. 

        if (currentBox.Max == currentBox.Min)
        {
            if (currentBox.NanobotsInRange.Count >= maxBotCount)
            {
                if (currentBox.NanobotsInRange.Count > maxBotCount)
                {
                    potentialAnswers.Clear();
                    maxBotCount = currentBox.NanobotsInRange.Count;
                }

                long taxi = Point3D.TaxiDistance(currentBox.Min);
                potentialAnswers.TryAdd(currentBox.Min, taxi);
            }
            continue;
        }

        
        foreach(Box newBox in currentBox.Subdivide())
        {
            // only bring forward the nanobots that intersect.
            foreach (Point3D nanoBot in currentBox.NanobotsInRange)
            {
                if(newBox.BoxIntersectsSphere(nanoBot, nanoSwarm[nanoBot]))
                {
                    newBox.NanobotsInRange.Add(nanoBot);
                }
            }

            // Only queue if the new box has potential.
            if (newBox.NanobotsInRange.Count >= maxBotCount)
            {
                queue.Enqueue(newBox, -newBox.NanobotsInRange.Count);
            }
        }
    }
    
    long part2Answer = potentialAnswers.OrderBy(ob => ob.Value).First().Value;

    Console.WriteLine($"Part 1: The nanobot with the largest signal radius has {part1Answer} bots in range.");
    Console.WriteLine($"Part 2: The distance to the closest point of the most signal overlap is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}