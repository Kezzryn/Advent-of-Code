using AoC_2021_Day_22;
using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<Cuboid> allCuboids = [];

    long part1Answer = 0;

    foreach (string line in puzzleInput)
    {
        int[] numbers = Regex.Matches(line, @"-?\d+").Select(x => int.Parse(x.Value)).ToArray();

        Cuboid newCuboid = new([numbers[0], numbers[2], numbers[4]], [numbers[1], numbers[3], numbers[5]]);

        for (int i = 0; i < allCuboids.Count; i++)
        {
            if (allCuboids[i].Overlap(newCuboid))
            {
                allCuboids.AddRange(allCuboids[i].Exclude(newCuboid));
                allCuboids.RemoveAt(i--);
            }
        }

        if (line.StartsWith("on")) allCuboids.Add(newCuboid);

        if (numbers.All(x => -50 <= x && x <= 50))
        {
            part1Answer = allCuboids.Sum(x => x.Volume);
        }
    }

    long part2Answer = allCuboids.Sum(x => x.Volume);

    Console.WriteLine($"Part 1: There are {part1Answer} blocks on after the startup sequence.");
    Console.WriteLine($"Part 2: Once the full boot is done, there are {part2Answer} blocks on.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}