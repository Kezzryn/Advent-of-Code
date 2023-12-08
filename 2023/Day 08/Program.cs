static List<int> Factor(int number)
{
    // From StackOverflow. 
    // https://stackoverflow.com/questions/239865/best-way-to-find-all-factors-of-a-given-number

    var factors = new List<int>();
    int max = (int)Math.Sqrt(number);  // Round down

    for (int factor = 1; factor <= max; ++factor) // Test from 1 to the square root, or the int below it, inclusive.
    {
        if (number % factor == 0)
        {
            factors.Add(factor);
            if (factor != number / factor) // Don't add the square root twice!  Thanks Jon
                factors.Add(number / factor);
        }
    }
    return factors;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    char[] steps = puzzleInput[0].ToCharArray();
    Dictionary<string, (string Left, string Right)> map = puzzleInput.Skip(2).ToDictionary(k => k[0..3], v => (v[7..10], v[^4..^1]));

    int GetSteps(string mapCursor, bool doPart2 = false)
    {
        int numSteps = 0;
        bool isDone = false;
        do
        {
            mapCursor = steps[numSteps++ % steps.Length] == 'L' ? map[mapCursor].Left : map[mapCursor].Right;
            isDone = doPart2 ? mapCursor[^1] == 'Z' : mapCursor == "ZZZ";
        }
        while (!isDone);

        return numSteps;
    }

    int part1Answer = GetSteps("AAA");

    //Calculate the Least Common Multiple via the Prime Factorization method.
    long part2Answer = map.Keys.Where(x => x[2] == 'A')
                        .Select(x => GetSteps(x, true))
                        .SelectMany(x => Factor(x).Where(w => w != x).Select(x => (long)x)) //remove the answer from the factorization and convert to long
                        .Distinct()
                        .Aggregate((x, y) => x * y);

    Console.WriteLine($"Part 1: It will take {part1Answer} steps to walk the path following the map.");
    Console.WriteLine($"Part 2: It takes ghosts {part2Answer} steps to walk all the paths at the same time.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}