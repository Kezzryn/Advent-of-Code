using AoC_2022_Day_13;

try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 13.txt";

    Packet dividerPacketA = new("[[2]]");
    Packet dividerPacketB = new("[[6]]");

    List<Packet> puzzleInput = File
        .ReadAllLines(PUZZLE_INPUT)
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => new Packet(x))
        .ToList();

    int indexSum = puzzleInput
            .Where((x, i) => i % 2 == 0)
            .Zip(puzzleInput
                .Where((x, i) => i % 2 != 0))
            .Select((x, i) => x.First < x.Second ? i + 1 : 0)
            .ToList()
            .Sum();

    Console.WriteLine($"Part one: The sum of the indices of the packets in the right order (a < b) is: {indexSum}");

    puzzleInput.Add(dividerPacketA);
    puzzleInput.Add(dividerPacketB);

    puzzleInput.Sort();

    //Remember! Add one to IndexOf 'cause zero bound, or spend an hour debugging. 
    Console.WriteLine($"Part two: Our decoder key is: {(puzzleInput.IndexOf(dividerPacketA) + 1) * (puzzleInput.IndexOf(dividerPacketB) + 1)}");
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}