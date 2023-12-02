using AoC_2021_Day_18;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    //List<string> puzzleInput= new()
    //{
    //    "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
    //    "[[[5,[2,8]],4],[5,[[9,9],0]]]",
    //    "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
    //    "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
    //    "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
    //    "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
    //    "[[[[5,4],[7,7]],8],[[8,3],8]]",
    //    "[[9,3],[[9,9],[6,[4,9]]]]",
    //    "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
    //    "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
    //};

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

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}