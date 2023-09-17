try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToList();

    int part1Answer = puzzleInput.Take(puzzleInput.Count - 1).Where((item, index) => item < puzzleInput[index + 1]).Count();

    List<int> threeWindow = new();
    for(int i = 0; i < puzzleInput.Count - 2; i++)
    {
        threeWindow.Add(puzzleInput.GetRange(i, 3).Sum());
    }

    int part2Answer = threeWindow.Take(threeWindow.Count - 1).Where((item, index) => item < threeWindow[index + 1]).Count();

    Console.WriteLine($"Part 1: The number of measurements that are larger than the previous measurement is {part1Answer}.");
    Console.WriteLine($"Part 2: With a three unit sliding window the number of larger measurements is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}