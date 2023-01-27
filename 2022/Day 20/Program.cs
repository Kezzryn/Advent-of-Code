using AoC_2022_Day_20;

try
{
    const string PUZZLE_INPUT = @"..\..\..\..\..\Input Files\Day 20.txt";
    const int decryptionKey = 811589153;

    Mixer codeMixerPart1 = new(File.ReadAllLines(PUZZLE_INPUT));
    Mixer codeMixerPart2 = new(File.ReadAllLines(PUZZLE_INPUT), decryptionKey);

    codeMixerPart1.Mix(1);
    codeMixerPart2.Mix(10);

    long part1Result = codeMixerPart1.ValueAt(1000) + codeMixerPart1.ValueAt(2000) + codeMixerPart1.ValueAt(3000);
    long part2Result = codeMixerPart2.ValueAt(1000) + codeMixerPart2.ValueAt(2000) + codeMixerPart2.ValueAt(3000);

    Console.WriteLine($"Part1: the sum of the three numbers that form the grove coordinates are: {part1Result}");
    Console.WriteLine($"Part2: the sum of the three numbers that form the grove coordinates are: {part2Result}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
