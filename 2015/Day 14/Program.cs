static int TravelDistance(int totalTime, int speed, int sprintTime, int restTime)
{
    // break the time into intervals, and calculate the distance first for full intervals, then for a potential part interval. 
    return totalTime / (sprintTime + restTime) * speed * sprintTime + (int.Min(totalTime % (sprintTime + restTime), sprintTime) * speed);
}

try
{
    const int totalTime = 2503;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    // Basic reindeer statistics. 
    Dictionary<string, (int speed, int sprint, int rest)> reindeer = new();

    // cumulative turn over turn score for Part2. 
    Dictionary<string, int> scorePart2 = new();

    foreach (string line in puzzleInput)
    {
        // Vixen can fly 18 km/s for 5 seconds, but then must rest for 84 seconds.
        // 0     1   2   3  4    5   6 7        8   9    10   11   12  13 14
        string[] puzzleData = line.Split(' ');

        reindeer.Add(puzzleData[0], (int.Parse(puzzleData[3]), int.Parse(puzzleData[6]), int.Parse(puzzleData[13])));
        scorePart2.Add(puzzleData[0], 0);
    }

    int scorePart1 = reindeer.Select(x => TravelDistance(totalTime, x.Value.speed, x.Value.sprint, x.Value.rest)).Max();

    for (int currentTime = 1; currentTime <= totalTime; currentTime++)
    {
        string[] roundWinners = reindeer
            .Select(x => (x.Key, TravelDistance(currentTime, x.Value.speed, x.Value.sprint, x.Value.rest)))
            .OrderByDescending(ob => ob.Item2)
            .GroupBy(item => item.Item2)
            .First()
            .Select(x => x.Key)
            .ToArray();

        foreach(string winner in roundWinners) scorePart2[winner] += 1;
    }

    Console.WriteLine($"The winning reindeer will travel {scorePart1}.");
    Console.WriteLine($"The winning reindeer will have a score of {scorePart2.Select(x => x.Value).Max()}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}