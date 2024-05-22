static int GetSurfaceArea(List<int> d) => (2 * d[0] * d[1]) + (2 * d[1] * d[2]) + (2 * d[2] * d[0]);
static int GetVolume(List<int> d) => d.Aggregate((a,b) => a * b);
static int GetSlack(List<int> d) => d.Take(2).Aggregate((a,b) => a * b);
static int GetSmallestPerimeter(List<int> d) => d.Take(2).Aggregate((a, b) => 2 * (a + b));

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<List<int>> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split('x')
            .Select(int.Parse)
            .OrderBy(x => x).ToList()
            ).ToList();

    int part1Answer = puzzleInput.Sum(x => GetSurfaceArea(x) + GetSlack(x));
    int part2Answer = puzzleInput.Sum(x => GetVolume(x) + GetSmallestPerimeter(x));

    Console.WriteLine($"Part 1: The we will need a total of {part1Answer} cubic feet of paper.");
    Console.WriteLine($"Part 2: The we will need {part2Answer} cubic feet of ribben.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}