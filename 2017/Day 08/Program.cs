static bool EvaluateCondition(string op, int left, int right)
{
    return op switch
    {
        ">" => left > right,
        "<" => left < right,
        ">=" => left >= right,
        "<=" => left <= right,
        "==" => left == right,
        "!=" => left != right,
        _ => throw new NotImplementedException(op)
        };
}

try
{

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int OP_TARGET = 0;
    const int OP_INSTRUCTION = 1;
    const int OP_TARGET_VALUE = 2;
    const int COMP_TARGET = 4;
    const int COMP_TYPE = 5;
    const int COMP_VALUE = 6;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, int> registers = new();
    int part2Answer = 0;

    foreach (string line in  puzzleInput)
    {
        string[] splitLine = line.Split(' ', StringSplitOptions.TrimEntries);

        registers.TryAdd(splitLine[OP_TARGET], 0);
        registers.TryAdd(splitLine[COMP_TARGET], 0);

        int comp_value = int.Parse(splitLine[COMP_VALUE]);
        int target_value = int.Parse(splitLine[OP_TARGET_VALUE]) * (splitLine[OP_INSTRUCTION] == "inc" ? 1 : -1);

        if(EvaluateCondition(splitLine[COMP_TYPE], registers[splitLine[COMP_TARGET]], comp_value))
        {
            registers[splitLine[OP_TARGET]] += target_value;
            if (registers[splitLine[OP_TARGET]] > part2Answer) part2Answer = registers[splitLine[OP_TARGET]];
        }
    }

    int part1Answer = registers.Values.Max();

    Console.WriteLine($"Part 1: The largest value at the end of the sequence is: {part1Answer}.");
    Console.WriteLine($"Part 2: The largest value ever is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}