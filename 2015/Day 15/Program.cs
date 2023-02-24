try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int[] answer = new int[puzzleInput.Length];

    Dictionary<string, Dictionary<CookieProperties, int>> ingrediants = new();
    
    foreach (string puzzle in puzzleInput)
    {
        // Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
        // Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3

        string[] splitColon = puzzle.Split(":");
        ingrediants.Add(splitColon[0], new());

        foreach (CookieProperties property in Enum.GetValues(typeof(CookieProperties)))
        {
            // Find the space after the lower cased name of our current property
            int startIndex = splitColon[1].IndexOf(' ', splitColon[1].IndexOf(Enum.GetName(typeof(CookieProperties), property)!.ToLower()));

            // find the next comma, then test to see if we're off the end of the string. 
            int length = splitColon[1].IndexOf(',', startIndex);
            length = (length == -1) ? splitColon[1].Length - startIndex : length - startIndex;

            ingrediants[splitColon[0]].Add(property, int.Parse(splitColon[1].Substring(startIndex, length)));
        }
    }

    int ValueFunction(Dictionary<string, int> recipe, bool countCalories = false)
    {
        int returnValue = 1;
        foreach (CookieProperties property in Enum.GetValues(typeof(CookieProperties)))
        {
            int subValue = 0;
            foreach (string ingredient in recipe.Keys)
            {
                if (property == CookieProperties.Calories)
                {
                    if (!countCalories) subValue = 1;
                } 
                else
                {
                    subValue += recipe[ingredient] * ingrediants[ingredient][property];
                }
            }
            if (subValue <= 0) return 0; // no negatives permitted, and 0s will zero out the entire calculation. 
            returnValue *= subValue;
        }
        return returnValue;
    }

    Dictionary<string, int> testCookie = new()
    {
        {"Butterscotch", 44 },
        {"Cinnamon", 56 }
    };

    Console.WriteLine($"Test Check: {ValueFunction(testCookie)}");

    







}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum CookieProperties
{
    Capacity,
    Durability,
    Flavor,
    Texture,
    Calories
};