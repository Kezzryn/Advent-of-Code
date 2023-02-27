using Synacor_Challenge;

try
{
    const ushort MAX_VALUE = 36768;
    const ushort ABSOLUTE_MAX = 36776;

    const string CHALLENGE_BIN = "challenge.bin";

    ushort[] mainMemory = new ushort[MAX_VALUE];
    ushort[] registers = new ushort[8];
    Stack<ushort> stack = new();

    ushort cursor = 0;
    bool isRunning = true;

    ushort GetValue(ushort address)
    {
        return address switch
        {
            <= MAX_VALUE => mainMemory[address],
            > MAX_VALUE and <= ABSOLUTE_MAX => registers[address % MAX_VALUE],
            _ => throw new Exception($"{address} is out of bounds")
        };
    }

    ushort DoInstruction(ushort cursor, ushort[] param)
    {
        ushort instruction = mainMemory[cursor]; // will throw an error if we're out of bounds, we'll keep that for now. 
        ushort nextInstruction = cursor;

        ushort a = (param.Length >= 1) ? param[0] : (ushort)0;
        ushort b = (param.Length >= 2) ? param[1] : (ushort)0;
        ushort c = (param.Length >= 3) ? param[2] : (ushort)0;

        switch (instruction)
        {
            case 0:     // halt         stop execution and terminate the program
                isRunning = false;
                break;
            case 1:     // set: 1 a b       set register <a> to the value of <b>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 3);
                break;
            case 2:     // push: 2 a	    push <a> onto the stack
                stack.Push(a);
                nextInstruction = (ushort)(cursor + 2);
                break;
            case 3:     // pop: 3 a	        remove the top element from the stack and write it into <a>; empty stack = error
                Console.WriteLine($"{ instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 2);
                break;
            case 4:     // eq: 4 a b c      set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 5:     // gt: 5 a b c      set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 6:     // jmp: 6 a 	    jump to <a>
                nextInstruction = GetValue(a);
                break;
            case 7:     // jt: 7 a b	    if <a> is nonzero, jump to <b>
                nextInstruction = (GetValue(a) != 0) ? b : (ushort)(cursor + 3);
                break;
            case 8:     // jf: 8 a b    	if <a> is zero, jump to <b>
                nextInstruction = (GetValue(a) == 0) ? b : (ushort)(cursor + 3);
                break;
            case 9:     // add: 9 a b c     assign into <a> the sum of <b> and <c> (modulo 32768)
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 10:    // mult: 10 a b c	store into <a> the product of <b> and <c> (modulo 32768)
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 11:    // mod: 11 a b c	store into <a> the remainder of <b> divided by <c>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 12:    // and: 12 a b c	stores into <a> the bitwise and of <b> and <c>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 13:    // or: 13 a b c	    stores into <a> the bitwise or of <b> and <c>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 4);
                break;
            case 14:    // not: 14 a b	    stores 15-bit bitwise inverse of <b> in <a>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 3);
                break;
            case 15:    // rmem: 15 a b	    read memory at address <b> and write it to <a>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 3);
                break;
            case 16:    // wmem: 16 a b	    write the value from <b> into memory at address <a>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 3);
                break;
            case 17:    // call: 17 a	    write the address of the next instruction to the stack and jump to <a>
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 2);
                break;
            case 18:    // ret: 18	        remove the top element from the stack and jump to it; empty stack = halt
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 1);
                break;
            case 19:    // out: 19 a        write the character represented by ascii code <a> to the terminal
                Console.Write((char)a);
                nextInstruction = (ushort)(cursor + 2);
                break;
            case 20:    // in: 20 a	        read a character from the terminal and write its ascii code to <a>; it can be assumed that once input starts, it will continue until a newline is encountered; this means that you can safely read whole lines from the keyboard and trust that they will be fully read
                Console.WriteLine($"{instruction} not implemented yet");
                nextInstruction = (ushort)(cursor + 2);
                break;
            case 21:    // noop: 21         no operation
                nextInstruction = (ushort)(cursor + 1);
                break;
            default:
                throw new NotImplementedException($"{instruction} {a} {b} {c}");
        }

        return nextInstruction;
    }

    // LOAD
    using BinaryReader r = new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read));
    while (r.BaseStream.Position < r.BaseStream.Length)
    {
        mainMemory[r.BaseStream.Position / 2] = r.ReadUInt16();
    }

    // RUN
    while (isRunning)
    {
        ushort[] param = mainMemory[int.Min(cursor + 1, MAX_VALUE -1)..int.Min(cursor + 4, MAX_VALUE - 1)];
        //Console.Write($"{Decompiler.instructionSet[mainMemory[cursor]].inst}, {param[0]}, {param[1]}, {param[2]} ");

        cursor = DoInstruction(cursor, param); // error handle this, 'cause I bet it blows up one day. 
        //Console.WriteLine();
    }

    //Decompiler.DumpIt(r);
    //r.Close();
}
catch (Exception e)
{
    Console.WriteLine(e);
}


