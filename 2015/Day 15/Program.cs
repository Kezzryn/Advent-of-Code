using System.Diagnostics;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

//    List<int[]> stepKey = new()
//    {
        //new int[] { +1, -1 },
    //};

    List<int[]> stepKey = new()
    {
        new int[] { +1, -1,  0,  0 },
        new int[] { +1,  0, -1,  0 },
        new int[] { +1,  0,  0, -1 },
        new int[] { +2, -1, -1,  0 },
        new int[] { +2, -1,  0, -1 },
        new int[] { +2,  0, -1, -1 },
        new int[] { +3, -1, -1, -1 },
    };


    int[] answer = new int[puzzleInput.Length];
    int[] stepPatterns = new int[puzzleInput.Length];

    Dictionary<string, Dictionary<CookieProperties, int>> ingredients = new();
    Dictionary<int, string> ingredientIndex = new();

    for (int i = 0; i < puzzleInput.Length;i++)
    {
        // Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
        // Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3

        string[] splitColon = puzzleInput[i].Split(":");
        ingredients.Add(splitColon[0], new());
        ingredientIndex.Add(i, splitColon[0]);

        foreach (CookieProperties property in Enum.GetValues(typeof(CookieProperties)))
        {
            // Find the space after the lower case-d name of our current property
            int startIndex = splitColon[1].IndexOf(' ', splitColon[1].IndexOf(Enum.GetName(typeof(CookieProperties), property)!.ToLower()));

            // find the next comma, then test to see if we're off the end of the string. 
            int length = splitColon[1].IndexOf(',', startIndex);
            length = (length == -1) ? splitColon[1].Length - startIndex : length - startIndex;

            ingredients[splitColon[0]].Add(property, int.Parse(splitColon[1].Substring(startIndex, length)));
        }
    }

    int ValueFunction(int[] recipe, bool countCalories = false)
    {
        int returnValue = 1;
        foreach (CookieProperties property in Enum.GetValues(typeof(CookieProperties)))
        {
            int subValue = 0;
            for(int i = recipe.GetLowerBound(0); i <= recipe.GetUpperBound(0); i++)
            {
                if (property == CookieProperties.Calories)
                {
                    if (!countCalories) subValue = 1;
                } 
                else
                {
                    subValue += recipe[i] * ingredients[ingredientIndex[i]][property];
                }
            }
            if (subValue <= 0) return 0; // no negatives permitted, and 0s will zero out the entire calculation. 
            returnValue *= subValue;
        }
        return returnValue;
    }

//      int[] testCookie = new[] { 44, 56 };
    //Console.WriteLine($"Test Check: {ValueFunction(testCookie)}");

    PriorityQueue<int[], int> queue = new();
    HashSet<string> testedSolutions = new();

    //prepopulate the search queue with some potential answers.
    queue.Enqueue(new int[] { 50, 50 }, -ValueFunction(new int[] { 50, 50 }));
    testedSolutions.Add("25252525");

    int bestAnswer = int.MinValue;

    while (queue.Count > 0)
    {
        if (!queue.TryDequeue(out int[]? potentialAnswer, out int priority)) break;
        priority *= -1;

        if (priority >= bestAnswer)
        {
            bestAnswer = priority;
        } 

        foreach (int[] step in stepKey)
        {
            int[] newAnswer = new int[step.Length];
            // DO ROLLING STARTING POINT HERE
            for(int i = 0; i < newAnswer.Length; i++)
            {
                newAnswer[i] = step[i] + potentialAnswer[i];
            }
            if (newAnswer.Sum() != 100) continue;

            string answerKey = newAnswer.Select(x => x.ToString()).Aggregate((x, y) => x + y);
            if (!testedSolutions.Contains(answerKey))
            {
                int newValue = ValueFunction(newAnswer);
                if (newValue > priority) // only enqueue 'better' solutions. 
                {
                    queue.Enqueue(newAnswer, -newValue);
                    testedSolutions.Add(answerKey);
                }
            }
        }

    }

    Console.WriteLine($"The best we can do is: {bestAnswer}");


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