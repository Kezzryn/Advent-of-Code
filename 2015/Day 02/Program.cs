int GetArea(int[] d) => (2 * d[0] * d[1]) + (2 * d[1] * d[2]) + (2 * d[2] * d[0]);
int GetVolume(int[] d) => d.Aggregate((a,b) => a * b);
int GetSlack(int[] d) => d.Take(2).Aggregate((a,b) => a * b);
int GetPerimeter(int[] d) => d.Take(2).Aggregate((a, b) => 2 * (a + b));

try
{
    // This could be further simplified by merging the four formulas to a pair.
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[][] boxes = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Split('x').Select(int.Parse).OrderBy(x => x).ToArray()).ToArray();
    
    int part1 = boxes.Sum(x => GetArea(x) + GetSlack(x));
    Console.WriteLine($"The total paper we will need is {part1} cubic feet.");

    int part2 = boxes.Sum(x => GetVolume(x) + GetPerimeter(x));
    Console.WriteLine($"The total amount of ribbon we will need is {part2} feet.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}