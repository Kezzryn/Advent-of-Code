try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART2 = true;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int assembunnyVM(string[] instructionSet, bool isPart2 = false)
    {
        Dictionary<string, int> registers = new()
        {
            { "a", 0 },
            { "b", 0 },
            { "c", 0 },
            { "d", 0 }
        };
        if (isPart2) registers["c"] = 1;

        int instPtr = 0;
        bool isDone = false;
        while (!isDone)
        {
            string[] inst = puzzleInput[instPtr].Split(' ').ToArray();

            switch (inst[0])
            {
                case "cpy":
                    registers[inst[2]] = int.TryParse(inst[1], out int cpy_result) ? cpy_result : registers[inst[1]];
                    instPtr++;
                    break;
                case "inc":
                    registers[inst[1]]++;
                    instPtr++;
                    break;
                case "dec":
                    registers[inst[1]]--;
                    instPtr++;
                    break;
                case "jnz":
                    int jnz_result = int.TryParse(inst[1], out int jnz_test) ? jnz_test : registers[inst[1]];
                    int target = int.TryParse(inst[2], out int jnz_target) ? jnz_target : registers[inst[2]];
                    instPtr += (jnz_result != 0) ? target : 1;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (instPtr >= puzzleInput.Length) isDone = true;
        }

        return registers["a"];
    }

    Console.WriteLine($"Part 1: The monorail code is: {assembunnyVM(puzzleInput)}");
    Console.WriteLine($"Part 2: After setting the ignition key the new monorail code is: {assembunnyVM(puzzleInput, DO_PART2)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}