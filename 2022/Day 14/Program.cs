using AoC_2022_Day_14;

try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 14.txt";

    Map chasm = new(File.ReadAllLines(PUZZLE_INPUT), false);

    while (chasm.DropSand()) ;
    Console.WriteLine($"Part 1 SandCount: {chasm.CountSand()}");
    //chasm.PrintMap();
    chasm.SweepUp();
    chasm.AddFloor();
    while (chasm.DropSand()) ;
    Console.WriteLine($"Part 2 SandCount: {chasm.CountSand()}");
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}
