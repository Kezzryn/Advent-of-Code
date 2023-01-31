using AoC_2022_Day_16;

try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";

    CaveComplex volcano = new(File.ReadAllLines(PUZZLE_INPUT));

    Dictionary<string, string> prev;
    Dictionary<string, int> dist; 

    volcano.Dijkstra("AA",out prev, out dist);

    foreach (KeyValuePair<string, int> kvp in dist)
    {
        Console.WriteLine($"DIST: {kvp.Key} = {kvp.Value}");
    }

    foreach (KeyValuePair<string, string> kvp in prev)
    {
        Console.WriteLine($"PREV: {kvp.Key} = {kvp.Value}");
    }

    volcano.PrintLayout();

   // int pressure = volcano.DontBlowUp(30);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
