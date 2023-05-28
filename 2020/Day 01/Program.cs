try
{
    const int TARGET_SUM = 2020;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToArray();

    IEnumerable<int> part1Answer = from a in puzzleInput
                      from b in puzzleInput
                      where a + b == TARGET_SUM
                      select (a * b);

    IEnumerable<int> part2Answer = from a in puzzleInput
                      from b in puzzleInput
                      from c in puzzleInput
                      where a + b + c == TARGET_SUM
                      select (a * b * c);


    Console.WriteLine($"Part 1: The product of the two numbers that sum to {TARGET_SUM} is {part1Answer.FirstOrDefault(-1)}.");
    Console.WriteLine($"Part 2: The product of the three numbers that sum to {TARGET_SUM} is {part2Answer.FirstOrDefault(-1)}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}