using AoC_2019_IntcodeVM;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int RESULT_POS = 0;
    const int PART_TWO_TARGET = 19690720;
    const int NOUN = 1;
    const int VERB = 2;
    const int PART_ONE_NOUN = 12;
    const int PART_ONE_VERB = 2;


    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(int.Parse).ToArray();

    IntcodeVM vm = new(puzzleInput);
    vm.Mem_Write(NOUN, PART_ONE_NOUN);
    vm.Mem_Write(VERB, PART_ONE_VERB);
    vm.Run();
    int part1Answer = vm.Mem_Read(RESULT_POS);
    
    int part2Answer = 0;
    foreach ((int noun, int verb) in from n in Enumerable.Range(0,99)
                                     from v in Enumerable.Range(0,99)
                                     select(n,v))
    {
        vm.Reset(puzzleInput);
        vm.Mem_Write(NOUN, noun);
        vm.Mem_Write(VERB, verb);
        vm.Run();
        if(vm.Mem_Read(0) == PART_TWO_TARGET)
        {
            part2Answer = (100 * noun) + verb;
            break;
        }
    }

    Console.WriteLine($"Part 1: The value at 0 is {part1Answer}.");
    Console.WriteLine($"Part 2: The code that produces {PART_TWO_TARGET} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}