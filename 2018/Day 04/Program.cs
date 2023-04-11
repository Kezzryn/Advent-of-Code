try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    const string GUARD = "Guard";
    const string FALLS_ASLEEP = "falls asleep";
    const string WAKES_UP = "wakes up";

    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();
    puzzleInput.Sort();

    Dictionary<int, int[]> guards = new();

    int currentGuard = 0;
    int sleepStart = 0;

    foreach (string line in puzzleInput)
    {
        // [1518-11-01 00:00]
        // 012345678901234567
        int hours = int.Parse(line[12..14]);
        int minutes = int.Parse(line[15..17]);
        
        if (line.Contains(GUARD))
        {
            currentGuard = int.Parse(line[(line.IndexOf("#")+1)..].Split(' ').First());
            guards.TryAdd(currentGuard, new int[60]);
        }

        if (line.Contains(FALLS_ASLEEP))
        {
            sleepStart = minutes;
        }

        if (line.Contains(WAKES_UP))
        {
            guards.TryGetValue(currentGuard, out int[]? guardSchedule);
            if (guardSchedule is null) throw new Exception();

            foreach (int i in Enumerable.Range(sleepStart, minutes - sleepStart))
            {
                guardSchedule[i]++;
            }
        }
    }

    int part1Guard = guards.OrderByDescending(x => x.Value.Sum()).First().Key;
    int part2Guard = guards.OrderByDescending(x => x.Value.Max()).First().Key;

    int part1SleepyMinute = Array.IndexOf(guards[part1Guard], guards[part1Guard].Max());
    int part2SleepyMinute = Array.IndexOf(guards[part2Guard], guards[part2Guard].Max());

    int part1Answer = part1Guard * part1SleepyMinute;
    int part2Answer = part2Guard * part2SleepyMinute;


    Console.WriteLine($"Part 1: The guard that spends the most time asleep is {part1Guard} with {part1SleepyMinute} being the most common. Sleep score: {part1Answer}");
    Console.WriteLine($"Part 2: The guard that falls asleep most consistantly is {part2Guard} at minute {part2SleepyMinute}. Sleep score: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}