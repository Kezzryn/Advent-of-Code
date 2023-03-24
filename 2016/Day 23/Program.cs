try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART2 = true;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    using StreamWriter sw = new("output.txt");

    int assembunnyVM(string[] instructionSet, bool isPart2 = false)
    {
        Dictionary<string, int> registers = new()
        {
            { "a", isPart2 ? 12 : 7 },
            { "b", 0 },
            { "c", 0 },
            { "d", 0 }
        };

        int instPtr = 0;
        bool isDone = false;
        while (!isDone)
        {
            string[] inst = instructionSet[instPtr].Split(' ').ToArray();

            sw.WriteLine($"{instPtr,-5}{instructionSet[instPtr],-12}{registers["a"],7}{registers["b"],7}{registers["c"],7}{registers["d"],7}");

            switch (inst[0])
            {
                case "cpy":
                    if (registers.ContainsKey(inst[2]))
                    { 
                        registers[inst[2]] = int.TryParse(inst[1], out int cpy_result) ? cpy_result : registers[inst[1]];
                    }
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
                    if ((int.TryParse(inst[1], out int jnz_test_value) ? jnz_test_value : registers[inst[1]]) == 0)
                    {
                        instPtr++;
                        break;
                    }
                    int jnz_offset = int.TryParse(inst[2], out int jnz_amount) ? jnz_amount : registers[inst[2]];
                    instPtr += jnz_offset;
                    break;
                case "tgl":
                    int tgl_offset = instPtr + (int.TryParse(inst[1], out int tgl_target) ? tgl_target : registers[inst[1]]);

                    // target outside program, do nothing. 
                    if (tgl_offset < 0 || tgl_offset > instructionSet.GetUpperBound(0))
                    {
                        instPtr++;
                        break;
                    }
                    string[] temp = instructionSet[tgl_offset].Split(' ').ToArray();
                    
                    switch (temp.Length)
                    {
                        case 2:
                            temp[0] = temp[0] == "inc" ? "dec" : "inc";
                            break;
                        case 3:
                            temp[0] = temp[0] == "jnz" ? "cpy" : "jnz";
                            break;
                        default:
                            Console.WriteLine($"{temp} UNKNOWN LENGTH");
                            break;
                    }

                    sw.WriteLine($"{tgl_offset} is reset from {instructionSet[tgl_offset]} to {String.Join(" ", temp)}");
                    instructionSet[tgl_offset] = String.Join(" ", temp);
                    instPtr++;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (instPtr >= instructionSet.Length) isDone = true;
        }

        return registers["a"];
    }

    Console.WriteLine($"Part 1: The safe code is: {assembunnyVM(puzzleInput)}");
    Console.WriteLine($"Part 2: After resetting register a, the safe code is: {assembunnyVM(puzzleInput, DO_PART2)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}