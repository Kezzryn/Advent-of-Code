using AoC_2019_IntcodeVM;

List<string> part1Instructions = new()
{
    // Jump if any of A, B, C are holes and D is a safe landing spot.
    // if (D AND (!A or !B or !C)) 
    "NOT A J" ,
    "NOT B T" ,
    "OR J T"  ,
    "NOT C J" ,
    "OR T J"  ,
    "AND D J" ,
    "WALK"
};
 
List<string> part2Instructions = new()
{
    // building from part 1.
    // from D, safe configurations: 
    // ???H? jump immediatly 
    // E???I step, jump
    // EF??? step, step, jump (and pray)
    // if (D and (!A or !B or !C))
    // AND (H or (E and (F or I))

    "NOT A J" , //1 redo part 1
    "NOT B T" , //2
    "OR J T"  , //3
    "NOT C J" , //4
    "OR T J"  , //5
    "AND D J" , //6
    "NOT D T",  //7 //set T to false.  Alternate reddit answer: NOT I T, NOT T T
    "AND D T",  //8
    "OR F T",   //9 working backwards: (F or I) 
    "OR I T",   //0
    "AND E T",  //1 and E  
    "OR H T",   //2 or H 
    "AND T J" , //3 safe to jump, do we?
    "RUN"
};

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    //Load the VM

    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();
    IntcodeVM walkBot = new(puzzleInput);

    Console.WriteLine("Part 1 result:");
    part1Instructions.ForEach(walkBot.SetInput);
    State state = State.Running;
    while (state != State.Halted)
    {
        state = walkBot.Run();
        while (walkBot.GetOutput(out char walkOut)) Console.Write(walkOut);
    };
    walkBot.GetOutput(out long part1Answer);
    Console.WriteLine($"Part 1: Walk bot resports {part1Answer} damage.");

    IntcodeVM runBot = new(puzzleInput);
    Console.WriteLine();
    Console.WriteLine("Part 2 result:");
    part2Instructions.ForEach(runBot.SetInput);
    state = State.Running;
    while (state != State.Halted)
    {
        state = runBot.Run();
        while (runBot.GetOutput(out char runOut)) Console.Write(runOut);
    };

    runBot.GetOutput(out long part2Answer);
    Console.WriteLine($"Part 2: Run bot resports {part2Answer} damage.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}