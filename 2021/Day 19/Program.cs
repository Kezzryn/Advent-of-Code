using AoC_2021_Day_19;
using System.Numerics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    List<List<Vector3>> puzzleInput =
        File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF)
            .Select(c => c.Split(CRLF).Skip(1)
                    .Select(v => new Vector3(v.Split(',').Select(float.Parse).ToArray())
                ).ToList()
            ).ToList();

    BeaconMap theMap = new(puzzleInput.First());
    Queue<BeaconMap> queue = new(puzzleInput.Skip(1).Select(x => new BeaconMap(x)));

    while (queue.TryDequeue(out BeaconMap? mapFragment))
    {
        if (!theMap.TryAddBeaconMap(mapFragment))
            queue.Enqueue(mapFragment);
    }

    int part1Answer = theMap.Beacons.Count;
    int part2Answer = theMap.GetLargestScannerDistance();

    Console.WriteLine($"Part 1: There are {part1Answer} beacons.");
    Console.WriteLine($"Part 2: The greatest distance between two scanners is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
