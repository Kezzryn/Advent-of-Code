using static System.Runtime.InteropServices.JavaScript.JSType;
try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    int[] puzzleInput = new[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11 };//File.ReadAllLines(PUZZLE_INPUT).Select(int.Parse).ToArray();

    int[] w = new int[puzzleInput.Length + 1];
    int[] v = new int[puzzleInput.Length + 1];

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        w[i + 1] = puzzleInput[i];
        v[i + 1] = puzzleInput[i];
    }

    
    int n = puzzleInput.Length;
    int W = puzzleInput.Sum() / 3;

    // Input:
    // Values (stored in array v)
    // Weights (stored in array w)
    // Number of distinct items (n)
    // Knapsack capacity (W)
    // NOTE: The array "v" and array "w" are assumed to store all relevant values starting at index 1.

    //Initialize all value[i, j] = -1
    int[,] value = new int[n+1, W+1]; // push this out one, 'cause zero bound vs 1 bound. 

    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < W; j++)
        {
            value[i, j] = -1;
        }
    }

    /*
    function knapsack(i: int, j: int): Set<int> {
        if i == 0 then:
            return { }
        if m[i, j] > m[i - 1, j] then:
            return { i} ∪ knapsack(i - 1, j - w[i])
    else:
        return knapsack(i - 1, j)
    }
    
    knapsack(n, W)
    */


    // Define function m so that it represents the maximum value we can get under the condition:
    // use first i items, total weight limit is j
    void m(int i, int j)
    {
        if (i == 0 || j <= 0)
        {
            value[i, j] = 0;
            return;
        }

        // m[i-1, j] has not been calculated, we have to call function m
        if (value[i - 1, j] == -1) m(i - 1, j);

        // item cannot fit in the bag
        if (w[i] > j)
        {
            value[i, j] = value[i - 1, j];
        }
        else
        {
            // m[i-1,j-w[i]] has not been calculated, we have to call function m
            if (value[i - 1, j - w[i]] == -1)
            {
                m(i - 1, j - w[i]);
            }
            value[i, j] = int.Max(value[i - 1, j], value[i - 1, j - w[i]] + v[i]);
        }
        return;
    }

    m(n, W);

    Console.WriteLine("");
}
catch (Exception e)
{
    Console.WriteLine(e);
}