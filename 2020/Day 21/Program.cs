using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, List<List<string>>> ingrediantsByAllergen = new();
    Dictionary<string, List<string>> allergenWorkspace = new();
    Dictionary<string, string> actualAllergen = new();
    List<List<string>> shoppingList = new();

    foreach (string line in puzzleInput)
    {
        //mxmxvkd kfcds sqjhc nhms(contains dairy, fish)
        string[] step1 = line.Replace("(","").Replace(")","").Split("contains");
        List<string> listedIngredients = step1[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        List<string> listedAllergens = step1[1].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        
        shoppingList.Add(new(listedIngredients));
        foreach(string listedAllergen in listedAllergens)
        {
            ingrediantsByAllergen.TryAdd(listedAllergen, new());
            ingrediantsByAllergen[listedAllergen].Add(new(listedIngredients));
        }
    }

    foreach(var kvp in ingrediantsByAllergen)
    {
        allergenWorkspace[kvp.Key] = kvp.Value.Aggregate((x, y) => x.Intersect(y).ToList());
    }
    
    while (allergenWorkspace.Any(x => x.Value.Count > 1))
    {
        foreach ((string solved, string key) in from s in allergenWorkspace.Where(x => x.Value.Count == 1).Select(x => x.Value.FirstOrDefault(""))
                                                from k in allergenWorkspace.Where(x => x.Value.Count > 1).Select(x => x.Key)
                                                select  (s, k))
        {
            allergenWorkspace[key].Remove(solved);
        }
    }

    StringBuilder sb = new();
    foreach(var kvp in allergenWorkspace.OrderBy(x => x.Key))
    {
        actualAllergen[kvp.Key] = kvp.Value.First();
        sb.Append(actualAllergen[kvp.Key]);
        sb.Append(",");
    }

    string part2Answer = sb.ToString();

    int part1Answer = 0;
    int totalIngredents = 0; 

    foreach (List<string> ingrediantList in shoppingList)
    {
        totalIngredents += ingrediantList.Count;
        foreach (string allergen in actualAllergen.Values)
        {
            part1Answer += ingrediantList.Count(x => x == allergen);
        }
    }

    part1Answer = totalIngredents - part1Answer;

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}