using AoC_2022_Day_24;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Map theMap = new(puzzleInput);

    theMap.A_Star(new Point3D(theMap.DefaultStart, 0), new Point3D(theMap.DefaultEnd, 0), out int part1Steps);
    Console.WriteLine($"Part 1: We made it across in {part1Steps}.");

    theMap.A_Star(new Point3D(theMap.DefaultEnd, part1Steps), new Point3D(theMap.DefaultStart, 0), out int part2ASteps);
    Console.WriteLine($"Part 2: But then we had to go back for snacks, and it took us {part2ASteps}.");

    theMap.A_Star(new Point3D(theMap.DefaultStart, part1Steps + part2ASteps), new Point3D(theMap.DefaultEnd, 0), out int part2BSteps);
    Console.WriteLine($"Part 2: We got the snacks, and rushed back. Our second crossing took us {part2BSteps}.");

    Console.WriteLine($"Part 2: Whew. We're across with all the snacks, and it only took us {part1Steps + part2ASteps + part2BSteps}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}
