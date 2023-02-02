using AoC_2022_Day_16;

try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";
    const int totalTimePart1 = 30;
    const int totalTimePart2 = 26;

    CaveComplex volcano = new(File.ReadAllLines(PUZZLE_INPUT));
    volcano.ToggleVerbose();

    List<string> path = volcano.BnBSolve("AA", volcano.AllCaves(),totalTimePart1);
    int totalpressure = volcano.PathCost(path, totalTimePart1, out _ );

    Console.WriteLine($"{totalpressure} released, achieved by path {CaveComplex.ListToString(path)}");

    /*
    foreach (string exit in volcano._caves["AA"].ExitList())
    {
        List<string> fromCaves = new()
        {
            "AA",
            exit
        };

        List<string> searchCaves = volcano.AllCaves().Except(fromCaves).ToList();

        int timeRemaining = 30;

        int of = volcano.ObjectiveFunction(fromCaves, ref timeRemaining);
        timeRemaining -= volcano._caves[fromCaves.Last()].ExitDistance(searchCaves.First());
        int sc = volcano.ObjectiveFunction(searchCaves, ref timeRemaining);

        Console.WriteLine($" AA -> {exit} =  {of} + {sc} = {of+sc}");
    }
    */

    //    testing sequence: AABB at time 30
    //testing sequence: CCDDEEHHJJ at time 28
    //AA->BB = 364 + 1284 = 1648

    /*
    Dictionary<string, (string, int)> path;

    volcano.Dijkstra("AA",out path);

    foreach (KeyValuePair<string, (string prev, int dist)> kvp in path)
    {
        Console.WriteLine($"{kvp.Key} = {kvp.Value.dist} {kvp.Value}");
    }
    */



    // int pressure = volcano.DontBlowUp(30);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
