using System.Net;

namespace Synacor_Challenge

{
    internal class Synacor9000
    {
        const ushort MEMORY_MAX = 32767; // 0 bound address max
        const ushort MODULO = 32768;
        // 32768 to 32775 are registers. 
        const ushort INVALID_MEMORY = 32776; // start of invalid memory numbers. 

        ushort[] _mainMemory = new ushort[32768]; 
        ushort[] _registers = new ushort[8];
        
        private new Stack<ushort> _stack = new();

        private ushort _instPtr = 0;  // instruction pointer. 

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
                if (_mainMemory[_instPtr] == 0) break;
                Dispatcher(Mem_Read());
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
                case 9 or 10 or 11:
                    Math_Basic(instruction); 
                    break;
                case 12 or 13 or 14:
                    Math_Bitwise(instruction);
                    break;
                case 19 or 20:
                    Terminal(instruction);
                    break;
                case 21:    // noop: 21         no operation
                    // do nothing. 
                    break;
                default:
                    throw new NotImplementedException($"{instruction}");
            }
        }
        private void Comparison(ushort instruction)
        {
            ushort address = Mem_ReadRaw();
            ushort b = Mem_Read();
            ushort c = Mem_Read();

            ushort value = instruction switch
            {
                4 => (b == c) ? (ushort)1 : (ushort)0,
                    // eq: 4 a b c      set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise    
                5 => (b > c) ? (ushort)1 : (ushort)0,
                    // gt: 5 a b c      set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
                _ => throw new NotImplementedException()
            };
            Mem_Write(address, value);
        }
        private ushort Jump(ushort instruction)
        {
            ushort a; // ONLY read if needed
            ushort b; // ONLY read if needed

            switch (instruction)
            {
                // jmp: 6 a 	jump to <a>
                case 6:
                    return Mem_Read();

                // jt: 7 a b	if <a> is nonzero, jump to <b>
                case 7:
                    a = Mem_Read();
                    b = Mem_Read();
                    return (a != 0) ? b : _instPtr;

                // jf: 8 a b    if <a> is zero, jump to <b>
                case 8:
                    a = Mem_Read();
                    b = Mem_Read();
                    return (a == 0) ? b : _instPtr;

                // call: 17 a   write the address of the next instruction to the stack and jump to <a> 
                case 17:
                    a = Mem_Read();
                    _stack.Push(_instPtr); 
                    return a;

                // ret: 18    remove the top element from the stack and jump to it; empty stack = halt
                case 18:
                    if (_stack.Count == 0) throw new Exception("Stack is empty");
                    return _stack.Pop();

                default:
                    throw new NotImplementedException();
            }
        }
        private void Math_Basic(ushort instruction)
        {
            ushort address = Mem_ReadRaw();
            ushort b = Mem_Read();
            ushort c = Mem_Read();
            ushort value = instruction switch
            {
                9 => (ushort)((b + c) % MODULO),
                    // add: 9 a b c     assign into <a> the sum of <b> and <c> (modulo 32768)
                10 => (ushort)((b * c) % MODULO),
                    // mult: 10 a b c	store into <a> the product of <b> and <c> (modulo 32768)
                11 => (ushort)(b % c),
                    // mod: 11 a b c	store into <a> the remainder of <b> divided by <c>
                _ => throw new NotImplementedException()
            };
            Mem_Write(address, value);
        }
        private void Math_Bitwise(ushort instruction)
        {
            ushort address = Mem_ReadRaw();
            ushort b = Mem_Read();
            ushort c = (instruction != 14) ? Mem_Read() : (ushort)0;
            ushort value = instruction switch
            {
                12 => (ushort)(b & c),
                    // and: 12 a b c	stores into <a> the bitwise and of <b> and <c>
                13 => (ushort)(b | c),
                    // or: 13 a b c	    stores into <a> the bitwise or of <b> and <c>
                14 => (ushort)((ushort)((ushort)~b << 1) >> 1),
                    // not: 14 a b	    stores 15-bit bitwise inverse of <b> in <a>
                _ => throw new NotImplementedException()
                    // Why are we even here? 
            };
            Mem_Write(address, value);
        }
        private void Mem_Manipulation(ushort instruction)
        {
            ushort address;
            ushort value;

            switch (instruction)
            {
                // set: 1 a b       set register <a> to the value of <b>
                case 1:
                    address = Mem_ReadRaw();
                    value = Mem_Read();
                    if (address <= MEMORY_MAX)
                        throw new Exception($"Bad address for: set {address} {value}");
                    Mem_Write(address, value);
                    break;

                // rmem: 15 a b	    read memory at address <b> and write it to <a>
                case 15:
                    address = Mem_ReadRaw();
                    value = Mem_Read();
                    Mem_Write(address, value);
                    break;

                // wmem: 16 a b	    write the value from <b> into memory at address <a>
                case 16:
                    address = Mem_ReadRaw();
                    value = Mem_ReadRaw();
                    Mem_Write(address, value);
                    break;

                default:
                    // Why are we even here? 
                    throw new NotImplementedException();
            }
        }
        private ushort Mem_Read()
        {
            ushort returnValue = Mem_ReadRaw();

            return returnValue switch
                    {
                        <= MEMORY_MAX => returnValue,
                        > MEMORY_MAX and < INVALID_MEMORY => _registers[returnValue % MODULO],
                        _ => throw new Exception($"{returnValue} is out of bounds")
                    };
        }
        private ushort Mem_ReadRaw()
        {
            ushort returnValue = _mainMemory[_instPtr];
            _instPtr++;
            return returnValue;
        }
        private void Mem_Stack(ushort instruction)
        {
            switch (instruction)
            {   // push: 2 a    push <a> onto the stack
                case 2:
                    _stack.Push(Mem_Read());
                    break;
                // pop: 3 a     remove the top element from the stack and write it into <a>;
                // empty stack = error 
                case 3:
                    if (_stack.Count == 0) throw new Exception("Stack is empty");
                    Mem_Write(Mem_ReadRaw(), _stack.Pop());
                    break;
                default:
                    // Why are we even here? 
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
        private void Terminal(ushort instruction)
        {
            switch (instruction)
            {
                // out: 19 a        write the character represented by ascii code <a> to the terminal
                case 19:
                    Console.Write((char)Mem_Read());
                    break;
                case 20:
                    // in: 20 a
                    // read a character from the terminal and write its ascii code to <a>;
                    // it can be assumed that once input starts, it will continue until a newline is encountered;
                    // this means that you can safely read whole lines from the keyboard and trust that they will be fully read
                    Console.WriteLine($"{instruction} not implemented yet");
                    _instPtr++;
                    break;
                default:
                    // Why are we even here? 
                    throw new NotImplementedException();
            }
        }
    }
}
