using AoC_2017_KnotHash;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    //KnotHash knot = new();
    string part1Answer = KnotHash.Part1Answer2017Day10(puzzleInput);

    string part2Answer = KnotHash.TieKnot(puzzleInput);

    Console.WriteLine($"Part 1: The product of the first two elements of the list is {part1Answer}.");
    Console.WriteLine($"Part 2: The knot hash of the puzzle input is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}