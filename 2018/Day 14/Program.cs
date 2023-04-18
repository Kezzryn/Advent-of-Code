using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART_1_NEXT_X = 10;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    int[] puzzleDigits = puzzleInput.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();

    int numDigits = puzzleInput.Length;
    int part1NumRecipes = int.Parse(puzzleInput);

    List<int> recipes = new()
    {
        3,
        7
    };

    int elfOne = 0;
    int elfTwo = 1;

    bool part1Done = false;
    bool part2Done = false;

    StringBuilder part1Answer = new();
    int part2Answer = 0;
    Queue<int> recipeQueue = new();

    do
    {
        int newValue = recipes[elfOne] + recipes[elfTwo];
        if (newValue >= 10) recipeQueue.Enqueue(1); 
        recipeQueue.Enqueue(newValue % 10);
        while (recipeQueue.TryDequeue(out int currentRecipe))
        {
            recipes.Add(currentRecipe);

            if (!part1Done && recipes.Count == part1NumRecipes + PART_1_NEXT_X)
            {
                foreach (int i in recipes.TakeLast(PART_1_NEXT_X))
                {
                    part1Answer.Append(i);
                }
                part1Done = true;
            }

            if (!part2Done && recipes.Count > numDigits && currentRecipe == puzzleDigits[numDigits - 1])
            {
                bool isMatch = true;

                for(int i = 0; i < numDigits; i++)
                {
                    if (recipes[recipes.Count - numDigits + i] != puzzleDigits[i])
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    part2Answer = recipes.Count - numDigits;
                    part2Done = true;
                }
            }
        }

        elfOne = (elfOne + recipes[elfOne] + 1) % recipes.Count;
        elfTwo = (elfTwo + recipes[elfTwo] + 1) % recipes.Count;

    } while (!(part1Done && part2Done));

    Console.WriteLine($"Part 1: The scores of the next 10 recipies are: {part1Answer}");

    Console.WriteLine($"Part 2: The number of recipies that appear before the target score is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}