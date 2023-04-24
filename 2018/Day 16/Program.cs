using AoC_2018_Day_16;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    const int PUZZLE_PART1 = 0;
    const int PUZZLE_PART2 = 1;
    const int PART1_BEFORE = 0;
    const int PART1_OPCODES = 1;
    const int PART1_AFTER = 2;
    const int OPCODE = 0;
    const int IN_A = 1;
    const int IN_B = 2;
    const int OUTPUT = 3;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    string[] puzzleParts = puzzleInput.Split(CRLF + CRLF + CRLF);

    int[][][] puzzlePart1 = puzzleParts[PUZZLE_PART1]
        .Split(CRLF + CRLF)
            .Select(x => x.Split(CRLF)
                .Select(y => (y.Contains(':') ? y.Replace(",","").Trim("[]BoAfter: ".ToCharArray()) : y).Split(' ').Select(int.Parse).ToArray()
                ).ToArray()
            ).ToArray();

    int[][] puzzlePart2 = puzzleParts[PUZZLE_PART2]
        .Split(CRLF, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(' ').Select(int.Parse).ToArray()
        ).ToArray();

    int part1Answer = 0;

    ChronalVM vm = new();

    Dictionary<int, HashSet<int>> potentialOpCodes = new();
    for (int i = 0; i < 16; i++)
    {
        potentialOpCodes.Add(i, new());
    }

    foreach (int[][] opCodeTest in puzzlePart1)
    {
        int result = 0;

        for (int i = 0; i < 16; i++)
        {
            vm.SetRegisters(opCodeTest[PART1_BEFORE]);
            vm.Dispatcher(i, opCodeTest[PART1_OPCODES][IN_A], opCodeTest[PART1_OPCODES][IN_B], opCodeTest[PART1_OPCODES][OUTPUT]);
            if (vm.CompareToRegisters(opCodeTest[PART1_AFTER]))
            {
                result += 1;
                potentialOpCodes[opCodeTest[PART1_OPCODES][OPCODE]].Add(i);
            }
        }
        if (result >= 3) part1Answer++;
    }

    // Filter down the opcodes list.
    while(potentialOpCodes.Any(x => x.Value.Count > 1))
    {
        foreach (var kvp in potentialOpCodes.Where(x => x.Value.Count == 1))
        {
            foreach (var kvp2 in potentialOpCodes.Where(x => x.Value.Count > 1))
            {
                kvp2.Value.Remove(kvp.Value.FirstOrDefault(-1));
            }
        }
    }

    vm.ClearRegisters();
    foreach (int[] opCode in puzzlePart2)
    {
        vm.Dispatcher(potentialOpCodes[opCode[OPCODE]].FirstOrDefault(-1), opCode[IN_A], opCode[IN_B], opCode[OUTPUT]);
    }
    
    int part2Answer = vm.GetRegisterValue(0);

    Console.WriteLine($"Part 1: The number of samples that behave like three or more opcodes are: {part1Answer}");
    Console.WriteLine($"Part 2: The value in register 0 after running the program is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}