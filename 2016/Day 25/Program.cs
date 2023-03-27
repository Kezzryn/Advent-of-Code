using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    static string assembunnyVM(string[] instructionSet, int a)
    {
        Dictionary<string, int> registers = new()
        {
            { "a", a },
            { "b", 0 },
            { "c", 0 },
            { "d", 0 }
        };
        StringBuilder sb = new();

        int instPtr = 0;
        bool isDone = false;
        int outCount = 0;
        while (!isDone)
        {
            string[] inst = instructionSet[instPtr].Split(' ').ToArray();
            instPtr++;

            switch (inst[0])
            {
                case "cpy":
                    if (registers.ContainsKey(inst[2]))
                    {
                        registers[inst[2]] = int.TryParse(inst[1], out int cpy_result) ? cpy_result : registers[inst[1]];
                    }
                    break;
                case "inc":
                    registers[inst[1]]++;
                    break;
                case "dec":
                    registers[inst[1]]--;
                    break;
                case "jnz":
                    if ((int.TryParse(inst[1], out int jnz_test_value) ? jnz_test_value : registers[inst[1]]) == 0) break;

                    int jnz_offset = int.TryParse(inst[2], out int jnz_amount) ? jnz_amount : registers[inst[2]];
                    instPtr--;
                    instPtr += jnz_offset;
                    break;
                case "out":
                    outCount++;
                    sb.Append($"{registers[inst[1]]}");
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (instPtr >= instructionSet.Length || outCount > 24) isDone = true;
        }
        return sb.ToString();
    }

    for (int i = 0; i < 256;  i++)
    {
        string temp = assembunnyVM(puzzleInput, i);
        if (temp.StartsWith("10101010") || temp.StartsWith("01010101"))
        {
            Console.WriteLine($"A possible signal has been found: {i} => {temp}");
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}