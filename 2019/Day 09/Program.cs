using AoC_2019_IntcodeVM;

try  
{
    /*
     * Thanks to this post for guidance on the unspoken modes.
     https://www.reddit.com/r/adventofcode/comments/e8aw9j/2019_day_9_part_1_how_to_fix_203_error/
     */

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();

    IntcodeVM vm = new(puzzleInput);
    vm.SetInput(1);  
    vm.Run();

    Console.Write($"Part 1: The results of the test mode is: ");
    vm.GetOutput(out long? part1Answer);
    Console.WriteLine(part1Answer);
    
    vm.Reset(puzzleInput);
    vm.SetInput(2);
    vm.Run();

    Console.Write ($"Part 2: The coordinates are: ");
    vm.GetOutput(out long? part2Answer);
    Console.WriteLine(part2Answer);
}
catch (Exception e)
{
    Console.WriteLine(e);
}