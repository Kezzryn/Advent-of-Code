using AoC_2022_Day_16;

try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";

    CaveComplex volcano = new(File.ReadAllLines(PUZZLE_INPUT));
    
    volcano.PrintLayout();

    volcano.FullLinkage();

    /*
    Dictionary<string, (string, int)> path;
    
    volcano.Dijkstra("AA",out path);
    
    foreach (KeyValuePair<string, (string prev, int dist)> kvp in path)
    {
        Console.WriteLine($"{kvp.Key} = {kvp.Value.dist} {kvp.Value}");
    }
    */

    volcano.PrintLayout();

   // int pressure = volcano.DontBlowUp(30);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
