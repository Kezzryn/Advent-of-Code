using AoC_2018_Day_25;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    HashSet<Point4D> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => new Point4D(x)).OrderBy(x =>  x.DistFromZero).ToHashSet();
     
    Dictionary<int, List<Point4D>> constellations = new();
    int clusterID = 0;


    while (puzzleInput.Count > 0)
    {
        if (!constellations.ContainsKey(clusterID))
        { 
            constellations.Add(clusterID, new() { puzzleInput.First() });
            puzzleInput.Remove(constellations[clusterID].First());
        }

        List<Point4D> matches = new();
        foreach (Point4D point in constellations[clusterID])
        {
            matches.AddRange(puzzleInput.Where(x => Point4D.TaxiDistance4D(x, point) <= 3).ToList());
        }

        if (matches.Count == 0)
        {
            clusterID++;
        }
        else
        {
            foreach(Point4D point in matches)
            {
                constellations[clusterID].Add(point);
                puzzleInput.Remove(point);
            }
        }
    }

    Console.WriteLine($"Part 1: There are {constellations.Count} constellations.");
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}