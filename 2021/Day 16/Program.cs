using AoC_2021_Day_16;
static int SumVersion(Packet p) => p.Version + p.SubPackets.Select(SumVersion).Sum();

static long CalcPacket(Packet p)
{
    return p.TypeID switch
    {
        0 => p.SubPackets.Select(CalcPacket).Sum(),
        1 => p.SubPackets.Select(CalcPacket).Aggregate((x, y) => x * y),
        2 => p.SubPackets.Select(CalcPacket).Min(),
        3 => p.SubPackets.Select(CalcPacket).Max(),
        4 => p.Value,
        5 => CalcPacket(p.SubPackets[0]) > CalcPacket(p.SubPackets[1]) ? 1 : 0,
        6 => CalcPacket(p.SubPackets[0]) < CalcPacket(p.SubPackets[1]) ? 1 : 0,
        7 => CalcPacket(p.SubPackets[0]) == CalcPacket(p.SubPackets[1]) ? 1 : 0,
        _ => throw new NotImplementedException(p.TypeID.ToString())
    };
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    HexStream hexStream = new(puzzleInput);

    hexStream.TryGetPacket(out Packet? p);

    int part1Answer = SumVersion(p!);
    long part2Answer = CalcPacket(p!);
    
    Console.WriteLine($"Part 1: The sum of the version numbers is {part1Answer}.");
    Console.WriteLine($"Part 2: The calculated value of the packets is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}