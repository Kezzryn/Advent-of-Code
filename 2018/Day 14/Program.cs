try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART_1_NEXT_X = 10;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    List<int> puzzleDigits = puzzleInput.Select(x => x - '0').ToList();

    int numDigits = puzzleInput.Length;
    int part1NumRecipes = int.Parse(puzzleInput);

    List<int> recipes = [3,7];

    int elfOne = 0;
    int elfTwo = 1;
    
    string part1Answer = String.Empty;
    int part2Answer = -1;

    Queue<int> recipeQueue = new();
    do
    {
        int newValue = recipes[elfOne] + recipes[elfTwo];
        if (newValue >= 10) recipeQueue.Enqueue(1); 
        recipeQueue.Enqueue(newValue % 10);
        while (recipeQueue.TryDequeue(out int currentRecipe))
        {
            recipes.Add(currentRecipe);

            if (part1Answer == String.Empty 
                && recipes.Count == part1NumRecipes + PART_1_NEXT_X)
            {
                part1Answer = String.Join("", recipes.TakeLast(PART_1_NEXT_X));
            }

            if (part2Answer == -1
                && recipes.Count > numDigits 
                && currentRecipe == puzzleDigits[numDigits - 1] 
                && puzzleDigits.Select((x, i) => recipes[recipes.Count - numDigits + i] == x).All(x => x == true)) 
            {
                part2Answer = recipes.Count - numDigits;
            }
        }

        elfOne = (elfOne + recipes[elfOne] + 1) % recipes.Count;
        elfTwo = (elfTwo + recipes[elfTwo] + 1) % recipes.Count;

    } while (part1Answer == String.Empty || part2Answer == -1);

    Console.WriteLine($"Part 1: The scores of the next 10 recipies are: {part1Answer}");
    Console.WriteLine($"Part 2: The number of recipies that appear before the target score is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}