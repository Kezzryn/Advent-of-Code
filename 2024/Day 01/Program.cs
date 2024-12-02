try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<int> leftList = [];
    List<int> rightList = [];

    foreach(string line in puzzleInput)
    {
        int[] parsed = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        leftList.Add(parsed[0]);
        rightList.Add(parsed[1]);
    }

    leftList.Sort();
    rightList.Sort();

    int part1Answer = leftList.Select((L, i) => Math.Abs(L - rightList[i])).Sum();
    int part2Answer = leftList.Sum(L => L * rightList.Count(R => R == L)); 

    Console.WriteLine($"Part 1: The total list distance is {part1Answer}.");
    Console.WriteLine($"Part 2: The similarity score is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}