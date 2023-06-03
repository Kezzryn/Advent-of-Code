try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    long currentTime = int.Parse(puzzleInput[0]);
    List<long> busses = puzzleInput[1].Replace("x","-1").Split(',').Select(long.Parse).ToList();
    
    long part1Answer = long.MaxValue;
    long busID = 0;

    // Part 1 is a simple comparison.
    foreach (long bus in busses.Where(x => x != -1))
    {
        long nextBusTime = currentTime + (bus - (currentTime % bus) - currentTime);
        if (nextBusTime < part1Answer)
        {
            part1Answer = nextBusTime;
            busID = bus;
        }
    }

    part1Answer *= busID;

    Console.WriteLine($"Part 1: The checksum for the next bus and the time to wait is {part1Answer}.");

    // Part 2 find a match, then roll it forward, cumulating the steps with the Least Common Multipliter.
    // As our data is all prime, we can shortcut the LCM with a simple multiple.
    Dictionary<long, long> busTimeSlots = new();
    busses.ForEach(x => busTimeSlots.Add(x, busses.IndexOf(x)));

    long part2Answer = busTimeSlots.First().Key;
    long step = busTimeSlots.First().Key;

    foreach ((long bus, long offset) in busTimeSlots.Skip(1))
    {
        while (((part2Answer + offset) % bus) != 0) 
        {
            part2Answer += step;
        }
        step *= bus;
    }

    Console.WriteLine($"Part 2: The busses line up every {part2Answer} minutes.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}