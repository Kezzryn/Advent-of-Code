using AoC_2016_Day_11;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
  
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    // length - 1 for items, +2 for Elevator and steps
    
    int[] initialStatePart1 = new int[puzzleInput.Split(" a ").Length + 1];
    initialStatePart1[RTFElevator.INDEX_ELEVATOR] = 1;

    Dictionary<string, int> nameIndex = [];

    //The [first|second|third|fourth] floor contains (0-x)[a [<item>-compatible|<item>] [microchip|generator],] and (1)[a [<item>-compatible|<item>] [microchip|generator]].
    foreach (string line in puzzleInput.Split(CRLF).ToList())
    {
        if (line.Contains("nothing relevant.")) continue;

        int floor = line.Split(' ')[1] switch
        {
            "first" => 1,
            "second" => 2,
            "third" => 3,
            "fourth" => 4,
            _ => -1
        };

        string[] items = line[line.IndexOf(' ', line.IndexOf("contains"))..].Replace("-compatible","").Split(" a ", StringSplitOptions.RemoveEmptyEntries);

        foreach (string item in items)
        {
            string[] t = item.Split(' ').Select(x => x.Trim(".,".ToCharArray())).ToArray();
            if (!nameIndex.TryGetValue(t[0], out int value))
            {
                nameIndex.Add(t[0], nameIndex.Values.Select(x => x).DefaultIfEmpty().Max() + 2);
            }

            initialStatePart1[t[1] == "generator" ? nameIndex[t[0]] : nameIndex[t[0]]+1] = floor; 
        }
    }

    // Part two adds two more item pairs to really stress test the pruning algorithm.
    int[] initialStatePart2 = new int[initialStatePart1.Length + 4];

    for(int i = 0; i <  initialStatePart2.Length; i++)
    {
        initialStatePart2[i] = i < initialStatePart1.Length ? initialStatePart1[i] : 1;
    }

    Console.WriteLine($"Part 1: The solution is: {RTFElevator.CountTheSteps(initialStatePart1)}");
    Console.WriteLine($"Part 2: The solution is: {RTFElevator.CountTheSteps(initialStatePart2)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}