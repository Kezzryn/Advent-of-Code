using BKH.EnumExtentions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    IEnumerable<int> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Select(x => x == '(' ? 1 : -1);

    int part1Answer = puzzleInput.Sum();
    int part2Answer = puzzleInput.CumulativeSum().Select((x, i) => (Floor: x, Index: (i + 1))).Where(x => x.Floor < 0).First().Index;
 
    Console.WriteLine($"Part 1: Santa is on floor: {part1Answer}");
    Console.WriteLine($"Part 2: Santa first enters the basement at {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}