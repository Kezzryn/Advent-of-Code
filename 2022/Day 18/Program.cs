using AoC_2022_Day_18;
using System.Diagnostics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    Stopwatch sw = Stopwatch.StartNew();

    Droplet obs = new(PUZZLE_INPUT);

    Console.WriteLine($"Total surface area is {obs.GetObsidianArea()}");
    Console.WriteLine($"Interior surface area is {obs.GetAirPocketArea()}");
    Console.WriteLine($"Exterior surface area is {obs.GetObsidianArea() - obs.GetAirPocketArea()}");

    sw.Stop();

    Console.WriteLine($"Stopwatch {sw.ElapsedMilliseconds}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}


