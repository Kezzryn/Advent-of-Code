try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').ToList();

    static int HASH(string input) => input.Aggregate(0, (x, y) => (((x + y) * 17) % 256));

    int part1Answer = puzzleInput.Sum(HASH);

    List<List<string>> boxes = new();
    for (int i = 0; i < 256; i++) boxes.Add(new());
    
    const char OP_MINUS = '-';
    const char OP_EQUAL = '=';
    foreach (string input in puzzleInput)
    {
        char opCode = input.Contains(OP_EQUAL) ? OP_EQUAL : OP_MINUS;

        string label = input[..input.IndexOfAny("-=".ToCharArray())];
        int hashIndex = HASH(label);

        int index = boxes[hashIndex].FindIndex(x => x.StartsWith(label));

        if (opCode == OP_MINUS && index != -1) boxes[hashIndex].RemoveAt(index);
        if (opCode == OP_EQUAL && index == -1) boxes[hashIndex].Add(input);
        if (opCode == OP_EQUAL && index != -1) boxes[hashIndex][index] = input;
    }

    int part2Answer = boxes.Select((box, boxIdx) => box.Select((lens, lensIdx) => (1 + boxIdx) * (1 + lensIdx) * (lens[^1] - '0')).Sum()).Sum();

    Console.WriteLine($"Part 1: The hash of each line gives a sum of {part1Answer}.");
    Console.WriteLine($"Part 2: The focusing power of the lens array is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
