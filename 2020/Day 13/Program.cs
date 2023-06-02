try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int currentTime = int.Parse(puzzleInput[0]);
    List<int> busses = puzzleInput[1].Replace("x","-1").Split(',').Select(int.Parse).ToList();

    Dictionary<int, int> busSlot = new();

    int part1Answer = int.MaxValue;
    int busID = 0;

    foreach(int bus in busses.Where(x => x != -1))
    {
        int nextBus = currentTime + (bus - (currentTime % bus) - currentTime);
        busSlot.Add(bus, bus - busses.IndexOf(bus));

        if (nextBus < part1Answer)
        {
            part1Answer = nextBus;
            busID = bus;
        }
    }

    part1Answer *= busID;


    //17,x,13,19 is 3417
    //busses = new() { 1789, 37, 47, 1889 };
    //busSlot = new()
    //{
    //    { 1789, 0 },
    //    { 37, 36 },
    //    { 47, 45 },
    //    { 1889, 1886 }
    //};
    
    busses.RemoveAll(x => x == -1);

    long factor = busses.Aggregate((a, b) => a * b);
    long part2Answer = 0;
    long time = 0;
    while (part2Answer == 0)
    {
        bool isFound = true;
        foreach(int bus in busses.Skip(1))
        {
            if ((time % bus) != busSlot[bus])
            {
                isFound = false;
                break;
            }
        }
        if(isFound) part2Answer = time;

        time += busses.First();
        if (time < 0) part2Answer = -1; // overflow check :P
    }


    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}