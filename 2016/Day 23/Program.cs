try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART2 = true;

    const int PART1_VALUE = 7;
    const int PART2_VALUE = 12;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    /* Program is made into two parts.  First part calculcates the factorial of the number in register A.
     * the tgl command resets commands in the last part of the loop and provides an escape hatch when C reaches 1.
     * line 1 is initializaton, run once. 
     * 2 - 19, factorial loop
     * b holds a slowly decreasing value of a
     * 2-5 initalizes values. 
     * 6-10  multiplies c by d 
     * 11-13 sets up the next loop 
     * 14-16 adds register d to reg c
     * 17 is the tlg test 
     * line 18 is a jump target for line 19. 
     * line 19 is reset by tlg to break the loop when c == 2,and is immediatly overwritten by line 20 
     * 20-21 set c and d to 72 and 77 respectivly 
     * 22-26 multiplies 72 and 77, adding the result to a 
     * lines 25, 23 and 21 are all reset by tgl, but not run until after line 19 is reset. 
     * 
     * The entire program can be expressed as: a = x! + 5544;
    */


    static int assembunnyVM(string[] instructionSet, bool isPart2 = false)
    {
        Dictionary<string, int> registers = new()
        {
            { "a", isPart2 ? PART2_VALUE : PART1_VALUE },
            { "b", 0 },
            { "c", 0 },
            { "d", 0 }
        };
        //using StreamWriter sw = new("output.txt");

        int instPtr = 0;
        bool isDone = false;
        while (!isDone)
        {
            string[] inst = instructionSet[instPtr].Split(' ').ToArray();
            //sw.WriteLine($"{instPtr,-5}{instructionSet[instPtr],-12}{registers["a"],7}{registers["b"],7}{registers["c"],7}{registers["d"],7}");
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
                    //sw.WriteLine($"{tgl_offset} is reset from {instructionSet[tgl_offset]} to {String.Join(" ", temp)}");
                    instructionSet[tgl_offset] = String.Join(" ", temp);
                    instPtr++;
                    break;
                case "mul":
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (instPtr >= instructionSet.Length) isDone = true;
        }

        return registers["a"];
    }

    Console.WriteLine($"Part 1: The safe code is: {assembunnyVM(puzzleInput)}");

    static int assmbunnyProg(int input)
    {
        int rv = input;
        for(int i = input - 1; i > 1; i--)
        {
            rv *= i;
        }

        return rv + 5544; 
    }

    Console.WriteLine($"Part 2: After resetting register a, the safe code is: {assmbunnyProg(PART2_VALUE)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}