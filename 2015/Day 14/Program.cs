static int TravelDistance(int totalTime, int speed, int sprintTime, int restTime)
{
    // break the time into intervals, and calculate the distance first for full intervals, then for a potential part interval. 
    return totalTime / (sprintTime + restTime) * speed * sprintTime + (int.Min(totalTime % (sprintTime + restTime), sprintTime) * speed);
}

try
{
    const int TOTAL_TIME = 2503;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    // Basic reindeer statistics. 
    Dictionary<string, (int speed, int sprint, int rest)> reindeer = [];

    // cumulative turn over turn score for Part2. 
    Dictionary<string, int> part2Answer = [];

    foreach (string line in puzzleInput)
    {
        // Vixen can fly 18 km/s for 5 seconds, but then must rest for 84 seconds.
        // 0     1   2   3  4    5   6 7        8   9    10   11   12  13 14
        string[] puzzleData = line.Split(' ');

        reindeer.Add(puzzleData[0], (int.Parse(puzzleData[3]), int.Parse(puzzleData[6]), int.Parse(puzzleData[13])));
        part2Answer.Add(puzzleData[0], 0);
    }

    int part1Answer = reindeer.Select(x => TravelDistance(TOTAL_TIME, x.Value.speed, x.Value.sprint, x.Value.rest)).Max();

    for (int currentTime = 1; currentTime <= TOTAL_TIME; currentTime++)
    {
        reindeer
            .Select(x => (x.Key, TravelDist: TravelDistance(currentTime, x.Value.speed, x.Value.sprint, x.Value.rest)))
            .OrderByDescending(x => x.TravelDist)
            .GroupBy(x => x.TravelDist)
            .First()
            .ToList().ForEach(x => part2Answer[x.Key] += 1);
    }

    Console.WriteLine($"The winning reindeer will travel {part1Answer}.");
    Console.WriteLine($"The winning reindeer will have a score of {part2Answer.Max(x => x.Value)}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}