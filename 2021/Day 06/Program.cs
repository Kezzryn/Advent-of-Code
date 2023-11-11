try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const long NUM_DAYS_PART1 = 80;
    const int NUM_DAYS = 256;

    List<long> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(",").Select(long.Parse).ToList();

    Queue<long> lanternFish = new();
    Queue<long> lanternFishChildren = new();
    lanternFishChildren.Enqueue(0); // padding for the extra two days
    lanternFishChildren.Enqueue(0);

    for (int i = 0; i < 7; i++)
    {
        lanternFish.Enqueue(puzzleInput.Count(x => x == i));
        lanternFishChildren.Enqueue(0);
    }

    long part1Answer = 0;
    for (int day = 1; day <= NUM_DAYS; day++)
    {
        if (!lanternFish.TryDequeue(out long numFish)) Console.WriteLine($"{day} NO FISH!");
        if(!lanternFishChildren.TryDequeue(out long numChildren)) Console.WriteLine($"{day} NO CHILD FISH!");

        long freshFish = numFish + numChildren;

        lanternFish.Enqueue(freshFish);
        lanternFishChildren.Enqueue(freshFish);

        if(day == NUM_DAYS_PART1) part1Answer = lanternFish.Sum() + lanternFishChildren.Sum();
    }

    long part2Answer = lanternFish.Sum() + lanternFishChildren.Sum(); ; 

    Console.WriteLine($"Part 1: After {NUM_DAYS_PART1} days there will be {part1Answer} lanternfish.");
    Console.WriteLine($"Part 2: After {NUM_DAYS} days there will be {part2Answer} lanternfish.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}