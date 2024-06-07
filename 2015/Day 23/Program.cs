static Dictionary<char, int>  RunProgram(Dictionary<char, int> registers, string[] puzzleInput)
{
    int instPtr = 0;
    do
    {
        switch (puzzleInput[instPtr][0..3])
        {   
            case "hlf":
                //hlf r sets register r to half its current value, then continues with the next instruction.
                registers[puzzleInput[instPtr][^1]] /= 2;
                instPtr++;
                break;
            case "tpl":
                //tpl r sets register r to triple its current value, then continues with the next instruction.
                registers[puzzleInput[instPtr][^1]] *= 3;
                instPtr++;
                break;
            case "inc":
                //inc r increments register r, adding 1 to it, then continues with the next instruction.
                registers[puzzleInput[instPtr][^1]]++;
                instPtr++;
                break;
            case "jmp":
                //jmp offset is a jump; it continues with the instruction offset away relative to itself.
                instPtr += int.Parse(puzzleInput[instPtr][3..]);
                break;
            case "jie":
                //jie r, offset is like jmp, but only jumps if register r is even("jump if even").
                instPtr += (registers[puzzleInput[instPtr][4]] % 2 == 0) ? int.Parse(puzzleInput[instPtr][7..]) : 1;
                break;
            case "jio":
                //jio r, offset is like jmp, but only jumps if register r is 1("jump if one", not odd).
                instPtr += (registers[puzzleInput[instPtr][4]] == 1) ? int.Parse(puzzleInput[instPtr][7..]) : 1;
                break;
            default:
                throw new Exception($"Unknown instruction. {puzzleInput[instPtr][0..2]}");
        }
    } while (instPtr < puzzleInput.Length);

    return registers;
}


try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<char, int> part1Answer = RunProgram(new() { { 'a', 0 }, { 'b', 0 } }, puzzleInput);

    Dictionary<char, int> part2Answer = RunProgram(new() { { 'a', 1 }, { 'b', 0 } }, puzzleInput);
    
    Console.WriteLine($"Part 1: After the program has run, a = {part1Answer['a']} b = {part1Answer['b']}");

    Console.WriteLine($"Part 2: After the program has run with a=1, a = {part2Answer['a']} b = {part2Answer['b']}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
