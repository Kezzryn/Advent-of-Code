Dictionary<char, int>  RunProgram(Dictionary<char, int> registers, string[] puzzleInput)
{
    int i = 0;
    do
    {
        switch (puzzleInput[i][0..3])
        {
            case "hlf":
                //hlf r sets register r to half its current value, then continues with the next instruction.
                registers[puzzleInput[i][^1]] /= 2;
                i++;
                break;
            case "tpl":
                //tpl r sets register r to triple its current value, then continues with the next instruction.
                registers[puzzleInput[i][^1]] *= 3;
                i++;
                break;
            case "inc":
                //inc r increments register r, adding 1 to it, then continues with the next instruction.
                registers[puzzleInput[i][^1]]++;
                i++;
                break;
            case "jmp":
                //jmp offset is a jump; it continues with the instruction offset away relative to itself.
                i += int.Parse(puzzleInput[i][3..]);
                break;
            case "jie":
                //jie r, offset is like jmp, but only jumps if register r is even("jump if even").
                i += (registers[puzzleInput[i][4]] % 2 == 0) ? int.Parse(puzzleInput[i][7..]) : 1;
                break;
            case "jio":
                //jio r, offset is like jmp, but only jumps if register r is 1("jump if one", not odd).
                i += (registers[puzzleInput[i][4]] == 1) ? int.Parse(puzzleInput[i][7..]) : 1;
                break;
            default:
                throw new Exception($"Unknown instruction. {puzzleInput[i][0..2]}");
        }
    } while (i < puzzleInput.Length);

    return registers;
}


try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<char, int> programResultsPart1 = RunProgram(new() { { 'a', 0 }, { 'b', 0 } }, puzzleInput);

    Dictionary<char, int> programResultsPart2 = RunProgram(new() { { 'a', 1 }, { 'b', 0 } }, puzzleInput);
    
    Console.WriteLine($"Part 1: After the program has run, a = {programResultsPart1['a']} b = {programResultsPart1['b']}");

    Console.WriteLine($"Part 2: After the program has run with a=1, a = {programResultsPart2['a']} b = {programResultsPart2['b']}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
