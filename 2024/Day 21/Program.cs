using AoC_2024_Day_21;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Robot keypadRobot_Part1 = new(Robot.NumericKeypad, 2);
    long part1Answer = puzzleInput.Sum(doorCode => keypadRobot_Part1.ProcessDoorCode(doorCode) * int.Parse(doorCode[..^1]));

    Robot keypadRobot_Part2 = new(Robot.NumericKeypad, 25);
    long part2Answer = puzzleInput.Sum(doorCode => keypadRobot_Part2.ProcessDoorCode(doorCode) * int.Parse(doorCode[..^1]));

    Console.WriteLine($"Part 1: There are {part1Answer} moves for two robots.");
    Console.WriteLine($"Part 2: When there are 25 robots, it'll take {part2Answer} keypresses to free the historians.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}