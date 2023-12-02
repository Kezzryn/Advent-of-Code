using AoC_2021_Day_18;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    Node summedNode = puzzleInput.Select(x => new Node(x)).Aggregate((x,y) => Node.Add(x,y));
    int part1Answer = summedNode.FindMagnitude();

    List<Node> part2Nodes = new();
    foreach ((string l, string r) in from left in puzzleInput
                                     from right in puzzleInput
                                     where !left.Equals(right)
                                     select (left, right))
    {
        part2Nodes.Add(Node.Add(l,r));
    }

    int part2Answer = part2Nodes.Select(x => x.FindMagnitude()).Max();

    Console.WriteLine($"Part 1: The magnitude of the sum of the list is {part1Answer}.");
    Console.WriteLine($"Part 2: The largest magintude of any pair of numbers is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}