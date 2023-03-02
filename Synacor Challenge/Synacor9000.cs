using System.Net;

namespace Synacor_Challenge

{
    internal class Synacor9000
    {
        // got tired of (ushort)0 (ushort)1
        private const ushort USHORT_0 = 0;
        private const ushort USHORT_1 = 1;

        const ushort MEMORY_MAX = 32767; // 0 bound address max
        const ushort MODULO = 32768;
        const ushort INVALID_MEMORY = 32776; // start of invalid memory numbers. 

        // memory regions. 
        private readonly ushort[] _mainMemory = new ushort[32768];
        private readonly ushort[] _registers = new ushort[8];
        private readonly Stack<ushort> _stack = new();

        // instruction pointer. 
        private ushort _instPtr = 0;
        public Synacor9000() {}
        public void Load(BinaryReader reader)
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                _mainMemory[reader.BaseStream.Position / 2] = reader.ReadUInt16();
            }
        }
        public void Run()
        {
            while (true)
            {
                if (Ptr_Peek() == 0) break;
                Dispatcher(Ptr_Read());
            }
        }
        private void Dispatcher(ushort instruction)
        {
            switch (instruction)
            {
                // halt     stop execution and terminate the program
                // we should never pass the Dispatcher this instruction.
                case 0:
                    throw new NotImplementedException();
                case 1 or 15 or 16:
                    Mem_Manipulation(instruction);
                    break;
                case 2 or 3:
                    Mem_Stack(instruction);
                    break;
                case 4 or 5:
                    Comparison(instruction); 
                    break;
                case 6 or 7 or 8 or 17 or 18:
                    _instPtr = Jump(instruction);
                    break;
                case >= 9 and <= 14: 
                    Math_Synacor(instruction); 
                    break;
                case 19 or 20:
                    Terminal(instruction);
                    break;
                case 21:
                    // noop: 21  no operation 
                    break;
                default:
                    throw new NotImplementedException($"{instruction}");
            }
        }
        private void Comparison(ushort instruction)
        {
            ushort address = Ptr_ValueAt();
            ushort b = Ptr_Read();
            ushort c = Ptr_Read();

            ushort value = instruction switch
            {
                4 => (b == c) ? USHORT_1 : USHORT_0,        // eq: 4 a b c  set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise    
                5 => (b  > c) ? USHORT_1 : USHORT_0,        // gt: 5 a b c  set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
                _ => throw new NotImplementedException()    // Comparison shmarison.
            };
            Mem_Write(address, value);
        }
        private ushort Jump(ushort instruction)
        {
            ushort a; // do not pre-assign. ret could break things 
            ushort b; 

            switch (instruction)
            {
                case 6:                             // jmp: 6 a     jump to <a>
                     a = Ptr_Read();
                    return a;
                case 7:                             // jt: 7 a b	if <a> is nonzero, jump to <b>
                    a = Ptr_Read();
                    b = Ptr_Read();
                    return (a != 0) ? b : _instPtr;
                case 8:                             // jf: 8 a b    if <a> is zero, jump to <b>
                    a = Ptr_Read();
                    b = Ptr_Read();
                    return (a == 0) ? b : _instPtr;
                case 17:                            // call: 17 a   write the address of the next instruction to the stack and jump to <a> 
                    a = Ptr_Read();
                    _stack.Push(_instPtr); 
                    return a;
                case 18:                            // ret: 18    remove the top element from the stack and jump to it; empty stack = halt
                    if (_stack.Count == 0) throw new Exception("Stack is empty");
                    return _stack.Pop();
                default:                            // Jump around, jump around, get up, get up and go thump.
                    throw new NotImplementedException();
            }
        }
        private void Math_Synacor(ushort instruction)
        {
            ushort address = Ptr_ValueAt();
            ushort b = Ptr_Read();
            ushort c = (instruction != 14) ? Ptr_Read() : USHORT_1;
            ushort value = instruction switch
            {
                 9 => (ushort)((b + c) % MODULO),           //  add:  9 a b c   assign into <a> the sum of <b> and <c> (modulo 32768)
                10 => (ushort)(b * c % MODULO),             // mult: 10 a b c	store into <a> the product of <b> and <c> (modulo 32768)
                11 => (ushort)(b % c),                      //  mod: 11 a b c	store into <a> the remainder of <b> divided by <c>
                12 => (ushort)(b & c),                      //  and: 12 a b c   stores into <a> the bitwise and of <b> and <c>
                13 => (ushort)(b | c),                      //   or: 13 a b c	stores into <a> the bitwise or of <b> and <c>
                14 => (ushort)((ushort)(~b << c) >> c),     //  not: 14 a b	    stores 15-bit bitwise inverse of <b> in <a>
                _ => throw new NotImplementedException()    // Math is MATH! 
            };
            Mem_Write(address, value);
        }
        private void Mem_Manipulation(ushort instruction)
        {
            ushort address;
            ushort value;

            switch (instruction)
            {
                case 1:                         // set: 1 a b       set register <a> to the value of <b>
                    address = Ptr_ValueAt();
                    value = Ptr_Read();
                    break;
                case 15:                        // rmem: 15 a b	    read memory at address <b> and write it to <a>
                    address = Ptr_ValueAt();
                    value = Mem_Read(Ptr_Read());
                    break;
                case 16:                        // wmem: 16 a b	    write the value from <b> into memory at address <a>
                    address = Ptr_ValueAt();
                    value = Ptr_Read();
                    break;
                default:                                    // I don't remember where I came from. 
                    throw new NotImplementedException();    
            }
            Mem_Write(address, value);
        }
        private ushort Mem_Read(ushort address)
        {
            return address switch
                {
                    <= MEMORY_MAX => _mainMemory[address],
                    > MEMORY_MAX and < INVALID_MEMORY => _registers[address % MODULO],
                    _ => throw new Exception($"{address} is out of bounds")
                };
        }
        private void Mem_Stack(ushort instruction)
        {
            switch (instruction)
            {   
                case 2:                                     // push: 2 a    push <a> onto the stack
                    _stack.Push(Ptr_ValueAt());
                    break;
                case 3:                                     //  pop: 3 a    remove the top element from the stack and write it into <a>; empty stack = error 
                    if (_stack.Count == 0) throw new Exception("Stack is empty");
                    Mem_Write(Ptr_ValueAt(), _stack.Pop());
                    break;
                default:                                    // We suck at Jenga
                    throw new NotImplementedException();
            }
        }
        private void Mem_Write(ushort address, ushort value)
        {
            switch (address)
            {
                case <= MEMORY_MAX: 
                    _mainMemory[address] = value;
                    break;
                case > MEMORY_MAX and < INVALID_MEMORY:
                    _registers[address % MODULO] = value;
                    break;
                default:
                    throw new Exception($"{address} is out of bounds");
            };
        }
        private ushort Ptr_Peek() => Mem_Read(_instPtr);
        private ushort Ptr_Read() => (Ptr_Peek() <= MEMORY_MAX) ? Mem_Read(_instPtr++) : Mem_Read(Mem_Read(_instPtr++));
        private ushort Ptr_ValueAt() => _mainMemory[_instPtr++];
        private void Terminal(ushort instruction)
        {
            switch (instruction)
            {
                case 19:                                    // out: 19 a    write the character represented by ascii code <a> to the terminal
                    Console.Write((char)Ptr_Read());
                    break;
                case 20:                                    // in: 20 a     read a character from the terminal and write its ascii code to <a>;
                    // it can be assumed that once input starts, it will continue until a newline is encountered;
                    // this means that you can safely read whole lines from the keyboard and trust that they will be fully read
                    ushort address = Ptr_Read();
                    ushort value = (ushort)Console.Read();
                    Mem_Write(address, value);
                    break;
                default:
                    throw new NotImplementedException();    // PEBCAK 
            }
        }
    }
}
