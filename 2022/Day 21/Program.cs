using AoC_2022_Day_21;

try
{
    const string PUZZLE_INPUT = @"..\..\..\..\..\Input Files\Day 21.txt";
    MonkeyTree monkeys = new(File.ReadAllLines(PUZZLE_INPUT));
    //monkeys.ToggleVerbose();

    long part1Value = monkeys.GetValue("root");
    Console.WriteLine($"Part 1 answer is {part1Value}");

    MTNode newMonkey = new()
    {
        Operand = "="
    };

    monkeys.UpdateMonkey("root", newMonkey);
    monkeys.UpdateMonkeyValue("humn", null);

    long part2Value = monkeys.GetNodeValue("humn");
    Console.WriteLine($"Part 2 answer is {part2Value}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
